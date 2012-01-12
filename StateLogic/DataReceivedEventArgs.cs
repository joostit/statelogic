using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrrrBayBay.StateLogic
{
	public delegate void DataReceivedEventHandler(Object sender, DataReceivedEventArgs args);

	public class DataReceivedEventArgs : EventArgs
	{
		public byte[] Data { get; private set; }

		public DataReceivedEventArgs(byte[] data)
		{
			this.Data = data;
		}
	}
}
