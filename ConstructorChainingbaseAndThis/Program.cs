using System;

namespace ConstructorChainingExample
{
    // Base sınıf: Person
    class Person
    {
        protected string Name;

        // Parametresiz yapıcı: diğer yapıcıyı çağırır (this ile)
        public Person() : this("Unknown")
        {
            // Bu gövde, this(...) ile çağrılan yapıcının çalışmasından sonra çalışır.
            Console.WriteLine("Person() body çalıştı");
        }

        // Asıl yapıcı: isim atanır
        public Person(string name)
        {
            Name = name;
            Console.WriteLine($"Person(string) çalıştı, Name = {Name}");
        }
    }

    // Türetilmiş sınıf: Employee
    class Employee : Person
    {
        public int Id;

        // Parametresiz: aynı sınıf içindeki başka yapıcıyı çağırıyor (: this(...))
        public Employee() : this(0, "NoName")
        {
            Console.WriteLine("Employee() body çalıştı");
        }

        // Tek parametre: id verildiğinde diğer yapıcıya yönlendir (this)
        public Employee(int id) : this(id, "Default")
        {
            Console.WriteLine("Employee(int) body çalıştı");
        }

        // Asıl yapıcı: hem base sınıfın uygun yapıcısını çağırır (: base(name))
        public Employee(int id, string name) : base(name)
        {
            Id = id;
            Console.WriteLine($"Employee(int, string) çalıştı, Id = {Id}, Name(from base) = {Name}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== new Employee() çağrısı ===");
            var e1 = new Employee();
            Console.WriteLine();

            Console.WriteLine("=== new Employee(10) çağrısı ===");
            var e2 = new Employee(10);
            Console.WriteLine();

            Console.WriteLine("=== new Employee(20, \"Elvan\") çağrısı ===");
            var e3 = new Employee(20, "Elvan");
            Console.WriteLine();

            // Çıktı mantığı gösterimi: hangi yapıcıların ve hangi sırayla çalıştığı
            // Not: base(...) ile üst sınıf yapıcısı doğrudan çağrılır; :this(...) aynı sınıf içindeki bir başka yapıcıyı çağırır.
        }
    }
}