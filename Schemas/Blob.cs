using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	public class Blob
	{
		private byte[] data;

		public byte[] Data
		{
			get
			{
				return data;
			}

			set
			{
				data = value;
			}
		}
	}
}
