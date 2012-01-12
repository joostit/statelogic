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
	public partial class AboutForm : Form
	{
		/// <summary>
		/// Gets or sets the application version
		/// </summary>
		public Version appVersion { get; set; }

		public AboutForm()
		{
			appVersion = new Version("0.0.0.0");
			InitializeComponent();
		}

		private void AboutForm_Load(object sender, EventArgs e)
		{
			versionLabel.Text = appVersion.ToString();
		}
	}
}
