using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TierMaker {
	public partial class frmDAL : Form {


		#region Variables

		private string __strProject;
		private string __strBLL;
		private string __strDAL;
		private string __strModel;
		private string __strModel2;
		private string __strModel3;
		private string __strModel4;
		private string[][] __ModelData;
		private const int __maxSize = 4;
		private const int __maxParams = 3;
		private string[] __procedures;
		private string[][] __fields;
		private string[][] __parameters;
		private bool __modelMade = false;
		private StringBuilder rs = new StringBuilder( 100 );

		#endregion

		#region Forms

		public string Project {
			get { return __strProject; }
			set { __strProject = value; }
		}

		public frmDAL () {
			InitializeComponent();
		}

		private void btnAbout_Click ( object sender, EventArgs e ) {
			frmAbout frm = new frmAbout();
			frm.Show();
		}

		private void btnDone_Click ( object sender, EventArgs e ) {
			string strFileName = string.Empty;
			try {
				__strBLL = txtDALName.Text.Trim();
				__strDAL = __strBLL;
				strFileName = Path.GetTempPath() + __strBLL;
				if ( rbCS.Checked ) {
					strFileName += ".cs";
				} else if ( rbVB.Checked ) {
					strFileName += ".vb";
				}

				string[][][] txt = new string[__maxSize][][];
				for ( int i = 0 ; i < __maxSize ; i++ ) {
					string[] p = new string[__maxParams], t = new string[__maxParams];
					for ( int j = 0 ; j < __maxParams ; j++ ) {
						p[j] = txtParams[i][j].Text.Trim();
						t[j] = txtParamTypes[i][j].Text.Trim();
					}
					string[] k = { txtTypes[i].Text.Trim() };
					string[] n = { txtNames[i].Text.Trim() };
					txt[i] = new string[4][];
					txt[i][0] = n;
					txt[i][1] = k;
					txt[i][2] = p;
					txt[i][3] = t;
				}

				string s = __strProject;
				s = s.Substring( 0, s.LastIndexOf( "\\" ) );
				if ( !Directory.Exists( s + "\\BLL" ) ) {
					if ( !Directory.Exists( s + "\\App_Code" ) ) {
						Directory.CreateDirectory( s + "\\App_Code" );
					}
					if ( !Directory.Exists( s + "\\App_Code\\BLL" ) ) {
						Directory.CreateDirectory( s + "\\App_Code\\BLL" );
					}
					if ( !Directory.Exists( s + "\\App_Code\\DAL" ) ) {
						Directory.CreateDirectory( s + "\\App_Code\\DAL" );
					}
					s += "\\App_Code\\";
				}

				if ( rbCS.Checked ) {
					File.WriteAllText( strFileName, CreateCSBLL( ref txt ) );
					File.Move( strFileName, s + "BLL\\" + __strBLL + ".cs" );
					File.WriteAllText( strFileName, CreateCSDAL( ref txt ) );
					File.Move( strFileName, s + "DAL\\" + __strDAL + ".cs" );
				} else if ( rbVB.Checked ) {
					File.WriteAllText( strFileName, CreateVBBLL( ref txt ) );
					File.Move( strFileName, s + "BLL\\" + __strBLL + ".vb" );
					File.WriteAllText( strFileName, CreateVBDAL( ref txt ) );
					File.Move( strFileName, s + "DAL\\" + __strDAL + ".vb" );
				}
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
			// close the entire application, including the frmFirst
			Application.Exit();
		}

		private void frmDAL_Load ( object sender, EventArgs e ) {
			__procedures = new string[__maxSize];
			__fields = new string[__maxSize][];
			__parameters = new string[__maxSize][];
			txtParams = new TextBox[__maxSize][];
			txtParamTypes = new ComboBox[__maxSize][];
			txtNames = new TextBox[__maxSize];
			txtTypes = new ComboBox[__maxSize];
			strTypes = new object[] { "Boolean", "Byte", "Char", "Decimal", "Double", "Float", 
									  "Integer", "Long", "Object", "Short", "String" };

			TextBox txt;
			ComboBox cmb;
			TextBox[] par;
			ComboBox[] typ;

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				txt = new TextBox();
				txt.Location = new System.Drawing.Point( 17, GetY( i ) );
				txt.Size = new System.Drawing.Size( 191, 20 );
				txt.TabIndex = i * ( __maxParams * 2 + 2 );
				this.Controls.Add( txt );
				txtNames[i] = txt;

				cmb = new ComboBox();
				cmb.FormattingEnabled = true;
				cmb.Location = new System.Drawing.Point( 219, GetY( i ) );
				cmb.Size = new System.Drawing.Size( 191, 20 );
				cmb.TabIndex = i * ( __maxParams * 2 + 2 ) + 1;
				cmb.Items.AddRange( strTypes );
				this.Controls.Add( cmb );
				txtTypes[i] = cmb;

				par = new TextBox[__maxParams];
				typ = new ComboBox[__maxParams];
				TextBox par2;
				ComboBox typ2;
				for ( int j = 0 ; j < __maxParams ; j++ ) {
					par2 = new TextBox();
					par2.Location = new System.Drawing.Point( 421, GetY2( i, j ) );
					par2.Size = new System.Drawing.Size( 80, 20 );
					par2.TabIndex = i * ( __maxParams * 2 + 2 ) + 2 + j;
					this.Controls.Add( par2 );
					par[j] = par2;

					typ2 = new ComboBox();
					typ2.Location = new System.Drawing.Point( 512, GetY2( i, j ) );
					typ2.Size = new System.Drawing.Size( 80, 20 );
					typ2.TabIndex = i * ( __maxParams * 2 + 2 ) + 2 + j;
					typ2.Items.AddRange( strTypes );
					this.Controls.Add( typ2 );
					typ[j] = typ2;
				}
				txtParams[i] = par;
				txtParamTypes[i] = typ;
			}
		}

		public string ModelName {
			get { return __strModel; }
			set {
				__strModel = value;
				if ( !__strModel.Contains( "." ) )
					__strModel = "Model." + __strModel;
				__strModel2 = value;
				if ( value.EndsWith( "y" ) ) {
					__strModel3 = __strModel3.Substring( __strModel3.Length - 2 ) + "ies";
				} else {
					__strModel3 = __strModel2 + "s";
				}
				__strModel4 = __strModel3;
				if ( !__strModel3.Contains( "." ) )
					__strModel3 = "Model." + __strModel3;
			}
		}

		public string[][] ModelData {
			get { return __ModelData; }
			set {
				string[][] r = value;
				string[][] val;
				int count = 0;
				int[] c = new int[r.Length];
				for ( int i = 0 ; i < r.Length ; i++ ) {
					c[i] = 0;
					for ( int j = 0 ; j < r[i].Length - 1 ; j++ ) {
						if ( r[i][j] != string.Empty ) {
							if ( j == 1 ) count++;
							c[i]++;
						}
					}
				}

				val = new string[count][];
				int e = 0;
				for ( int i = 0 ; i < r.Length ; i++ ) {
					if ( c[i] != 0 ) {
						val[e] = new string[c[i]];
						int d = 0;
						for ( int j = 0 ; j < r[i].Length - 1 ; j++ ) {
							if ( r[i][j] != string.Empty ) {
								val[e][d] = r[i][j];
								d++;
							}
						}
						e++;
					}
					if ( e == count ) break;
				}

				__ModelData = val;
			}
		}

		#endregion

		// I really wish I could remember wtf I was doing ...
		// But hey, it works, so who gives a shit?

		#region VB

		private string CreateVBBLL ( ref string[][][] txt ) {
			StringBuilder s = new StringBuilder( "Imports System\r\n\r\nNamespace BLL \r\n\r\n", 2000 );

			s.Append( "    Public Class " + __strBLL + " \r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;

				int c = 0;
				string ret = txt[i][1][0] == string.Empty ? "Sub" : txt[i][1][0];
				s.Append( "        Public Shared " );
				if ( ret != "Sub" ) {
					s.Append( "Function " );
				} else {
					s.Append( "Sub " );
				}
				s.Append( txt[i][0][0] + " ( " );

				for ( int j = 0 ; j < __maxParams ; j++ ) {
					if ( txt[i][2][j] != string.Empty ) {
						if ( c != 0 ) s.Append( ", " );
						string t = txt[i][3][j] == string.Empty ? "Object" : txt[i][3][j];
						if ( t == __strModel2 ) {
							s.Append( __strModel + " " + txt[i][2][j] );
						} else if ( t == __strModel4 ) {
							s.Append( __strModel3 + " " + txt[i][2][j] );
						} else {
							s.Append( "ByVal " + txt[i][2][j] + " As " + t );
						}
						c++;
					}
				}
				c = 0;

				s.Append( ") " );
				if ( ret == __strModel2 ) {
					ret = __strModel;
				} else if ( ret == __strModel4 ) {
					ret = __strModel3;
				}
				if ( ret != "Sub" ) s.Append( "As " + ret );
				s.Append( "\r\n            " );
				if ( ret != "Sub" ) s.Append( "Return " );
				s.Append( "DAL." + __strDAL + "." + txt[i][0][0] + "( " );
				for ( int j = 0 ; j < __maxParams ; j++ ) {
					if ( txt[i][2][j] != string.Empty ) {
						if ( c != 0 ) s.Append( ", " );
						s.Append( txt[i][2][j] );
						c++;
					}
				}
				s.Append( ")\r\n" );
				s.Append( "        End " );
				if ( ret == "Sub" ) {
					s.Append( "Sub" );
				} else {
					s.Append( "Function" );
				}
				s.Append( "\r\n\r\n" );
			}
			s.Append( "    End Class\r\n" );
			s.Append( "End Namespace" );
			return s.ToString();
		}

		private string CreateVBDAL ( ref string[][][] txt ) {
			StringBuilder s = new StringBuilder( "Imports System\r\nImports System.Data\r\n", 2000 );

			s.Append( "Imports System.Data.SqlClient\r\n\r\n" );
			s.Append( "Namespace DAL \r\n\r\n" );
			s.Append( "    Public Class " + __strDAL + "\r\n        Inherits DALBase\r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;
				CreateVBProcedure( ref txt[i][0][0], ref __procedures[i], ref s );
			}
			//s.Append( "\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][1][0] == string.Empty ) continue;
				CreateVBFields( ref txt[i][1][0], ref __fields[i], ref s );
			}
			s.Append( "\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][2][0] == string.Empty ) continue;
				CreateVBParameters( ref txt[i][2], ref __parameters[i], ref s );
			}
			s.Append( "\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;
				CreateVBFunction( ref txt[i], ref s, ref i );
			}

			s.Append( rs.ToString() );
			s.Append( "    End Class\r\n" );
			s.Append( "End Namespace\r\n" );

			return s.ToString();
		}

		private void CreateVBProcedure ( ref string txt, ref string procedure, ref StringBuilder s ) {
			procedure = "STP";
			string[] parts = new string[txt.Length / 2 + 1];
			bool prev = false;
			int count = -1;

			for ( int i = 0 ; i < txt.Length ; i++ ) {
				if ( i == 0 || txt[i].ToString().ToUpper() == txt[i].ToString() ) {
					if ( !prev ) {
						count++;
						prev = true;
					}
				} else {
					prev = false;
				}

				parts[count] += txt[i].ToString().ToUpper();
			}

			for ( int i = 0 ; i <= count ; i++ ) {
				procedure += "_" + parts[i];
			}

			s.Append( "        Private Const " );
			s.Append( procedure + " As String = \"stp" + txt + "\"\r\n" );
		}

		private void CreateVBFields ( ref string txt, ref string[] fields, ref StringBuilder s ) {
			if ( txt == string.Empty ) {
				fields = new string[0];
				return;
			}
			if ( txt != __strModel2 && txt != __strModel4 ) {
				fields = new string[1];
				fields[0] = "FLD";
				string[] parts = new string[txt.Length / 2 + 1];
				bool prev = false;
				int count = -1;

				for ( int i = 0 ; i < txt.Length ; i++ ) {
					if ( i == 0 || txt[i].ToString().ToUpper() == txt[i].ToString() ) {
						if ( !prev ) {
							count++;
							prev = true;
						}
					} else {
						prev = false;
					}

					parts[count] += txt[i].ToString().ToUpper();
				}

				for ( int i = 0 ; i <= count ; i++ ) {
					fields[0] += "_" + parts[i];
				}

				s.Append( "        Private Const " );
				s.Append( fields[0] + " As String = \"" + txt + "\"\r\n" );
			} else {
				if ( __modelMade ) return;
				s.Append( "\r\n" );
				__modelMade = true;
				fields = new string[__ModelData.Length];
				for ( int i = 0 ; i < __ModelData.Length ; i++ ) {
					fields[i] = "FLD";
					string[] parts = new string[__ModelData[i][0].Length / 2 + 1];
					bool prev = false;
					int count = -1;

					for ( int j = 0 ; j < __ModelData[i][0].Length ; j++ ) {
						if ( j == 0 || __ModelData[i][0][j].ToString().ToUpper() == __ModelData[i][0][j].ToString() ) {
							if ( !prev ) {
								count++;
								prev = true;
							}
						} else {
							prev = false;
						}

						parts[count] += __ModelData[i][0][j].ToString().ToUpper();
					}

					for ( int j = 0 ; j <= count ; j++ ) {
						fields[i] += "_" + parts[j];
					}

					s.Append( "        Private Const " );
					s.Append( fields[i] + " As String = \"" + __ModelData[i][0] + "\"\r\n" );
				}
				CreateVBData2Model( ref __ModelData, ref rs, ref fields );
			}
		}

		private void CreateVBData2Model ( ref string[][] txt, ref StringBuilder s, ref string[] fields ) {
			s.Append( "\r\n        Private Shared Function " );
			s.Append( " Data2" + __strModel2 + "( ByVal dr As SqlDataReader ) As " );
			s.Append( __strModel + "\r\n" );
			s.Append( "            Dim " + __strModel2 + " As " + __strModel + " = New " );
			s.Append( __strModel + "()\r\n" );
			for ( int i = 0 ; i < txt.Length ; i++ ) {
				if ( cbDALBase.Checked ) {
					s.Append( "            " + __strModel2 + "." + txt[i][0] + " = CS" );
					switch ( txt[i][1].ToLower() ) {
						case "integer":
						case "int":
							s.Append( "Int" );
							break;
						case "double":
							s.Append( "Dbl" );
							break;
						case "string":
							s.Append( "Str" );
							break;
						case "date":
							s.Append( "Date" );
							break;
						case "single":
							s.Append( "Sng" );
							break;
					}
				} else {
					s.Append( "            if Not (IsDBNull(dr(" + fields[i] );
					s.Append( "))) Then " + __strModel2 + "." + txt[i][0] + " = " );
					switch ( txt[i][1].ToLower() ) {
						case "integer":
						case "int":
							s.Append( "CInt" );
							break;
						case "double":
							s.Append( "CDbl" );
							break;
						case "string":
							s.Append( "CStr" );
							break;
						case "date":
							s.Append( "CDate" );
							break;
						case "single":
							s.Append( "CSng" );
							break;
					}
				}
				s.Append( "(dr(" + fields[i] + "))\r\n" );
			}
			s.Append( "            Return " + __strModel2 + "\r\n" );
			s.Append( "        End Function\r\n\r\n" );
		}

		private void CreateVBParameters ( ref string[] txt, ref string[] parameters, ref StringBuilder s ) {
			parameters = new string[__maxParams];

			for ( int i = 0 ; i < __maxParams ; i++ ) {
				if ( txt[i] == string.Empty ) continue;
				parameters[i] = "PAR";
				string[] parts = new string[txt[i].Length / 2 + 1];
				bool prev = false;
				int count = -1;

				for ( int j = 0 ; j < txt[i].Length ; j++ ) {
					if ( j == 0 || txt[i][j].ToString().ToUpper() == txt[i][j].ToString() ) {
						if ( !prev ) {
							count++;
							prev = true;
						}
					} else {
						prev = false;
					}

					parts[count] += txt[i][j].ToString().ToUpper();
				}

				for ( int j = 0 ; j <= count ; j++ ) {
					parameters[i] += "_" + parts[j];
				}

				s.Append( "        Private Const " );
				s.Append( parameters[i] + " As String = \"@" + txt[i] + "\"\r\n" );
			}
		}

		private void CreateVBFunction ( ref string[][] txt, ref StringBuilder s, ref int i ) {
			int c = 0;
			string ret = txt[1][0] == string.Empty ? "Sub" : txt[1][0];
			string ret2;
			string[] zet = new string[__maxParams];
			if ( ret == __strModel2 ) ret = __strModel;
			ret2 = ret;
			if ( ret2 == __strModel4 ) ret2 = __strModel3;
			s.Append( "        Public Shared " );
			if ( ret == "Sub" ) {
				s.Append( "Sub " );
			} else {
				s.Append( "Function " );
			}
			s.Append( txt[0][0] + " ( " );

			for ( int j = 0 ; j < __maxParams ; j++ ) {
				if ( txt[2][j] != string.Empty ) {
					if ( c != 0 ) s.Append( ", " );
					string t = txt[3][j] == string.Empty ? "Object" : txt[3][j];
					if ( t == __strModel2 ) {
						s.Append( "ByVal " + txt[2][j] + " As " + __strModel );
					} else if ( t == __strModel4 ) {
						s.Append( "ByVal " + txt[2][j] + " As " + __strModel3 );
					} else {
						s.Append( "ByVal " + txt[2][j] + " As " + t );
					}
					c++;
					zet[j] = t;
				}
			}
			c = 0;

			s.Append( " ) " );
			if ( ret != "Sub" ) {
				s.Append( "As " + ret2 );
			}
			s.Append( "\r\n" );
			s.Append( "            Dim sqlConn As New SqlConnection( GetConnectionString() )\r\n" );
			s.Append( "            Dim sqlCmd As new SqlCommand( " );
			s.Append( __procedures[i] + ", sqlConn )\r\n" );

			if ( ret != "Sub" ) {
				s.Append( "            Dim dr As SqlDataReader = Nothing\r\n" );
				if ( ret == __strModel2 ) {
					s.Append( "            Dim " + __strModel + " As" );
					s.Append( " New " + __strModel2 + "()\r\n" );
				} else if ( ret == __strModel4 ) {
					s.Append( "            Dim " + __strModel4 + " As" );
					s.Append( " New " + __strModel3 + "()\r\n" );
				}
			}
			s.Append( "\r\n" );
			s.Append( "            sqlCmd.CommandType = CommandType.StoredProcedure\r\n" );

			for ( int j = 0 ; __parameters[i] != null && j < __parameters[i].Length ; j++ ) {
				if ( __parameters[i][j] == null ) continue;
				s.Append( "            sqlCmd.Parameters.Add( CreateParameter( " );
				s.Append( __parameters[i][j] + ", SqlDbType." );
				switch ( zet[j].ToLower() ) {
					case "integer":
					case "int":
					case "single":
						s.Append( "Int" );
						break;
					case "string":
						s.Append( "VarChar" );
						break;
					case "double":
						s.Append( "Float" );
						break;
					case "boolean":
						s.Append( "Bit" );
						break;
				}
				s.Append( ", " + txt[2][j] );
				if ( zet[j].ToLower() == "string" )
					s.Append( ", 50" );
				s.Append( " ) )\r\n" );
			}
			s.Append( "\r\n" );
			s.Append( "            Try \r\n" );
			s.Append( "                sqlConn.Open()\r\n" );
			s.Append( "                " );
			if ( ret != "Sub" ) {
				s.Append( "dr = sqlCmd.ExecuteReader()\r\n" );
				s.Append( "                Do While ( dr.Read() ) \r\n" );
				s.Append( "                    " );
				if ( ret == __strModel ) {
					s.Append( "Return Data2" + __strModel2 + "( dr )\r\n" );
				} else if ( ret == __strModel4 ) {
					s.Append( __strModel4 + ".Add( Data2" + __strModel2 + "( dr ) )\r\n" );
				} else {
					s.Append( "Return dr(" + __fields[i][0] + ")\r\n" );
				}
				s.Append( "                Loop\r\n" );
				if ( ret == __strModel4 )
					s.Append( "                Return " + __strModel4 + "\r\n" );
			} else {
				s.Append( "sqlCmd.ExecuteNonQuery()\r\n" );
			}
			s.Append( "            Catch ex As Exception\r\n" );
			s.Append( "                Throw ex\r\n" );
			s.Append( "            Finally \r\n" );
			s.Append( "                If ( sqlConn.State <> ConnectionState.Closed ) Then _\r\n" );
			s.Append( "                    sqlConn.Close()\r\n" );
			if ( ret != "Sub" ) {
				s.Append( "                if ( Not IsNothing(dr) ) Then _\r\n" );
				s.Append( "                    dr.Close()\r\n" );
			}
			s.Append( "            End Try\r\n" );
			if ( ret != "Sub" ) {
				s.Append( "            Return Nothing\r\n" );
			}
			s.Append( "        End " );
			if ( ret == "Sub" ) {
				s.Append( "Sub" );
			} else {
				s.Append( "Function" );
			}
			s.Append( "\r\n\r\n" );
		}

		#endregion

		#region CS

		private string CreateCSBLL ( ref string[][][] txt ) {
			StringBuilder s = new StringBuilder( "using System;\r\n\r\nnamespace BLL {\r\n\r\n", 2000 );

			s.Append( "    public class " + __strBLL + " {\r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;

				int c = 0;
				string ret = txt[i][1][0] == string.Empty ? "void" : txt[i][1][0];
				if ( ret == __strModel2 ) {
					ret = __strModel;
				} else if ( ret == __strModel4 ) {
					ret = __strModel3;
				} 
				s.Append( "        public static " + ret + " " + txt[i][0][0] + " ( " );

				for ( int j = 0 ; j < __maxParams ; j++ ) {
					if ( txt[i][2][j] != string.Empty ) {
						if ( c != 0 ) s.Append( ", " );
						string t = txt[i][3][j] == string.Empty ? "object" : txt[i][3][j];
						if ( t == __strModel2 ) {
							s.Append( __strModel + " " + txt[i][2][j] );
						} else if ( t == __strModel4 ) {
							s.Append( __strModel3 + " " + txt[i][2][j] );
						} else {
							s.Append( t + " " + txt[i][2][j] );
						}
						c++;
					}
				}
				c = 0;

				s.Append( ") {\r\n" );
				s.Append( "            " );
				if ( ret != "void" ) s.Append( "return " );
				s.Append( "DAL." + __strDAL + "." + txt[i][0][0] + "( " );
				for ( int j = 0 ; j < __maxParams ; j++ ) {
					if ( txt[i][2][j] != string.Empty ) {
						if ( c != 0 ) s.Append( ", " );
						s.Append( txt[i][2][j] );
						c++;
					}
				}
				s.Append( ");\r\n" );
				s.Append( "        }\r\n\r\n" );
			}
			s.Append( "    }\r\n" );
			s.Append( "}" );
			return s.ToString();
		}

		private string CreateCSDAL ( ref string[][][] txt ) {
			StringBuilder s = new StringBuilder( "using System;\r\nusing System.Data;\r\n", 2000 );

			s.Append( "using System.Data.SqlClient;\r\n\r\n" );
			s.Append( "namespace DAL {\r\n\r\n" );
			s.Append( "    public class " + __strDAL + " : DALBase {\r\n\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;
				CreateCSProcedure( ref txt[i][0][0], ref __procedures[i], ref s );
			}
			//s.Append( "\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][1][0] == string.Empty ) continue;
				CreateCSFields( ref txt[i][1][0], ref __fields[i], ref s );
			}
			s.Append( "\r\n" );
			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][2][0] == string.Empty ) continue;
				CreateCSParameters( ref txt[i][2], ref __parameters[i], ref s );
			}
			s.Append( "\r\n" );

			for ( int i = 0 ; i < __maxSize ; i++ ) {
				if ( txt[i][0][0] == string.Empty ) continue;
				CreateCSFunction( ref txt[i], ref s, ref i );
			}

			s.Append( rs.ToString() );
			s.Append( "    }\r\n" );
			s.Append( "}\r\n" );

			return s.ToString();
		}

		private void CreateCSProcedure ( ref string txt, ref string procedure, ref StringBuilder s ) {
			procedure = "STP";
			string[] parts = new string[txt.Length / 2 + 1];
			bool prev = false;
			int count = -1;

			for ( int i = 0 ; i < txt.Length ; i++ ) {
				if ( i == 0 || txt[i].ToString().ToUpper() == txt[i].ToString() ) {
					if ( !prev ) {
						count++;
						prev = true;
					}
				} else {
					prev = false;
				}

				parts[count] += txt[i].ToString().ToUpper();
			}

			for ( int i = 0 ; i <= count ; i++ ) {
				procedure += "_" + parts[i];
			}

			s.Append( "        private const string " );
			s.Append( procedure + " = \"stp" + txt + "\";\r\n" );
		}

		private void CreateCSFields ( ref string txt, ref string[] fields, ref StringBuilder s ) {
			if ( txt == string.Empty ) {
				fields = new string[0];
				return;
			}
			if ( txt != __strModel2 && txt != __strModel4 ) {
				fields = new string[1];
				fields[0] = "FLD";
				string[] parts = new string[txt.Length / 2 + 1];
				bool prev = false;
				int count = -1;

				for ( int i = 0 ; i < txt.Length ; i++ ) {
					if ( i == 0 || txt[i].ToString().ToUpper() == txt[i].ToString() ) {
						if ( !prev ) {
							count++;
							prev = true;
						}
					} else {
						prev = false;
					}

					parts[count] += txt[i].ToString().ToUpper();
				}

				for ( int i = 0 ; i <= count ; i++ ) {
					fields[0] += "_" + parts[i];
				}

				s.Append( "        private const string " );
				s.Append( fields[0] + " = \"" + txt + "\";\r\n" );
			} else {
				if ( __modelMade ) return;
				s.Append( "\r\n" );
				__modelMade = true;
				fields = new string[__ModelData.Length];
				for ( int i = 0 ; i < __ModelData.Length ; i++ ) {
					fields[i] = "FLD";
					string[] parts = new string[__ModelData[i][0].Length / 2 + 1];
					bool prev = false;
					int count = -1;

					for ( int j = 0 ; j < __ModelData[i][0].Length ; j++ ) {
						if ( j == 0 || __ModelData[i][0][j].ToString().ToUpper() == __ModelData[i][0][j].ToString() ) {
							if ( !prev ) {
								count++;
								prev = true;
							}
						} else {
							prev = false;
						}

						parts[count] += __ModelData[i][0][j].ToString().ToUpper();
					}

					for ( int j = 0 ; j <= count ; j++ ) {
						fields[i] += "_" + parts[j];
					}

					s.Append( "        private const string " );
					s.Append( fields[i] + " = \"" + __ModelData[i][0] + "\";\r\n" );
				}
				CreateCSData2Model( ref __ModelData, ref rs, ref fields );
			}
		}

		private void CreateCSData2Model ( ref string[][] txt, ref StringBuilder s, ref string[] fields ) {
			s.Append( "\r\n        private static " );
			s.Append( __strModel );
			s.Append( " Data2" + __strModel2 + "( SqlDataReader dr ) {\r\n" );
			s.Append( "            " + __strModel + " " + __strModel2 + " = new " );
			s.Append( __strModel + "();\r\n" );
			for ( int i = 0 ; i < txt.Length ; i++ ) {
				if ( cbDALBase.Checked ) {
					s.Append( "            " + __strModel2 + "." + txt[i][0] + " = CS" );
					switch ( txt[i][1].ToLower() ) {
						case "integer":
						case "int":
							s.Append( "Int" );
							break;
						case "double":
							s.Append( "Dbl" );
							break;
						case "string":
							s.Append( "Str" );
							break;
						case "date":
							s.Append( "Date" );
							break;
						case "single":
							s.Append( "Sng" );
							break;
					}
				} else {
					s.Append( "            if (!Convert.IsDBNull(dr[" + fields[i] );
					s.Append( "])) " + __strModel2 + "." + txt[i][0] + " = " );
					switch ( txt[i][1].ToLower() ) {
						case "integer":
						case "int":
							s.Append( "Convert.ToInt32" );
							break;
						case "double":
							s.Append( "Convert.ToDouble" );
							break;
						case "string":
							s.Append( "Convert.ToString" );
							break;
						case "date":
							s.Append( "Convert.ToDate" );
							break;
						case "single":
							s.Append( "Convert.ToSingle" );
							break;
					}
				}
				s.Append( "(dr[" + fields[i] + "]);\r\n" );
			}
			s.Append( "            return " + __strModel2 + ";\r\n" );
			s.Append( "        }\r\n\r\n" );
		}

		private void CreateCSParameters ( ref string[] txt, ref string[] parameters, ref StringBuilder s ) {
			parameters = new string[__maxParams];

			for ( int i = 0 ; i < __maxParams ; i++ ) {
				if ( txt[i] == string.Empty ) continue;
				parameters[i] = "PAR";
				string[] parts = new string[txt[i].Length / 2 + 1];
				bool prev = false;
				int count = -1;

				for ( int j = 0 ; j < txt[i].Length ; j++ ) {
					if ( j == 0 || txt[i][j].ToString().ToUpper() == txt[i][j].ToString() ) {
						if ( !prev ) {
							count++;
							prev = true;
						}
					} else {
						prev = false;
					}

					parts[count] += txt[i][j].ToString().ToUpper();
				}

				for ( int j = 0 ; j <= count ; j++ ) {
					parameters[i] += "_" + parts[j];
				}

				s.Append( "        private const string " );
				s.Append( parameters[i] + " = \"@" + txt[i] + "\";\r\n" );
			}
		}

		private void CreateCSFunction ( ref string[][] txt, ref StringBuilder s, ref int i ) {
			int c = 0;
			string ret = txt[1][0] == string.Empty ? "void" : txt[1][0];
			string ret2;
			string[] zet = new string[__maxParams];
			if ( ret == __strModel2 ) ret = __strModel;
			ret2 = ret;
			if ( ret2 == __strModel4 ) ret2 = __strModel3;
			s.Append( "        public static " + ret2 + " " + txt[0][0] + " ( " );

			for ( int j = 0 ; j < __maxParams ; j++ ) {
				if ( txt[2][j] != string.Empty ) {
					if ( c != 0 ) s.Append( ", " );
					string t = txt[3][j] == string.Empty ? "object" : txt[3][j];
					if ( t == __strModel2 ) {
						s.Append( __strModel + " " + txt[2][j] );
					} else if ( t == __strModel4 ) {
						s.Append( __strModel3 + " " + txt[2][j] );
					} else {
						s.Append( t + " " + txt[2][j] );
					}
					c++;
					zet[j] = t;
				}
			}
			c = 0;

			s.Append( " ) {\r\n" );
			s.Append( "            SqlConnection sqlConn = new SqlConnection( GetConnectionString() );\r\n" );
			s.Append( "            SqlCommand sqlCmd = new SqlCommand( " );
			s.Append( __procedures[i] + ", sqlConn );\r\n" );

			if ( ret != "void" ) {
				s.Append( "            SqlDataReader dr = null;\r\n" );
				if ( ret == __strModel2 ) {
					s.Append( "            " + __strModel2 + " " + __strModel );
					s.Append( " = new " + __strModel2 + "();\r\n" );
				} else if ( ret == __strModel4 ) {
					s.Append( "            " + __strModel3 + " " + __strModel4 );
					s.Append( " = new " + __strModel3 + "();\r\n" );
				}
			}
			s.Append( "\r\n" );
			s.Append( "            sqlCmd.CommandType = CommandType.StoredProcedure;\r\n" );

			for ( int j = 0 ; __parameters[i] != null && j < __parameters[i].Length ; j++ ) {
				if ( __parameters[i][j] == null ) continue;
				s.Append( "            sqlCmd.Parameters.Add( CreateParameter( " );
				s.Append( __parameters[i][j] + ", SqlDbType." );
				switch ( zet[j].ToLower() ) {
					case "integer":
					case "int":
					case "single":
						s.Append( "Int" );
						break;
					case "string":
						s.Append( "VarChar" );
						break;
					case "double":
						s.Append( "Float" );
						break;
					case "boolean":
						s.Append( "Bit" );
						break;
				}
				s.Append( ", " + txt[2][j] );
				if ( zet[j].ToLower() == "string" )
					s.Append( ", 50" );
				s.Append( " ) );\r\n" );
			}
			s.Append( "\r\n" );
			s.Append( "            try {\r\n" );
			s.Append( "                sqlConn.Open();\r\n" );
			s.Append( "                " );
			if ( ret != "void" ) {
				s.Append( "dr = sqlCmd.ExecuteReader();\r\n" );
				s.Append( "                while ( dr.Read() ) {\r\n" );
				s.Append( "                    " );
				if ( ret == __strModel ) {
					s.Append( "return Data2" + __strModel2 + "(dr);\r\n" );
				} else if ( ret == __strModel4 ) {
					s.Append( __strModel4 + ".Add( Data2" + __strModel2 + "( dr ) );\r\n" );
				} else {
					s.Append( "return dr[" + __fields[i][0] + "];\r\n" );
				}
				s.Append( "                }\r\n" );
				if ( ret == __strModel4 )
					s.Append( "                return " + __strModel4 + ";\r\n" );
			} else {
				s.Append( "sqlCmd.ExecuteNonQuery();\r\n" );
			}
			s.Append( "            } catch ( Exception ex ) {\r\n" );
			s.Append( "                throw ex;\r\n" );
			s.Append( "            } finally {\r\n" );
			s.Append( "                if ( sqlConn.State != ConnectionState.Closed )\r\n" );
			s.Append( "                    sqlConn.Close();\r\n" );
			if ( ret != "void" ) {
				s.Append( "                if ( dr != null )\r\n" );
				s.Append( "                    dr.Close();\r\n" );
			}
			s.Append( "            }\r\n" );
			if ( ret != "void" ) {
				s.Append( "            return null;\r\n" );
			}
			s.Append( "        }\r\n\r\n" );
		}

		#endregion

	}
}