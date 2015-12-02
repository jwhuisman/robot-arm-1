using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkListener : MonoBehaviour
{
    public Text text;
    public RobotArmController _RobotArmController;

    void Start()
    {
        Application.runInBackground = true;

        int port = 9876;
        IPAddress localhost = IPAddress.Parse("127.0.0.1");
        _server = new TcpListener(localhost, port);
        _server.Start();
        _server.BeginAcceptTcpClient(new AsyncCallback(OnAcceptTcpClient), _server);
    }

    void Update()
    {
        if (_client != null && _client.Available > 0)
        {
            string message = FilterDataIntoMessage();
            if (message != "")
            {
                _RobotArmController.Actions(message);
                text.text = message;
                Debug.Log(string.Format("Message received:\n{0}", message));
            }
        }
    }

    private string FilterDataIntoMessage()
    {
        const char NEW_LINE = (char)10;
        const char CARRIAGE_RETURN = (char)13;

        Debug.Assert(_client != null, "The client is empty.");
        Debug.Assert(_client.Available > 0, "The client is not available.");

        NetworkStream stream = null;

        stream = _client.GetStream();

        while (stream.DataAvailable) 
        {
            int i = stream.ReadByte();
            if (i == NEW_LINE)
            {
                string msg = message.ToString();
                message = new StringBuilder();
                return msg.ToString();
            }
            else if (i == CARRIAGE_RETURN) {
                // In some OS'es the byte 13 has the same fucntion as the byte 10.
                // byte 10 the mostly used, thats why it gets the functionality.
            }
            else
            {
                message.Append((char) i);
            }
    }

        return String.Empty;
    }

    private void OnAcceptTcpClient(IAsyncResult result)
    {
        TcpListener _server = (TcpListener) result.AsyncState;
        _client = _server.EndAcceptTcpClient(result);
        Debug.Log("Client connected.");
    }

    private TcpListener _server;
    private TcpClient _client;
    private StringBuilder message = new StringBuilder();
}