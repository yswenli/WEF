/****************************************************************************
*项目名称：WEF.ModelGenerator.Forms
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.ModelGenerator.Forms
*类 名 称：SQLTemplateForm
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/2/19 9:35:47
*描述：
*=====================================================================
*修改时间：2019/2/19 9:35:47
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Properties;

namespace WEF.ModelGenerator.Forms
{
    public partial class SQLTemplateForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        Dictionary<string, string> _dic;

        public SQLTemplateForm()
        {
            InitializeComponent();
            this.CloseButtonVisible = false;

            _dic = new Dictionary<string, string>();

            _dic.Add("创建数据库", Resources.创建数据库);
            _dic.Add("创建索引", Resources.创建索引);
            _dic.Add("创建视图", Resources.创建视图);
            _dic.Add("删除主键", Resources.删除主键);
            _dic.Add("删除数据库", Resources.删除数据库);
            _dic.Add("删除索引", Resources.删除索引);
            _dic.Add("删除表", Resources.删除表);
            _dic.Add("删除视图", Resources.删除视图);
            _dic.Add("压缩数据库", Resources.压缩数据库);
            _dic.Add("基本的sql语句", Resources.基本的sql语句);
            _dic.Add("增加列", Resources.增加列);
            _dic.Add("备份SqlServer", Resources.备份SqlServer);
            _dic.Add("按姓氏笔画排序", Resources.按姓氏笔画排序);
            _dic.Add("收缩数据和日志", Resources.收缩数据和日志);
            _dic.Add("数据库加密", Resources.数据库加密);
            _dic.Add("根据已有的表创建新表", Resources.根据已有的表创建新表);
            _dic.Add("添加主键", Resources.添加主键);
            _dic.Add("用户表", Resources.用户表);
            _dic.Add("创建存储过程", Resources.创建存储过程);
            _dic.Add("游标", Resources.游标);
            _dic.Add("重建索引", Resources.重建索引);
            _dic.Add("查看存储过程", Resources.查看存储过程);
            _dic.Add("分页", Resources.分页);
            _dic.Add("分割字段并分组统计", Resources.分割字段并分组统计);
            _dic.Add("MSSQL启用ServiceBroker", Resources.MSSQL启用ServiceBroker);
            _dic.Add("SqlServer慢查询", Resources.SqlServer慢查询);


            comboBox1.Items.AddRange(_dic.Keys.ToArray());

            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var txt = comboBox1.Items[comboBox1.SelectedIndex].ToString();

            textBox1.Text = _dic[txt];
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            ShortcutKeyHelper.AllSelect(sender, e);
        }
    }
}
