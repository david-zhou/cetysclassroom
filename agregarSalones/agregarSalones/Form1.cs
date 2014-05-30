using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace agregarSalones
{
    public partial class Form1 : Form
    {
        int opcion = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            //MessageBox.Show(coordinates.X + "," + coordinates.Y);
            if (opcion == 1)
            {
                agregarSalon s = new agregarSalon(coordinates);
                s.Show();
                opcion = 0;
                label1.Text = "Seleccione opcion";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            opcion = 1;
            label1.Text = "Seleccione el punto";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            agregarTag a = new agregarTag();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            agregarRuta a = new agregarRuta();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            quitarSalon a = new quitarSalon();
            a.Show();
        }
    }
}
