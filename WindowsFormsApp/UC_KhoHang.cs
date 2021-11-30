﻿using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO; // MemoryStream


namespace WindowsFormsApp
{
    public partial class UC_KhoHang : UserControl
    {
        public UC_KhoHang()
        {
            InitializeComponent();
            loadData();
        }

        public void loadData()
        {
            dgv.DataSource = HangHoaBUS.Intance.getListSanPham();
            dgv.Columns[0].HeaderText = "Mã Mặt Hàng";
            dgv.Columns["DonVi"].HeaderText = "Đơn Vị Tính";
            dgv.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgv.Columns["GiaGoc"].HeaderText = "Giá Gốc";
            dgv.Columns["GiaBan"].HeaderText = "Giá Bán";
            dgv.Columns[1].HeaderText = "Tên Hàng";

            DataTable dataDVTinh = DataProvider.Instance.ExecuteQuery("select * from DonViTinh");
            cbbDVT.DataSource = dataDVTinh;
            cbbDVT.ValueMember = "MaDVT";
            cbbDVT.DisplayMember = "TenDVT";

            dgv.AllowUserToAddRows = false;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;

            pcbHangHoa.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnThemMatHangMoi_Click(object sender, EventArgs e)
        {
            FormThemSanPham tmsp = new FormThemSanPham();
            tmsp.ShowDialog();
        }


        string imgLocation = Application.StartupPath + "\\Resources\\hanghoa.png";

        public void resetData()
        {
            txtTenMH.Text = "";
            txtSoLuong.Text = "0";
            txtGiaGoc.Text = "0";
            txtGiaBan.Text = "0";
            pcbHangHoa.Image = null;
        }
        public bool check = true;

        bool KiemTraNhap()
        {
            int a;
            if (txtTenMH.Text == "")
            {
                MessageBox.Show("Hãy nhập tên hàng hóa", "Thông báo");
                txtTenMH.Focus();
                return false;
            }
            else if (cbbDVT.SelectedIndex == -1)
            {
                MessageBox.Show("Hãy chọn đơn vị tính", "Thông báo");
                cbbDVT.Focus();
                return false;
            }
            else if (!int.TryParse(txtGiaGoc.Text, out a))
            {
                MessageBox.Show("Giá gốc phải là một số", "Thông báo");
                txtGiaGoc.Focus();
                return false;
            }
            else if (!int.TryParse(txtGiaBan.Text, out a))
            {
                MessageBox.Show("Giá bán phải là một số", "Thông báo");
                txtGiaBan.Focus();
                return false;
            }
            else if (!int.TryParse(txtSoLuong.Text, out a))
            {
                MessageBox.Show("Số lượng phải là một số", "Thông báo");
                txtSoLuong.Focus();
                return false;
            }
            return true;
        }

        void Binding()
        {
            txtMaMH.DataBindings.Add(new Binding("Text", dgv.DataSource, "MaMH", true, DataSourceUpdateMode.Never));
            txtTenMH.DataBindings.Add(new Binding("Text", dgv.DataSource, "TenMH", true, DataSourceUpdateMode.Never));
            txtSoLuong.DataBindings.Add(new Binding("Text", dgv.DataSource, "SoLuong", true, DataSourceUpdateMode.Never));
            txtGiaGoc.DataBindings.Add(new Binding("Text", dgv.DataSource, "GiaGoc", true, DataSourceUpdateMode.Never));
            txtGiaBan.DataBindings.Add(new Binding("Text", dgv.DataSource, "GiaBan", true, DataSourceUpdateMode.Never));
        }

        void ClearBinding()
        {
            txtMaMH.DataBindings.Clear();
            txtTenMH.DataBindings.Clear();
            txtSoLuong.DataBindings.Clear();
            txtGiaGoc.DataBindings.Clear();
            txtGiaBan.DataBindings.Clear();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            dgv.DataSource = HangHoaBUS.Intance.searchGoods(txtTimKiem.Text);
            dgv.Columns["Anh"].Visible = false;
        }

        private void dgvHangHoa_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count > 0)
            {
                ClearBinding();
                Binding();
                DataGridViewRow row = dgv.SelectedCells[0].OwningRow;
                try
                {
                    string MaMH = row.Cells["MaMH"].Value.ToString();
                    if (HangHoaBUS.Intance.getAnhByID(MaMH) == null)
                    {
                        pcbHangHoa.Image = null;
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(HangHoaBUS.Intance.getAnhByID(MaMH));
                        pcbHangHoa.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception) { }

                cbbDVT.SelectedValue = row.Cells["DonVi"].Value;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaMH.Text = HangHoaBUS.Intance.loadIDGoods();
            if (check == true)
            {
                check = !check;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThem.Text = "Lưu";
                resetData();
                txtTenMH.Enabled = true;
                txtTenMH.Focus();
                cbbDVT.Enabled = true;
                txtSoLuong.Enabled = true;
                txtGiaBan.Enabled = true;
                txtGiaGoc.Enabled = true;
            }
            else
            {
                if (KiemTraNhap())
                {
                    check = !check;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnThem.Text = "Thêm";
                    HangHoaDTO data = new HangHoaDTO();
                    data.MaMH = txtMaMH.Text;
                    data.TenMH = txtTenMH.Text;
                    data.SoLuong = int.Parse(txtSoLuong.Text);
                    data.GiaBan = int.Parse(txtGiaBan.Text);
                    data.GiaGoc = int.Parse(txtGiaGoc.Text);
                    data.DonVi = cbbDVT.SelectedValue.ToString();
                    if (HangHoaBUS.Intance.temHH(data, imgLocation))
                    {
                        MessageBox.Show("Thêm Thành Công");
                        imgLocation = Application.StartupPath + "\\Resources\\hanghoa.png";
                        resetData();
                        cbbDVT.SelectedValue = dgv.Rows[0].Cells["DonVi"].Value;
                        loadData();
                    }
                }

            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count > 0)
            {
                if (HangHoaBUS.Intance.suaHH(txtMaMH.Text, txtTenMH.Text, (string)cbbDVT.SelectedValue, int.Parse(txtSoLuong.Text), int.Parse(txtGiaGoc.Text), int.Parse(txtGiaBan.Text)))
                {
                    if (imgLocation != Application.StartupPath + "\\Resources\\hanghoa.png")
                    {
                        HangHoaBUS.Intance.capNhatHinh(imgLocation, txtMaMH.Text);
                    }
                    loadData();
                    cbbDVT.SelectedValue = dgv.Rows[0].Cells["DonVi"].Value;
                    imgLocation = Application.StartupPath + "\\Resources\\hanghoa.png";
                    MessageBox.Show("Sửa Thành Công");
                }
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult dlr = MessageBox.Show("Bạn có muốn xóa không?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                if (HangHoaBUS.Intance.checkDelete(txtMaMH.Text))
                {
                    HangHoaBUS.Intance.deleteGoods(txtMaMH.Text);
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    loadData();
                }
                else
                {
                    MessageBox.Show("Bạn không được xóa bản ghi này!", "Thông báo");
                }

            }
        }


        // xoa
        private void guna2Button1_Click_2(object sender, EventArgs e)
        {
            check = !check;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Text = "Thêm";
            loadData();
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "PNG files(*.png)|*.png|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dlgOpen.FileName.ToString();
                pcbHangHoa.Image = Image.FromFile(dlgOpen.FileName);
            }
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            FormDonViTinh FormUnit = new FormDonViTinh(this);
            FormUnit.ShowDialog();
        }
    }
}