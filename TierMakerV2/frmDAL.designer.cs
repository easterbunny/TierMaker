namespace TierMaker {
	partial class frmDAL {
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnDone = new System.Windows.Forms.Button();
			this.rbVB = new System.Windows.Forms.RadioButton();
			this.rbCS = new System.Windows.Forms.RadioButton();
			this.btnAbout = new System.Windows.Forms.Button();
			this.cbDALBase = new System.Windows.Forms.CheckBox();
			this.txtDALName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 14, 16 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 171, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Please insert the name of the DAL:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 14, 53 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 79, 13 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Function Name";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 216, 53 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 66, 13 );
			this.label3.TabIndex = 4;
			this.label3.Text = "Return Type";
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point( 522, 381 );
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size( 75, 23 );
			this.btnDone.TabIndex = 20;
			this.btnDone.Text = "Create >>";
			this.btnDone.UseVisualStyleBackColor = true;
			this.btnDone.Click += new System.EventHandler( this.btnDone_Click );
			// 
			// rbVB
			// 
			this.rbVB.AutoSize = true;
			this.rbVB.Checked = true;
			this.rbVB.Location = new System.Drawing.Point( 17, 384 );
			this.rbVB.Name = "rbVB";
			this.rbVB.Size = new System.Drawing.Size( 82, 17 );
			this.rbVB.TabIndex = 21;
			this.rbVB.TabStop = true;
			this.rbVB.Text = "Visual Basic";
			this.rbVB.UseVisualStyleBackColor = true;
			// 
			// rbCS
			// 
			this.rbCS.AutoSize = true;
			this.rbCS.Location = new System.Drawing.Point( 116, 384 );
			this.rbCS.Name = "rbCS";
			this.rbCS.Size = new System.Drawing.Size( 39, 17 );
			this.rbCS.TabIndex = 22;
			this.rbCS.Text = "C#";
			this.rbCS.UseVisualStyleBackColor = true;
			// 
			// btnAbout
			// 
			this.btnAbout.Location = new System.Drawing.Point( 208, 381 );
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size( 75, 23 );
			this.btnAbout.TabIndex = 23;
			this.btnAbout.Text = "About";
			this.btnAbout.UseVisualStyleBackColor = true;
			this.btnAbout.Click += new System.EventHandler( this.btnAbout_Click );
			// 
			// cbDALBase
			// 
			this.cbDALBase.AutoSize = true;
			this.cbDALBase.Location = new System.Drawing.Point( 350, 385 );
			this.cbDALBase.Name = "cbDALBase";
			this.cbDALBase.Size = new System.Drawing.Size( 93, 17 );
			this.cbDALBase.TabIndex = 24;
			this.cbDALBase.Text = "Use DALBase";
			this.cbDALBase.UseVisualStyleBackColor = true;
			// 
			// txtDALName
			// 
			this.txtDALName.Location = new System.Drawing.Point( 208, 13 );
			this.txtDALName.Name = "txtDALName";
			this.txtDALName.Size = new System.Drawing.Size( 224, 20 );
			this.txtDALName.TabIndex = 0;
			// 
			// frmDAL
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 609, 413 );
			this.Controls.Add( this.cbDALBase );
			this.Controls.Add( this.btnAbout );
			this.Controls.Add( this.rbCS );
			this.Controls.Add( this.rbVB );
			this.Controls.Add( this.btnDone );
			this.Controls.Add( this.txtDALName );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label3 );
			this.Name = "frmDAL";
			this.Text = "DAL Creator";
			this.Load += new System.EventHandler( this.frmDAL_Load );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		private const int __top = 69;
		private const int __height = 24;

		private int GetY ( int i ) {
			return __top + __height * ( __maxParams * i );
		}

		private int GetY2 ( int i, int j ) {
			return __top + __height * ( i * __maxParams + j );
		}

		#endregion

		private System.Windows.Forms.TextBox[] txtNames;
		private System.Windows.Forms.TextBox[][] txtParams;
		private System.Windows.Forms.ComboBox[][] txtParamTypes;
		private System.Windows.Forms.ComboBox[] txtTypes;
		private object[] strTypes;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.RadioButton rbVB;
		private System.Windows.Forms.RadioButton rbCS;
		private System.Windows.Forms.CheckBox cbDALBase;
		private System.Windows.Forms.TextBox txtDALName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

	}
}