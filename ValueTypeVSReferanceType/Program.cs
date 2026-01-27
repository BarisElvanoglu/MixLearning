// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");



// VALUE TYPE
int a = 10;
int b = a;   // kopya alındı
b = 20;

Console.WriteLine(a); // 10
Console.WriteLine(b); // 20

// REFERENCE TYPE add 25S

Box box1 = new Box { Value = 10 };
Box box2 = box1;
box2.Value = 20;

Console.WriteLine(box1.Value);
Console.WriteLine(box2.Value);

class Box
{
    public int Value { get; set; }
}



//Value Type(int, struct, bool) → değerin kendisi saklanır.
//Genelde Stack’te tutulur, kopyalandığında tam kopya oluşur.

//Reference Type (class, array, string) → adres (referans) saklanır.
//Nesnenin kendisi Heap’tedir, Stack’te sadece adres vardır. Kopyalayınca aynı nesneyi gösterir.

//Sonuç:
//Value type = hızlı, bağımsız kopya
//Reference type = paylaşılan nesne, GC ile yönetilir
