using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Network : MonoBehaviour
{
	// Buffer for reading data
	Byte[] bytes = new Byte[256];
	String data = null;
	TcpListener tcpListener = null;
	TcpClient client;
	NetworkStream stream;

	// Use this for initialization
    void Start()
	{
		// TcpListener tcpListener = new TcpListener(port);
		tcpListener = new TcpListener(IPAddress.Loopback, 9876);
		
		// Start listening for client requests.
		tcpListener.Start();
		
		// Perform a blocking call to accept requests.
		// You could also user tcpListener.AcceptSocket() here.
		client = tcpListener.AcceptTcpClient(); 

		Debug.Log ("The client is succesfully connected.");
		Console.WriteLine("Connected!");

		string responseString = "You have successfully connected to me";

		// Get a stream object for reading and writing
		stream = client.GetStream();
		
		//Forms and sends a response string to the connected client.
		Byte[] sendBytes = Encoding.ASCII.GetBytes(responseString);
		stream.Write(sendBytes, 0, sendBytes.Length);
		Debug.Log("Message Sent /> : " + responseString);
	} 
	
	// Update is called once per frame
	void Update()
	{ 
		if (stream.CanRead)
		{
			// Reads NetworkStream into a byte buffer.
			byte[] bytes = new byte[client.ReceiveBufferSize];
			
			// Read can return anything from 0 to numBytesToRead. 
			// This method blocks until at least one byte is read.
			stream.Read (bytes, 0, (int)client.ReceiveBufferSize);
			
			// Returns the data received from the host to the console.
			string returndata = Encoding.UTF8.GetString (bytes);
			
			Debug.Log ("This is what the host returned to you: " + returndata);
		}
		else
		{
			Debug.Log ("You cannot read data from this stream.");
			client.Close ();
			
			// Closing the tcpClient instance does not close the network stream.
			stream.Close ();
			return;
		}
		/*

		data = null;
		
		int i;
		
		// Loop to receive all the data sent by the client.
		while((i = stream.Read(bytes, 0, bytes.Length))!=0) 
		{   
			// Translate data bytes to a ASCII string.
			data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
			Console.WriteLine("Received: {0}", data);
			
			// Process the data sent by the client.
			data = data.ToUpper();
			
			byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
			
			// Send back a response.
			stream.Write(msg, 0, msg.Length);
			Console.WriteLine("Sent: {0}", data);            
		} 
		
		// Shutdown and end connection
		//client.Close(); */
		tcpListener.Stop(); 
	}
}