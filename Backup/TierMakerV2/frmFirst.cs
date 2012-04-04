using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TierMaker;

namespace TierMakerV2 {
	public partial class frmFirst : Form {
		public frmFirst () {
			InitializeComponent();
		}

		private void btnBrowse_Click ( object sender, EventArgs e ) {
			if ( fbdBrowse.ShowDialog() == DialogResult.OK )
				txtLocation.Text = fbdBrowse.SelectedPath;
		}

		private void btnNext_Click ( object sender, EventArgs e ) {
			if ( txtLocation.Text.Trim() != string.Empty ) {
				frmMain frm = new frmMain();
				string t = txtLocation.Text.Trim();
				if ( !t.EndsWith( "\\" ) ) t += "\\";
				frm.Project = t;
				frm.Show();
				// close closes everything, including frm, and I really
				// can't be arsed to look up how to fix it, so I'll cheat
				// and just hide this form
				this.Hide();
			}
		}
	}
}