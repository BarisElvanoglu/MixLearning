using System;

//
// Enum (enumeration) nedir?
// - Adlandırılmış sabit değerler kümesi tanımlar.
// - Okunabilirliği artırır, "magic number" kullanımını engeller.
// - Derleyici tip kontrolü sağlar; kod daha güvenli ve anlaşılır olur.
//
// Bu dosyada:
// 1) Basit enum örneği
// 2) Alt tip (underlying type) örneği
// 3) [Flags] bit bayrakları örneği (birden çok değerin birleşimi)
// 4) Parse, TryParse ve cast örnekleri
//

// 1) Basit enum: günler (varsayılan underlying type: int, başlangıç değeri 0)
enum DayOfWeekSimple
{
    Sunday,    // 0
    Monday,    // 1
    Tuesday,   // 2
    Wednesday, // 3
    Thursday,  // 4
    Friday,    // 5
    Saturday   // 6
}

// 2) Alt tip belirleme: byte kullanarak bellek tüketimini küçültebiliriz
enum StatusCode : byte
{
    Ok = 0,
    NotFound = 1,
    Unauthorized = 2,
    ServerError = 3
}

// 3) [Flags] örneği: bit alanı (bitwise) kullanımı — birden çok seçeneği tek bir değişkende saklayabiliriz
[Flags]
enum FilePermissions
{
    None = 0,
    Read = 1 << 0,    // 1
    Write = 1 << 1,   // 2
    Execute = 1 << 2, // 4
    // kombinasyonları da doğrudan tanımlayabiliriz
    ReadWrite = Read | Write
}

// Top-level program (çalıştırılabilir örnek)
Console.WriteLine("=== Enum örnekleri ===\n");

// Basit enum kullanımı
DayOfWeekSimple today = DayOfWeekSimple.Wednesday;
Console.WriteLine($"Today: {today} ({(int)today})"); // cast ile underlying int değeri alırız

// Alt tip (underlying) örneği
StatusCode code = StatusCode.NotFound;
byte raw = (byte)code; // explicit cast gerekli
Console.WriteLine($"StatusCode: {code}, raw byte: {raw}");

// [Flags] kullanım örneği
FilePermissions perms = FilePermissions.Read | FilePermissions.Write; // Read + Write
Console.WriteLine($"Permissions: {perms}");

// Kontrol etme yolları
bool canRead = perms.HasFlag(FilePermissions.Read); // okunabilir mi?
bool canExecute = (perms & FilePermissions.Execute) == FilePermissions.Execute; // bitwise kontrol
Console.WriteLine($"CanRead: {canRead}, CanExecute: {canExecute}");

// Enum parse örnekleri
string input = "Friday";
if (Enum.TryParse<DayOfWeekSimple>(input, ignoreCase: true, out var parsedDay))
{
    Console.WriteLine($"Parsed '{input}' -> {parsedDay} ({(int)parsedDay})");
}
else
{
    Console.WriteLine($"'{input}' parse edilemedi.");
}

// Numeric -> enum cast (dikkat: geçersiz değerler de cast edilebilir)
int possible = 10;
var unknown = (DayOfWeekSimple)possible;
Console.WriteLine($"Cast from int {possible} -> {unknown} (bu tanımlı bir değer olmayabilir)");

// Özet kısa notlar:
// - Enum'lar sabit, adlandırılmış değerler sunar; hataları azaltır ve kodu daha anlaşılır kılar.
// - Varsayılan underlying type int'tir; isterseniz byte, short vs. tanımlayabilirsiniz.
// - [Flags] kombinasyonel (bit) senaryoları için uygundur. HasFlag veya bitwise operatörlerle kontrol edebilirsiniz.