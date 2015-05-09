using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Task.Run(
                    () =>
                    {
                        this.button1.Invoke((Action)delegate { button1.Enabled = false; });
                        this.button2.Invoke((Action)delegate { button2.Enabled = false; });
                        this.button3.Invoke((Action)delegate { button3.Enabled = false; });
                        
                        if (radioButton1.Checked == true)
                        {
                            SortingBigFile.SortingBigFile.QuickSortInt64(this.textBox1.Text);
                        }
                        else if (radioButton2.Checked == true)
                        {
                            SortingBigFile.SortingBigFile.BubbleSortInt64(this.textBox1.Text);
                        }
                        else
                        {
                            SortingBigFile.SortingBigFile.CountingSortByte(this.textBox1.Text);
                        }

                        this.button1.Invoke((Action)delegate { button1.Enabled = true; });
                        this.button2.Invoke((Action)delegate { button2.Enabled = true; });
                        this.button3.Invoke((Action)delegate { button3.Enabled = true; });
                        
                    }
                );
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.KeyChar = (char)0;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.KeyChar = (char)0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int32 sizeData = 0;
            if (radioButton1.Checked == true || radioButton2.Checked)
            {
                sizeData = sizeof(Int64);
            }
            else
            {
                sizeData = sizeof(Byte);
            }

            Int64 leftIndex = 0; 
            Int64 rightIndex = 0;
            if (textBox2.Text != "")
            {
                Int64.Parse(textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                Int64.Parse(textBox3.Text);
            }
            listBox1.DataSource = SortingBigFile.SortingBigFile.GetRangeOfFile(textBox1.Text, sizeData, leftIndex, rightIndex);
        }

    }
}
