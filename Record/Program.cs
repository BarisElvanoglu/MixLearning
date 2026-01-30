// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;



// record: değer-eşitliği, otomatik ToString, 'with', deconstruct
// class: referans-eşitliği (varsayılan), mutable olabilir

// positional record -> deconstruct destekli, value-based equality
public record Person(string FirstName, string LastName);

// normal class -> reference-based equality by default
public class PersonClass
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public PersonClass(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    // ToString override sadece görsellik için (opsiyonel)
    public override string ToString() => $"{FirstName} {LastName}";
}

// Kullanım örnekleri
var r1 = new Person("John", "Doe");
var r2 = new Person("John", "Doe");
Console.WriteLine(r1 == r2);                 // True (değer-eşitliği)
Console.WriteLine(ReferenceEquals(r1, r2));  // False (farklı referanslar)
var r3 = r1 with { FirstName = "Jane" };     // Yeni instance (immutable pattern)
Console.WriteLine(r1);                        // Person { FirstName = John, LastName = Doe }
Console.WriteLine(r3);                        // Person { FirstName = Jane, LastName = Doe }
var (fn, ln) = r3;                            // Deconstruction
Console.WriteLine(fn);                        // Jane

var c1 = new PersonClass("John", "Doe");
var c2 = new PersonClass("John", "Doe");
Console.WriteLine(c1 == c2);                 // False (referans-eşitliği)
Console.WriteLine(c1.Equals(c2));            // False (varsayılan Equals referans bazlı)
Console.WriteLine(ReferenceEquals(c1, c2));  // False
Console.WriteLine(c1);                       // John Doe (ToString override gösteriyor)



