using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Assets.Models;
using Assets.Scripts.View;

public class NetworkListener : MonoBehaviour
{
    public Text text;
    public RobotArm _robotArm;
    public CommandRunner commandRunner;


    void Start()
    {
        Debug.Log("NetworkListener.cs is used!");
        Application.runInBackground = true;

        IPAddress localhost = IPAddress.Parse("127.0.0.1");
        int port = 9876;

        _server = new TcpListener(localhost, port);

        StartListening();
    }

    void Update()
    {
        if (connected)
        {
            if (_client != null && _client.Available > 0)
            {
                string data = GetData();
                if (data != "")
                {
                    Debug.Log(string.Format("Message received:\n{0}", data));
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        ReturnMessage("bye");

        CloseTcpConnection();
    }

    public void AddCommand(string data)
    {
        var cmd = commandBuilder.BuildCommand(data.ToLower());
        if (cmd != null)
        {
            commandRunner.Add(cmd);
        }
    }

    public void ReturnMessage(string message)
    {
        // sends a message to the telnet client.
        if (connected && _client != null && _client.GetStream() != null)
        {
            message = message == string.Empty ? "*no answer*" : message;

            _client.GetStream().Write(Encoding.ASCII.GetBytes(message.ToLower()), 0, message.Length);
            _client.GetStream().Flush();
        }
    }

    private void StartListening()
    {
        _server.Start();
        _server.BeginAcceptTcpClient(new AsyncCallback(OnAcceptTcpClient), _server);

        Debug.Log("Started listening..");
    }

    private void OnAcceptTcpClient(IAsyncResult result)
    {
        if (connected)
        {
            CloseTcpConnection();
        }

        _client = ((TcpListener)result.AsyncState).EndAcceptTcpClient(result);

        connected = true;
        ReturnMessage("hello");
        Debug.Log("Client connected.");

        // start listening for a new client
        StartListening();
    }

    private void CloseTcpConnection()
    {
        if (connected)
        {
            _client.Close();
    
            connected = false;
            Debug.Log("Client disconnected!");
        }
    }

    private string GetData()
    {
        NetworkStream stream = _client.GetStream();

        StringBuilder data = new StringBuilder();
        while (stream.DataAvailable)
        {
            data.Append((char)stream.ReadByte());
        }

        AddCommand(data.ToString());

        return data.ToString();
    }

    private string FilterDataIntoMessage()
    {
        const char NEW_LINE = (char)10;
        const char CARRIAGE_RETURN = (char)13;

        Debug.Assert(_client != null, "The client is empty.");
        Debug.Assert(_client.Available > 0, "The client is not available.");

        NetworkStream stream = _client.GetStream();

        int c = 0;
        while (stream.DataAvailable)
        {
            int i = stream.ReadByte();
            if (i == NEW_LINE)
            {
                string msg = message.ToString().ToLower().Replace("\r", "").Replace("\n", "");
                message = new StringBuilder();

                AddCommand(msg);

                return msg.ToString().ToLower();
            }
            else if (i == CARRIAGE_RETURN && c > 0)
            {
                // In some OS'es the byte 13 has the same fucntion as the byte 10.
                // byte 10 the mostly used, thats why it gets the functionality.

                // for some reason this doesnt work...
                //message.Remove(c, 1);
            }
            else
            {
                message.Append((char)i);
            }

            c++;
        }

        return message.ToString().ToLower();
    }


    private bool connected = false;

    private TcpListener _server;
    private TcpClient _client;
    private StringBuilder message = new StringBuilder();
    private CommandBuilder commandBuilder = new CommandBuilder();
}