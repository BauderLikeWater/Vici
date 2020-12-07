using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Client class to facilitate a TCP connection with a server.
///
/// This client and server are designed to communciate using strings. Either client or server can transmit a string of information and the other can receive it.
/// </summary>
public class Client
{
    private string ip;
    private TcpClient tcp;
    private Stream stream;
    private ASCIIEncoding encoder;
    private bool log;

    private static int PORT = 8001;

    /// <summary>
    /// Simplest constructor. Sets log variable to false.
    /// </summary>
    /// <param name="ip">The IP address of the server the client needs to connect to.</param>
	public Client(string ip) : this(ip, false) { }

    /// <summary>
    /// Constuctor.
    /// </summary>
    /// <param name="ip">The IP address of the server the client needs to connect to.</param>
    /// <param name="log">Whether or not to log status message to the console. Useful for debugging.</param>
    public Client(string ip, bool log)
    {
        this.ip = ip;
        this.log = log;
        tcp = new TcpClient();
        encoder = new ASCIIEncoding();
    }

    /// <summary>
    /// Starts the server. Must be called after Start() is called on the server side, otherwise the client will throw an error.
    /// </summary>
    public void Start()
    {
        if (log)
            Console.WriteLine("[Client] Connecting to Server...");

        tcp.Connect(ip, PORT);
        stream = tcp.GetStream();

        if (log)
            Console.WriteLine("[Client] Connected!");
    }

    /// <summary>
    /// Transmits a message to the server.
    /// </summary>
    /// <param name="message">The message to be transmitted.</param>
    public void Transmit(string message)
    {
        byte[] messageBytes = encoder.GetBytes(message + "\n");

        if (log)
            Console.WriteLine("[Client] Transmitting: " + message);

        stream.Write(messageBytes, 0, messageBytes.Length);
        stream.Flush();
    }

    /// <summary>
    /// Returns an array of all the messages the server transmitted. If the server hasn't sent anything then returns an empty array.
    /// </summary>
    /// <returns>All the strings the server transmitted (or empty array if none).</returns>
    public string[] Receive()
    {
        byte[] bb = new byte[1028];
        int k = stream.Read(bb, 0, 1028);


        LinkedList<string> list = new LinkedList<string>();
        string message = "";
        for (int i = 0; i < k; i++)
        {
            char c = Convert.ToChar(bb[i]);
            if (c == '\n')
            {
                list.AddLast(message);
                message = "";
            } else
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
                Console.WriteLine("[Client] Received message: " + result[i]);
        }

        return result;
    }

    /// <summary>
    /// Stops the server and closes all socket/tcp resources.
    /// </summary>
    public void Stop()
    {
        if (log)
            Console.WriteLine("[Client] Stopping client...");

        stream.Close();
        tcp.Close();
    }
}