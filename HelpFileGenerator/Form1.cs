using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpFileGenerator
{
    public partial class Form1 : Form
    {
        private const string Filter = "JavaScript Object Notation (*.json)|*.json|All Files (*.*)|*.*";

        private string _loadedFile = "";

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            aboutToolStripMenuItem.Text = $"About {Text}...";
        }

        private void SaveFile(bool saveAs)
        {
            string json = JsonConvert.SerializeObject(itemLst.Items, Formatting.Indented);

            if (!File.Exists(_loadedFile) || saveAs)
            {
                SaveFileDialog sfd = new() { Filter = Filter };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, json);
                    _loadedFile = sfd.FileName;
                }
            }
        }

        private void removeItemBtn_Click(object sender, EventArgs e)
        {
            if (itemLst.SelectedItem is not null)
            {
                DialogResult dr = MessageBox.Show(this, "Are you sure you'd like to remove this item?", "Remove Item?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(dr == DialogResult.Yes)
                {
                    itemLst.Items.RemoveAt(itemLst.SelectedIndex);
                }
            }
        }

        private void modifyItemBtn_Click(object sender, EventArgs e)
        {
            if(itemLst.SelectedItem is not null)
            {
                ItemGenFrm i = new((HelpItem)itemLst.SelectedItem);
                if(i.ShowDialog() == DialogResult.OK)
                    itemLst.Items[itemLst.SelectedIndex] = i.Item;
            }
        }

        private void newItemBtn_Click(object sender, EventArgs e)
        {
            ItemGenFrm i = new();
            if (i.ShowDialog() == DialogResult.OK)
                itemLst.Items.Add(i.Item);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new() { Filter = Filter};
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                _loadedFile = ofd.FileName;
                string json = File.ReadAllText(ofd.FileName);
                HelpItem[] hi = JsonConvert.DeserializeObject<HelpItem[]>(json);
                itemLst.Items.Clear();
                foreach(HelpItem h in hi)
                {
                    itemLst.Items.Add(h);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Implemenet About Box
            // GOD THAT BRINGS BACK HORRIBLE MEMORIES! NO! DO NOT ASK!
            AboutProgram aboutProgram = new();
            aboutProgram.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Please make sure that the file you're about to convert is a Text File and is formatted in the following way:\n\nCommand — Description\n\nThe separator is an emdash or (Alt+0151). Any line improperly formatted will not be accepted. When the conversion is complete, please check the JSON by loading it into the program.", "SECRET CONVERSION TOOL!");
            OpenFileDialog ofd = new() { Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*"};
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string[] data = File.ReadAllLines(ofd.FileName);
                List<HelpItem> lst = new();
                List<string> errorStr = new();
                foreach(string line in data)
                {
                    try
                    {
                        string[] commandItem = line.Split('—');
                        lst.Add(new HelpItem(commandItem[0], commandItem[1], false));
                    } catch (Exception ex)
                    {
                        // Will throw an out of error exception, but I don't care.
                        errorStr.Add(line);
                    }
                }
                string json = JsonConvert.SerializeObject(lst);
                File.WriteAllText($"converted-{DateTime.Now:yyyyyMMddHHmmss}.json", json);
                if(errorStr.Count > 0)
                {
                    MessageBox.Show(this, $"There {(errorStr.Count != 1 ? $"are {errorStr.Count} errors" : $"is {errorStr.Count} error")} found. Please check the document and try to convert again.", "Found errors");
                }
                MessageBox.Show(this, "Conversion complete! Please check the JSON for any missing elements");
            }
        }
    }
}
