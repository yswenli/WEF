using System;

using CCWin;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class CollectViewForm : Skin_Mac
    {
        CollectInfo _collectInfo;

        CollectViewForm()
        {
            InitializeComponent();
        }

        public CollectViewForm(CollectInfo collectInfo) : this()
        {
            CollectInfo = collectInfo;

            if (CollectInfo == null)
            {
                CollectInfo = new CollectInfo()
                {
                    Name = $"未命名-{DateTime.Now:yyyyMMddHHmmss}",
                    Description = "-",
                    Content = "-",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                };
            };

            skinWaterTextBox1.Text = CollectInfo.Name;
            skinWaterTextBox2.Text = CollectInfo.Description;
            skinWaterTextBox3.Text = CollectInfo.Content;
        }

        public CollectInfo CollectInfo { get => _collectInfo; set => _collectInfo = value; }

        public event Action<CollectViewForm, CollectInfo> OnSeted;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CollectInfo.Name = skinWaterTextBox1.Text;
            CollectInfo.Description = skinWaterTextBox2.Text;
            CollectInfo.Content = skinWaterTextBox3.Text;
            OnSeted?.Invoke(this, CollectInfo);
        }
    }
}
