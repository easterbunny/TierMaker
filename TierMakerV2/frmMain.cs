using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TierMaker {
	public partial class frmMain : Form {

		#region Variables

		private string __strModel; // Model name
		private string __strModels; // Plural form
		private string __strProject; // Location of the project
		private const int __maxSize = 8; // The number of elements

		#endregion

		#region Forms

		public frmMain () {
			InitializeComponent();
		}

		public string Project {
			set { __strProject = value; }
			get { return __strProject; }
		}

		private void frmMain_Load ( object sender, EventArgs e ) {

			txtNames = new TextBox[__maxSize];
			txtTypes = new ComboBox[__maxSize];
			txtReadOnly = new CheckBox[__maxSize];
			strTypes = new object[] { "Boolean", "Byte", "Char", "Decimal", "Double", "Float", 
									  "Integer", "Long", "Object", "Short", "String" };

			TextBox txt;
			ComboBox cmb;
			CheckBox cb;

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				txt = new TextBox();
				txt.Location = new System.Drawing.Point( 17, GetY( i ) );
				txt.Size = new System.Drawing.Size( 191, 20 );
				txt.TabIndex = i * 2;
				this.Controls.Add( txt );
				txtNames[i] = txt;

				cmb = new ComboBox();
				cmb.FormattingEnabled = true;
				cmb.Location = new System.Drawing.Point( 219, GetY( i ) );
				cmb.Size = new System.Drawing.Size( 191, 20 );
				cmb.TabIndex = i * 2 + 1;
				cmb.Items.AddRange( strTypes );
				this.Controls.Add( cmb );
				txtTypes[i] = cmb;

				cb = new CheckBox();
				cb.Location = new Point( 421, GetY( i ) );
				cb.TabIndex = 80 + i;
				this.Controls.Add( cb );
				txtReadOnly[i] = cb;
			}
		}

		private void btnDone_Click ( object sender, EventArgs e ) {
			string strFileName = string.Empty;
			try {
				__strModel = txtModelName.Text.Trim();
				// <b>simple</b> grammar check:
				// words then end in y, tend to be ies in plural
				__strModels = ( __strModel.EndsWith( "y" ) ? __strModel.Substring( 0, __strModel.Length - 1 ) + "ies" : __strModel + "s" );
				strFileName = Path.GetTempPath() + __strModels;
				if ( rbCS.Checked ) {
					strFileName += ".cs";
				} else if ( rbVB.Checked ) {
					strFileName += ".vb";
				}

				string[][] txt = new string[__maxSize][];
				for ( int i = 0 ; i < __maxSize ; i++ ) {
					txt[i] = new string[] { txtNames[i].Text.Trim(),
											txtTypes[i].Text.Trim(),
											txtReadOnly[i].Checked.ToString() };
				}
				string s = __strProject;
				s = s.Substring( 0, s.LastIndexOf( "\\" ) );
				if ( !Directory.Exists( s + "\\Model" ) ) {
					if ( !Directory.Exists( s + "\\App_Code" ) ) {
						Directory.CreateDirectory( s + "\\App_Code" );
					}
					if ( !Directory.Exists( s + "\\App_Code\\Model" ) ) {
						Directory.CreateDirectory( s + "\\App_Code\\Model" );
					}
					s += "\\App_Code\\";
				}
				s += "Model\\";

				if ( rbCS.Checked ) {
					File.WriteAllText( strFileName, CreateCSFile( ref txt ) );
					File.Move( strFileName, s + __strModels + ".cs" );
				} else if ( rbVB.Checked ) {
					File.WriteAllText( strFileName, CreateVBFile( ref txt ) );
					File.Move( strFileName, s + __strModels + ".vb" );
				}

				frmDAL frm = new frmDAL();
				frm.Project = __strProject;
				frm.ModelData = txt;
				frm.ModelName = __strModel;
				frm.Show();
				this.Close();
			} catch ( Exception ex ) {
#if DEBUG || TRACE
				MessageBox.Show( "Error Occured:\r\n" + ex.ToString(), "Error",
								 MessageBoxButtons.OK, MessageBoxIcon.Error,
								 MessageBoxDefaultButton.Button1 );
#else
				MessageBox.Show( "Error Occured", "Error",
								 MessageBoxButtons.OK, MessageBoxIcon.Error,
								 MessageBoxDefaultButton.Button1 );
#endif
			} finally {
				if ( File.Exists( strFileName ) )
					File.Delete( strFileName );
			}
		}

		#endregion

		#region Visual Basic

		private string CreateVBFile ( ref string[][] txt ) {
			StringBuilder str = new StringBuilder( "Namespace Model\r\n\r\n", 2000 );
			str.Append( "    Public Class " + __strModel + "\r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				CreateVBField( ref txt[i][0], ref txt[i][1], ref str );
			}
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				CreateVBProperty( ref txt[i][0], ref txt[i][1], ref txt[i][2], ref str );
			}

			str.Append( "\r\n    End Class\r\n\r\n\r\n" );
			str.Append( "    Public Partial Class " + __strModels + "\r\n" );
			if ( !cbModelBase.Checked ) {
				str.Append( "        Inherits System.Collections.CollectionBase\r\n\r\n" );
				str.Append( "        Default Public ReadOnly Property Item(ByVal index As Integer) As " + __strModel + "\r\n" );
				str.Append( "            Get\r\n" );
				str.Append( "                Return List(index)\r\n" );
				str.Append( "            End Get\r\n" );
				str.Append( "        End Property\r\n\r\n" );
				str.Append( "        Public Function Add(ByVal " + __strModel + " As " + __strModel + ") As Integer\r\n" );
				str.Append( "            Return List.Add(" + __strModel + ")\r\n" );
				str.Append( "        End Function\r\n\r\n" );
			} else {
				str.Append( "        Inherits ModelBase(Of " + __strModel + ")\r\n\r\n" );
			}
			str.Append( "        Public " + ( cbModelBase.Checked ? "Shadows " : "" ) + "Function Copy() As " + __strModels + "\r\n" );
			str.Append( "            Dim model As New " + __strModels + "()\r\n" );
			str.Append( "            For Each p As " + __strModel + " In List\r\n" );
			str.Append( "                Dim m As New " + __strModel + "()\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][1] != string.Empty && !Convert.ToBoolean( txt[i][2] ) ) {
					str.Append( "                m." + txt[i][0] + " = p." + txt[i][0] + "\r\n" );
				}
			}
			str.Append( "                model.Add(m)\r\n" );
			str.Append( "            Next\r\n" );
			str.Append( "            Return model\r\n" );
			str.Append( "        End Function\r\n" );
			str.Append( "    End Class\r\n\r\n" );
			str.Append( "End Namespace" );
			return str.ToString();
		}

		private void CreateVBField ( ref string name, ref string type, ref StringBuilder str ) {
			if ( name != string.Empty ) {
				str.Append( "        Private __" + name + " As " );
				if ( type != string.Empty ) {
					str.Append( type );
				} else {
					str.Append( "Object" );
				}
				str.Append( "\r\n" );
			}
		}

		private void CreateVBProperty ( ref string name, ref string type, ref string ReadOnly, ref StringBuilder str ) {
			if ( name != string.Empty ) {
				if ( type == string.Empty ) { type = "Object"; }
				str.Append( "\r\n        Public " + ( Convert.ToBoolean( ReadOnly ) ? "ReadOnly " : "" ) + " Property " + name + " As " + type + "\r\n" );
				str.Append( "            Get\r\n" );
				str.Append( "                Return __" + name + "\r\n" );
				str.Append( "            End Get\r\n" );
				if ( !Convert.ToBoolean( ReadOnly ) ) {
					str.Append( "            Set (ByVal value As " + type + ")\r\n" );
					str.Append( "                __" + name + " = value\r\n" );
					str.Append( "            End Set\r\n" );
				}
				str.Append( "        End Property\r\n" );
			}
		}

		#endregion

		#region C#

		private string CreateCSFile ( ref string[][] txt ) {
			StringBuilder str = new StringBuilder( "namespace Model {\r\n\r\n", 2000 );
			str.Append( "    public class " + __strModel + " {\r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				CreateCSField( ref txt[i][0], ref txt[i][1], ref str );
			}
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				CreateCSProperty( ref txt[i][0], ref txt[i][1], ref txt[i][2], ref str );
			}

			str.Append( "\r\n    }\r\n\r\n\r\n" );
			str.Append( "    public partial class " + __strModels );
			if ( !cbModelBase.Checked ) {
				str.Append( " : System.Collections.CollectionBase {\r\n\r\n" );
				str.Append( "        public " + __strModel + " this[int index] {\r\n" );
				str.Append( "            get { return (" + __strModel + ")List[index]; }\r\n" );
				str.Append( "        }\r\n\r\n" );
				str.Append( "        public int Add(" + __strModel + " " + __strModel + ") {\r\n" );
				str.Append( "            return List.Add(" + __strModel + ");\r\n" );
				str.Append( "        }\r\n\r\n" );
			} else {
				str.Append( " : ModelBase<" + __strModel + "> {\r\n\r\n" );
			}
			str.Append( "        public " + ( cbModelBase.Checked ? "new " : "" ) + __strModels + " Copy() {\r\n" );
			str.Append( "            " + __strModels + " model = new " + __strModels + "();\r\n" );
			str.Append( "            foreach ( " + __strModel + " r in List ) {\r\n" );
			str.Append( "                " + __strModel + " m = new " + __strModel + "();\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0] != string.Empty && !Convert.ToBoolean( txt[i][2] ) ) {
					str.Append( "                m." + txt[i][0] + " = r." + txt[i][0] + ";\r\n" );
				}
			}
			str.Append( "                model.Add( m );\r\n" );
			str.Append( "            }\r\n" );
			str.Append( "            return model;\r\n" );
			str.Append( "        }\r\n\r\n" );
			str.Append( "    }\r\n\r\n" );
			str.Append( "}" );
			return str.ToString();
		}

		private void CreateCSField ( ref string name, ref string type, ref StringBuilder str ) {
			if ( name != string.Empty ) {
				if ( type == string.Empty ) { type = "object"; }
				str.Append( "        private " + type + " __" + name + ";\r\n" );
			}
		}

		private void CreateCSProperty ( ref string name, ref string type, ref string ReadOnly, ref StringBuilder str ) {
			if ( name != string.Empty ) {
				if ( type == string.Empty ) { type = "object"; }
				str.Append( "\r\n        public " + type + " " + name + " {\r\n" );
				str.Append( "            get { return __" + name + "; }\r\n" );
				if ( !Convert.ToBoolean( ReadOnly ) ) {
					str.Append( "            set { __" + name + " = value; }\r\n" );
				}
				str.Append( "        }\r\n" );
			}
		}

		#endregion

		#region Other

		private void btnAbout_Click ( object sender, EventArgs e ) {
			frmAbout frm = new frmAbout();
			frm.Show();
		}

		#endregion

	}
}