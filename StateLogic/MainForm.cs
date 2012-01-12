using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrrrBayBay.StateLogic
{
	public partial class MainForm : Form
	{

		private Version appVersion = new Version("1.0.0.0");

		/// <summary>
		/// Defines the interval at which the GUI is updated with new state values
		/// </summary>
		private const int updateInterval = 100;

		private const LogicSampleRate defaultSampleRate = LogicSampleRate.f25k;

		private ChannelStateAnalyzer stateAnalyzer;

		private LogicConnector logicConnector;

		private System.Windows.Forms.Timer updateTimer;

		private TextBox labelEditBox;



		
		/// <summary>
		/// Holds the colors for each channel
		/// </summary>
		private static Color[] channelColors = { Color.Black, Color.Brown, Color.Red, Color.Orange, Color.Yellow, Color.Lime, Color.Blue, Color.Violet };

		private List<Label> channelStateLabels;

		private List<Label> channelTextLabels;

		private List<Label> channelColorLabels;

		private Font boldFont;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{

				createLabels();
				stateAnalyzer = new ChannelStateAnalyzer();
				updateTimer = new Timer();
				updateTimer.Interval = updateInterval;
				updateTimer.Tick += new EventHandler(updateTimer_Tick);
				updateTimer.Start();

				logicConnector = new LogicConnector();
				logicConnector.OnError += new EventHandler(logicConnector_OnError);
				logicConnector.OnLogicConnect += new EventHandler(logicConnector_OnLogicConnect);
				logicConnector.OnLogicDisconnect += new EventHandler(logicConnector_OnLogicDisconnect);
				logicConnector.RunningStateChanged += new EventHandler(logicConnector_RunningStateChanged);
				logicConnector.DataReceived += new DataReceivedEventHandler(logicConnector_DataReceived);

				logicConnector.SampleRate = defaultSampleRate;

				updateStatusLabels();

				labelEditBox = new TextBox();
				labelEditBox.Visible = false;
				labelEditBox.KeyDown += new KeyEventHandler(labelEditBox_KeyDown);
				labelEditBox.KeyPress += new KeyPressEventHandler(labelEditBox_KeyPress);
				labelEditBox.LostFocus += new EventHandler(labelEditBox_LostFocus);
				labelEditBox.Leave += new EventHandler(labelEditBox_LostFocus);
				channelStateGroup.Controls.Add(labelEditBox);
			}
			catch (Exception exc)
			{
				MessageBox.Show("An error occured during loading. Restart the application.\n\nError information:" + exc.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void labelEditBox_LostFocus(object sender, EventArgs e)
		{
			restoreLabeledit();
		}

	

		void labelEditBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar == (char)Keys.Escape) || (e.KeyChar == (char)Keys.Return))
			{
				e.Handled = true;
			}
		}

		void labelEditBox_Leave(object sender, EventArgs e)
		{
			restoreLabeledit();
		}

		void labelEditBox_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Return:
					channelTextLabels[(int)labelEditBox.Tag].Text = labelEditBox.Text;
					restoreLabeledit();
					e.Handled = true;
					break;

				case Keys.Escape:
					restoreLabeledit();
					e.Handled = true;
					break;
			}
		}


		void textLabel_DoubleClick(object sndr, EventArgs e)
		{
			Label sender = (Label)sndr;
			Label labelToEdit = channelTextLabels[(int)sender.Tag];

			if (labelEditBox.Visible == true)
			{
				restoreLabeledit();
			}

			labelEditBox.Location = labelToEdit.Location;
			labelEditBox.Size = labelToEdit.Size;
			labelEditBox.Text = labelToEdit.Text;
			labelEditBox.Tag = labelToEdit.Tag;
			labelEditBox.Visible = true;
			labelToEdit.Visible = false;
		}


		private void restoreLabeledit()
		{
			labelEditBox.Visible = false;
			channelTextLabels[(int)labelEditBox.Tag].Visible = true;
		}


		void logicConnector_DataReceived(object sender, DataReceivedEventArgs args)
		{
			stateAnalyzer.analyzeData(args.Data);
		}

		void logicConnector_RunningStateChanged(object sender, EventArgs e)
		{
			updateStatusLabelsThreadSafe();
		}

		void logicConnector_OnLogicDisconnect(object sender, EventArgs e)
		{
			updateStatusLabelsThreadSafe();
		}

		void logicConnector_OnLogicConnect(object sender, EventArgs e)
		{
			updateStatusLabelsThreadSafe();
		}

		void logicConnector_OnError(object sender, EventArgs e)
		{
			
		}


		private void updateStatusLabelsThreadSafe()
		{
			this.Invoke(new MethodInvoker(delegate { this.updateStatusLabels(); }));
		}


		/// <summary>
		/// Updates the status labels. Should not be called directly! Use the updateStatusLabelsThreadSafe() method instead.
		/// </summary>
		private void updateStatusLabels()
		{
			if (logicConnector.Connected)
			{
				connectionLabel.Text = "Connected";
				startButton.Enabled = true;

				if (logicConnector.Running)
				{
					runStateLabel.Text = "Running";
					startButton.Text = "Stop";
				}
				else
				{
					runStateLabel.Text = "Stopped";
					startButton.Text = "Start";
				}
			}
			else
			{
				connectionLabel.Text = "Not connected";
				runStateLabel.Text = "Stopped";
				startButton.Enabled = false;
			}
		}


		void updateTimer_Tick(object sender, EventArgs e)
		{
			List<ChannelStates> states = stateAnalyzer.CurrentChannelStates;
			Label currentChannelLabel;
			for (int i = 0; i < 8; i++)
			{
				currentChannelLabel = channelStateLabels[i];
				currentChannelLabel.Text = states[i].ToString();

				switch (states[i])
				{
					case ChannelStates.Unknown:
						currentChannelLabel.BackColor = Color.LightGray;
						currentChannelLabel.ForeColor = Color.DarkGray;	
						break;
					case ChannelStates.Low:
						currentChannelLabel.BackColor = Color.Red;
						currentChannelLabel.ForeColor = Color.Black;
						break;
					case ChannelStates.High:
						currentChannelLabel.BackColor = Color.Lime;
						currentChannelLabel.ForeColor = Color.Black;
						break;
					case ChannelStates.Signal:
						currentChannelLabel.BackColor = Color.Yellow;
						currentChannelLabel.ForeColor = Color.Black;
						break;
					default:
						MessageBox.Show("Unknown channel state received from analyzer");
						break;
				}
			}
		}


		private void createLabels()
		{
			channelStateLabels = new List<Label>();
			channelTextLabels = new List<Label>();
			channelColorLabels = new List<Label>();

			Label colorLabel = new Label();
			Label textLabel;
			Label stateLabel;

			int hPos = 10;
			int vPos = 20;

			boldFont = new Font(colorLabel.Font.FontFamily, 9, FontStyle.Bold);
			
			for (int i = 0; i < 8; i++)
			{
				colorLabel = new Label();
				colorLabel.AutoSize = false;
				colorLabel.Size = new System.Drawing.Size(15, 15);
				colorLabel.Location = new Point(hPos, vPos);
				colorLabel.BorderStyle = BorderStyle.FixedSingle;
				colorLabel.Text = "";
				colorLabel.BackColor = channelColors[i];
				colorLabel.Tag = i;
				colorLabel.DoubleClick += new EventHandler(textLabel_DoubleClick);
				
				textLabel = new Label();
				textLabel.AutoSize = false;
				textLabel.TextAlign = ContentAlignment.MiddleCenter;
				textLabel.BorderStyle = BorderStyle.FixedSingle;
				textLabel.Size = new System.Drawing.Size(100, 15);
				textLabel.Text = "Channel " + (i + 1).ToString();
				textLabel.Location = new Point(hPos + 14, vPos);
				textLabel.Tag = i;
				textLabel.DoubleClick += new EventHandler(textLabel_DoubleClick);
				
				stateLabel = new Label();
				stateLabel.AutoSize = false;
				stateLabel.Font = boldFont;
				stateLabel.TextAlign = ContentAlignment.MiddleCenter;
				stateLabel.BorderStyle = BorderStyle.FixedSingle;
				stateLabel.Size = new System.Drawing.Size(100, 19);
				stateLabel.Location = new Point(hPos + 113, vPos - 2);
				stateLabel.Text = "";
				
				channelColorLabels.Add(colorLabel);
				channelTextLabels.Add(textLabel);
				channelStateLabels.Add(stateLabel);

				channelStateGroup.Controls.Add(colorLabel);
				channelStateGroup.Controls.Add(textLabel);
				channelStateGroup.Controls.Add(stateLabel);

				vPos += 30;
			}
		}

		
		private void aboutButton_Click(object sender, EventArgs e)
		{
			AboutForm frm = new AboutForm();
			frm.appVersion = appVersion;
			frm.ShowDialog(this);
			frm.Dispose();
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			if (logicConnector.Running)
			{
				logicConnector.Stop();
			}
			else
			{
				logicConnector.Start();
			}
		}

	

	}
}
