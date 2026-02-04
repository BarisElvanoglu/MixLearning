using System;
using System.Collections.Generic;

namespace PolymorphismDemo
{
    // Abstract base class: ortak kontrat

    // Method override ve Method overload
    abstract class Animal
    {
        public string Name { get; }
        protected Animal(string name) => Name = name;

        // Abstract method -> türetilen sınıflar override etmek zorunda (runtime polymorphism)
        public abstract void Speak();

        // Virtual method -> isteğe bağlı override edilebilir
        public virtual string Info() => $"Tür: {GetType().Name}, İsim: {Name}";
    }

    // Interface örneği -> farklı tiplerde aynı yeteneği tanımlar
    interface IFlyable
    {
        void Fly();
    }

    class Dog : Animal
    {
        public Dog(string name) : base(name) { }
        public override void Speak() => Console.WriteLine($"{Name}: Hav hav!");
    }

    class Cat : Animal
    {
        public Cat(string name) : base(name) { }
        public override void Speak() => Console.WriteLine($"{Name}: Miyav!");
    }

    class Bird : Animal, IFlyable
    {
        public Bird(string name) : base(name) { }
        public override void Speak() => Console.WriteLine($"{Name}: Cik cik!");
        public void Fly() => Console.WriteLine($"{Name} uçuyor.");
        public override string Info() => base.Info() + " (kanatlı)";
    }

    // ========== METHOD OVERLOADING ÖRNEĞİ ==========
    // Aynı isim, farklı parametre imzaları -> compile-time polymorphism
    class Calculator
    {
        // İki tam sayı topla
        public int Add(int a, int b)
        {
            Console.WriteLine("Add(int, int) çağrıldı");
            return a + b;
        }

        // Üç tam sayı topla (overload)
        public int Add(int a, int b, int c)
        {
            Console.WriteLine("Add(int, int, int) çağrıldı");
            return a + b + c;
        }

        // Double tipleri topla (farklı parametre tipi => overload)
        public double Add(double a, double b)
        {
            Console.WriteLine("Add(double, double) çağrıldı");
            return a + b;
        }

        // params ile dizi olarak toplama (overload)
        public int Add(params int[] numbers)
        {
            Console.WriteLine("Add(params int[]) çağrıldı");
            int sum = 0;
            foreach (var n in numbers) sum += n;
            return sum;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Runtime Polymorphism (Override) ===\n");

            // Base türü Animal olan bir koleksiyona farklı derived nesneler koyuyoruz
            List<Animal> animals = new()
            {
                new Dog("Karabas"),
                new Cat("Minnoş"),
                new Bird("Güvercin")
            };

            foreach (var a in animals)
            {
                // Aynı metot çağrısı (Speak) her nesnede farklı davranır => runtime polymorphism
                a.Speak();

                // Virtual metodu override eden sınıfın versiyonu çağrılır
                Console.WriteLine(a.Info());

                // Interface kontrolü ile ek yetenekleri çağırabiliriz
                if (a is IFlyable f)
                    f.Fly();

                Console.WriteLine();
            }

            Console.WriteLine("=== Compile-time Polymorphism (Overload) ===\n");

            var calc = new Calculator();

            // Hangi Add overload'ının çağrılacağı derleme zamanında belirlenir
            int r1 = calc.Add(2, 3);                 // Add(int, int)
            Console.WriteLine($"2 + 3 = {r1}\n");

            int r2 = calc.Add(1, 2, 3);              // Add(int, int, int)
            Console.WriteLine($"1 + 2 + 3 = {r2}\n");

            double d = calc.Add(1.5, 2.25);          // Add(double, double)
            Console.WriteLine($"1.5 + 2.25 = {d}\n");

            int r3 = calc.Add(4, 5, 6, 7);           // Add(params int[]) -> params overload seçilir
            Console.WriteLine($"4 + 5 + 6 + 7 = {r3}\n");

            // Not: Overload seçiminde dönüş tipi (return type) göz önüne alınmaz; parametre imzası belirleyicidir.
            Console.WriteLine("Özet:");
            Console.WriteLine("- Override: runtime'da hangi implementasyon çalışacağı belirlenir (virtual/override/abstract).");
            Console.WriteLine("- Overload: compile-time'da hangi metot imzasının çağrılacağı belirlenir (aynı isim, farklı parametreler).");
        }
    }
}

