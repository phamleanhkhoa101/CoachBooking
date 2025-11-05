using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.User
{
	public partial class FrmMainUser : Form
	{
		private TaiKhoan taiKhoan;

		public FrmMainUser()
		{
			InitializeComponent();
		}

		public FrmMainUser(TaiKhoan taiKhoan)
		{
			this.taiKhoan = taiKhoan;
		}
	}
}
