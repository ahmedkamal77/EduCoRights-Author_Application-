using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LoginPage
{
    public class DatabaseConnection
    {
        public MySqlConnection conn;   //connection variable
        public string connString;      //string connection
        public MySqlCommand cmd;
        private string Course_Description;
        private int Course_ID;
        private string Course_Link;
        private byte[] Course_Image;
        private string Course_Name;
        private string Home_Course_Description;
        private string Course_auther;
        private string Course_date;
        private string Course_lang;
        private string Course_price;
        string U_courses;
        Sign_In S;

        public string Course_Description1 { get => Course_Description; set => Course_Description = value; }

        public int Course_ID1 { get => Course_ID; set => Course_ID = value; }
        public string Course_Link1 { get => Course_Link; set => Course_Link = value; }
        public byte[] Course_Image1 { get => Course_Image; set => Course_Image = value; }
        public string Course_Name1 { get => Course_Name; set => Course_Name = value; }
        public string Home_Course_Description1 { get => Home_Course_Description; set => Home_Course_Description = value; }
        public string Course_auther1 { get => Course_auther; set => Course_auther = value; }
        public string Course_date1 { get => Course_date; set => Course_date = value; }
        public string Course_lang1 { get => Course_lang; set => Course_lang = value; }
        public string Course_price1 { get => Course_price; set => Course_price = value; }

        //Singleton
        private static DatabaseConnection instance = null;
        private static object lockobj = new object();
        private DatabaseConnection() { connect_to_database(); }

        public static DatabaseConnection GetInstance()
        {
            lock (lockobj)
            {
                if (instance == null)
                    instance = new DatabaseConnection();
                return instance;
            }
        }

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

        public string execute_scalar_query(string query)
        {

            string result = string.Empty;
            while (conn.State == ConnectionState.Open) ;
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            result = Convert.ToString(cmd.ExecuteScalar());
            conn.Close();
            return result;

        }

        public void execute_normal_query(string query)
        {
            while (conn.State == ConnectionState.Open) ;
            try
            {
                conn.Open();
                if (this.conn != null)
                {
                    cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Your Information isn't added");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        

        public List<int> extract_courses_ID(string IDs)
        {
            string temp = "";
            List<int> ID = new List<int>();
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] == ',')
                {
                    ID.Add(Convert.ToInt32(temp));
                    temp = "";
                }
                else if (i == (IDs.Length - 1))
                {
                    temp += IDs[i];
                    ID.Add(Convert.ToInt32(temp));
                    temp = "";
                }
                else
                    temp += IDs[i];
            }
            ID.Sort();
            return ID;
        }


    }
}


