
using System.Windows.Forms;

using CCWin;

namespace WEF.Standard.DevelopTools.Forms
{
    public partial class TextForm : Skin_Mac
    {
        public TextForm()
        {
            InitializeComponent();
        }

        public TextForm(string title, string content, bool readOnly = false) : this()
        {
            Title = title;
            Content = content;
            textBox1.ReadOnly = readOnly;
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }
        /// <summary>
        /// Content
        /// </summary>
        public string Content
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                textBox1.SelectAll();
            }
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if (!string.IsNullOrEmpty(textBox1.SelectedText))
                {
                    Clipboard.SetText(textBox1.SelectedText);
                }
            }
        }
    }
}
