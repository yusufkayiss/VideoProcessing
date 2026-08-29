# 🎬 VideoProcessing - Asynchronous Video Processing Architecture (.NET 8 & RabbitMQ)

Asynchronous backend architecture built with .NET 8, RabbitMQ, Docker, and Worker Services for non-blocking video processing.

---

## 🌐 English

### 🚀 Overview
An event-driven backend system developed to offload heavy, CPU-intensive tasks (such as video transcoding and compression) from the primary API, ensuring maximum application responsiveness and zero thread blocking.

### 🛠 Tech Stack & Architecture
- **.NET 8 (Web API)**: Serves as the **Producer** layer that receives video upload requests and dispatches messages to the broker.
- **.NET 8 (Worker Service)**: Operates as the **Consumer** layer, running in the background to listen to queues and process video simulation tasks.
- **RabbitMQ**: The message broker driving asynchronous communication and queue management between the API and Worker.
- **Docker**: Used to run RabbitMQ inside an isolated container for simplified deployment and environment setup.

### ⚙️ How It Works
1. The client uploads a video payload via the Web API (Swagger).
2. The API saves the raw file to disk and publishes a message containing the file path to the **`video-processing-queue`**.
3. The API immediately returns a `200 OK (Accepted)` response to the user, eliminating request timeout or blocking.
4. The background **Worker Service** automatically picks up the message and performs asynchronous processing (`10%... 50%... 100%`).

### 📈 Key Benefits
- **High Performance**: Heavy encoding operations are offloaded, keeping the primary HTTP API fast and responsive.
- **Scalability**: Under high traffic, additional **Worker** instances can be spawned independently to distribute processing loads without modifying the Web API.
- **Durability**: If the worker service goes offline, incoming messages safely persist in the RabbitMQ queue until consumption resumes.

### 💻 Getting Started (How to Run)

**Prerequisites:** .NET 8 SDK and Docker installed.

1. **Start RabbitMQ via Docker:**
   ```bash
   docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
   Run the Worker Service: Open a terminal in the Worker project folder and run dotnet run.

Run the Web API: Open another terminal in the API project folder and run dotnet run.

Test: Navigate to http://localhost:<port>/swagger to upload a video and watch the Worker terminal process it in the background!

🔗 Related Core Engine
💡 Note: To inspect the underlying core algorithm and processing motor responsible for video compression and transcoding, visit the VideoCodec - Core Video Processing Engine repository.
📍 Türkçe
🚀 Genel Bakış
Web uygulamalarında video işleme gibi zaman alan ağır operasyonların ana uygulamayı kilitlemesini önlemek amacıyla Olay Güdümlü Mimari (Event-Driven Architecture) kullanılarak geliştirilmiş asenkron backend sistemi.

🛠 Teknolojiler & Mimari
.NET 8 (Web API): Video yükleme isteklerini karşılayan ve kuyruğa mesaj fırlatan Producer (Üretici) katmanı.

.NET 8 (Worker Service): Arka planda kuyruğu sürekli dinleyen ve video işleme simülasyonunu yürüten Consumer (Tüketici) katmanı.

RabbitMQ: API ile Worker arasındaki asenkron iletişimi ve mesaj yönetimini sağlayan mesaj kuyruğu sistemi.

Docker: RabbitMQ sunucusunu izole bir konteyner içinde ayağa kaldırmak ve ortamı standartlaştırmak için kullanıldı.

⚙️ Nasıl Çalışır?
Kullanıcı API (Swagger) üzerinden bir video yükleme isteği atar.

API videoyu diske kaydeder ve dosya yolunu video-processing-queue kuyruğuna mesaj olarak yayınlar.

API kullanıcıya anında 200 OK (Sıraya Alındı) yanıtı döner, kullanıcı tarayıcıda işleme sürecini beklemek zorunda kalmaz.

Arka planda çalışan Worker Service, kuyruktaki mesajı yakalayarak asenkron biçimde işleme adımını yürütür (%10... %50... %100).

📈 Projenin Sağladığı Avantajlar
Yüksek Performans: Ağır video işleme yükleri API katmanını kilitlenmekten korur, web servisi sürekli hızlı kalır.

Ölçeklenebilirlik (Scalability): Yük arttığında, API koduna dokunmadan arka plandaki Worker sayısı artırılarak yük dengelenebilir.

Veri Güvenliği (Durability): Worker servisi kapalı olsa bile gelen istekler RabbitMQ kuyruğunda güvenle bekler, servis açıldığında eritmeye devam eder.

💻 Nasıl Çalıştırılır?
Ön Koşullar: Bilgisayarınızda .NET 8 SDK ve Docker yüklü olmalıdır.

RabbitMQ'yu Docker ile Başlatın:

Bash
docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
Worker Servisi Çalıştırın: Worker projesinin dizininde terminal açıp dotnet run komutunu girin.

Web API'yi Çalıştırın: API projesinin dizininde yeni bir terminal açıp dotnet run komutunu girin.

Test Edin: Tarayıcıda http://localhost:<port>/swagger adresine giderek video yükleme isteği atın ve arka planda Worker terminalinin isteği nasıl işlediğini izleyin!

🔗 Bağlantılı Çekirdek Motor
💡 Not: Bu mimarinin arka planda video sıkıştırma ve dönüştürme algoritmalarını yöneten asıl işleyici motorunu incelemek için VideoCodec - Core Video Processing Engine reposuna göz atabilirsiniz.
