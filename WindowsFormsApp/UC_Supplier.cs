﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DTO;

namespace WindowsFormsApp
{
    public partial class UC_Supplier : UserControl
    {
        public UC_Supplier()
        {
            InitializeComponent();
            LoadData();
        }
        void loadBinding()
        {
            txtMaNCC.DataBindings.Add(new Binding("Text", guna2dgvThongTinNCC.DataSource, "MaNCC", true, DataSourceUpdateMode.Never));
            txtTenNCC.DataBindings.Add(new Binding("Text", guna2dgvThongTinNCC.DataSource, "TenNCC", true, DataSourceUpdateMode.Never));
            txtEmailNCC.DataBindings.Add(new Binding("Text", guna2dgvThongTinNCC.DataSource, "Email", true, DataSourceUpdateMode.Never));
            txtDienThoaiNCC.DataBindings.Add(new Binding("Text", guna2dgvThongTinNCC.DataSource, "SDT", true, DataSourceUpdateMode.Never));
            rtbDiaChiKhachNCC.DataBindings.Add(new Binding("Text", guna2dgvThongTinNCC.DataSource, "DiaChi", true, DataSourceUpdateMode.Never));
        }
        void LoadData()
        {
            guna2dgvThongTinNCC.DataSource = SupplierBUS.Intance.getListNCC();
            guna2dgvThongTinNCC.Columns["MaNCC"].HeaderText = "Mã NCC";
            guna2dgvThongTinNCC.Columns["TenNCC"].HeaderText = "Tên NCC";
            guna2dgvThongTinNCC.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            guna2dgvThongTinNCC.Columns["SDT"].HeaderText = "Số Điện Thoại";
            //   dgvNCC.Columns[5].Visible = false;
        }

        void ClearBinding()
        {
            txtMaNCC.DataBindings.Clear();
            txtTenNCC.DataBindings.Clear();
            txtEmailNCC.DataBindings.Clear();
            txtDienThoaiNCC.DataBindings.Clear();
            rtbDiaChiKhachNCC.DataBindings.Clear();
        }

        public void resetData()
        {
            txtTenNCC.Text = "";
            txtEmailNCC.Text = "";
            txtDienThoaiNCC.Text = "";
            rtbDiaChiKhachNCC.Text = "";
        }

        public bool KiemTraNhap()
        {
            if (txtTenNCC.Text == "")
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống", "Thông báo");
                txtTenNCC.Focus();
                return false;
            }
            else if (txtDienThoaiNCC.Text == "")
            {
                MessageBox.Show("Số điện thoại không được để trống", "Thông báo");
                txtDienThoaiNCC.Focus();
                return false;
            }
            else if (rtbDiaChiKhachNCC.Text == "")
            {
                MessageBox.Show("Địa chỉ không được để trống", "Thông báo");
                rtbDiaChiKhachNCC.Focus();
                return false;
            }
            else if (txtEmailNCC.Text == "")
            {
                MessageBox.Show("Địa chỉ không được để trống", "Thông báo");
                txtEmailNCC.Focus();
                return false;
            }
            return true;
        }
        public bool check = true;
        private void btnLuuNCC_Click(object sender, EventArgs e)
        {
            txtMaNCC.Text = SupplierBUS.Intance.loadMaNCC();
            if (check == true)
            {
                check = !check;
                btnSuaNCC.Enabled = false;
                btnXoaNCC.Enabled = false;
                btnLuuNCC.Text = "Lưu";
                resetData();
                txtTenNCC.Focus();
            }
            else
            {
                if (KiemTraNhap())
                {
                    check = !check;
                    btnSuaNCC.Enabled = true;
                    btnXoaNCC.Enabled = true;
                    btnLuuNCC.Text = "Thêm";
                    DTO_NhaCungCap ncc = new DTO_NhaCungCap();
                    ncc.MaNCC = txtMaNCC.Text;
                    ncc.TenNCC = txtTenNCC.Text;
                    ncc.DiaChi = rtbDiaChiKhachNCC.Text;
                    ncc.Email = txtEmailNCC.Text;
                    ncc.SDT = int.Parse(txtDienThoaiNCC.Text);
                    if (SupplierBUS.Intance.themNCC(ncc))
                    {
                        MessageBox.Show("Thêm thành công!", "Thông báo");
                        LoadData();
                    }
                }

            }
        }

        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            if (guna2dgvThongTinNCC.SelectedCells.Count > 0)
            {
                DTO_NhaCungCap ncc = new DTO_NhaCungCap();
                ncc.MaNCC = txtMaNCC.Text;
                ncc.TenNCC = txtTenNCC.Text;
                ncc.DiaChi = rtbDiaChiKhachNCC.Text;
                ncc.Email = txtEmailNCC.Text;
                ncc.SDT = int.Parse(txtDienThoaiNCC.Text);
                if (SupplierBUS.Intance.suaNCC(ncc))
                {
                    MessageBox.Show("Sửa thành công!", "Thông báo");
                    LoadData();
                }
            }

        }

        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            DialogResult dlr = MessageBox.Show("Bạn có muốn xóa không?",
            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                SupplierBUS.Intance.xoaNCC(txtMaNCC.Text);
                MessageBox.Show("Xóa thành công!", "Thông báo");
                LoadData();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            check = !check;
            btnSuaNCC.Enabled = true;
            btnXoaNCC.Enabled = true;
            btnLuuNCC.Text = "Thêm";
            LoadData();
        }

        private void txtTimKiemNCC_TextChanged(object sender, EventArgs e)
        {
            guna2dgvThongTinNCC.DataSource = SupplierBUS.Intance.TimKiemNCC(txtTimKiemNCC.Text);
        }

        private void dgvNCC_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2dgvThongTinNCC.SelectedCells.Count > 0)
            {
                ClearBinding();
                loadBinding();
            }
        }
    }
}
