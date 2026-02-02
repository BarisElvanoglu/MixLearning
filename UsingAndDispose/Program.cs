using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;

public class GrandMasterMemory : IDisposable
{
    private static List<object> _staticLeak = new List<object>();
    private IntPtr _nativeBuffer;
    private int[] _pinnedArray;

    // --- TEST SINIFLARI ---
    public class CachedService
    {
        public static int GlobalData = 100;
        static CachedService()
        {
            for (int i = 0; i < 1000; i++) { var t = i * i; }
        }
    }
    public class ContainerNode { public SimpleNode Child; public byte[] Padding = new byte[1024]; }
    public class SimpleNode { public int Id; }
    public class ManagedData { public byte[] Data = new byte[1024]; }

    public class SimulatedDbConnection : IDisposable
    {
        public IntPtr Handle; public string Name;
        public SimulatedDbConnection(string name) { Name = name; Handle = Marshal.AllocHGlobal(100); }
        ~SimulatedDbConnection()
        {
            Console.WriteLine($"    [Finalizer] {Name}: ÖLÜYORDU AMA HORTLADI! (GC Handle'ı kapatıyor...)");
            Release();
        }
        public void Dispose()
        {
            Console.WriteLine($"    [Dispose] {Name}: Yazılımcı efendi gibi kapattı.");
            Release();
            GC.SuppressFinalize(this);
        }
        private void Release() { if (Handle != IntPtr.Zero) { Marshal.FreeHGlobal(Handle); Handle = IntPtr.Zero; } }
    }
    public class Publisher { public event EventHandler Olay; }
    public class Subscriber { public void Dinle(object s, EventArgs e) { } }
    public class EmptyFinalizerClass { ~EmptyFinalizerClass() { } }

    public GrandMasterMemory()
    {
        _nativeBuffer = Marshal.AllocHGlobal(2048);
        _pinnedArray = GC.AllocateArray<int>(10, pinned: true);
    }

