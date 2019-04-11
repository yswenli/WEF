using System.Diagnostics;
using System.Windows.Forms;

namespace WEF.ModelGenerator.Forms
{
    public partial class AboutForm : CCWin.Skin_Mac
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
            label1.Text = $"{System.Reflection.Assembly.GetExecutingAssembly().FullName}";
        }
    }
}
