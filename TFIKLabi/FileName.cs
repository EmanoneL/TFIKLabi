using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TFIKLabi
{
    public partial class FileName : Form
    {
        public string fileName { get; set; }
        public FileName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Поле должно быть заполнено!");
            }
            else
            {
                this.Hide();
            }
        }
    }
}
