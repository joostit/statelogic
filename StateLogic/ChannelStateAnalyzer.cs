using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BrrrBayBay.StateLogic
{
	public class ChannelStateAnalyzer : IDisposable
	{

		private Object channelStateLock = new object();

		private volatile List<ChannelStates> channelStateCache;

		private volatile System.Threading.Timer watchdogTimer;

		private byte[] channelMasks;

		/// <summary>
		/// Defines the timeout after which the data input is to be lost
		/// </summary>
		private const int WATCHDOG_TIMEOUT = 100;

		/// <summary>
		/// Returns a collection containing the current state of each channel
		/// </summary>
		/// <remarks>
		/// The collection is a deep clone of the internal state collection
		/// </remarks>
		public List<ChannelStates> CurrentChannelStates
		{
			get
			{
				List<ChannelStates> retVal = createChannelStateList();
				lock (channelStateLock)
				{
					for (int i = 0; i < 8; i++)
					{
						retVal[i] = channelStateCache[i];
					}
				}
				return retVal;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ChannelStateAnalyzer()
		{
			channelMasks = new byte[8];
			channelMasks[0] = 1;
			channelMasks[1] = 2;
			channelMasks[2] = 4;
			channelMasks[3] = 8;
			channelMasks[4] = 16;
			channelMasks[5] = 32;
			channelMasks[6] = 64;
			channelMasks[7] = 128;

			channelStateCache = createChannelStateList();
			watchdogTimer = new Timer(new TimerCallback(watchDogCallback), null, WATCHDOG_TIMEOUT, Timeout.Infinite);
		}


		private void watchDogCallback(Object state)
		{
			lock (channelStateLock)
			{
				for (int i = 0; i < 8; i++)
				{
					channelStateCache[i] = ChannelStates.Unknown;
				}
			}
		}

		/// <summary>
		/// Takes data and analyzes it. Results are available in the CurrentChannelStates property
		/// </summary>
		/// <remarks>
		/// The new analysis result overwrite the existing results stored in the CurrentChannelStates property
		/// </remarks>
		/// <param name="data"></param>
		public void analyzeData(byte[] data)
		{
			bool[] lastStates = new bool[8];
			int[] changeCount = new int[8];
			int bitIndex = 0;
			int bitValue;
			Boolean newState = false;


			// First determine the bitstate of the first byte
			for (bitIndex = 0; bitIndex < 8; bitIndex++)
			{
				bitValue = data[0] & channelMasks[bitIndex];	// Get the channel state
				lastStates[bitIndex] = (bitValue != 0);
			}


			// Determine if the subsequent bytes in the data change, and how often that happens
			for (int byteIndex = 0; byteIndex < data.Length; byteIndex++)
			{
				for (bitIndex = 0; bitIndex < 8; bitIndex++)
				{
					bitValue = data[byteIndex] & channelMasks[bitIndex];	// Get the channel state

					newState = (bitValue != 0);

					if (lastStates[bitIndex] != newState)
					{
						changeCount[bitIndex]++;
						lastStates[bitIndex] = newState;
					}
				}
			}


			// Determine the new state of each channel
			lock (channelStateLock)
			{
				for (bitIndex = 0; bitIndex < 8; bitIndex++)
				{
					switch (changeCount[bitIndex])
					{
						case 0:
						case 1:
							channelStateCache[bitIndex] = bitToState(lastStates[bitIndex]);
							break;
						default:
							channelStateCache[bitIndex] = ChannelStates.Signal;
							break;
					}
				}
			}

			watchdogTimer.Change(WATCHDOG_TIMEOUT, Timeout.Infinite);
		}

		/// <summary>
		/// Converts a boolean bit value to ChannelState enum
		/// </summary>
		/// <param name="bitState"></param>
		/// <returns></returns>
		private ChannelStates bitToState(Boolean bitState)
		{
			if (bitState == true)
			{
				return ChannelStates.High;
			}
			else
			{
				return ChannelStates.Low;
			}
		}


		private List<ChannelStates> createChannelStateList()
		{
			List<ChannelStates> retVal = new List<ChannelStates>();
			for (int i = 0; i < 8; i++)
			{
				retVal.Add(ChannelStates.Unknown);
			}
			return retVal;
		}



		public void Dispose()
		{
			watchdogTimer.Change(Timeout.Infinite, Timeout.Infinite);
			watchdogTimer.Dispose();
		}
	}
}
