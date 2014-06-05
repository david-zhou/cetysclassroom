using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace agregarSalones
{
    public partial class agregarSalon : Form
    {
        Point punto;
        List<int> tagsID = new List<int>();
        public agregarSalon(Point p)
        {
            punto = p;
            InitializeComponent();
        }

        private void agregarSalon_Load(object sender, EventArgs e)
        {
            label3.Text = punto.X.ToString();
            label4.Text = punto.Y.ToString();
            
            StreamReader sr = new StreamReader("RP.txt");
            List<String> rp = new List<string>();
            while (sr.Peek() != -1)
            {
                rp.Add(sr.ReadLine());
            }
            comboBox1.DataSource = rp;
            sr.Close();
            /*
            sr = new StreamReader("Tags.txt");
            List<String> tags = new List<string>();
            while (sr.Peek() != -1)
            {
                tags.Add(sr.ReadLine());
            }
            listBox1.DataSource = tags;
            sr.Close();
             */
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select id,Tag_Name from classrooms_tags;", conn);
                cmd.ExecuteNonQuery();
                List<String> tags = new List<string>();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);     // Display the value of the key and value column for every row
                        tags.Add(reader[1].ToString());
                        tagsID.Add(int.Parse(reader[0].ToString()));
                    }
                }
                listBox1.DataSource = tags;
                conn.Close();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Nombre vacio");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Imagen vacia");
                return;
            }
            if (textBox1.Text.Length > 30)
            {
                MessageBox.Show("Nombre muy largo");
                return;
            }
            if (textBox2.Text.Length > 10)
            {
                MessageBox.Show("Imagen muy larga");
                return;
            }
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select C.id from classrooms_classroom as C where C.Classroom_name='" + textBox1.Text + "';", conn);
                cmd.ExecuteNonQuery();
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Ese salon ya existe");
                    conn.Close();
                    //MessageBox.Show(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);     // Display the value of the key and value column for every row
                }
                else
                {
                    //insert salon
                    string insertSalon = "Insert into classrooms_classroom (Classroom_Name,Classroom_Image,Classroom_CoordX,Classroom_CoordY,Classroom_ReferencePoint) Values ('" + textBox1.Text + "','" + textBox2.Text + "',"+(int.Parse(label3.Text)-10)+","+(int.Parse(label4.Text)-40)+","+(comboBox1.SelectedIndex+1)+");";
                    SQLiteCommand insertSalonCmd = new SQLiteCommand(insertSalon, conn);
                    insertSalonCmd.ExecuteNonQuery();
                    
                    //select salon id
                    string selectSalon = "Select id from classrooms_classroom where Classroom_Name = '" + textBox1.Text + "'";
                    SQLiteCommand selectSalonCmd = new SQLiteCommand(selectSalon, conn);
                    selectSalonCmd.ExecuteNonQuery();
                    reader = selectSalonCmd.ExecuteReader();
                    int id = 0;
                    if (reader.Read())
                    {
                        id = int.Parse(reader[0].ToString());
                    }
                    //MessageBox.Show(id.ToString());
                    
                    //insert tags
                    string insertTags = "";
                    foreach (int i in listBox1.SelectedIndices)
                    {
                        insertTags += "INSERT INTO classrooms_classroom_tags (Classroom_ID_id,Tag_ID_id) VALUES("+id+","+tagsID[i]+");\n";
                    }
                    //MessageBox.Show(insertTags);
                    SQLiteCommand insertTagsCmd = new SQLiteCommand(insertTags, conn);
                    insertTagsCmd.ExecuteNonQuery();
                    MessageBox.Show("Salon agregado");
                    conn.Close();
                    this.Close();
                }
                conn.Close();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show(ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
