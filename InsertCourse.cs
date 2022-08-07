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
using aes256withsalt;
using tribleedes;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace LoginPage
{
    public partial class InsertCourse : Telerik.WinControls.UI.RadForm
    {
    
        public InsertCourse()
        {
            InitializeComponent();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd
            , int wmsg, int wparam, int lparam);
       
    
        #region control btns
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



        private string folderPath = "";
        public static string AES_Key = "";
        public static string TDES_Key = "";
        byte[] encryptedfile = null;
        private void BrowseCourse_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please ensure that files in the folder are working correctly.");
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }
            

            if (folderPath != "")
            {
                int backslashIndex = 0;
                for (int i = 0; i < folderPath.Length; i++)
                    if (folderPath[i] == '\\')
                        backslashIndex = i;

                string filetoDelete = folderPath.Substring(backslashIndex + 1);
                filetoDelete += ".zip";
                filetoDelete = folderPath.Substring(0, backslashIndex + 1) + filetoDelete;
                //deleting any ZIP file with the same folder's name and the folder itself
                string[] fileEntries = Directory.GetFiles(folderPath.Substring(0, backslashIndex));  //+1
                foreach (string fileName in fileEntries)
                    if (fileName == filetoDelete)
                    {
                        MessageBox.Show("Invalid Folder");
                        File.Delete(filetoDelete);
                        Directory.Delete(folderPath, true);
                        folderPath = string.Empty;
                    }
            }

        }

        private void insert_To_upload_Click(object sender, EventArgs e)
        {
            if (folderPath != "")
            {
                //Generaing Random Keys
                AES_Key = Random_Key_Generator.GetUniqueKey(8);
                TDES_Key = Random_Key_Generator.GetUniqueKeyOriginal_BIASED(24);

                //Encrypting Files in the Directory 
                var files = from file in Directory.EnumerateFiles(folderPath) select file;
                foreach (var file in files)
                {
                    //AES Encryption
                    encryptedfile = cryptor.getEncryptor(AES_Key, File.ReadAllBytes(file), "asd@asd_er$#wwer125m,,844poiyfheawab");
                    if (encryptedfile == null)
                    {
                        MessageBox.Show("Error");
                        return;
                    }
                    else
                    {
                        File.WriteAllBytes(file, encryptedfile);
                        //MessageBox.Show("the file is encrybted With AES by"+AES_Key);
                    }

                    //TDES Encryption
                    try
                    {
                        TDES tdes = new TDES(TDES_Key);
                        tdes.encryptfile(file);
                        //MessageBox.Show("file is encrypt with TDES by "+TDES_Key);
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                //Folder Compression
                ZipFile.CreateFromDirectory(@folderPath, @folderPath + ".zip");
                new Upload().Show();
                this.Hide();
            }

            else
                MessageBox.Show("Please select the folder at first.");
        }

        internal Random_Key_Generator Random_Key_Generator
        {
            get => default;
            set
            {
            }
        }

        public TDES TDES
        {
            get => default;
            set
            {
            }
        }

        internal cryptor cryptor
        {
            get => default;
            set
            {
            }
        }

        public Upload Upload
        {
            get => default;
            set
            {
            }
        }
    }
}
