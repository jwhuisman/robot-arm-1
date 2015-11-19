using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class Network : MonoBehaviour
{
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
			string message = ReceiveMessage();
            if (message != "")
            {
                Debug.Log(string.Format("Message received:\n{0}", message));
            }
			//_server.BeginAcceptTcpClient(new AsyncCallback(OnAcceptTcpClient), _server);

		}
	}
	
	private string ReceiveMessage()
	{
		Debug.Assert(_client != null, "The client is empty.");
		Debug.Assert(_client.Available > 0, "The client is not available.");
		
		NetworkStream stream = null;
		
		stream = _client.GetStream();
			
		while (stream.DataAvailable) 
		{
            //Bytes een voor een uitlezen om te izen of de byte een enter is.
			//Byte[] bytes = new Byte[256];
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

        //byte[] response = Encoding.ASCII.GetBytes("HTTP/1.1 204 No Content\r\n\r\n");
        //stream.Write(response, 0, response.Length);

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