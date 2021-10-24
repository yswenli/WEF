using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class CollectForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        static List<CollectInfo> _collectInfos = new List<CollectInfo>();

        public CollectForm()
        {
            InitializeComponent();

            this.CloseButtonVisible = false;

            //listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);            

            InitListView();
        }

        void InitListView()
        {
            _collectInfos.Clear();
            _collectInfos = CollectInfo.Read();
            if (_collectInfos == null)
            {
                _collectInfos = new List<CollectInfo>();
            }
            LoadListView(_collectInfos);
        }

        void LoadListView(List<CollectInfo> collectInfos)
        {
            if (_collectInfos == null)
            {
                _collectInfos = new List<CollectInfo>();
            }
            listView1.Items.Clear();
            foreach (var item in collectInfos)
            {
                var view = new ListViewItem(item.Name);
                view.Tag = item;
                view.ToolTipText = item.Description;
                listView1.Items.Add(view);
            }
        }

        public static List<CollectInfo> GetCollectNameList(string key)
        {
            return _collectInfos.Where(q => q.Name.IndexOf(key, StringComparison.OrdinalIgnoreCase) > -1 || q.Description.IndexOf(key, StringComparison.OrdinalIgnoreCase) > -1).ToList();
        }

        public static CollectInfo GetCollectInfo(string name)
        {
            return _collectInfos.Where(q => q.Name == name).FirstOrDefault();
        }

        #region menus


        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var cvf = new CollectViewForm(null);
            cvf.OnSeted += Cvf_OnSeted;
            cvf.ShowDialog(this);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select = listView1.SelectedItems;
            if (select == null || select.Count == 0)
            {
                return;
            }
            var old = (CollectInfo)select[0].Tag;
            if (old == null) return;
            var cvf = new CollectViewForm(old);
            cvf.OnSeted += Cvf_OnSeted;
            cvf.ShowDialog(this);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var select = listView1.SelectedItems;

            if (select == null || select.Count == 0)
            {
                return;
            }

            var old = (CollectInfo)select[0].Tag;

            if (old == null) return;

            if (MessageBox.Show(this, "确定要删除此项吗？", "删除收藏", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _collectInfos.Remove(old);
                CollectInfo.Save(_collectInfos);
                InitListView();
            }
        }

        private void Cvf_OnSeted(CollectViewForm arg1, CollectInfo arg2)
        {
            if (arg2 != null && !string.IsNullOrEmpty(arg2.Name))
            {
                var old = GetCollectInfo(arg2.Name);
                if (old != null)
                {
                    _collectInfos.Remove(old);
                }
                _collectInfos.Insert(0, arg2);

                CollectInfo.Save(_collectInfos);
                InitListView();
            }
            else
            {
                MessageBox.Show(this, "必填项不能为空");
            }
            arg1?.Close();
        }

        #endregion


        private void skinWaterTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(skinWaterTextBox1.Text))
            {
                LoadListView(_collectInfos);
            }
            else
            {
                var list = _collectInfos.Where(q => q.Name.IndexOf(skinWaterTextBox1.Text, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                LoadListView(list);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            editToolStripMenuItem_Click(null, null);
        }

        ListViewItem _oldListViewItem = null;

        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (_oldListViewItem != e.Item)
            {
                if (_oldListViewItem != null)
                {
                    _oldListViewItem.BackColor = SystemColors.Window;
                    _oldListViewItem.ForeColor = SystemColors.ControlText;
                }
                _oldListViewItem = e.Item;
                _oldListViewItem.BackColor = SystemColors.HotTrack;
                _oldListViewItem.ForeColor = SystemColors.HighlightText;
            }
        }
    }
}
