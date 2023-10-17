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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;
using WEF.Standard.Mongo;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBMongo : CCWin.Skin_Mac
    {
        public DBMongo()
        {
            InitializeComponent();
        }

        ConnectionModel _connectionModel = new ConnectionModel();

        public DBMongo(ConnectionModel cm) : this()
        {
            _connectionModel = cm;
        }


        string connectStr = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                connectStr = textBox5.Text;
            }
            else
            {
                connectStr = $"mongodb://{textBox3.Text}:{textBox4.Text}@${textBox1.Text}/{textBox6.Text}?authSource=admin";
            }

            LoadForm.ShowLoading(this);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    MongoDBTool.Connect(connectStr).GetDataBases();

                    this.Invoke(new Action(() =>
                    {
                        button2.Enabled = true;
                        LoadForm.HideLoading(this);
                    }));
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading(this);
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
                    var mongoTool = MongoDBTool.Connect(connectStr);

                    var dataBaseNames = mongoTool.GetDataBases();

                    var dataBaseName = "";

                    if (dataBaseNames != null && dataBaseNames.Any())
                    {
                        if (System.Linq.Enumerable.Count(dataBaseNames) == 1)
                            dataBaseName = dataBaseNames.First();
                        else
                            dataBaseName = "all";
                    }

                    this.Invoke(new Action(() =>
                    {
                        _connectionModel.Database = dataBaseName;
                        if (_connectionModel.ID == Guid.Empty)
                            _connectionModel.ID = Guid.NewGuid();
                        _connectionModel.Name = mongoTool.ServerInfo + "(MongoDB)[" + _connectionModel.Database + "]";
                        _connectionModel.DbType = DatabaseType.MongoDB.ToString();
                        _connectionModel.ConnectionString = connectStr;

                        UtilsHelper.UpdateConnection(_connectionModel);

                        this.DialogResult = DialogResult.OK;

                        this.Close();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                }
                LoadForm.HideLoading(this);
            });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
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
    }


    public class MongoTestEntity : MongoEntity
    {

    }
}
