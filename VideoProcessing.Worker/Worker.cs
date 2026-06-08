using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace VideoProcessing.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "video-processing-queue";
        private IConnection? _connection;
        private IChannel? _channel;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        // Worker Service ilk çalışmaya başladığında tetiklenen ana metot
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            // 1. Asenkron olarak RabbitMQ bağlantısını kuruyoruz
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            // 2. Dinleyeceğimiz kuyruğu tanımlıyoruz (API'deki isimle birebir aynı)
            await _channel.QueueDeclareAsync(queue: _queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null,
                                            cancellationToken: stoppingToken);

            _logger.LogInformation(" [*] Tavşan uykudan uyandı, video mesajlarını bekliyor...");

            // 3. Mesajları dinleyecek (Consumer) mekanizmayı hazırlıyoruz
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var videoPath = Encoding.UTF8.GetString(body);

                _logger.LogInformation($" [⬇️] Kuyruktan yeni bir video yolu yakalandı: {videoPath}");

                try
                {
                    // 🎬 VIDEO İŞLEME SİMÜLASYONU START!
                    _logger.LogInformation(" [🎬] Video işleme (sıkıştırma) işlemi arka planda başlatıldı...");

                    for (int i = 10; i <= 100; i += 10)
                    {
                        await Task.Delay(1000, stoppingToken); // Her saniye %10 ilerlesin
                        _logger.LogInformation($"       [🔄] Video İşleniyor: %{i}");
                    }

                    _logger.LogInformation(" [✅] Tebrikler! Video pürüzsüzce işlendi ve hazır hale getirildi.");

                    // 4. İşlem başarıyla bittiği için RabbitMQ'ya "Ben bu mesajı başarıyla erittim, kuyruktan silebilirsin" diyoruz.
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($" [❌] Video işlenirken bir sorun çıktı: {ex.Message}");
                }
            };

            // 5. Dinleme işlemini başlatıyoruz
            await _channel.BasicConsumeAsync(queue: _queueName,
                                             autoAck: false, // Manuel onaylama açık (BasicAckAsync kullanabilmek için)
                                             consumer: consumer,
                                             cancellationToken: stoppingToken);

            // Uygulama kapatılana kadar arka planı canlı tutuyoruz
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        // Uygulama tamamen durdurulduğunda bağlantıları düzgünce kapatıyoruz
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null) await _channel.CloseAsync(cancellationToken);
            if (_connection != null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}