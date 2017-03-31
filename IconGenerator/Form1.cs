using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IconGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbbType.Items.AddRange(GenerateSetting.getTypes());
            cbbType.SelectedIndex = 0;
            List<string> configs = ConfigManager.load();
            foreach (string config in configs) {
                var item = listView1.Items.Add(config);
                item.Tag = GenerateSetting.parse(config);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (ListViewItem item in listView1.Items) {
                list.Add(item.Tag.ToString());
            }
            ConfigManager.save(list.ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {//Generate
            if (pictureBox1.Image == null) {
                selectPicture();
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string rootPath = folderBrowserDialog1.SelectedPath 
                    + Path.DirectorySeparatorChar;
                foreach (ListViewItem item in listView1.Items)
                {
                    GenerateSetting gs = item.Tag as GenerateSetting;
                    using (Bitmap bitmap = new Bitmap(pictureBox1.Image, new Size(gs.Width, gs.Height)))
                    {
                        FileInfo file = new FileInfo(rootPath + gs.getFileName());
                        if (!file.Directory.Exists)
                        {
                            file.Directory.Create();
                        }
                        bitmap.Save(file.FullName, gs.Type);
                    }
                }
                MessageBox.Show("done");

            }
        }
        private void selectPicture()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            selectPicture();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm", "delete selected setting?",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    listView1.Items.Remove(item);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//add
            string filename = tbxFilename.Text;
            if (string.IsNullOrEmpty(filename)) {
                MessageBox.Show("please input Filename");
                return;
            }
            string path = tbxPath.Text;
            foreach (var c in Path.GetInvalidPathChars()) {
                if (path.Contains(c) || filename.Contains(c))
                {
                    MessageBox.Show("invalid path chars:" + c);
                    return;
                }
            }
            ImageFormat type = cbbType.SelectedItem as ImageFormat;
            if (type == null)
            {
                MessageBox.Show("please select a type");
                return;
            }
            foreach (ListViewItem item in listView1.Items) {
                GenerateSetting gs = item.Tag as GenerateSetting;
                if (gs != null) {
                    if (gs.FileName.Equals(filename)
                        && gs.Path.Equals(path)
                        && gs.Type.Equals(type)) {
                        MessageBox.Show("has this path/filename already");
                        return;
                    }
                }
            }
            int height = Convert.ToInt32(nudHeight.Value);
            int width = Convert.ToInt32(nudWidth.Value);
            GenerateSetting generateSetting = new GenerateSetting()
            {
                FileName = filename,
                Height = height,
                Path = path,
                Type = type,
                Width = width
            };
            ListViewItem listViewItem = listView1.Items.Add(generateSetting.ToString());
            listViewItem.Tag = generateSetting;
        }

    }
}
