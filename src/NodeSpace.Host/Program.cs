using System;
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

		enum HostState
		{
			NotConnected,
			ConnectRequested,
			Connected,
			MessageReceived,
			ResponsePending,
		}

		enum HostCommand
		{
			Connection,
			ConnectionEstablished,
			ConnectionFailed,
			Message,
			Processed,
			Responeded

		}

		[STAThread]
		static void Main(string[] args)
		{
			var hostStateMachine = new Stateless.StateMachine<HostState, HostCommand>(
			byte[] master = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
			byte[] salt = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 };
			byte[] result1 = new byte[128];
			byte[] result2 = new byte[128];

			CryptSharp.Utility.SCrypt.ComputeKey(master, salt, (int)Math.Pow(2, 14), 8, 1, 2, result1);
			CryptSharp.Utility.SCrypt.ComputeKey(master, salt, (int)Math.Pow(2, 14), 8, 1, 2, result2);

			System.Diagnostics.Debug.Assert(result1.SequenceEqual(result2));

			using(var zmqContext = new ZMQ.Context())
			using(var upstreamSocket = zmqContext.Socket(ZMQ.SocketType.REP))
			using(var downstreamSocket = zmqContext.Socket(ZMQ.SocketType.PUB))
			{
				upstreamSocket.Bind(UpstreamAddress);
				downstreamSocket.Bind(DownstreamAddress);

				while(true)
				{
					var message = upstreamSocket.RecvAll();
					
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
	}
}
