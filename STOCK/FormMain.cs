using System;
using System.Windows.Forms;

namespace STOCK
{
    public partial class FormMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private string userRole;
        private string employeeName;
        private string username;

        public FormMain(string userRole, string employeeName, string username)
        {
            InitializeComponent();
            this.userRole = userRole;
            this.employeeName = employeeName;
            this.username = username;


            barStaticItem1.Caption = this.employeeName;
            if (userRole == "Nhân viên")
            {
                dmTaiKhoan.Visible = false; // Ẩn menu Tài khoản
                dmNhaCC.Visible = false;    // Ẩn menu Nhà cung cấp
            }

            ShowHomePage();
        }

        public FormMain()
        {
            InitializeComponent();
            ShowHomePage();
        }
        uc_NhaCC uc_NhaCC;
       // uc_DVT uc_DVT;
      //  uc_NhomHH uc_NhomHH;
        uc_HangHoa uc_HangHoa;
        uc_NhanVien uc_NhanVien;
        uc_KhachHang uc_KhachHang;
        uc_NhapMua uc_NhapMua;
        uc_Xuat uc_Xuat;
        Uc_TonKho uc_TonKho;
        uc_Home uc_Home;
        uc_HomeNV uc_HomeNV;
        uc_BaoCao uc_BaoCao;
        uc_TaiKhoan uc_TaiKhoan;
        uc_BaoCao2 uc_BaoCao2;

        private void ShowHomePage()
        {
            if (userRole == "Quản lý")
            {
                if (uc_Home == null)
                {
                    uc_Home = new uc_Home();
                    uc_Home.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(uc_Home);
                }
                uc_Home.BringToFront();
            }
            else if (userRole == "Nhân viên")
            {
                if (uc_HomeNV == null)
                {
                    uc_HomeNV = new uc_HomeNV(); 
                    uc_HomeNV.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(uc_HomeNV);
                }
                uc_HomeNV.BringToFront();
            }
        }
        private void dmNhaCC_Click(object sender, EventArgs e)
        {
            if (uc_NhaCC == null)
            {
                uc_NhaCC = new uc_NhaCC();
                uc_NhaCC.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_NhaCC);
                uc_NhaCC.BringToFront();
            }
            else
                uc_NhaCC.BringToFront();
        }
     
        //private void dmDVT_Click(object sender, EventArgs e)
        //{
        //    if (uc_DVT == null)
        //    {
        //        uc_DVT = new uc_DVT();
        //        uc_DVT.Dock = DockStyle.Fill;
        //        mainContainer.Controls.Add(uc_DVT);
        //        uc_DVT.BringToFront();
        //    }
        //    else
        //        uc_DVT.BringToFront();
        //}

        //private void dmNhomHH_Click(object sender, EventArgs e)
        //{
        //    if (uc_NhomHH == null)
        //    {
        //        uc_NhomHH = new uc_NhomHH();
        //        uc_NhomHH.Dock = DockStyle.Fill;
        //        mainContainer.Controls.Add(uc_NhomHH);
        //        uc_NhomHH.BringToFront();
        //    }
        //    else
        //        uc_NhomHH.BringToFront();
        //}

        private void dmHH_Click(object sender, EventArgs e)
        {
            if (uc_HangHoa == null)
            {
                uc_HangHoa = new uc_HangHoa();
                uc_HangHoa.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_HangHoa);
                uc_HangHoa.BringToFront();
            }
            else
                uc_HangHoa.BringToFront();
        }

        private void dmNhanVien_Click(object sender, EventArgs e)
        {
            if (uc_NhanVien == null)
            {
                uc_NhanVien = new uc_NhanVien();
                uc_NhanVien.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_NhanVien);
                uc_NhanVien.BringToFront();
            }
            else
                uc_NhanVien.BringToFront();
        }

        private void dmKhachHang_Click(object sender, EventArgs e)
        {
            if (uc_KhachHang == null)
            {
                uc_KhachHang = new uc_KhachHang();
                uc_KhachHang.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_KhachHang);
                uc_KhachHang.BringToFront();
            }
            else
                uc_KhachHang.BringToFront();
        }

        private void dmHangHoa_Click(object sender, EventArgs e)
        {
            if (uc_HangHoa == null)
            {
                uc_HangHoa = new uc_HangHoa();
                uc_HangHoa.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_HangHoa);
                uc_HangHoa.BringToFront();
            }
            else
                uc_HangHoa.BringToFront();
        }

        private void dmNhapMua_Click(object sender, EventArgs e)
        {
            if (uc_NhapMua == null)
            {
                uc_NhapMua = new uc_NhapMua(userRole);
                uc_NhapMua.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_NhapMua);
                uc_NhapMua.BringToFront();
            }
            else
                uc_NhapMua.BringToFront();
        }

        private void dmXuat_Click(object sender, EventArgs e)
        {
            if (uc_Xuat == null)
            {
                uc_Xuat = new uc_Xuat(userRole);
                uc_Xuat.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_Xuat);
                uc_Xuat.BringToFront();
            }
            else
                uc_Xuat.BringToFront();
        }


        private void accordionControlElement4_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void accordionControlElement5_Click(object sender, EventArgs e)
        {
            // Kiểm tra vai trò của người dùng
            if (userRole == "Quản lý") // Nếu là quản lý
            {
                if (uc_Home == null)
                {
                    uc_Home = new uc_Home();
                    uc_Home.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(uc_Home);
                    uc_Home.BringToFront();
                }
                else
                {
                    uc_Home.BringToFront();
                }
            }
            else if (userRole == "Nhân viên") // Nếu là nhân viên
            {
                if (uc_HomeNV == null) 
                {
                    uc_HomeNV = new uc_HomeNV();
                    uc_HomeNV.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(uc_HomeNV);
                    uc_HomeNV.BringToFront();
                }
                else
                {
                    uc_HomeNV.BringToFront();
                }
            }
        }


        private void taTonKho_Click(object sender, EventArgs e)
        {
            if (uc_TonKho == null)
            {
                uc_TonKho = new Uc_TonKho(employeeName);
                uc_TonKho.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_TonKho);
                uc_TonKho.BringToFront();
            }
            else
                uc_TonKho.BringToFront();
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            if (uc_BaoCao == null)
            {
                uc_BaoCao = new uc_BaoCao(employeeName);
                uc_BaoCao.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_BaoCao);
                uc_BaoCao.BringToFront();
            }
            else
                uc_BaoCao.BringToFront();
        }

        private void dmTaiKhoan_Click(object sender, EventArgs e)
        {
            if (uc_TaiKhoan == null)
            {
                uc_TaiKhoan = new uc_TaiKhoan();
                uc_TaiKhoan.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_TaiKhoan);
                uc_TaiKhoan.BringToFront();
            }
            else
                uc_TaiKhoan.BringToFront();
        }


        private void barStaticItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDoiMK frm = new frmDoiMK(employeeName, username);

            frm.ShowDialog();
        }

        private void accordionControlElement7_Click(object sender, EventArgs e)
        {
            if (uc_BaoCao2 == null)
            {
                uc_BaoCao2 = new uc_BaoCao2(employeeName);
                uc_BaoCao2.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(uc_BaoCao2);
                uc_BaoCao2.BringToFront();
            }
            else
                uc_BaoCao2.BringToFront();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
               
                this.Hide();

           
                frmLogin loginForm = new frmLogin();
                loginForm.Show();

            
                this.Close();
            }
        }
    }
    
}
