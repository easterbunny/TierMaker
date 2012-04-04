namespace TierMakerV2 {
	partial class frmFirst {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.fbdBrowse = new System.Windows.Forms.FolderBrowserDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 63 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 222, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "First, please specify the location of the project";
			// 
			// txtLocation
			// 
			this.txtLocation.Location = new System.Drawing.Point( 15, 80 );
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.Size = new System.Drawing.Size( 385, 20 );
			this.txtLocation.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point( 406, 80 );
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size( 75, 23 );
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler( this.btnBrowse_Click );
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point( 204, 125 );
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size( 75, 23 );
			this.btnNext.TabIndex = 3;
			this.btnNext.Text = "Next >>";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler( this.btnNext_Click );
			// 
			// frmFirst
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 506, 196 );
			this.Controls.Add( this.btnNext );
			this.Controls.Add( this.btnBrowse );
			this.Controls.Add( this.txtLocation );
			this.Controls.Add( this.label1 );
			this.Name = "frmFirst";
			this.Text = "Main";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog fbdBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnNext;
	}
}