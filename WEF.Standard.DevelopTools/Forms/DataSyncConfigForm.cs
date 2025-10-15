using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.Forms
{
    public partial class DataSyncConfigForm : Skin_Mac
    {

        DataSyncConfig _dataSyncConfig = null;

        DataSyncConfigForm()
        {
            InitializeComponent();
            label1.Dock = DockStyle.Fill;
        }

        public DataSyncConfigForm(DataSyncConfig dataSyncConfig) : this()
        {
            _dataSyncConfig = dataSyncConfig;
        }


        private void DataSyncConfigForm_Load(object sender, EventArgs e)
        {
            if (_dataSyncConfig == null)
            {
                _dataSyncConfig = new DataSyncConfig();
                _dataSyncConfig.ID = Guid.NewGuid().ToString();
            }

            InitAsync();
        }

        async void InitAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Invoke(() =>
                    {
                        label1.Visible = true;
                        if (!string.IsNullOrEmpty(_dataSyncConfig.Name))
                        {
                            skinWaterTextBox2.Text = _dataSyncConfig.Name;
                        }
                    });

                    var clist = UtilsHelper.GetConnectionList();

                    this.Invoke(() =>
                    {
                        comboBox1.Items.Clear();

                        foreach (var item in clist)
                        {
                            comboBox1.Items.Add(item.Name);

                            if (_dataSyncConfig.Source != null && _dataSyncConfig.Source.Name == item.Name)
                            {
                                comboBox1.SelectedItem = item.Name;
                            }
                        }
                    });

                    this.Invoke(() =>
                    {
                        comboBox2.Items.Clear();
                        foreach (var item in clist)
                        {
                            comboBox2.Items.Add(item.Name);

                            if (_dataSyncConfig.Target != null && _dataSyncConfig.Target.Name == item.Name)
                            {
                                comboBox2.SelectedItem = item.Name;
                            }
                        }

                        textBox1.Text = _dataSyncConfig.Source?.Sql ?? "";
                    });


                    if (_dataSyncConfig.Target == null)
                    {
                        return;
                    }

                    var dbObject = DBObjectHelper.GetDBObject(_dataSyncConfig.Target);

                    var tables = dbObject.GetTables(_dataSyncConfig.Target.Database);

                    if (tables != null && tables.Rows != null && tables.Rows.Count > 0)
                    {
                        this.Invoke(() =>
                        {
                            comboBox3.Items.Clear();

                            foreach (DataRow dr in tables.Rows)
                            {
                                comboBox3.Items.Add(dr[0].ToString());

                                if (dr[0].ToString() == _dataSyncConfig.Target.TableName)
                                {
                                    comboBox3.SelectedItem = dr[0].ToString();
                                }
                            }
                        });
                    }

                    this.Invoke(() =>
                    {
                        checkBox1.Checked = _dataSyncConfig.IsEnabled;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载配置有误，" + ex.Message);
                    this.Invoke(() =>
                    {
                        Close();
                    });
                }
                finally
                {
                    this.Invoke(() => label1.Visible = false);
                }
            });
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectValue = comboBox2.SelectedItem.ToString();
            var owner = this;
            LoadForm.ShowLoading(owner);
            Task.Run(() =>
            {
                try
                {
                    var clist = UtilsHelper.GetConnectionList();

                    if (clist == null) return;

                    var connectModel = clist.FirstOrDefault(b => b.Name == selectValue);

                    if (connectModel == null)
                    {
                        return;
                    }

                    var dbObject = DBObjectHelper.GetDBObject(connectModel);

                    var tables = dbObject.GetTables(connectModel.Database);

                    if (tables != null && tables.Rows != null && tables.Rows.Count > 0)
                    {
                        owner.Invoke(new Action(() =>
                        {
                            comboBox3.Items.Clear();

                            foreach (DataRow dr in tables.Rows)
                            {
                                comboBox3.Items.Add(dr[0].ToString());

                                if (_dataSyncConfig.Target != null && dr[0].ToString() == _dataSyncConfig.Target.TableName)
                                {
                                    comboBox3.SelectedItem = dr[0].ToString();
                                }
                            }
                        }));
                    }
                }
                catch
                {
                    owner.Invoke(new Action(() => MessageBox.Show(owner, "当前目的数据源无效，未配置数据库")));
                }
                finally
                {
                    LoadForm.HideLoading(owner);
                }
            });
        }


        void ShowSelectDBInit()
        {
            DatabaseSelect dbSelect = new DatabaseSelect();

            if (dbSelect.ShowDialog() == DialogResult.OK)
            {
                DialogResult dia = DialogResult.No;

                switch (DatabaseSelect.databaseType)
                {
                    case DatabaseType.SqlServer:
                    case DatabaseType.SqlServer9:
                        DbSelect.DBSqlServer dbsqlserver = new WEF.Standard.DevelopTools.DbSelect.DBSqlServer();
                        dia = dbsqlserver.ShowDialog();
                        break;
                    case DatabaseType.MsAccess:
                        DbSelect.DBMsAccess dbMsAccess = new WEF.Standard.DevelopTools.DbSelect.DBMsAccess();
                        dia = dbMsAccess.ShowDialog();
                        break;
                    case DatabaseType.Oracle:
                        DbSelect.DBOracle dbOracle = new WEF.Standard.DevelopTools.DbSelect.DBOracle();
                        dia = dbOracle.ShowDialog();
                        break;
                    case DatabaseType.Sqlite3:
                        DbSelect.DbSqlite dbSqlite = new WEF.Standard.DevelopTools.DbSelect.DbSqlite();
                        dia = dbSqlite.ShowDialog();
                        break;
                    case DatabaseType.MySql:
                        DbSelect.DBMySql dbMySql = new WEF.Standard.DevelopTools.DbSelect.DBMySql();
                        dia = dbMySql.ShowDialog();
                        break;
                    case DatabaseType.PostgreSQL:
                        DbSelect.DBPostgre dbPostgreSql = new WEF.Standard.DevelopTools.DbSelect.DBPostgre();
                        dia = dbPostgreSql.ShowDialog();
                        break;
                    case DatabaseType.MongoDB:
                        DbSelect.DBMongo dbMongo = new WEF.Standard.DevelopTools.DbSelect.DBMongo();
                        dia = dbMongo.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowSelectDBInit();
            InitAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowSelectDBInit();
            InitAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(skinWaterTextBox2.Text))
            {
                MessageBox.Show(this, "数据同步配置名称不能为空");
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show(this, "源数据不能为空");
                return;
            }

            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show(this, "目的数据不能为空");
                return;
            }

            var sql = textBox1.Text;

            if (string.IsNullOrEmpty(sql))
            {
                MessageBox.Show(this, "源数据SQL不能为空");
                return;
            }

            if (comboBox3.SelectedItem == null)
            {
                MessageBox.Show(this, "目的数据表名不能为空");
                return;
            }

            var clist = UtilsHelper.GetConnectionList();

            _dataSyncConfig.Name = skinWaterTextBox2.Text;
            _dataSyncConfig.Source = clist.FirstOrDefault(b => b.Name == comboBox1.SelectedItem.ToString());
            if (string.IsNullOrEmpty(_dataSyncConfig.Source.Database) || _dataSyncConfig.Source.Database == "all")
            {
                MessageBox.Show(this, "源数据配置中数据库未指定，请重新配置源数据配置");
                return;
            }
            _dataSyncConfig.Target = clist.FirstOrDefault(b => b.Name == comboBox2.SelectedItem.ToString());
            _dataSyncConfig.Source.Sql = sql;
            _dataSyncConfig.Target.TableName = comboBox3.SelectedItem.ToString();
            _dataSyncConfig.IsEnabled = checkBox1.Checked;


            UtilsHelper.DeleteConnection(_dataSyncConfig.Source.ID.ToString());
            UtilsHelper.AddConnection(_dataSyncConfig.Source);

            UtilsHelper.DeleteConnection(_dataSyncConfig.Target.ID.ToString());
            UtilsHelper.AddConnection(_dataSyncConfig.Target);

            SetCongfig(_dataSyncConfig);

            Close();
        }


        void SetCongfig(DataSyncConfig dataSyncConfig)
        {
            try
            {
                var configs = DataSyncConfig.Read();

                if (configs == null)
                {
                    configs = new List<DataSyncConfig>();
                }
                var old = configs.FirstOrDefault(b => b.ID == dataSyncConfig.ID);
                var created = DateTime.Now;
                if (old != null)
                {
                    created = old.Created;
                    configs.Remove(old);
                }
                dataSyncConfig.Created = created;
                configs.Add(dataSyncConfig);
                DataSyncConfig.Save(configs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "操作失败，ex:" + ex.Message);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_dataSyncConfig != null)
            {
                _dataSyncConfig.IsEnabled = checkBox1.Checked;
            }
        }


    }
}
