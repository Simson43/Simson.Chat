using Simson.Chat.ConsoleClient;
using System;
using System.Net;

namespace ConsoleClient
{
    internal static class ConsoleHelper
    {
        internal static Uri GetAddress()
        {
            var protocol = "https";
            Console.WriteLine($"Used protocol: {protocol}");
            var ip = GetIp();
            var port = GetPort();
            var result = $"{protocol}://{ip}:{port}";
            Console.WriteLine($"Used address: {result}");
            return new Uri(result);
        }

        internal static string GetIp()
        {
            string result;
            do
            {
                Console.Write("IP: ");
                result = Console.ReadLine().ToLower();
            } while (result != "localhost" && !IPAddress.TryParse(result, out _));
            return result;
        }

        internal static string GetPort()
        {
            string result;
            do
            {
                Console.Write("Port: ");
                result = Console.ReadLine();
            } while (!ushort.TryParse(result, out _));
            return result;
        }

        internal static void WriteSeparator()
        {
            Console.WriteLine("--------------------------------");
        }
    }
}
