using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace agregarSalones
{
    public partial class agregarRuta : Form
    {
        enum State : int
        {
            SinPunto,
            UnPunto,
            MasDeUnPunto
        }
        int estado = -1;
        Point ant;
        List<Point> puntos;
        public agregarRuta()
        {
            InitializeComponent();
        }

        private void agregarRuta_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("RP.txt");
            List<String> rp = new List<string>();
            List<String> rp2 = new List<string>();
            while (sr.Peek() != -1)
            {
                string s = sr.ReadLine();
                rp.Add(s);
                rp2.Add(s);
            }
            comboBox1.DataSource = rp;
            comboBox2.DataSource = rp2;
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                label3.Text = "Seleccione dos distintos";
                return;
            }
            estado = (int)State.SinPunto;
            clearRoutes();
            label3.Text = "Seleccione el punto de salida";
            button1.Enabled = false;
            puntos = new List<Point>();
            button4.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (estado == (int)State.SinPunto)
            {
                estado = (int)State.UnPunto;
                MouseEventArgs click = (MouseEventArgs)e;
                ant = click.Location;
                label3.Text = "Seleccione el siguiente punto";
                puntos.Add(ant);
                //puntos.Add(ant);
                button3.Enabled = true;
                return;
            }
            if (estado == (int)State.UnPunto)
            {
                estado = (int)State.MasDeUnPunto;
                MouseEventArgs click = (MouseEventArgs)e;
                button2.Enabled = true;
                ant = click.Location;
                puntos.Add(ant);
                drawRoute();
                return;
            }
            if (estado == (int)State.MasDeUnPunto)
            {
                MouseEventArgs click = (MouseEventArgs)e;
                button2.Enabled = true;
                ant = click.Location;
                puntos.Add(ant);
                drawRoute();
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            puntos.RemoveAt(puntos.Count - 1);
            
            if (puntos.Count > 1)
            {
                drawRoute();
                return;
            }
            if (puntos.Count == 1)
            {
                estado = (int)State.UnPunto;
                clearRoutes();
                return;
            }
            if (puntos.Count == 0)
            {
                estado = (int)State.SinPunto;
                button3.Enabled = false;
                clearRoutes();
                return;
            }
        }
        private void clearRoutes()
        {
            pictureBox1.Image = Image.FromFile("croquisCetysNames2.PNG");

        }
        private void drawRoute()
        {
            
            Image map = Image.FromFile("croquisCetysNames2.PNG");
            Pen pen = new Pen(Color.Blue, 4);
            Graphics g = Graphics.FromImage(map);
            for (int i = puntos.Count - 1; i > 0; i--)
            {
                g.DrawLine(pen, puntos[i], puntos[i-1]);
            }
            //g.DrawLine(pen, puntos[1], puntos[0]);
            g.Save();
            pictureBox1.Image = map;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "rutas\\" + (comboBox1.SelectedIndex + 1) + "-" + (comboBox2.SelectedIndex + 1) + ".txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (Point p in puntos)
                    {
                        sw.WriteLine(p.X + "," + p.Y);
                    }
                    //sw.WriteLine("0");
                    sw.Close();

                    //crear el otro archivo
                    string path2= "rutas\\html_rutas.js";

                    StreamWriter sw2 = File.AppendText(path2);

                    sw2.WriteLine("//" + (comboBox1.SelectedIndex+1) + "-" + (comboBox2.SelectedIndex+1));
                    sw2.WriteLine("function rutas" + (comboBox1.SelectedIndex + 1) + "_" + (comboBox2.SelectedIndex + 1) + "(ctx,screen)");
                    sw2.WriteLine("{");

                    sw2.WriteLine("var x=" + puntos[0].X + "*screen.width/563;");
                    sw2.WriteLine("var y=" + puntos[0].Y + "*screen.width/563;");
                    sw2.WriteLine("ctx.moveTo(x,y);");

                    for (int i = 1; i < puntos.Count; i++)
                    {
                        sw2.WriteLine("x=" + puntos[i].X + "*screen.width/563;");
                        sw2.WriteLine("y=" + puntos[i].Y + "*screen.width/563;");
                        sw2.WriteLine("ctx.lineTo(x,y);");
                    }
                    sw2.WriteLine("}");
                    sw2.Close();

                    MessageBox.Show("Ruta creada");
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button1.Enabled = false;
                    button4.Enabled = true;
                    clearRoutes();
                    estado = (int)State.SinPunto;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                }

            }
            else
            {
                var option = MessageBox.Show("Esa ruta ya existia, desea sobreescribir?", "Confirm", MessageBoxButtons.YesNo);
                if (option == System.Windows.Forms.DialogResult.Yes)
                {
                    StreamWriter sw = new StreamWriter(path);
                    foreach (Point p in puntos)
                    {
                        sw.WriteLine(p.X + "," + p.Y);
                    }
                    sw.Close();

                    //crear el otro archivo
                    string path2 = "rutas\\html_rutas.js";
                    
                    StreamReader sr = new StreamReader(path2);
                    List<String> rutas = new List<string>();
                    while (sr.Peek() != -1)
                    {
                        rutas.Add(sr.ReadLine());
                    }
                    sr.Close();

                    StreamWriter sw2 = new StreamWriter(path2);

                    for (int j = 0; j < rutas.Count; j++)
                    {
                        if (rutas[j] == ("//" + (comboBox1.SelectedIndex + 1) + "-" + (comboBox2.SelectedIndex + 1)))
                        {
                            while (rutas[j] != "}")
                            {
                                j++;
                            }
                            continue;
                        }
                        sw2.WriteLine(rutas[j]);
                    }

                    sw2.WriteLine("//" + (comboBox1.SelectedIndex + 1) + "-" + (comboBox2.SelectedIndex + 1));
                    sw2.WriteLine("function rutas" + (comboBox1.SelectedIndex + 1) + "_" + (comboBox2.SelectedIndex + 1) + "(ctx,screen)");
                    sw2.WriteLine("{");

                    sw2.WriteLine("var x=" + puntos[0].X + "*screen.width/563;");
                    sw2.WriteLine("var y=" + puntos[0].Y + "*screen.width/563;");
                    sw2.WriteLine("ctx.moveTo(x,y);");

                    for (int i = 1; i < puntos.Count; i++)
                    {
                        sw2.WriteLine("x=" + puntos[i].X + "*screen.width/563;");
                        sw2.WriteLine("y=" + puntos[i].Y + "*screen.width/563;");
                        sw2.WriteLine("ctx.lineTo(x,y);");
                    }
                    sw2.WriteLine("}");
                    sw2.Close();


                    MessageBox.Show("Ruta creada");
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button1.Enabled = false;
                    button4.Enabled = true;
                    clearRoutes();
                    estado = (int)State.SinPunto;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                label3.Text = "Seleccione dos distintos";
                return;
            }
            //estado = (int)State.SinPunto;
            string path = "rutas\\" + (comboBox1.SelectedIndex + 1) + "-" + (comboBox2.SelectedIndex + 1) + ".txt";
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            button1.Enabled = false;
            if (!File.Exists(path))
            {
                button1.Enabled = true;
                label3.Text = "Aun no existe ruta";
                clearRoutes();
            }
            else
            {
                //traza ruta
                StreamReader sr = new StreamReader(path);
                puntos = new List<Point>();
                string line = "";
                int x, y;
                while (sr.Peek() != -1)
                {
                    line = sr.ReadLine();
                    string[] bits = line.Split(',');
                    x = Convert.ToInt32(bits[0]);
                    y = Convert.ToInt32(bits[1]);
                    Point p = new Point(x, y);
                    puntos.Add(p);
                }
                drawRoute();
                label3.Text = "Ruta actual";
                button1.Enabled = true;
                sr.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            clearRoutes();
            puntos = new List<Point>();
        }
    }
}
