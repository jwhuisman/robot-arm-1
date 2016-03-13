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

    const char NEW_LINE = (char)10;
    const char CARRIAGE_RETURN = (char)13;

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
        if (IsConnected && _client != null && _client.Available > 0)
        {
            string data = FilterDataIntoMessage();
            if (data != "")
            {
                AddCommand(data);
                Debug.Log(string.Format("Data received: \n{0}", data));
            }
        }
    }

    void OnApplicationQuit()
    {
        CloseTcpConnection();
    }

    public void AddCommand(string data)
    {
        var cmd = commandBuilder.BuildCommand(data);
        if (cmd != null)
        {
            commandRunner.Add(cmd);
        }
    }

    public void ReturnMessage(string message)
    {
        if (IsConnected)
        {
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

    public bool IsConnected
    {
        get
        {
            try
            {
                if (_client != null && _client.Client != null && _client.Client.Connected)
                {
                    /* pear to the documentation on Poll:
                     * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                     * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                     * -or- true if data is available for reading; 
                     * -or- true if the connection has been closed, reset, or terminated; 
                     * otherwise, returns false
                     */

                    // Detect if client disconnected
                    if (_client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (_client.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    private void OnAcceptTcpClient(IAsyncResult result)
    {
        CloseTcpConnection();

        _client = ((TcpListener)result.AsyncState).EndAcceptTcpClient(result);

        ReturnMessage("hello");
        Debug.Log("Client connected.");

        // start listening for a new client
        StartListening();
    }

    private void CloseTcpConnection()
    {
        _server.Stop();

        if (IsConnected)
        {
            ReturnMessage("Bye");

            _client.GetStream().Close();
            _client.Close();

            Debug.Log("Client disconnected!");
        }
    }

    private string FilterDataIntoMessage()
    {
        NetworkStream stream = _client.GetStream();

        while (stream.DataAvailable)
        {
            int i = stream.ReadByte();
            char j = Convert.ToChar(i);
            if (j.Equals(NEW_LINE) || j.Equals(CARRIAGE_RETURN))
            {
                string msg = message.ToString().ToLower().Replace("\r", "").Replace("\n", "");

                message = new StringBuilder();

                return msg;
            }
            else
            {
                message.Append(j);
            }
        }

        return string.Empty;
    }


    private TcpListener _server;
    private TcpClient _client;
    private StringBuilder message = new StringBuilder();
    private CommandBuilder commandBuilder = new CommandBuilder();
}