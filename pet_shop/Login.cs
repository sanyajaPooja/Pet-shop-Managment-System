using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pet_shop
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//login 
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();

        }
        private void Reset()///reset pagee
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private void label6_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
