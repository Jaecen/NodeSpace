using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSpace.Host
{
	enum HostCommand
	{
		ConnectionRequested,
		ConnectionEstablished,
		ConnectionFailed,
		MessageReceived,
		ResponseSent,
	}
}
