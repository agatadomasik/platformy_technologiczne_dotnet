using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8000);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Client connected...");

                _ = Task.Run(() => HandleClient(client));
            }
        }

        private static async Task HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    while (client.Connected)
                    {
                        rndGen obj = await DeserializeAsync<rndGen>(stream);
                        if (obj == null) break;

                        Console.WriteLine($"Received: {obj}");

                        obj.rnd += new Random().Next(0, 100);
                        Console.WriteLine($"Processed: {obj}");

                        await SerializeAsync(stream, obj);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Client disconnected: {e.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private static async Task<T> DeserializeAsync<T>(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

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
            return $"RndGen(counter: {rnd})";
        }
    }
}
