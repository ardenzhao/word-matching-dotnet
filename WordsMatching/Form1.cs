using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WordsMatching
{
    public partial class MatchForm : Form
    {
        WordMatch _wm = null;
        public MatchForm()
        {
            InitializeComponent();
            _wm = new WordMatch();
            _wm.Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= 0)
            {
                MessageBox.Show(@"Please entry a match condition. *,? and % as wildcard.");
                return;
            }

            if (textBoxWildcard.Text.ToCharArray().Any(c => !char.IsLetter(c)))
            {
                MessageBox.Show(@"Please entry letters in wildcard definition.");
                return;
            }

            listBox1.Items.Clear();

            string patten = textBox1.Text;
            List<string> rltList = _wm.Match(patten, checkBox1.Checked ? int.Parse(comboBox1.Text) : 0,textBoxWildcard.Text);
            listBox1.Items.AddRange(rltList.ToArray());
            if (rltList.Count <= 0)
                label_rlt.Text = @"no words matched";
            //listBox1.Items.Add("no words matched");
            else
                label_rlt.Text = string.Format("{0} words matched", rltList.Count());
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox1.Checked;
        }
    }
}