    public unsafe void Analyze()
    {
        // ... (Bölüm 1-3 aynı kaldığı için yer kaplamasın diye hızlı geçiyorum, kodun tamamı aşağıdadır) ...

        Console.WriteLine("==============================================");
        Console.WriteLine("=== BÖLÜM 1: 8 BELLEK BÖLGESİ (HARİTA) ===");
        Console.WriteLine("==============================================\n");

        ManagedData item = new ManagedData();
        PrintAddr("LOADER HEAP", typeof(ManagedData).TypeHandle.Value, "Class Tarifi");
        var method = typeof(ManagedData).GetMethod("GetType");
        RuntimeHelpers.PrepareMethod(method.MethodHandle);
        PrintAddr("JIT (CODE)", method.MethodHandle.GetFunctionPointer(), "Method Kodu");
        ManagedData* pStack = &item;
        PrintAddr("STACK", (IntPtr)pStack, "Referans");
        string lit = "GrandMaster";
        fixed (char* p = lit) PrintAddr("STRING POOL", (IntPtr)p, "Literal String");
        PrintAddr("HEAP (Gen 0)", GetAddress(item), "Standart Obje");
        byte[] lohObj = new byte[90000];
        fixed (byte* p = lohObj) PrintAddr("HEAP (LOH)", (IntPtr)p, "Büyük Obje");
        fixed (int* p = _pinnedArray) PrintAddr("HEAP (POH)", (IntPtr)p, "Sabitlenmiş");
        PrintAddr("UNMANAGED", _nativeBuffer, "OS RAM");

        Console.WriteLine("\n==============================================");
        Console.WriteLine("=== BÖLÜM 2: EVRİM & DIRTY CARD ===");
        Console.WriteLine("==============================================\n");

        ContainerNode survivor = new ContainerNode();
        Console.WriteLine($"[Doğum] Gen {GC.GetGeneration(survivor)} | Adres: 0x{GetAddress(survivor).ToString("X")}");
        GC.Collect();
        Console.WriteLine($"[1. GC] Gen {GC.GetGeneration(survivor)} | Adres: 0x{GetAddress(survivor).ToString("X")} (DEĞİŞTİ)");
        GC.Collect();
        Console.WriteLine($"[2. GC] Gen {GC.GetGeneration(survivor)} | Adres: 0x{GetAddress(survivor).ToString("X")} (Yaşlı)");

        Console.WriteLine("\n--- Write Barrier / Dirty Card Testi ---");
        SimpleNode baby = new SimpleNode { Id = 999 };
        survivor.Child = baby;
        PrintAddr("-> Container", GetAddress(survivor), $"Gen {GC.GetGeneration(survivor)} (Kirli Kart!)");
        PrintAddr("-> Content", GetAddress(baby), $"Gen {GC.GetGeneration(baby)} (Genç Nesne)");

        Console.WriteLine("\n==============================================");
        Console.WriteLine("=== BÖLÜM 3: MANAGED vs UNMANAGED ===");
        Console.WriteLine("==============================================\n");

        StringBuilder sb = new StringBuilder("Text");
        WeakReference weakSb = new WeakReference(sb);
        sb = null; GC.Collect();
        if (!weakSb.IsAlive) Console.WriteLine("StringBuilder: Anında öldü.");

        var dbLeak = new SimulatedDbConnection("UnutulanDB");
        WeakReference weakDb = new WeakReference(dbLeak);
        dbLeak = null; GC.Collect();
        if (weakDb.IsAlive) Console.WriteLine("DbConnection: ÖLMEDİ! (Finalizer bekliyor)");
        GC.WaitForPendingFinalizers(); GC.Collect();
        if (!weakDb.IsAlive) Console.WriteLine("DbConnection: Şimdi öldü.");

        using (var dbClean = new SimulatedDbConnection("TemizDB")) { }

        Console.WriteLine("\n==============================================");
        Console.WriteLine("=== BÖLÜM 4: LOADER CACHE (PERFORMANS) ===");
        Console.WriteLine("==============================================\n");

        Stopwatch sw = new Stopwatch();

        // 1. ERİŞİM (Cold Start)
        sw.Start();
        int v1 = CachedService.GlobalData;
        sw.Stop();
        long t1 = sw.ElapsedTicks;

        // HATA ÖNLEYİCİ: Eğer işlem çok hızlıysa t1 0 gelebilir, en az 1 yapalım.
        if (t1 == 0) t1 = 1;

        Console.WriteLine($"1. ÇAĞRIM (Cold)   : {t1} Ticks (Yükleme + Static Ctor)");

        // 2. ERİŞİM (Warm/Cached)
        sw.Restart();
        int v2 = CachedService.GlobalData;
        sw.Stop();
        long t2 = sw.ElapsedTicks;

        // !!! DÜZELTME BURADA !!!
        // Bilgisayar çok hızlıysa t2 = 0 gelir. Bölme hatasını engellemek için:
        if (t2 == 0)
        {
            t2 = 1; // 0 tick sürdüyse en az 1 kabul et ki bölünebilsin.
            Console.WriteLine($"2. ÇAĞRIM (Cached) : ~0 Ticks (Ölçülemeyecek kadar hızlı!)");
        }
        else
        {
            Console.WriteLine($"2. ÇAĞRIM (Cached) : {t2} Ticks (Direkt Erişim)");
        }

        Console.WriteLine($"   -> Sonuç: Cache sayesinde {t1 / t2} kat daha hızlı!");


        Console.WriteLine("\n==============================================");
        Console.WriteLine("=== BÖLÜM 5: YAPILMAMASI GEREKENLER (HATA) ===");
        Console.WriteLine("==============================================\n");

        Console.WriteLine("--- Event Leak ---");
        Publisher pub = new Publisher();
        Subscriber sub = new Subscriber();
        pub.Olay += sub.Dinle;
        WeakReference weakSub = new WeakReference(sub);
        sub = null; GC.Collect();
        if (weakSub.IsAlive) Console.WriteLine("SONUÇ: Subscriber ÖLMEDİ! (Leak)");

        Console.WriteLine("\n--- String (+) ---");
        string str = "A";
        Console.WriteLine($"'{str}' Adres: 0x{GetAddress(str).ToString("X")}");
        str += "B";
        Console.WriteLine($"'{str}' Adres: 0x{GetAddress(str).ToString("X")} (Değişti)");

        Console.WriteLine("\n--- Boş Finalizer ---");
        EmptyFinalizerClass empty = new EmptyFinalizerClass();
        WeakReference weakEmpty = new WeakReference(empty);
        empty = null; GC.Collect();
        if (weakEmpty.IsAlive) Console.WriteLine("SONUÇ: İlk turda ÖLMEDİ!");
        GC.WaitForPendingFinalizers(); GC.Collect();
        if (!weakEmpty.IsAlive) Console.WriteLine("SONUÇ: İkinci turda öldü.");

        Console.WriteLine("\n==============================================");
        Console.WriteLine("=== BÖLÜM 6: MEMORY LEAK ===");
        Console.WriteLine("==============================================\n");
        _staticLeak.Add(lohObj);
        Console.WriteLine($"Leak: {lohObj.Length} byte static listeye kilitlendi.");
    }

    public void Dispose()
    {
        Console.WriteLine("\n--- DISPOSE ---");
        if (_nativeBuffer != IntPtr.Zero) { Marshal.FreeHGlobal(_nativeBuffer); _nativeBuffer = IntPtr.Zero; }
    }

    private unsafe IntPtr GetAddress(object obj)
    {
        if (obj == null) return IntPtr.Zero;
        TypedReference tr = __makeref(obj);
        return **(IntPtr**)(&tr);
    }
    private void PrintAddr(string bolge, IntPtr adres, string aciklama)
    {
        Console.WriteLine($"[{bolge.PadRight(15)}] : 0x{adres.ToString("X").PadRight(12)} | {aciklama}");
    }
}

class Program
{
    static void Main()
    {
        using (var master = new GrandMasterMemory())
        {
            master.Analyze();
        }
        Console.ReadLine();
    }
}