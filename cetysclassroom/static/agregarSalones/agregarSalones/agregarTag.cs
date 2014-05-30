using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace agregarSalones
{
    public partial class agregarTag : Form
    {
        List<string> tags = new List<string>();
        public agregarTag()
        {
            InitializeComponent();
        }

        private void agregarTag_Load(object sender, EventArgs e)
        {
            loadTags();
        }
        void loadTags()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("Select id,Tag_Name from classrooms_tags;", conn);
                cmd.ExecuteNonQuery();
                tags = new List<string>();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);     // Display the value of the key and value column for every row
                        tags.Add(reader[1].ToString());
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Nombre vacio");
                    return;
                }
                try
                {
                    bool exists = false;
                    foreach(string s in tags)
                    {
                        
                        if(s.ToLower().CompareTo(textBox1.Text.ToLower())==0)
                        {
                            exists=true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        MessageBox.Show("Ese tag ya existe");
                        //MessageBox.Show(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);     // Display the value of the key and value column for every row
                    }
                    else
                    {
                        SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\David\\Documents\\GitHub\\cetysclassroom\\cetysclassroom\\db.sqlite3");
                        conn.Open();
                        //insert tags
                        string insertTag = "Insert into classrooms_tags (Tag_Name) Values ('" + textBox1.Text.ToLower() + "');";
                        SQLiteCommand insertSalonCmd = new SQLiteCommand(insertTag, conn);
                        insertSalonCmd.ExecuteNonQuery();
                        conn.Close();
                        loadTags();
                        textBox1.Text = "";
                    }
                    
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
}
