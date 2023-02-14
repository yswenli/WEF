using System;
using System.Collections.Generic;
using System.IO;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator
{
    public partial class LogShow : CCWin.Skin_Mac
    {
        public LogShow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 日志查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogShow_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Logger.ErrorPath))
            {
                string[] files = Directory.GetFiles(Logger.ErrorPath, "*.txt", SearchOption.TopDirectoryOnly);
                List<Filepath> list = new List<Filepath>();
                foreach (string file in files)
                {
                    if (string.IsNullOrEmpty(file))
                        continue;

                    list.Add(new Filepath(file));
                }
                cbmerrorlist.Items.AddRange(list.ToArray());
                cbmerrorlist.SelectedIndex = cbmerrorlist.Items.Count - 1;

                geterror();
            }
        }

        public class Filepath
        {
            public string FileName;
            public string FilePath;


            public Filepath(string file)
            {
                FilePath = file;
                FileName = file.Substring(file.LastIndexOf('\\') + 1);
            }

            public override string ToString()
            {
                return FileName;
            }
        }

        void geterror()
        {
            if (cbmerrorlist.SelectedIndex == -1)
            {
                txtLog.Text = string.Empty;
            }
            else
            {
                txtLog.Text = File.ReadAllText(Path.Combine(Logger.ErrorPath, ((Filepath)cbmerrorlist.SelectedItem).FilePath));
            }
        }


        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbmerrorlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            geterror();
        }
    }
}
