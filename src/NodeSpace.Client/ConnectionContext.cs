using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NodeSpace.Client
{
	public class ConnectionContext
	{
		public ConnectionContext(string address, string privateKey)
		{
			//RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
			//RSAParameters

			byte[] master = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
			byte[] salt = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 };
			byte[] result = new byte[128];

			// cost, parallel, and blockSize all increase security and CPU usage to generate
			// starting with 2^14, 8, and 1 respectively.
			CryptSharp.Utility.SCrypt.ComputeKey(master, salt, (int)Math.Pow(2, 14), 8, 1, 2, result);
			
		}
	}
}
