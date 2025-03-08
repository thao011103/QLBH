using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STOCK
{
    public partial class frmConnect : Form
    {
        public frmConnect()
        {
            InitializeComponent();
        }
        SqlConnection GetCon(string server, string username, string pass, string database)
        {
            return new SqlConnection("Data Source=" + server + "; Initial Catalog=" + database + "; User ID=" + username + "; Password=" + pass + ";");
        }
        private void frmConnect_Load(object sender, EventArgs e)
        {

        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            SqlConnection con = GetCon(txbServer.Text, txbUsername.Text, txbPassword.Text, cboDatabase.Text);
            try
            {
                con.Open();
                MessageBox.Show("Success!");
            }
            catch (Exception)
            {
                MessageBox.Show("Unsuccess!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string svEncrypt = Encryptor.Encrypt(txbServer.Text, "qwertyuiop", true);
            string usEncrypt = Encryptor.Encrypt(txbUsername.Text, "qwertyuiop", true);
            string pasEncrypt = Encryptor.Encrypt(txbPassword.Text, "qwertyuiop", true);
            string dbEncrypt = Encryptor.Encrypt(cboDatabase.Text, "qwertyuiop", true);

            connect cn = new connect(svEncrypt, usEncrypt, pasEncrypt, dbEncrypt);
            cn.SaveFile();
            MessageBox.Show("Lưu file thành công!", "Thông báo");

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cboDatabase_Click(object sender, EventArgs e)
        {
            cboDatabase.Items.Clear();
            string conn = "server=" + txbServer.Text + "; User ID=" + txbUsername.Text + "; pwd=" + txbPassword.Text + ";";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            string qr = "SELECT NAME FROM SYS.DATABASES";
            SqlCommand cmd = new SqlCommand(qr, con);
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cboDatabase.Items.Add(dr[0].ToString());
            }
        }
    }
}
