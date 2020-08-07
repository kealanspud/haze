using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using WindowsApplication1;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace daw
{
    public partial class Form1 : Form
    {
        public string username;
        public string licence;
        public string skin;
        
        public Form1()
        {
            InitializeComponent();
            skin = "sky";
            username = Environment.UserName;
            licence = textBox1.Text;
            comboBox1.SelectedItem = "Sky";
            PrefrencesIsVisible(false);
            textBox1.Visible = false;
            panel2.Visible = false;
            UpdateSettings();

            Process regeditProcess = Process.Start("regedit.exe", "/s OpenDaw.reg");
            regeditProcess.WaitForExit();

            void fixFLP()
            {
                StreamReader file = new StreamReader("untitled.txt");
                string projectFileSource = file.ReadToEnd();
                string projectFile = projectFileSource.ToString();
                fix(projectFile);
                var decodedProject = fix(projectFile);
                StreamWriter writer = new StreamWriter("output.txt");
                writer.Write(decodedProject);
                writer.Close();

                string fix(string Content)
                {
                    // remove unicode chars from flp; 
                    String ret = Regex.Replace(Content.Trim(), "[^A-Za-z0-9_. ]+", "");
                    return ret.Replace(" ", "");
                }
            }

            fixFLP();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                RestoreDirectory = true,
                DefaultExt = "flp",
                Filter = "FL Studio Project files (*.flp)|*.flp|All files (*.*)|*.*"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(openFileDialog1.FileName, "Output");
            }
        }

        public void PrefrencesIsVisible(bool state)
        {
            panel1.Visible = state;
            closeBtn.Visible = state;
            label1.Visible = state;
            textBox1.Visible = state;
            key.Text = "Enter Key";
            label2.Text = "";
            panel2.Visible = state;

            string FLStudioFactoryData = Environment.SpecialFolder.ProgramFiles + @"/Image-Line/FL Studio 20/";

            StreamReader reader = new StreamReader("output.txt");
            var cleanedText = reader.ReadToEnd();
            cleanedText.Replace("FLhdFLdt20.6.2", "FL20.6.2");
            cleanedText.Replace("FLStudioFactoryData", FLStudioFactoryData);
            List<char> linesList = cleanedText.ToList();
            linesList.RemoveAt(6);
            linesList.RemoveAt(7);
            linesList.RemoveAt(8);
            StreamWriter writeToFile = new StreamWriter("decoded.txt");
            writeToFile.Write(cleanedText);
            writeToFile.Close();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            PrefrencesIsVisible(false);
            textBox1.Visible = false;
            panel2.Visible = false;
            textBox1.Text = "";
            textBox1.CharacterCasing = CharacterCasing.Upper;
            key.Text = "Enter Key";
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PrefrencesIsVisible(true);
            textBox1.Visible = false;
            panel2.Visible = false;
            textBox1.Text = "";
            key.Text = "Enter Key";
        }

        public void key_Click(object sender, EventArgs e)
        {
            key.Text = "Enter Key";
            panel2.Visible = true;
            textBox1.Visible = true;
            textBox1.Focus();
            textBox1.Text = "";
            textBox1.CharacterCasing = CharacterCasing.Upper;
            key.Text = "Clear";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int reminder;
            Math.DivRem(textBox1.Text.Length + 1, 5, out reminder);
            if ((textBox1.Text.Length < 25) & (reminder == 0))
            {
                textBox1.CharacterCasing = CharacterCasing.Upper;
                textBox1.Text += "-";
                textBox1.SelectionStart = textBox1.Text.Length;
            }

            if ((textBox1.Text.Length > 25) & (reminder == 0))
            {
                textBox1.ReadOnly = true;
                LicenceDialog();
            }

            licence = textBox1.Text;

        }

        public void LicenceDialog()
        {
            {
                if (textBox1.Text.Contains("0000" + "-"))
                {
                    MessageBox.Show("The key has been registered.", "Thank you for registering");
                    key.Visible = false;
                    textBox1.Visible = false;
                    panel2.Visible = false;
                    label2.Text = "Registered to " + username;
                }
                else
                {
                    MessageBox.Show("Error: The key is invalid.", "Error");
                }

                if (textBox1.Text.Contains("PROX"))
                {
                    MessageBox.Show("This is a pro licence. Enjoy your use of Haze!", "Info");
                    UpdateSettings();
                    key.Visible = false;
                }

                if (textBox1.Text.Contains("DEMO"))
                {
                    MessageBox.Show("This is a demo licence. Get the pro version for all features.", "Info");
                    UpdateSettings();
                    key.Visible = false;
                }
            }

        }
        public void UpdateSettings()
        {
            JObject num = new JObject(
                    new JProperty(licence, 1),
                    new JProperty(username, 2),
                    new JProperty(skin, 3));

            File.WriteAllText("settings.json", num.ToString());

            // write JSON directly to a file
            using (StreamWriter file = File.CreateText("settings.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                num.WriteTo(writer);
                file.Close();
            }

            StreamReader reader = new StreamReader("settings.json");
            JsonTextReader json = new JsonTextReader(reader);
            string CachedSettings = json.ToString();
            if (CachedSettings.Contains("DEMO") == true)
            {
                key.Visible = false;
                label2.Text = "Registered to " + username;
                reader.Close();
            }

            if (CachedSettings.Contains("PROX") == true)
            {
                key.Visible = false;
                label2.Text = "Registered to " + username;
                reader.Close();
            }

            else
            {
                key.Visible = true;
                reader.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();
            if (selected == "Dark")
            {
                Image myimage = new Bitmap(@"C:\Users\keala\Downloads\dark.png");
                this.BackgroundImage = myimage;
                skin = "dark";
                UpdateSettings();
                key.Visible = false;
            }

            else
            {
                Image myimage = new Bitmap(@"C:\Users\keala\Downloads\sky.png");
                this.BackgroundImage = myimage;
                skin = "sky";
                UpdateSettings();
                key.Visible = false;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void button4_Click(object sender, EventArgs e)
        {
            playlist form2 = new playlist();
            form2.Show();
        }
    }
}
