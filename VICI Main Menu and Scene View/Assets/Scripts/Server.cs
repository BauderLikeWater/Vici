using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Server class to facilitate a TCP connection with a client.
///
/// This client and server are designed to communciate using strings. Either client or server can transmit a string of information and the other can receive it.
/// </summary>
public class Server
{
    private IPAddress ip;
    private TcpListener listener;
    private Socket socket;
    private ASCIIEncoding encoder;
    private bool log;

    private static int PORT = 8001;

    /// <summary>
    /// Simplest Constructor. Sets log variable to false.
    /// </summary>
    public Server() : this(false) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="log">Whether or not to log status message to the console. Useful for debugging.</param>
    public Server(bool log)
	{
        this.log = log;
        this.ip = IPAddress.Parse(GetLocalIPAddress());
        listener = new TcpListener(this.ip, PORT);
        encoder = new ASCIIEncoding();
	}

    /// <summary>
    /// Returns the IP address of the server.
    /// </summary>
    /// <returns>IP Address in string form.</returns>
    public string GetIPAddress()
    {
        return ip.ToString();
    }

    /// <summary>
    /// Starts the server. Note: Must call server Start() before client Start().
    /// </summary>
    public void Start()
    {
        if (log)
            Console.WriteLine("[Server] Starting server...");

        listener.Start();

        if (log)
            Console.WriteLine("[Server] Waiting for a connection.....");

        socket = listener.AcceptSocket();
        if (log)
            Console.WriteLine("[Server] Connection accepted from " + socket.RemoteEndPoint);
    }

    /// <summary>
    /// Transmits the message to the client.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    public void Transmit(string message)
    {
        if (log)
            Console.WriteLine("[Server] Sending message: " + message);

        socket.Send(encoder.GetBytes(message + "\n"));
    }

    /// <summary>
    /// Returns an array of all the messages the client transmitted. If the client hasn't sent anything then returns an empty array.
    /// </summary>
    /// <returns>The received messages (or an empty array if none).</returns>
    public string[] Receive()
    {
        byte[] b = new byte[1028];
        int k = socket.Receive(b);

        LinkedList<string> list = new LinkedList<string>();
        string message = "";
        for (int i = 0; i < k; i++)
        {
            char c = Convert.ToChar(b[i]);
            if (c == '\n')
            {
                list.AddLast(message);
                message = "";
            }
            else
            {
                message += c;
            }
        }


        string[] result = new string[list.Count];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = list.First.Value;
            list.RemoveFirst();
            if (log)
                Console.WriteLine("[Server] Received message: " + result[i]);
        }

        return result;
    }

    /// <summary>
    /// Stops the server and cleans up resources.
    /// </summary>
    public void Stop()
    {
        if (log)
            Console.WriteLine("[Server} Stopping server...");
        socket.Close();
        listener.Stop();
    }

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}