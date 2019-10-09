using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEF.ModelGenerator
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {
            skinLabel2.Text = "1秒";
        }

        public void SetTime(int sec)
        {
            skinLabel2.Text = $"{sec}秒";
        }

        static LoadForm loadForm = null;



        public static void ShowLoading(Form parent)
        {
            int i = 0;

            loadForm = new LoadForm();

            Task.Factory.StartNew(() =>
            {
                while (loadForm != null)
                {
                    try
                    {
                        if (loadForm.IsHandleCreated)
                            loadForm.Invoke(new Action(() =>
                            {
                                i++;
                                if (loadForm != null)
                                    if (loadForm.Visible)
                                    {
                                        loadForm.SetTime(i);
                                    }
                                    else
                                    {
                                        i = 0;
                                    }
                            }));
                    }
                    catch
                    {
                        loadForm = null;
                        i = 0;
                    }
                    Thread.Sleep(1000);
                }
            });

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);

                parent.Invoke(new Action(() =>
                {
                    if (loadForm != null)
                        loadForm.ShowDialog(parent);
                }));
            });
        }

        public static void HideLoading(int sec = 0)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep((sec == 0 ? 1 : sec) * 1000);
                if (loadForm != null && loadForm.IsHandleCreated)
                    loadForm.BeginInvoke(new Action(() =>
                    {
                        loadForm.Close();
                        loadForm = null;
                    }), null);
            });
        }
    }
}
