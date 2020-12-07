using System;
using System.Threading;

namespace ViciServerTest
{
    class Program
    {

        static bool serverDone, clientDone = false;
        static void ServerThread()
        {
            Console.WriteLine("Starting Server Thread...");
            Server server = new Server(true);
            server.Start();
            Console.WriteLine("Server IP " + server.GetIPAddress());

            string[] message = new string[0];
            do
            {
                Thread.Sleep(100);
                message = server.Receive();
            } while (message.Length == 0 || message[message.Length - 1] != "Done");

            server.Transmit("Test_S1");
            server.Transmit("Test_S2");
            server.Transmit("Test_S3");
            server.Transmit("Test_S4");
            server.Transmit("Test_S5");
            server.Transmit("Done");

            server.Stop();
            serverDone = true;
        }

        static void ClientThread()
        {
            Console.WriteLine("Starting Client Thread...");
            Client client = new Client("192.168.1.220", true);
            client.Start();

            client.Transmit("Test_C1");
            client.Transmit("Test_C2");
            client.Transmit("Test_C3");
            client.Transmit("Test_C4");
            client.Transmit("Test_C5");
            client.Transmit("Done");

            string[] message = new string[0];
            do
            {
                Thread.Sleep(100);
                message = client.Receive();
            } while (message.Length == 0 || message[message.Length - 1] != "Done");

            client.Stop();
            clientDone = true;
        }

        static void Main(string[] args)
        {
            Thread serverThread = new Thread(new ThreadStart(ServerThread));
            Thread clientThread = new Thread(new ThreadStart(ClientThread));

            serverThread.Start();

            Thread.Sleep(100);

            clientThread.Start();

            while (!serverDone && !clientDone)
            {
                Thread.Sleep(100);
            }

            serverThread.Join();
            clientThread.Join();

            Console.WriteLine("Example finished running.");

        }
    }
}
