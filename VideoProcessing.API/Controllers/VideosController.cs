using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoProcessing.API.Services; // 1. Yazdığımız servisi görebilmesi için burayı ekledik

namespace VideoProcessing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly QueueService _queueService;

        // 2. Constructor aracılığıyla QueueService'i buraya enjekte ediyoruz (Dependency Injection)
        public VideosController(QueueService queueService)
        {
            _queueService = queueService;
        }
        [DisableRequestSizeLimit]
        // POST api/videos/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            // 1. Güvenlik Kontrolü: Dosya boş mu gelmiş?
            if (file == null || file.Length == 0)
            {
                return BadRequest("Lütfen geçerli bir video dosyası yükleyin.");
            }

            try
            {
                // 2. Videoları kaydedeceğimiz klasörü belirliyoruz
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedVideos");

                // Eğer klasör bilgisayarda yoksa otomatik oluşturuyoruz
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 3. Çakışma olmasın diye dosya adının başına benzersiz bir ID (Guid) ekliyoruz
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 4. Videoyu sunucuya fiziksel olarak kaydediyoruz
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 🔥 5. SİHİRLİ DOKUNUŞ: Videonun fiziksel yolunu RabbitMQ kuyruğuna fırlatıyoruz!D
                await _queueService.SendToQueueAsync(filePath);

                // 6. İşlem başarılı! Kullanıcıya mesaj ve dosya yolunu dönüyoruz
                return Ok(new
                {
                    Message = "Video sunucuya başarıyla yüklendi ve işleme kuyruğuna alındı!",
                    SavedPath = filePath
                });
            }
            catch (Exception ex)
            {
                // Bir hata oluşursa sunucu çökmesin, hatayı dönsün
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}