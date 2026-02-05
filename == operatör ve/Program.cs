//Bu soru, C# mülakatlarının vazgeçilmezidir. İkisi de "eşit mi?" diye sorar ancak birisi kimliğe (adres), 
//diğeri içeriğe (değer) odaklanma eğilimindedir.İşte aralarındaki temel farklar:1.
//Temel Fark: Referans vs. Değer== Operatörü: Genellikle referans karşılaştırması yapar. 
//Yani "Bu iki değişken bellekte aynı adresi mi gösteriyor?" diye bakar..Equals() Metodu: 
//Genellikle içerik karşılaştırması yapar. Yani "Bu iki nesnenin içindeki veriler aynı mı?" diye bakar.2. 
//String İstisnası (Kafa Karıştıran Nokta)C#'ta string bir referans tipi olmasına rağmen, 
//== operatörü string için özel olarak aşırı yüklenmiştir (overload). Bu yüzden her ikisi de stringlerde içeriğe bakar.
//int ,struct,double,float ==değerleri kıyaslar equals değeri kıyaslar
//class object ==referans equals equals refrans kıyaslar
//string == içerik equals içerik
//record == içerik equals içerik


using System;
using System.Text;

//string s1 = "merhaba";
//string s2 = "merhaba";


//Console.WriteLine(s1 == s2);      // True (İçerik aynı)
//Console.WriteLine(s1.Equals(s2)); // True (İçerik aynı)
////Ancak nesnelerde(object) durum tamamen değişir:C#object 
//object obj2 = new string(new char[] { 'a' });

//object obj1 = new string(new char[] { 'a' });
//Console.WriteLine(obj1 == obj2);      // False! (Adresler farklı)
//Console.WriteLine(obj1.Equals(obj2)); // True! (İçerik aynı)
                                      
////3.Özet Karşılaştırma TablosuÖzellik== Operatörü.Equals() MetoduTürBir operatördür.Bir sanal (virtual) metottur.
////Null DurumuSol taraf null ise hata vermez.Sol taraf null ise NullReferenceException fırlatır.PolimorfizmDerleme zamanında
////(static) bağlanır.Çalışma zamanında(dynamic/virtual) bağlanır.ÖzelleştirmeOverload edilebilir.Override(ezilebilir) edilebilir.
////4. Kritik Fark: Null GüvenliğiEğer değişkeninizin null olma ihtimali varsa == kullanmak daha güvenlidir.C#string s1 = null;

//Console.WriteLine(s1.Equals("test"));    // ÇÖKER! (NullReferenceException)
//Console.WriteLine(s1 == "test");         // False döner, güvenlidir.
////İpucu: Eğer modern C# kullanıyorsanız ve içeriğin kesinlikle aynı olup olmadığını, null durumlarını da
////yöneterek kontrol etmek istiyorsanız object.Equals(a, b) veya EqualityComparer<T>.Default kullanmak en
////profesyonel yaklaşımdır.Bu iki yöntem arasındaki farkı daha iyi anlamak için "Value Type" (int, bool gibi)
////değişkenlerde nasıl davrandıklarını incelememi ister misiniz?


namespace EqualsTestDemo
{
    



    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    record Person1(int Id, string name);

    class Program
    {
        static void Main()
        {
            Person person = new Person();
            person.Id = 1;
            person.Name = "Ahment";
            Person person1 = new Person();
            person1 = person;
            Person person2 = new Person();
            person2.Id = 1;
            person2.Name = "Ahmet";
            Console.WriteLine(person == person2);// False
            Console.WriteLine(person1.Equals(person2));// False
            Console.WriteLine(person2.Equals(person));// False
            Console.WriteLine(person == person1);// True
            Console.WriteLine(person == person2);// False
            Console.WriteLine(person1.Equals(person2));// False
            Console.WriteLine(person2.Equals(person));// False
            Person1 recordPerson1 = new Person1(Id: 1, name: "bARRIS");
            Person1 recordPerson2 = new Person1(Id: 1, name: "bARRIS");
            Console.WriteLine(recordPerson1 == recordPerson2);// True
            Console.WriteLine(recordPerson1.Equals(recordPerson2));// True
            person1.Name = "mustafa";
            Console.WriteLine(person.Name);// ahment  mustafa
            Console.WriteLine(person1.Name);// mustafa mustafa
            Console.WriteLine(person2.Name);//ahmet ahmet
            string s1 = "merhaba";
            string s2 = s1;
            s2 = "merhaba2";
            Console.WriteLine(s1 == s2); // True false
            Console.WriteLine(s1); //merhaba merhaba
            Console.WriteLine(s2);// merhaba2 merhaba2
            object obj1 = new string("Osman");
            object obj2 = new string("Osman");
            Console.WriteLine(obj1 == obj2); // False
            Console.WriteLine(obj1.Equals(obj2)); // true

            string stringintern = "bariş";
            string stringintern2 = "bariş";
            string stringheap = new string("bariş");
            StringBuilder sb = new StringBuilder();
            sb.Append("bariş");
            string stringStringBuilder= sb.ToString();
            Console.WriteLine(ReferenceEquals(stringintern, stringheap));// False
            Console.WriteLine(ReferenceEquals(stringintern, stringStringBuilder));// False
            Console.WriteLine(ReferenceEquals(stringintern, stringintern2));// True
            String.Intern(stringStringBuilder); 
            Console.WriteLine(ReferenceEquals(stringintern, stringStringBuilder));//False
            Console.ReadLine();

        }
    }
}




