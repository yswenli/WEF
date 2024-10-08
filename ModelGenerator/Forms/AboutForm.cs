using System;
using System.Diagnostics;
using System.Windows.Forms;

using CCWin;

namespace WEF.ModelGenerator.Forms
{
    public partial class AboutForm : Skin_Mac
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/yswenli/WEF/releases");
        }

        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            var str = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            var arr = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            label1.Text = $"{arr[0]} {arr[1]}";
            label2.Text = "developed by yswenli 2015-" + DateTime.Now.Year;
        }
    }
}
