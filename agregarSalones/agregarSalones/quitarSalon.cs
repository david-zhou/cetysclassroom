using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace agregarSalones
{
    public partial class quitarSalon : Form
    {
        List<string> salones, RP;
        public quitarSalon()
        {
            InitializeComponent();
        }

        private void quitarSalon_Load(object sender, EventArgs e)
        {
            readRP();
            resetForm();
        }
        private void readRP()
        {
            RP = new List<string>();
            StreamReader sr = new StreamReader("RP.txt");
            while (sr.Peek() != -1)
            {
                RP.Add(sr.ReadLine());
            }
            sr.Close();
        }
        private void resetForm()
        {
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select classroom_Name from classrooms_classroom order by classroom_name ASC;", conn);
                cmd.ExecuteNonQuery();
                salones = new List<string>();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);     // Display the value of the key and value column for every row
                        salones.Add(reader[0].ToString());
                    }
                }
                comboBox1.DataSource = salones;
                conn.Close();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select * from classrooms_classroom where Classroom_Name='"+comboBox1.SelectedValue+"';", conn);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("");
                SQLiteDataReader reader = cmd.ExecuteReader();
                string id = "";
                while (reader.Read())
                {
                    label7.Text = reader[3].ToString();
                    label8.Text = reader[4].ToString();
                    label9.Text = reader[2].ToString();
                    label10.Text = RP[Convert.ToInt32(reader[5].ToString()) - 1];
                    id = reader[0].ToString();
                }
                
                SQLiteCommand cmd2 = new SQLiteCommand("Select T.Tag_Name from classrooms_tags AS T INNER JOIN classrooms_classroom_tags AS CT ON CT.Tag_ID_id = T.id WHERE CT.Classroom_ID_id = "+id, conn);
                cmd2.ExecuteNonQuery();
                reader = cmd2.ExecuteReader();
                label11.Text = "";
                while(reader.Read())
                {
                    label11.Text+=reader[0].ToString()+",";
                }
                conn.Close();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select id from classrooms_classroom where Classroom_Name='" + comboBox1.SelectedValue + "';", conn);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("");
                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                string id = reader[0].ToString();

                SQLiteCommand cmd2 = new SQLiteCommand("DELETE FROM classrooms_classroom_tags WHERE Classroom_ID_id = " + id, conn);
                cmd2.ExecuteNonQuery();

                SQLiteCommand cmd3 = new SQLiteCommand("DELETE FROM classrooms_classroom WHERE id = " + id, conn);
                cmd3.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Salon borrado");

                resetForm();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }

}
