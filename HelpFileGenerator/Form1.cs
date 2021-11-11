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
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }
    }
}
