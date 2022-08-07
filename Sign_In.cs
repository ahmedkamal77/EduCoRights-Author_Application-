using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Runtime.InteropServices;
using System.IO;

namespace LoginPage
{
    public partial class Sign_In : Telerik.WinControls.UI.RadForm
    {

    
        DatabaseConnection signin_db;
        public Sign_In()
        {
            InitializeComponent();
            signin_db = DatabaseConnection.GetInstance();
  
         
        }

        public DatabaseConnection DatabaseConnection
        {
            get => default;
            set
            {
            }
        }

        internal Sign_Up Sign_Up
        {
            get => default;
            set
            {
            }
        }

        public Sign_up Sign_up
        {
            get => default;
            set
            {
            }
        }

        public InsertCourse InsertCourse
        {
            get => default;
            set
            {
            }
        }
        #region enter & leave

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd
            , int wmsg, int wparam, int lparam);

        private void textuser_Enter(object sender, EventArgs e)
        {

            if (textuser.Text == "Username")
            {
                textuser.Text = "";
                textuser.ForeColor = Color.LightGray;
            }
        }

        private void textuser_Leave(object sender, EventArgs e)
        {
            if (textuser.Text == "")
            {
                textuser.Text = "Username";
                textuser.ForeColor = Color.DimGray;
            }
        }


        private void textpassword_Enter(object sender, EventArgs e)
        {
            if (textpassword.Text != "")
            {
                if(textpassword.Text == "Password")
                    textpassword.Text = "";

                textpassword.ForeColor = Color.LightGray;
                textpassword.UseSystemPasswordChar = true;
            }
        }

        private void textpassword_Leave(object sender, EventArgs e)
        {
            if (textpassword.Text == "")
            {

                textpassword.Text = "Password";
                textpassword.ForeColor = Color.DimGray;
                textpassword.UseSystemPasswordChar = false;
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void RadForm1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void btnlogin_Click(object sender, EventArgs e)
        {
           
            if (IsUserExsisted(textuser.Text) && IsPasswordCorrect(textpassword.Text) )
            {
                txtExistedUser.Text = "";
                txtCorrectPassword.Text = "";
               

                new InsertCourse().Show();
                this.Hide();
            }

            else
            {
                if (IsUserExsisted(textuser.Text))
                    txtExistedUser.Text = "";
                else if (!IsUserExsisted(textuser.Text))
                    txtExistedUser.Text = "Invalid Username";

                if (IsPasswordCorrect(textpassword.Text))
                    txtCorrectPassword.Text = "";
                else if (!IsPasswordCorrect(textpassword.Text))
                    txtCorrectPassword.Text = "Incorrect Password";
            }

        }

        //Check User exsitance
        bool IsUserExsisted(string username)
        {

            username = username.TrimEnd();
            username = username.TrimStart();
            if (signin_db.execute_scalar_query("SELECT User_Name FROM authors WHERE User_Name = '" + username + "';").TrimEnd() == username) return true;
            else return false;
        }

        //Check Password correction
        bool IsPasswordCorrect(string password)
        {
            if (signin_db.execute_scalar_query("SELECT Password FROM authors WHERE Password = '" + password + "';") == password) return true;
            else return false;
        }

        private void Signuplink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Sign_Up().Show();
            this.Hide();
        }

       

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            if (textpassword.Text != "Password" && textpassword.UseSystemPasswordChar == true)
            {
                textpassword.UseSystemPasswordChar = false;
            }
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            if (textpassword.Text != "Password" && textpassword.UseSystemPasswordChar == false)
            {
                textpassword.UseSystemPasswordChar = true;
            }
        }
       
       
       
       
    }
}
