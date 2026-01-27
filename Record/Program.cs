// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;



Console.WriteLine("Hello, World!");


{
    Person person1 = new Person() { FirstName = "John", LastName = "Doe", };

    Person person4 = new Person() { FirstName = "John", LastName = "Doe", };


    if (person1 == person4)
    {
        Console.WriteLine("Değiştirilemedi");
    }
    else
    {
        Console.WriteLine("Değiştirildi");
    }
}


{
    Person person1 = new Person() { FirstName = "John", LastName = "Doe", };

    Person person4 = new Person() { FirstName = "John", LastName = "Doe", };


    if (person1 == person4)
    {
        Console.WriteLine("Değiştirilemedi");
    }
    else
    {
        Console.WriteLine("Değiştirildi");
    }

    Console.WriteLine(person1);

    Person2 person2 = new Person2() { FirstName = "Jane", LastName = "Doe", };
    Person2 person3 = new Person2() { FirstName = "Jane", LastName = "Doe", };

    if (person2 == person3)
    {
        Console.WriteLine("Değiştirilemedi");
    }
    else
    {
        Console.WriteLine("Değiştirildi");

    }
    Console.ReadLine();
}

public record Person()
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}

public class Person2()
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}



