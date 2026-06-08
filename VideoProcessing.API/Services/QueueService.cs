using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace VideoProcessing.API.Services
{
    public class QueueService
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "video-processing-queue";

        public async Task SendToQueueAsync(string videoPath)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            // 1. Bağlantıyı asenkron oluşturuyoruz
            using var connection = await factory.CreateConnectionAsync();

            // 2. DOĞRU METOT: CreateModelAsync yerine CreateChannelAsync kullanıyoruz!
            using var channel = await connection.CreateChannelAsync();

            // 3. Kuyruk tanımlama
            await channel.QueueDeclareAsync(queue: _queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            var body = Encoding.UTF8.GetBytes(videoPath);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            // 4. Mesajı fırlatma
            await channel.BasicPublishAsync(exchange: "",
                                            routingKey: _queueName,
                                            mandatory: false,
                                            basicProperties: properties,
                                            body: body);
        }
    }
}