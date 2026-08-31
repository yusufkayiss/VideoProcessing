# 🎬 VideoProcessing - Asynchronous Video Processing Architecture (.NET 8 & RabbitMQ)

[![.NET 8 CI](https://github.com/yusufkayiss/VideoProcessing/actions/workflows/dotnet.yml/badge.svg)](https://github.com/yusufkayiss/VideoProcessing/actions/workflows/dotnet.yml)

Asynchronous backend architecture built with .NET 8, RabbitMQ, Docker, and Worker Services for non-blocking video processing.

---

## 🌐 English

### 🚀 Overview
An event-driven backend system developed to offload heavy, CPU-intensive tasks (such as video transcoding and compression) from the primary API, ensuring maximum application responsiveness and zero thread blocking.

### 🛠 Tech Stack & Architecture
- **.NET 8 (Web API)**: Serves as the **Producer** layer that receives video upload requests and dispatches messages to the broker.
- **.NET 8 (Worker Service)**: Operates as the **Consumer** layer, running in the background to listen to queues and process video simulation tasks.
- **RabbitMQ**: The message broker driving asynchronous communication and queue management between the API and Worker.
- **Docker & Docker Compose**: Multi-container containerization for automated local setup and orchestration.

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

**Prerequisites:** Docker Desktop installed.

**One-Command Setup (Recommended):**
Run the following command in the root directory to build and start API, Worker, and RabbitMQ simultaneously:
```bash
docker-compose up --build
```
- **Web API Swagger**: `http://localhost:8080/swagger`
- **RabbitMQ Management Dashboard**: `http://localhost:15672` (Guest / Guest)

---

## 📍 Türkçe

### 🚀 Genel Bakış
Web uygulamalarında video işleme gibi zaman alan ağır operasyonların ana uygulamayı kilitlemesini önlemek amacıyla **Olay Güdümlü Mimari (Event-Driven Architecture)** kullanılarak geliştirilmiş asenkron backend sistemi.

### 🛠 Teknolojiler & Mimari
- **.NET 8 (Web API)**: Video yükleme isteklerini karşılayan ve kuyruğa mesaj fırlatan **Producer (Üretici)** katmanı.
- **.NET 8 (Worker Service)**: Arka planda kuyruğu sürekli dinleyen ve video işleme simülasyonunu yürüten **Consumer (Tüketici)** katmanı.
- **RabbitMQ**: API ile Worker arasındaki asenkron iletişimi ve mesaj yönetimini sağlayan mesaj kuyruğu sistemi.
- **Docker & Docker Compose**: API, Worker ve RabbitMQ servislerini tek komutla orkestre etmek için kullanılan konteyner mimarisi.

### ⚙️ Nasıl Çalışır?
1. Kullanıcı API (Swagger) üzerinden bir video yükleme isteği atar.
2. API videoyu diske kaydeder ve dosya yolunu **`video-processing-queue`** kuyruğuna mesaj olarak yayınlar.
3. API kullanıcıya anında `200 OK (Sıraya Alındı)` yanıtı döner, kullanıcı tarayıcıda işleme sürecini beklemek zorunda kalmaz.
4. Arka planda çalışan **Worker Service**, kuyruktaki mesajı yakalayarak asenkron biçimde işleme adımını yürütür (`%10... %50... %100`).

### 📈 Projenin Sağladığı Avantajlar
- **Yüksek Performans**: Ağır video işleme yükleri API katmanını kilitlenmekten korur, web servisi sürekli hızlı kalır.
- **Ölçeklenebilirlik (Scalability)**: Yük arttığında, API koduna dokunmadan arka plandaki Worker sayısı artırılarak yük dengelenebilir.
- **Veri Güvenliği (Durability)**: Worker servisi kapalı olsa bile gelen istekler RabbitMQ kuyruğunda güvenle bekler, servis açıldığında eritmeye devam eder.

### 💻 Nasıl Çalıştırılır?

**Ön Koşullar:** Bilgisayarınızda Docker Desktop yüklü olmalıdır.

**Tek Komutla Çalıştırma (Önerilen):**
Ana dizinde aşağıdaki komutu çalıştırarak API, Worker ve RabbitMQ servislerini aynı anda ayağa kaldırabilirsiniz:
```bash
docker-compose up --build
```
- **Web API Swagger Arayüzü**: `http://localhost:8080/swagger`
- **RabbitMQ Yönetim Paneli**: `http://localhost:15672` (Kullanıcı Adı / Şifre: guest)

### 🔗 Bağlantılı Çekirdek Motor
> 💡 **Not**: Bu mimarinin arka planda video sıkıştırma ve dönüştürme algoritmalarını yöneten asıl işleyici motorunu incelemek için [VideoCodec - Core Video Processing Engine](https://github.com/yusufkayiss/VideoCodec) reposuna göz atabilirsiniz.
