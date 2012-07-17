using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;

namespace NodeSpace.Host
{
	class ConnectionContext
	{
		public Stateless.StateMachine<HostState, HostCommand> State
		{ get; protected set; }

		public DateTime LastActivity
		{ get; protected set; }

		public ConnectionContext()
		{
			State = new StateMachine<HostState, HostCommand>(HostState.Disconnected);

			State.Configure(HostState.Disconnected)
				.Permit(HostCommand.ConnectionRequested, HostState.Connecting);

			State.Configure(HostState.Connecting)
				.Permit(HostCommand.ConnectionEstablished, HostState.Connected)
				.Permit(HostCommand.ConnectionFailed, HostState.Disconnected);

			State.Configure(HostState.Connected)
				.Permit(HostCommand.MessageReceived, HostState.Processing);

			State.Configure(HostState.Processing)
				.Permit(HostCommand.ResponseSent, HostState.Connected);

			LastActivity = DateTime.Now;
		}

		public void UpdateLastActivity()
		{
			LastActivity = DateTime.Now;
		}
	}
}
