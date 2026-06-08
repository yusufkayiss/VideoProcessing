# 🎬 RabbitMQ ile Asenkron Video İşleme Mimarisi (.NET 8)

Bu proje, web uygulamalarında uzun süren ve sistemi yoran ağır işlemlerin (örneğin video sıkıştırma/işleme) ana uygulamayı kilitlemesini önlemek amacıyla **Event-Driven Architecture (Olay Güdümlü Mimari)** kullanılarak geliştirilmiş asenkron bir backend sistemidir.

## 🚀 Kullanılan Teknolojiler & Mimariler
* **.NET 8 (Web API):** Video yükleme isteklerini karşılayan ve kuyruğa mesaj fırlatan katman (Producer).
* **.NET 8 (Worker Service):** Arka planda kuyruğu sürekli dinleyen ve video işleme simülasyonunu yürüten bağımsız işçi (Consumer).
* **RabbitMQ:** API ile Worker arasındaki asenkron iletişimi ve mesaj yönetimini sağlayan mesaj kuyruğu sistemi.
* **Docker:** RabbitMQ sunucusunu bilgisayara kurmaya gerek kalmadan, izole bir konteyner içinde ayağa kaldırmak için kullanıldı.

## 🛠️ Sistem Nasıl Çalışır?
1. Kullanıcı API (Swagger) üzerinden bir video yükler.
2. API videoyu diske kaydeder ve videonun dosya yolunu **RabbitMQ (video-processing-queue)** kuyruğuna bir mesaj olarak gönderir.
3. API kullanıcıya hemen `200 OK (Sıraya Alındı)` cevabı döner, böylece kullanıcı video işlenene kadar tarayıcıda beklemek zorunda kalmaz.
4. Arka planda çalışan **Worker Service**, kuyruğa düşen mesajı otomatik olarak yakalar ve videoyu `%10... %50... %100` olacak şekilde asenkron olarak işler.

## 📈 Projenin Sağladığı Avantajlar
* **Yüksek Performans:** Ağır video işlemleri API'yi kilitlemez, web sitesi her zaman hızlı kalır.
* **Ölçeklenebilirlik (Scalability):** İleride video yükleyen kullanıcı sayısı artarsa, API'ye dokunmadan mutfaktaki aşçı (Worker) sayısı artırılarak yük dengelenebilir.
* **Veri Güvenliği (Durability):** Worker (aşçı) o an kapalı olsa bile gelen siparişler RabbitMQ kuyruğunda güvenle bekler, sistem açıldığı an kaldığı yerden eritmeye devam eder.
