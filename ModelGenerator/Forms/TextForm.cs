
using CCWin;

namespace WEF.ModelGenerator.Forms
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


    }
}
