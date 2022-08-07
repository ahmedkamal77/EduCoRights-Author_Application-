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
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Management;
using Microsoft.VisualBasic;
using Microsoft.Win32;     //This namespace is used to work with Registry editor.
using System.IO;
namespace LoginPage
{
    public partial class Course_Info : Form
    {
        DatabaseConnection courseInfo;
        private int CourseID;
        private string courseLink;
        private string imagePath = string.Empty;
        private Image image;
        string AES_Key = string.Empty;
        string TDES_Key = string.Empty;
        

        public Course_Info()
        {
            InitializeComponent();
            courseInfo = DatabaseConnection.GetInstance(); 
            CourseID = Convert.ToInt16(courseInfo.execute_scalar_query("SELECT MAX(Course_ID)+1 as ABC FROM courses;"));
            courseLink = Upload.courseLink;
            AES_Key = InsertCourse.AES_Key;
            TDES_Key = InsertCourse.TDES_Key;
        }
        #region

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd
            , int wmsg, int wparam, int lparam);

        private void Sign_Up_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void Sign_Up_Load(object sender, EventArgs e)
        {

        }
        #endregion

        public MySqlConnection conn;   //connection variable
        public string connString;      //string connection
        public MySqlCommand cmd;
        private void connect_to_database()
        {
            connString = @"Server=MYSQL5047.site4now.net; Database=db_a82e8b_a3go;Uid=a82e8b_a3go;Pwd=vAxMvxqtTXAw8Vq; SslMode=None";
            try
            {
                conn = new MySqlConnection(connString);
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int n = 0; //number of words in description
        int n2 = 0; //number of words in summery
        private void Upload_Course_Click(object sender, EventArgs e)
        {
            string course_name = Course_Name.Text;
            string course_category = Course_category.Text; 
            string course_language = Course_Language.Text;
            string course_author = Author_Name.Text;
            string course_summery = Course_Summery.Text;
            string course_description = Course_Description.Text;
            string course_price = Course_Price.Text;
            int course_id = CourseID;
            string date = DateTime.UtcNow.ToShortDateString();
            string course_link = courseLink;
            var course_image  = imageToByte(image);

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            n = course_description.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            n2 = course_summery.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            if (course_name != "" && course_category != "" && course_language != "" && course_author != "" && IsValidSummery(n2) && IsValidDescription(n) && IsValidPrice(course_price) )
            {

                courseInfo.execute_normal_query("INSERT INTO `courses` (`Course_ID`, `Course_Link`, `Course_Name`, `Category`, `Language`, `Author_Name`, `Description`, `Date`, `AES_Password`, `TripleDES_Password`, `Home_Description`, `Price`) VALUES ('" + course_id + "', '" + course_link + "', '" + course_name + "', '" + course_category + "', '" + course_language + "', '" + course_author + "', '" + course_summery + "', '" + date + "', '" + AES_Key + "', '" + TDES_Key + "', '" + course_description + "', '" + course_price + "');");
                connect_to_database();
                conn.Open();
                if (this.conn != null)
                {
                    var command = new MySqlCommand("UPDATE `courses` SET `Course_Image` = @Course_Image WHERE `Course_ID` = '" + course_id + "';", conn);
                    //courseInfo.execute_normal_query("INSERT INTO `courses` (`Course_Image`) VALUES ('" + course_image + "');");
                    var paramUserImage = new MySqlParameter("@Course_Image", MySqlDbType.Blob, course_image.Length);
                    paramUserImage.Value = course_image;
                    command.Parameters.Add(paramUserImage);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Course is uploaded!");
                Application.Exit();
            }

            else
            {
                if (IsValidDescription(n))
                    txtValidDescription.Text = "";
                else if (!IsValidDescription(n))
                    txtValidDescription.Text = "Description must be at least 50 Words";

                if (IsValidSummery(n2))
                    txtValidSummery.Text = "";
                else if (!IsValidSummery(n2))
                    txtValidSummery.Text = "Summery must be at least 25 Words";

                if (IsValidPrice(course_price))
                    txtValidPrice.Text = "";
                else if (!IsValidPrice(course_price))
                    txtValidPrice.Text = "Price must contains numbers only";
            }
           
        }

        bool IsValidDescription (int n)
        {
            if (n >= 50)//50
                return true;
            else
                return false;
        }

        bool IsValidSummery(int n2)
        {
            if (n >= 25)//25
                return true;
            else
                return false;
        }

        bool IsValidPrice(string coursePrice)
        {
            return coursePrice.All(char.IsDigit);
        }

        private void Image_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();///open file on system
            if (fileDialog.ShowDialog() == DialogResult.OK)//file is choosen 
            {
                imagePath = fileDialog.FileName;
                Course_Image.Text = imagePath;
            }
            image = Image.FromFile(imagePath);
        }

        public byte[] imageToByte(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }


        #region textboxs control



        private void Course_Name_Enter(object sender, EventArgs e)
        {
            if (Course_Name.Text == "Course Name")
            {
                Course_Name.Text = "";
                Course_Name.ForeColor = Color.LightGray;
            }
        }

        private void Course_Name_Leave(object sender, EventArgs e)
        {
            if (Course_Name.Text == "")
            {
                Course_Name.Text = "Course Name";
                Course_Name.ForeColor = Color.DimGray;
            }
        }
      





        private void Course_category_Enter(object sender, EventArgs e)
        {
            if (Course_category.Text == "Course Category")
            {
                Course_category.Text = "";
                Course_category.ForeColor = Color.LightGray;
            }
        }

        private void Course_category_Leave(object sender, EventArgs e)
        {
            if (Course_category.Text == "")
            {
                Course_category.Text = "Course Category";
                Course_category.ForeColor = Color.DimGray;
            }
        }






        private void Course_Language_Enter(object sender, EventArgs e)
        {
            if (Course_Language.Text == "Course Language")
            {
                Course_Language.Text = "";
                Course_Language.ForeColor = Color.LightGray;
            }
        }
        private void Course_Language_Leave(object sender, EventArgs e)
        {
            if (Course_Language.Text == "")
            {
                Course_Language.Text = "Course Language";
                Course_Language.ForeColor = Color.DimGray;
            }
        }



       

        private void Author_Name_Enter(object sender, EventArgs e)
        {
            if (Author_Name.Text == "Author Name")
            {
                Author_Name.Text = "";
                Author_Name.ForeColor = Color.LightGray;
            }
        }

        private void Author_Name_Leave(object sender, EventArgs e)
        {
                if (Author_Name.Text == "")
                {
                    Author_Name.Text = "Author Name";
                    Author_Name.ForeColor = Color.DimGray;
                }
            
        }






        private void Course_Summery_Enter(object sender, EventArgs e)
        {
            if (Course_Summery.Text == "Course summery")
            {
                Course_Summery.Text = "";
                Course_Summery.ForeColor = Color.LightGray;
            }
        }

        private void Course_Summery_Leave(object sender, EventArgs e)
        {
          
               if (Course_Summery.Text == "")
                {
                Course_Summery.Text = "Course summery";
                Course_Summery.ForeColor = Color.DimGray;
                }
            
        }







        private void Course_Image_Enter(object sender, EventArgs e)
        {
            if (Course_Image.Text == "Course Image")
            {
                Course_Image.Text = "";
                Course_Image.ForeColor = Color.LightGray;
            }
        }

        private void Course_Image_Leave(object sender, EventArgs e)
        {
            if (Course_Image.Text == "")
            {
                Course_Image.Text = "Course Image";
                Course_Image.ForeColor = Color.DimGray;
            }  
        }






        private void Course_Description_Enter(object sender, EventArgs e)
        {
            if (Course_Description.Text == "Course Description ")
            {
                Course_Description.Text = "";
                Course_Description.ForeColor = Color.LightGray;
            }
        }

        private void Course_Description_Leave(object sender, EventArgs e)
        {
           
            if (Course_Description.Text == "")
            {
                Course_Description.Text = "Course Description ";
                Course_Description.ForeColor = Color.DimGray;
            }
          
        }



        private void Course_Price_Enter(object sender, EventArgs e)
        {
            if (Course_Price.Text == "Course Price")
            {
                Course_Price.Text = "";
                Course_Price.ForeColor = Color.LightGray;
            }
        }

        private void Course_Price_Leave(object sender, EventArgs e)
        {
           
                if (Course_Price.Text == "")
                {
                    Course_Price.Text = "Course Price";
                    Course_Price.ForeColor = Color.DimGray;
                }
            
        }


        #endregion

        public DatabaseConnection DatabaseConnection
        {
            get => default;
            set
            {
            }
        }
    }
}
