/****************************************************************************
*项目名称：WEF.ModelGenerator.DbSelect
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.ModelGenerator.DbSelect
*类 名 称：DBMongo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/4/25 16:49:33
*描述：
*=====================================================================
*修改时间：2019/4/25 16:49:33
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;
using WEF.NoSql;
using WEF.NoSql.Model;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBMongo : CCWin.Skin_Mac
    {
        public DBMongo()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                panel1.Enabled = false;
                textBox5.Enabled = true;
            }
            else
            {
                panel1.Enabled = true;
                textBox5.Enabled = false;
            }
        }

        string connectStr = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                connectStr = textBox5.Text;
            }
            else
            {
                connectStr = $"mongodb://{textBox3.Text}:{textBox4.Text}@${textBox1.Text}:{textBox2.Text}/{textBox6.Text}?authSource=admin";
            }

            LoadForm.ShowLoading(this);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var dbOperator = MongoDBFactory.Create<TestModel>(connectStr);
                    var cas = dbOperator.DataBaseName;
                    this.Invoke(new Action(() =>
                    {
                        button2.Enabled = true;
                        button1.Enabled = false;
                        LoadForm.HideLoading();
                    }));
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading();
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "连接失败!\n\r" + ex.Message);
                    }));

                }
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                LoadForm.ShowLoading(this);
                try
                {
                    var dbOperator = MongoDBFactory.Create<TestModel>(connectStr);

                    this.Invoke(new Action(() =>
                    {
                        Connection connectionModel = new Connection();
                        connectionModel.Database = dbOperator.DataBaseName ?? "all";
                        connectionModel.ID = Guid.NewGuid();
                        connectionModel.Name = dbOperator.ServerInfo + "(" + dbOperator.ServerInfo + ")[" + connectionModel.Database + "]";
                        connectionModel.DbType = DatabaseType.MongoDB.ToString();
                        connectionModel.ConnectionString = connectStr;

                        UtilsHelper.AddConnection(connectionModel);

                        this.DialogResult = DialogResult.OK;

                        this.Close();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                }
                LoadForm.HideLoading();
            });
        }
    }


    public class TestModel : MongoEntity
    {

    }
}
