using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaleaeDeviceSdkDotNet;
using System.Threading;

namespace BrrrBayBay.StateLogic
{
	public class LogicConnector
	{

		/// <summary>
		/// Gets a value that indicates if the device is running
		/// </summary>
		public Boolean Running
		{
			get
			{
				return running;
			}
		}

		/// <summary>
		/// Gets a value that indicates if the Logic device is connected (True if connected)
		/// </summary>
		public Boolean Connected
		{
			get
			{
				return (logicDevice != null);
			}
		}

		private ulong devId = 0;

		private LogicSampleRate sampleRate = LogicSampleRate.f4M;

		/// <summary>
		/// Gets or sets the sample rate. Thios should only be applied when the device isn't running
		/// </summary>
		public LogicSampleRate SampleRate
		{
			get
			{
				return sampleRate;
			}
			set
			{
				sampleRate = value;
				if (logicDevice != null)
				{
					logicDevice.SampleRateHz = (uint)value;
				}
			}
		}


		/// <summary>
		/// Gets raised when new data has been received from the Logic device
		/// </summary>
		public event DataReceivedEventHandler DataReceived;

		/// <summary>
		/// Gets raised when the Running state changes
		/// </summary>
		public event EventHandler RunningStateChanged;

		/// <summary>
		/// Gets raised when a Logic device is connected
		/// </summary>
		public event EventHandler OnLogicConnect;

		/// <summary>
		/// Gets raised when the nlogic device disconnects
		/// </summary>
		public event EventHandler OnLogicDisconnect;

		/// <summary>
		/// Gets raised when an error occurs
		/// </summary>
		public event EventHandler OnError;

		private MLogic logicDevice = null;

		private MSaleaeDevices deviceConnector;

		private Boolean running = false;


		public LogicConnector()
		{
			deviceConnector = new MSaleaeDevices();
			deviceConnector.OnLogicConnect += new MSaleaeDevices.OnLogicConnectDelegate(devices_OnConnect);
			deviceConnector.OnDisconnect += new MSaleaeDevices.OnDisconnectDelegate(devices_OnDisconnect);

			deviceConnector.BeginConnect();
		}


		public void Disconnect()
		{
			if (logicDevice != null)
			{
				logicDevice.Stop();
				logicDevice = null;
			}

			raiseRunningStateChanged();
		}

		/// <summary>
		/// Starts reading
		/// </summary>
		public void Start()
		{
			logicDevice.ReadStart();
			running = true;
			raiseRunningStateChanged();
		}

		/// <summary>
		/// Stops the reading 
		/// </summary>
		public void Stop()
		{
			try
			{
				running = false;
				Thread.Sleep(50);
				logicDevice.Stop();
				Thread.Sleep(200);				// Allow the device the complete its running tasks
				running = false;
				raiseRunningStateChanged();
			}
			catch { }
		}

		/// <summary>
		/// Raises the RunningStateChanged event
		/// </summary>
		private void raiseRunningStateChanged()
		{
			if (RunningStateChanged != null)
			{
				RunningStateChanged(this, new EventArgs());
			}
		}



		private void devices_OnDisconnect(ulong device_id)
		{
			running = false;
			logicDevice = null;
			device_id = 0;
			if (OnLogicDisconnect != null)
			{
				OnLogicDisconnect.Invoke(this, new EventArgs());
			}
		}


		private void devices_OnConnect(ulong device_id, MLogic logic)
		{
			running = false;
			if (logic != null)
			{
				logicDevice = logic;
				logicDevice.SampleRateHz = (uint)(sampleRate);
				this.devId = device_id;

				logicDevice.OnError += new MLogic.OnErrorDelegate(logicDevice_OnError);
				logicDevice.OnReadData += new MLogic.OnReadDataDelegate(logicDevice_OnReadData);
				logicDevice.OnWriteData += new MLogic.OnWriteDataDelegate(logicDevice_OnWriteData);

				if (OnLogicConnect != null)
				{
					OnLogicConnect.Invoke(this, new EventArgs());
				}
			}
		}

		/// <summary>
		/// Callback when the Logic device requests data to be written
		/// </summary>
		/// <param name="device_id">The requesting device ID</param>
		/// <param name="data">The array that needs to be filled</param>
		private void logicDevice_OnWriteData(ulong device_id, byte[] data)
		{
			// Not used
		}

		/// <summary>
		/// Callback when the Logic device has data to be read
		/// </summary>
		/// <remarks>This callback isn't used</remarks>
		/// <param name="device_id">The device ID from which data is available</param>
		/// <param name="data">The array which contains the data</param>
		private void logicDevice_OnReadData(ulong device_id, byte[] data)
		{
				byte[] dataClone = new byte[data.Length];
				Array.Copy(data, dataClone, data.Length);
				ThreadPool.QueueUserWorkItem(this.dispatchData, dataClone);
		}


		// gets called on a new thread to raise the DataReceived event
		private void dispatchData(Object dataArray)
		{
			if (DataReceived != null)
			{
				DataReceived.Invoke(this, new DataReceivedEventArgs((byte[])dataArray));
			}
		}

		/// <summary>
		/// Callback for when an error occured
		/// </summary>
		/// <param name="device_id">The device ID which created the error</param>
		private void logicDevice_OnError(ulong device_id)
		{
			if (running)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(raiseOnError));
			}
		}


		/// <summary>
		/// Raises the OnError event. This method is called on a threadpool thread and also stops current operations
		/// </summary>
		/// <param name="state">N/A</param>
		private void raiseOnError(Object state)
		{
			Stop(); // This happens here because

			if (OnError != null)
			{
				OnError(this, new EventArgs());
			}
		}


		public void Dispose()
		{
			this.Disconnect();
			Thread.Sleep(100);
			logicDevice = null;
			deviceConnector = null;
			running = false;

			GC.Collect();
		}


	}
}
