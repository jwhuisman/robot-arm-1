using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Network : MonoBehaviour
{
    public Text text;

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
                text.text = message;
                Debug.Log(string.Format("Message received:\n{0}", message));
            }
		}
	}
	
	private string FilterDataIntoMessage()
	{
		Debug.Assert(_client != null, "The client is empty.");
		Debug.Assert(_client.Available > 0, "The client is not available.");
		
		NetworkStream stream = null;

        stream = _client.GetStream();
			
		while (stream.DataAvailable) 
		{
			int i = stream.ReadByte();
            if (i == 10)
            {
                string msg = message.ToString();
                message = new StringBuilder();
                return msg.ToString();
            }
            else if (i == 13) { }
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