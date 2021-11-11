using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpFileGenerator
{
    public partial class ItemGenFrm : Form
    {
        private HelpItem helpItem;
        public HelpItem Item => helpItem;

        public ItemGenFrm()
        {
            InitializeComponent();
            Text = $"New Item";
        }

        public ItemGenFrm(HelpItem hi): this() {
            nameTxt.Text = hi.Name;
            descriptionTxt.Text = hi.Description;
            aliasChk.Checked = hi.IsAlias;
            Text = $"Editing {hi.Name}";
        }


        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            helpItem = new(nameTxt.Text, descriptionTxt.Text, aliasChk.Checked);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
