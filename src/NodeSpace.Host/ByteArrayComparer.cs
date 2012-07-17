using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeSpace.Host
{
	class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		public bool Equals(byte[] left, byte[] right)
		{
			if(left == null || right == null)
				return left == right;

			return left.SequenceEqual(right);
		}

		public int GetHashCode(byte[] value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			return value.Sum(b => b);
		}
	}
}
