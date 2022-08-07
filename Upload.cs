using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LoginPage
{
    public partial class Upload : Form
    {
        public Upload()
        {
            InitializeComponent();
        }
        private void  Welcom_1_Load(object sender, EventArgs e)
        {
           
        }
        #region 

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd
            , int wmsg, int wparam, int lparam);
      
        private void Upload_MouseDown(object sender, MouseEventArgs e)
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

        public static string courseLink = string.Empty;
        private void Next_TO_CourseInfo_Click(object sender, EventArgs e)
        {
            if (MediaFire_Link.Text != "")
            {
                courseLink = MediaFire_Link.Text;
                new Course_Info().Show();
                this.Hide();
            }

            else
                MessageBox.Show("Please enter mediafire link.");
        }

        private void mediafirlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore", "https://app.mediafire.com");
        }

        public Course_Info Course_Info
        {
            get => default;
            set
            {
            }
        }
    }
}
