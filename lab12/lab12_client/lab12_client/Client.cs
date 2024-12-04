using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 8000))
            using (NetworkStream stream = client.GetStream())
            {
                _ = Task.Run(async () =>
                {
                    while (client.Connected)
                    {
                        rndGen obj = await DeserializeAsync<rndGen>(stream);
                        if (obj == null) break;

                        Console.WriteLine($"Received processed data: {obj}");
                    }
                });

                while (client.Connected)
                {
                    rndGen obj = new rndGen { rnd = new Random().Next(0, 100) };
                    Console.WriteLine($"Sending: {obj}");
                    await SerializeAsync(stream, obj);
                    await Task.Delay(5000);
                }
            }
        }

        private static async Task<T> DeserializeAsync<T>(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0) return default;

            string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        private static async Task SerializeAsync<T>(NetworkStream stream, T obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }

    public class rndGen
    {
        public int rnd { get; set; }

        public override string ToString()
        {
            return $"RndGen(rnd: {rnd})";
        }
    }
}
