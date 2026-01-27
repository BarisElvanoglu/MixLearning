using System;

namespace DelegateExample
{
    // 1️⃣ Delegate tanımı
    public delegate void OrderCompletedHandler(string orderNo);

    // 2️⃣ Delegate’i kullanan servis
    public class OrderService
    {
        public OrderCompletedHandler OnOrderCompleted;

        public void CompleteOrder(string orderNo)
        {
            Console.WriteLine($"Sipariş tamamlandı: {orderNo}");

            // Delegate tetikleniyor
            OnOrderCompleted?.Invoke(orderNo);
        }
    }

    class Program
    {
        // 3️⃣ Farklı davranışlar (metotlar)
        static void SendMail(string orderNo)
        {
            Console.WriteLine($"Mail gönderildi: {orderNo}");
        }

        static void SendSms(string orderNo)
        {
            Console.WriteLine($"SMS gönderildi: {orderNo}");
        }

        static void WriteLog(string orderNo)
        {
            Console.WriteLine($"Log yazıldı: {orderNo}");
        }

        static void Main(string[] args)
        {
            // 4️⃣ Delegate bağlama (asıl olay burada)
            var orderService = new OrderService();

            orderService.OnOrderCompleted += SendMail;
            orderService.OnOrderCompleted += SendSms;
            orderService.OnOrderCompleted += WriteLog;

            orderService.CompleteOrder("ORD-123");

            Console.ReadLine();
        }
    }
}
