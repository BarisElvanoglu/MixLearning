using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericsExample
{
    // Generic sınıf: T yer tutucu tiptir — sınıfı herhangi bir tip ile kullanabilirsin.
    public class Container<T>
    {
        private T item;

        // Değer atama (T tipinde)
        public void SetItem(T value) => item = value;

        // Değer okuma (T tipinde)
        public T GetItem() => item;
    }

    public static class Program
    {
        // Generic metod: T tipi IComparable<T> uygulamalı — karşılaştırma yapabiliriz.
        public static T FindMax<T>(T[] items) where T : IComparable<T>
        {
            if (items == null || items.Length == 0) return default!;
            T max = items[0];
            foreach (var item in items)
            {
                if (item.CompareTo(max) > 0) max = item;
            }
            return max;
        }

        static void Main()
        {
            // 1) Generic class örneği
            var stringBox = new Container<string>();
            stringBox.SetItem("Merhaba");
            Console.WriteLine(stringBox.GetItem()); // Merhaba

            // Değer tipleriyle (int) kullanma — boxing yok
            var intBox = new Container<int>();
            intBox.SetItem(42);
            Console.WriteLine(intBox.GetItem()); // 42

            // 2) Generic koleksiyon: List<T> — tip güvenli, performanslı
            List<int> list = new List<int> { 1, 2, 3 };
            list.Add(4);
            int a = list[0];  
            Console.WriteLine($"List contains {list.Count} items");

            // 3) Non-generic koleksiyon: ArrayList — boxing/unboxing örneği
            ArrayList aList = new ArrayList();
            aList.Add(1); // int -> object (boxing)
            aList.Add(2); // boxing
            int first = (int)aList[0]; // object -> int (unboxing, explicit cast required)
            Console.WriteLine($"ArrayList first (after unboxing): {first}");

            // 4) Generic metod kullanımı (kısıtlama ile)
            int[] numbers = { 5, 2, 8, 1, 9 };
            Console.WriteLine($"Max: {FindMax(numbers)}"); // 9
        }
    }
}