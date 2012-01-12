namespace BrrrBayBay.StateLogic
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this.connectionLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.runStateLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.channelStateGroup = new System.Windows.Forms.GroupBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.startButton = new System.Windows.Forms.ToolStripButton();
			this.aboutButton = new System.Windows.Forms.ToolStripButton();
			this.mainStatusStrip.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainStatusStrip
			// 
			this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionLabel,
            this.runStateLabel});
			this.mainStatusStrip.Location = new System.Drawing.Point(0, 295);
			this.mainStatusStrip.Name = "mainStatusStrip";
			this.mainStatusStrip.Size = new System.Drawing.Size(257, 22);
			this.mainStatusStrip.SizingGrip = false;
			this.mainStatusStrip.TabIndex = 0;
			this.mainStatusStrip.Text = "statusStrip1";
			// 
			// connectionLabel
			// 
			this.connectionLabel.AutoSize = false;
			this.connectionLabel.Name = "connectionLabel";
			this.connectionLabel.Size = new System.Drawing.Size(100, 17);
			this.connectionLabel.Text = "Not connected";
			// 
			// runStateLabel
			// 
			this.runStateLabel.AutoSize = false;
			this.runStateLabel.Name = "runStateLabel";
			this.runStateLabel.Size = new System.Drawing.Size(100, 17);
			this.runStateLabel.Text = "Stopped";
			// 
			// channelStateGroup
			// 
			this.channelStateGroup.Location = new System.Drawing.Point(12, 28);
			this.channelStateGroup.Name = "channelStateGroup";
			this.channelStateGroup.Size = new System.Drawing.Size(230, 255);
			this.channelStateGroup.TabIndex = 1;
			this.channelStateGroup.TabStop = false;
			this.channelStateGroup.Text = "Channel States";
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startButton,
            this.aboutButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(257, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "MainToolStrip";
			// 
			// startButton
			// 
			this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
			this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(35, 22);
			this.startButton.Text = "Start";
			this.startButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.startButton.ToolTipText = "Start / Stop capture";
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// aboutButton
			// 
			this.aboutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.aboutButton.Image = ((System.Drawing.Image)(resources.GetObject("aboutButton.Image")));
			this.aboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.aboutButton.Name = "aboutButton";
			this.aboutButton.Size = new System.Drawing.Size(44, 22);
			this.aboutButton.Text = "About";
			this.aboutButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
			this.aboutButton.ToolTipText = "About StateLogic";
			this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(257, 317);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.channelStateGroup);
			this.Controls.Add(this.mainStatusStrip);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "StateLogic";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainStatusStrip.ResumeLayout(false);
			this.mainStatusStrip.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip mainStatusStrip;
		private System.Windows.Forms.GroupBox channelStateGroup;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton startButton;
		private System.Windows.Forms.ToolStripButton aboutButton;
		private System.Windows.Forms.ToolStripStatusLabel connectionLabel;
		private System.Windows.Forms.ToolStripStatusLabel runStateLabel;
	}
}

