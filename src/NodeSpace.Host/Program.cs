﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSpace.Host
{
	class Program
	{
		const string UpstreamAddress = "tcp://*:45000";
		const string DownstreamAddress = "tcp://*:45001";

		static Dictionary<byte[], ConnectionContext> Connections = new Dictionary<byte[], ConnectionContext>(new ByteArrayComparer());

		[STAThread]
		static void Main(string[] args)
		{	
			//byte[] master = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
			//byte[] salt = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 };
			//byte[] result1 = new byte[128];
			//byte[] result2 = new byte[128];

			//CryptSharp.Utility.SCrypt.ComputeKey(master, salt, (int)Math.Pow(2, 14), 8, 1, 2, result1);
			//CryptSharp.Utility.SCrypt.ComputeKey(master, salt, (int)Math.Pow(2, 14), 8, 1, 2, result2);

			//System.Diagnostics.Debug.Assert(result1.SequenceEqual(result2));


			using(var zmqContext = new ZMQ.Context())
			using(var upstreamSocket = zmqContext.Socket(ZMQ.SocketType.ROUTER))
			using(var downstreamSocket = zmqContext.Socket(ZMQ.SocketType.PUB))
			{
				upstreamSocket.Bind(UpstreamAddress);
				downstreamSocket.Bind(DownstreamAddress);

				while(true)
				{
					var message = upstreamSocket.RecvAll();
					var senderId = message.Dequeue();

					// Fetch the context of this sender
					ConnectionContext connectionContext;
					if(Connections.ContainsKey(senderId))
						connectionContext = Connections[senderId];
					else
					{
						connectionContext = CreateConnectionContext();
						Connections[senderId] = connectionContext;
					}

					// Validate the message

					// Broadcast the message
					byte[] messagePart = message.Dequeue();
					while(message.Any())
					{
						downstreamSocket.SendMore(messagePart);
						messagePart = message.Dequeue();
					}
					downstreamSocket.Send(messagePart);
				}

				upstreamSocket.Linger = 0;
				downstreamSocket.Linger = 0;
			}
		}

		private static ConnectionContext CreateConnectionContext()
		{
			return new ConnectionContext();
		}
	}
}
