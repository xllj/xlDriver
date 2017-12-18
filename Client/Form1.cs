using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xlDriverDIO;
using xlTools;
namespace PCI1762测试软件
{
    public partial class Form1 : Form
    {
        xlDriverDioBase dio = new PCI1762(); 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dio.Init();
            //dio.Write(0, 0);
            DataSet ds = new DataSet();
            try
            {
                ds = xlDataBase.Last("PSATS", "Table_Data", "ID", "产品型号,产品编号,测试报表", "产品型号='CH-112'");
                //MessageBox.Show(ds.Tables[0].Rows.Count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //MessageBox.Show(ds.Tables[0].Rows[0]["测试报表"]);
            //xlBase64.Encoded(Convert.to ds.Tables[0].Rows[0]["测试报表"]);
            dataGridView1.DataSource = ds.Tables[0];

            String str = "";
            Byte[] bytes = xlBase64.Decoded(str);

            str = xlBase64.Encoded("asdfsdfsdfsdfsdf");
            str = xlBase64.DecodedString(str);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Int32 val = dio.Read(0);
            textBox3.Text = val.ToString("X2");
            val = dio.Read(1);
            textBox4.Text = val.ToString("X2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dio.Write(0, 3, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dio.Write(0, 3, false);
        }
    }
}
