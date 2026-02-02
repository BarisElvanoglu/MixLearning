//Bu soru, C# mülakatlarının vazgeçilmezidir. İkisi de "eşit mi?" diye sorar ancak birisi kimliğe (adres), 
//diğeri içeriğe (değer) odaklanma eğilimindedir.İşte aralarındaki temel farklar:1.
//Temel Fark: Referans vs. Değer== Operatörü: Genellikle referans karşılaştırması yapar. 
//Yani "Bu iki değişken bellekte aynı adresi mi gösteriyor?" diye bakar..Equals() Metodu: 
//Genellikle içerik karşılaştırması yapar. Yani "Bu iki nesnenin içindeki veriler aynı mı?" diye bakar.2. 
//String İstisnası (Kafa Karıştıran Nokta)C#'ta string bir referans tipi olmasına rağmen, 
//== operatörü string için özel olarak aşırı yüklenmiştir (overload). Bu yüzden her ikisi de stringlerde içeriğe bakar.


string s1 = "merhaba";
string s2 = "merhaba";

Console.WriteLine(s1 == s2);      // True (İçerik aynı)
Console.WriteLine(s1.Equals(s2)); // True (İçerik aynı)
//Ancak nesnelerde(object) durum tamamen değişir:C#object 
object obj2 = new string(new char[] { 'a' });

object obj1 = new string(new char[] { 'a' });
Console.WriteLine(obj1 == obj2);      // False! (Adresler farklı)
Console.WriteLine(obj1.Equals(obj2)); // True! (İçerik aynı)
//3.Özet Karşılaştırma TablosuÖzellik== Operatörü.Equals() MetoduTürBir operatördür.Bir sanal (virtual) metottur.Null DurumuSol taraf null ise hata vermez.Sol taraf null ise NullReferenceException fırlatır.PolimorfizmDerleme zamanında (static) bağlanır.Çalışma zamanında(dynamic/virtual) bağlanır.ÖzelleştirmeOverload edilebilir.Override(ezilebilir) edilebilir.4. Kritik Fark: Null GüvenliğiEğer değişkeninizin null olma ihtimali varsa == kullanmak daha güvenlidir.C#string s1 = null;

// Console.WriteLine(s1.Equals("test")); // ÇÖKER! (NullReferenceException)
Console.WriteLine(s1 == "test");         // False döner, güvenlidir.
//İpucu: Eğer modern C# kullanıyorsanız ve içeriğin kesinlikle aynı olup olmadığını, null durumlarını da
//yöneterek kontrol etmek istiyorsanız object.Equals(a, b) veya EqualityComparer<T>.Default kullanmak en
//profesyonel yaklaşımdır.Bu iki yöntem arasındaki farkı daha iyi anlamak için "Value Type" (int, bool gibi)
//değişkenlerde nasıl davrandıklarını incelememi ister misiniz?