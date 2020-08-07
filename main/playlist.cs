using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace WindowsApplication1
{
    public partial class playlist : Form
    {
        public string selectedItem;

        public playlist()
        {
            InitializeComponent();
            DataTable table = new DataTable();
            dataGridView2.DataSource = table;
            table.Columns.Add("Files");
            string FLStudioFactoryData = @"C:\Program Files\Image-Line\FL Studio 20\Data\Patches";
            string[] allfiles = Directory.GetFiles(FLStudioFactoryData, "*.wav", SearchOption.AllDirectories);
            List<string> vs = allfiles.ToList();
            File.WriteAllLines("allfiles.txt", vs);
            // get lines from the text file
            string[] lines = File.ReadAllLines("allfiles.txt");
            string[] values;


            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                table.Rows.Add(row);
            }
    }

        private void Replace()
        {
            throw new NotImplementedException();
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
            {
                Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));               
                dataGridView1.Rows[dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex].Cells[dataGridView1.HitTest(clientPoint.X, clientPoint.Y).ColumnIndex].Value = (System.String)e.Data.GetData(typeof(System.String));
                
            }                  
               
       }

       
        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(4);         
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView2.DoDragDrop(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), DragDropEffects.Copy);
           
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                MessageBox.Show(selectedItem);
        }

        private void dataGridViewSelected(object sender, DataGridViewCellEventArgs e)
        {
                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                selectedItem = val;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
        }

        private void test()
        {
            throw new NotImplementedException();
        }

        public delegate void DataGridViewCellEventHandler(object sender, DataGridViewCellEventArgs e);

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedItem) == false)
            {
                WindowsMediaPlayer myplayer = new WindowsMediaPlayer();
                myplayer.URL = selectedItem;
                myplayer.controls.play();
            }

            else
            {
                MessageBox.Show("Select an item and try again");
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
            selectedItem = val;
            MessageBox.Show(selectedItem);
        }
    }
}