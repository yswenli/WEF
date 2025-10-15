/****************************************************************************
*项目名称：WEF.Standard.DevelopTools.Forms
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.Standard.DevelopTools.Forms
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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.DbDAL;
using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Forms;
using WEF.Standard.DevelopTools.Model;
using WEF.Standard.Mongo;

namespace WEF.Standard.DevelopTools
{
    public partial class LeftPanelForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        string _conConnectionString;

        public LeftPanelForm()
        {
            InitializeComponent();
            Treeview.ExpandAll();
        }

        public event Action<ConnectionModel> OnNewDesignForm;

        public delegate void OnNewContentFormHandler(ConnectionModel conModel);

        public event OnNewContentFormHandler OnNewContentForm;

        public event Action<ConnectionModel> OnNewSqlForm;

        public event Action<ConnectionModel> OnNewDataForm;

        /// <summary>
        /// 新建数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowDbSelect();
        }

        /// <summary>
        /// 编辑连接菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = Treeview.SelectedNode;

            if (node == null || node.Level != 1) return;

            var id = node.Tag?.ToString() ?? "";

            if (string.IsNullOrEmpty(id)) return;

            var cm = UtilsHelper.GetConnectionList().Where(q => q.ID == Guid.Parse(id)).FirstOrDefault();

            if (cm == null) return;

            DatabaseSelect.databaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), cm.DbType);

            DialogResult dia = DialogResult.No;

            switch (DatabaseSelect.databaseType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.SqlServer9:
                    DbSelect.DBSqlServer dbsqlserver = new WEF.Standard.DevelopTools.DbSelect.DBSqlServer(cm);
                    dia = dbsqlserver.ShowDialog();
                    break;
                case DatabaseType.MsAccess:
                    DbSelect.DBMsAccess dbMsAccess = new WEF.Standard.DevelopTools.DbSelect.DBMsAccess(cm);
                    dia = dbMsAccess.ShowDialog();
                    break;
                case DatabaseType.Oracle:
                    DbSelect.DBOracle dbOracle = new WEF.Standard.DevelopTools.DbSelect.DBOracle(cm);
                    dia = dbOracle.ShowDialog();
                    break;
                case DatabaseType.Sqlite3:
                    DbSelect.DbSqlite dbSqlite = new WEF.Standard.DevelopTools.DbSelect.DbSqlite(cm);
                    dia = dbSqlite.ShowDialog();
                    break;
                case DatabaseType.MySql:
                    DbSelect.DBMySql dbMySql = new WEF.Standard.DevelopTools.DbSelect.DBMySql(cm);
                    dia = dbMySql.ShowDialog();
                    break;
                case DatabaseType.PostgreSQL:
                    DbSelect.DBPostgre dbPostgreSql = new WEF.Standard.DevelopTools.DbSelect.DBPostgre(cm);
                    dia = dbPostgreSql.ShowDialog();
                    break;
                case DatabaseType.MongoDB:
                    DbSelect.DBMongo dbMongo = new WEF.Standard.DevelopTools.DbSelect.DBMongo(cm);
                    dia = dbMongo.ShowDialog();
                    break;
                default:
                    break;
            }

            if (dia == DialogResult.OK)
            {
                refreshConnectionList();
            }

        }
        /// <summary>
        /// 查看日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ShowLogs();
        }

        public void ShowLogs()
        {
            LogShow ls = new LogShow();
            ls.ShowDialog();
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        public void ShowDbSelect()
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

                if (dia == DialogResult.OK)
                {
                    refreshConnectionList();
                }
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        List<ConnectionModel> _connectList;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftPanel_Load(object sender, EventArgs e)
        {
            this.CloseButton = false;
            getServers();
        }

        #region top servers


        /// <summary>
        /// 获取服务器列表
        /// </summary>
        private void getServers()
        {
            Treeview.Nodes.Clear();

            Treeview.Nodes.Add("服务器", "数据库服务器", 0);

            _connectList = UtilsHelper.GetConnectionList();
            if (_connectList == null || _connectList.Count == 0) return;

            TreeNode node = Treeview.Nodes[0];

            node.ContextMenuStrip = contextMenuStripTop;

            foreach (ConnectionModel connection in _connectList)
            {
                TreeNode nnode = new TreeNode(connection.Name, 0, 0);
                nnode.ContextMenuStrip = contextMenuStripDatabase;
                nnode.Tag = connection.ID.ToString();
                nnode.ToolTipText = connection.ConnectionString;                
                node.Nodes.Add(nnode);
            }

            Treeview.ExpandAll();
        }

        /// <summary>
        /// 双击treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tview_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;

            if (node == null) return;

            if (node.Level == 0)
            {
                node.Nodes.Clear();
                getServers();
                Treeview.ExpandAll();
            }
            else if (node.Level == 1)
            {
                node.Nodes.Clear();

                MessageQueue.Instance.SubMsg("正在准备加载数据库结构");

                getDatabaseinfo(node);
            }
            else if (node.Level == 4)
            {
                if (null != OnNewContentForm)
                {
                    ConnectionModel conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(Treeview.SelectedNode.Parent.Parent.Parent.Tag.ToString()); });

                    if (conModel.DbType.Equals(DatabaseType.MongoDB.ToString()))
                    {
                        //todo

                        MongoDBTool.Connect(conModel.ConnectionString).GetList(node.Parent.Parent.Text, node.Text, "{\"find\":\"Account\", limit:20, sort:{AddTime:-1}}");
                    }
                    else
                    {
                        _conConnectionString = Treeview.SelectedNode.Parent.Parent.Tag.ToString();
                        try
                        {
                            conModel.TableName = Treeview.SelectedNode.Text;
                            conModel.Database = Treeview.SelectedNode.Parent.Parent.Text;
                            conModel.IsView = Treeview.SelectedNode.Tag.ToString().Equals("V");
                            OnNewContentForm?.Invoke(conModel);
                        }
                        catch { }
                    }
                }
            }

        }

        /// <summary>
        /// 右击选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((TreeView)sender).SelectedNode = e.Node;
            }
        }



        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDbSelect();
        }

        /// <summary>
        /// 刷新服务器
        /// </summary>
        private void refreshConnectionList()
        {
            if (_connectList == null) _connectList = new List<ConnectionModel>();

            List<ConnectionModel> connList = UtilsHelper.GetConnectionList();

            foreach (ConnectionModel conn in connList)
            {
                ConnectionModel tempconn = _connectList.Find(delegate (ConnectionModel connin) { return conn.ID.ToString().Equals(connin.ID.ToString()); });
                if (null == tempconn)
                {
                    TreeNode nnode = new TreeNode(conn.Name, 0, 0);
                    nnode.ContextMenuStrip = contextMenuStripDatabase;
                    nnode.Tag = conn.ID.ToString();
                    Treeview.Nodes[0].Nodes.Add(nnode);
                }
            }
            _connectList = connList;

            getServers();
        }


        /// <summary>
        /// 刷新菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshConnectionList();
            Treeview.ExpandAll();
        }

        /// <summary>
        /// remove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "确定要移除当前配置么？", "WEF数据库工具", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                string stringid = Treeview.SelectedNode.Tag.ToString();
                UtilsHelper.DeleteConnection(stringid);
                ConnectionModel tempconn = _connectList.Find(delegate (ConnectionModel conn) { return conn.ID.ToString().Equals(stringid); });
                if (null != tempconn)
                    _connectList.Remove(tempconn);
                Treeview.Nodes.Remove(Treeview.SelectedNode);
            }
        }
        #endregion

        #region database

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            if (node != null && node.Level > 1)
            {
                node.Nodes.Clear();
                getDatabaseinfo(node);
            }
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            if (node != null && node.Level > 1)
            {
                node.Nodes.Clear();
                getDatabaseinfo(node);
                Treeview.ExpandAll();
            }
        }

        private void viewConnectStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            var conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Tag.ToString()); });
            _conConnectionString = conModel.ConnectionString;
            Clipboard.SetText(_conConnectionString);
            MessageBox.Show($"已复制到剪切板，conConnectionString：\r\n{_conConnectionString}", "WEF数据库工具", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /// <summary>
        /// 异步获取数据库服务器
        /// </summary>
        private void getDatabaseinfo(TreeNode node)
        {
            LoadForm.ShowLoading(this);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    ConnectionModel conModel = null;

                    this.Invoke(new Action(() =>
                    {
                        conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Tag?.ToString() ?? ""); });
                    }));

                    _conConnectionString = conModel.ConnectionString;

                    IDbObject dbObject;

                    if (conModel.DbType.Equals(DatabaseType.MsAccess.ToString()))
                    {
                        dbObject = new WEF.DbDAL.OleDb.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node);
                    }
                    else if (conModel.DbType.Equals(DatabaseType.Sqlite3.ToString()))
                    {
                        dbObject = new WEF.DbDAL.SQLite.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node);
                    }
                    else if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()) || conModel.DbType.Equals(DatabaseType.SqlServer9.ToString()))
                    {
                        if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()))
                            dbObject = new WEF.DbDAL.SQL2000.DbObject(_conConnectionString);
                        else
                            dbObject = new WEF.DbDAL.SQLServer.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node, "master");
                    }
                    else if (conModel.DbType.Equals(DatabaseType.Oracle.ToString()))
                    {
                        dbObject = new WEF.DbDAL.Oracle.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node, "oracle");
                    }
                    else if (conModel.DbType.Equals(DatabaseType.MySql.ToString()))
                    {
                        dbObject = new WEF.DbDAL.MySql.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node, "master");
                    }
                    else if (conModel.DbType.Equals(DatabaseType.PostgreSQL.ToString()))
                    {
                        dbObject = new WEF.DbDAL.PostgreSQL.DbObject(_conConnectionString);

                        LoadDataBaseForNode(dbObject, conModel, node, "postgres");
                    }
                    else if (conModel.DbType.Equals(DatabaseType.MongoDB.ToString()))
                    {
                        var dataBaseName = "admin";

                        var mongoDBTool = MongoDBTool.Connect(_conConnectionString);

                        if (conModel.Database.Equals("all"))
                        {
                            var dataBaseNames = mongoDBTool.GetDataBases();

                            foreach (var dbs in dataBaseNames)
                            {
                                TreeNode tnode = null;

                                this.Invoke(new Action(() =>
                                {
                                    tnode = new TreeNode(dbs, 1, 1);
                                    tnode.Tag = _conConnectionString.Replace(dataBaseName, dbs);
                                    tnode.ContextMenuStrip = contextMenuStripOneMongoDB;
                                    node.Nodes.Add(tnode);
                                }));

                                var cs = mongoDBTool.GetCollections(dbs);

                                this.Invoke(new Action(() =>
                                {
                                    ShowCollections(tnode, cs);
                                }));
                            }

                        }
                        else
                        {
                            dataBaseName = conModel.Database;

                            TreeNode tnode = null;

                            this.Invoke(new Action(() =>
                            {
                                tnode = new TreeNode(conModel.Database, 1, 1);
                                tnode.Tag = _conConnectionString;
                                tnode.ContextMenuStrip = contextMenuStripOneMongoDB;
                                node.Nodes.Add(tnode);
                            }));

                            var collections = mongoDBTool.GetCollections(dataBaseName);

                            this.Invoke(new Action(() =>
                            {
                                ShowCollections(tnode, collections);
                            }));
                        }


                    }
                    LoadForm.HideLoading(this); ;
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading(this); ;
                    MessageBox.Show(ex.Message, "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        node.Expand();
                    }));
                }
            });
        }


        void LoadDataBaseForNode(IDbObject dbObject, ConnectionModel conModel, TreeNode node, string replaceStr)
        {
            MessageQueue.Instance.SubMsg($"正在加载服务器数据库结构...");

            if (conModel.Database.Equals("all"))
            {
                DataTable dt = dbObject.GetDBList();

                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode tnode = null;

                    this.Invoke(new Action(() =>
                    {
                        tnode = new TreeNode(dr[0].ToString(), 1, 1);
                        tnode.Tag = _conConnectionString.Replace(replaceStr, dr[0].ToString());
                        tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                        node.Nodes.Add(tnode);
                        MessageQueue.Instance.SubMsg($"正在加载数据库{tnode.Text}结构...");
                    }));

                    var txt = tnode.Text;

                    Task.Run(() =>
                    {
                        var tables = dbObject.GetTables(txt);

                        var views = dbObject.GetVIEWs(txt);

                        this.Invoke(new Action(() =>
                        {
                            ShowTablesAndViews(tnode, tables, views);
                            MessageQueue.Instance.SubMsg($"加载数据库{txt}结构已完成");
                        }));
                    });
                }
            }
            else
            {
                TreeNode tnode = null;

                this.Invoke(new Action(() =>
                {
                    tnode = new TreeNode(conModel.Database, 1, 1);
                    tnode.Tag = _conConnectionString;
                    tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                    node.Nodes.Add(tnode);
                    MessageQueue.Instance.SubMsg($"正在加载数据库{tnode.Text}结构...");
                }));

                var txt = tnode.Text;

                Task.Run(() =>
                {
                    var tables = dbObject.GetTables(txt);

                    var views = dbObject.GetVIEWs(txt);

                    this.Invoke(new Action(() =>
                    {
                        ShowTablesAndViews(tnode, tables, views);
                        MessageQueue.Instance.SubMsg($"加载数据库{txt}结构已完成");
                    }));
                });
            }
        }

        void LoadDataBaseForNode(IDbObject dbObject, ConnectionModel conModel, TreeNode node)
        {
            MessageQueue.Instance.SubMsg($"正在加载服务器数据库结构...");

            TreeNode tnode = null;

            this.Invoke(new Action(() =>
            {
                tnode = new TreeNode(conModel.Database, 1, 1);
                tnode.Tag = _conConnectionString;
                tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                node.Nodes.Add(tnode);
            }));

            Task.Run(() =>
            {
                var tables = dbObject.GetTables("");

                var views = dbObject.GetVIEWs("");

                this.Invoke(new Action(() =>
                {
                    ShowTablesAndViews(tnode, tables, views);
                    MessageQueue.Instance.SubMsg("服务器数据库结构已完成");
                }));
            });

        }

        /// <summary>
        /// 获取数据表信息
        /// </summary>
        /// <param name="databaseNode"></param>
        /// <param name="tables"></param>
        /// <param name="views"></param>
        private void ShowTablesAndViews(TreeNode databaseNode, DataTable tables, DataTable views)
        {
            if (databaseNode.Level == 3)
            {
                if (databaseNode.Text == "数据表")
                {
                    databaseNode.ContextMenuStrip = contextMenuStripOneDataBase;

                    if (null != tables && tables.Rows.Count > 0)
                    {
                        DataRow[] tablesdrs = tables.Select("", "name asc");
                        foreach (DataRow tablesDR in tablesdrs)
                        {
                            TreeNode tnode = new TreeNode(tablesDR[0].ToString(), 4, 4);
                            tnode.Tag = "T";
                            tnode.ContextMenuStrip = contextMenuStripTable;
                            databaseNode.Nodes.Add(tnode);

                            if (!StringPlus.SQLKeyWords.Contains(tnode.Text))
                            {
                                StringPlus.SQLKeyWords.Add(tnode.Text);
                            }
                        }
                    }
                }

                if (databaseNode.Text == "视图")
                {
                    databaseNode.ContextMenuStrip = contextMenuStripOneDataBase;

                    DataRow[] viewsdrs = views.Select("", "name asc");

                    foreach (DataRow viewsDR in viewsdrs)
                    {
                        TreeNode tnode = new TreeNode(viewsDR[0].ToString(), 4, 4);
                        tnode.Tag = "V";
                        tnode.ContextMenuStrip = contextMenuStripTable;
                        databaseNode.Nodes.Add(tnode);

                        if (!StringPlus.SQLKeyWords.Contains(tnode.Text))
                        {
                            StringPlus.SQLKeyWords.Add(tnode.Text);
                        }
                    }
                }
            }
            else
            {
                TreeNode tableNode = new TreeNode("数据表", 2, 3);

                tableNode.ContextMenuStrip = contextMenuStripOneDataBase;

                if (null != tables && tables.Rows.Count > 0)
                {
                    DataRow[] tablesdrs = tables.Select("", "name asc");
                    foreach (DataRow tablesDR in tablesdrs)
                    {
                        TreeNode tnode = new TreeNode(tablesDR[0].ToString(), 4, 4);
                        tnode.Tag = "T";
                        tnode.ContextMenuStrip = contextMenuStripTable;
                        tableNode.Nodes.Add(tnode);

                        if (!StringPlus.SQLKeyWords.Contains(tnode.Text))
                        {
                            StringPlus.SQLKeyWords.Add(tnode.Text);
                        }
                    }
                }
                databaseNode.Nodes.Add(tableNode);

                TreeNode viewNode = new TreeNode("视图", 2, 3);

                viewNode.ContextMenuStrip = contextMenuStripOneDataBase;

                DataRow[] viewsdrs = views.Select("", "name asc");

                foreach (DataRow viewsDR in viewsdrs)
                {
                    TreeNode tnode = new TreeNode(viewsDR[0].ToString(), 4, 4);
                    tnode.Tag = "V";
                    tnode.ContextMenuStrip = contextMenuStripTable;
                    viewNode.Nodes.Add(tnode);

                    if (!StringPlus.SQLKeyWords.Contains(tnode.Text))
                    {
                        StringPlus.SQLKeyWords.Add(tnode.Text);
                    }
                }
                databaseNode.Nodes.Add(viewNode);
            }
        }


        /// <summary>
        /// 获取数据集合信息
        /// </summary>
        /// <param name="databaseNodel"></param>
        /// <param name="collections"></param>
        private void ShowCollections(TreeNode databaseNodel, IEnumerable<string> collections)
        {
            if (databaseNodel.Level == 3)
            {
                if (databaseNodel.Text == "数据集合")
                {
                    databaseNodel.ContextMenuStrip = contextMenuStripOneMongoDB;

                    if (null != collections && collections.Any())
                    {
                        foreach (var item in collections)
                        {
                            TreeNode tnode = new TreeNode(item, 4, 4);
                            tnode.Tag = "T";
                            tnode.ContextMenuStrip = contextMenuStripOneMongoDB;
                            databaseNodel.Nodes.Add(tnode);
                        }
                    }
                }
            }
            else
            {
                TreeNode tableNode = new TreeNode("数据集合", 2, 3);

                tableNode.ContextMenuStrip = contextMenuStripOneMongoDB;

                if (null != collections && collections.Any())
                {
                    foreach (var item in collections)
                    {
                        TreeNode tnode = new TreeNode(item, 4, 4);
                        tnode.Tag = "T";
                        tnode.ContextMenuStrip = contextMenuStripOneMongoDB;
                        tableNode.Nodes.Add(tnode);
                    }
                }
                databaseNodel.Nodes.Add(tableNode);
            }
        }
        #endregion


        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 生成代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != OnNewContentForm)
            {
                ConnectionModel conModel = _connectList.Find(
                    delegate (ConnectionModel con)
                    {
                        return con.ID.ToString().Equals(Treeview.SelectedNode.Parent.Parent.Parent.Tag.ToString());
                    });
                _conConnectionString = Treeview.SelectedNode.Parent.Parent.Tag.ToString();
                conModel.TableName = Treeview.SelectedNode.Text;
                conModel.Database = Treeview.SelectedNode.Parent.Parent.Text;
                conModel.IsView = Treeview.SelectedNode.Tag.ToString().Equals("V");
                OnNewContentForm(conModel);
            }
        }

        /// <summary>
        /// 删除表/视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                TreeNode node = Treeview.SelectedNode;

                if (node != null && node.Level == 4)
                {
                    if (MessageBox.Show(this, "确定要删除表/视图", "WEF数据库工具", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                    var tableName = node.Text;
                    var conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
                    conModel.Database = node.Parent.Parent.Text;
                    WEF.DbDAL.IDbObject dbObject = DBObjectHelper.GetDBObject(conModel);

                    var result = false;

                    if (node.Parent.Text == "数据表")
                    {
                        result = dbObject.DeleteTable(conModel.Database, tableName);
                    }
                    else if (node.Parent.Text == "视图")
                    {
                        result = dbObject.DeleteView(conModel.Database, tableName);
                    }

                    if (result)
                    {
                        MessageBox.Show(this, "删除表/视图操作成功", "WEF数据库工具");
                        Treeview.Nodes.Remove(node);
                        return;
                    }
                    MessageBox.Show(this, "删除表/视图操作失败", "WEF数据库工具");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "删除表操作失败,Error:" + ex.Message, "WEF数据库工具");
            }
        }

        /// <summary>
        /// 刷新数据库表和视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;

            if (node.Level == 3)
            {
                node = node.Parent;
            }
            if (node.Level == 4)
            {
                node = node.Parent.Parent;
            }

            node.Nodes.Clear();

            ConnectionModel conModel = null;
            try
            {
                conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });

                IDbObject dbObject;

                if (conModel.DbType.Equals(DatabaseType.MsAccess.ToString()))
                {
                    dbObject = new WEF.DbDAL.OleDb.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(""), dbObject.GetVIEWs(""));
                }
                else if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()))
                {
                    dbObject = new WEF.DbDAL.SQL2000.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
                }
                else if (conModel.DbType.Equals(DatabaseType.SqlServer9.ToString()))
                {
                    dbObject = new WEF.DbDAL.SQLServer.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
                }
                else if (conModel.DbType.Equals(DatabaseType.Oracle.ToString()))
                {
                    dbObject = new WEF.DbDAL.Oracle.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
                }
                else if (conModel.DbType.Equals(DatabaseType.MySql.ToString()))
                {
                    dbObject = new WEF.DbDAL.MySql.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
                }
                else if (conModel.DbType.Equals(DatabaseType.Sqlite3.ToString()))
                {
                    dbObject = new WEF.DbDAL.SQLite.DbObject(_conConnectionString);
                    ShowTablesAndViews(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
                }
                else if (conModel.DbType.Equals(DatabaseType.MongoDB.ToString()))
                {
                    var mongoDBTool = MongoDBTool.Connect(_conConnectionString);
                    ShowCollections(node, mongoDBTool.GetCollections(node.Text));
                }
                node.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 批量生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 批量生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            ConnectionModel conModel = null;
            try
            {
                conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            }
            catch { }
            try
            {
                if (conModel == null)
                {
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
                }
            }
            catch { }
            BatchForm bf = new BatchForm(node.Text, conModel);
            bf.ShowDialog();
        }

        #region tree菜单

        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 执行SQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSQLForm();
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sQL查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tableName = "";

            TreeNode node = Treeview.SelectedNode;

            if (node != null && node.Level == 4)
            {
                tableName = node.Text;
            }

            if (string.IsNullOrEmpty(tableName)) return;

            ShowSQLForm(tableName);
        }

        private void copyFieldNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            Clipboard.SetText(node.Text);
        }

        private void modifyTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //快捷生成业务代码
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var tableName = "";

            TreeNode node = Treeview.SelectedNode;

            if (node != null && node.Level == 4)
            {
                tableName = node.Text;
            }

            if (string.IsNullOrEmpty(tableName)) return;

            if (tableName.StartsWith("DB"))
            {
                tableName = $"{tableName.Trim().Replace(" ", "").Replace("_", "")}";
            }
            else
            {
                tableName = $"DB{tableName.Trim().Replace(" ", "").Replace("_", "")}";
            }

            new TemplateToCodeFastForm(tableName).ShowDialog();
        }

        /// <summary>
        /// 数据编辑窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataOperateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            if (node != null && !string.IsNullOrEmpty(node.Text) && node.Level == 4)
            {
                ShowDataForm(node);
            }
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            if (node != null && !string.IsNullOrEmpty(node.Text) && node.Level == 4)
            {
                ShowSQLExportForm(node);
            }
        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            if (node != null && !string.IsNullOrEmpty(node.Text) && node.Level == 4)
            {
                ShowSQLImportForm(node);
            }
        }
        #endregion

        /// <summary>
        /// 显示设计窗口
        /// </summary>
        /// <param name="tableName"></param>
        public void ShowDesignForm(string tableName = "")
        {
            TreeNode node = Treeview.SelectedNode;

            ConnectionModel conModel = null;

            switch (node.Level)
            {
                case 3:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
                    conModel.Database = node.Parent.Text;
                    break;
                case 4:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
                    conModel.Database = node.Parent.Parent.Text;

                    break;
                default:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
                    conModel.Database = node.Text;
                    break;
            }

            #region mysql
            var index1 = conModel.ConnectionString.IndexOf("database=");

            if (index1 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index1);

                var str2 = conModel.ConnectionString.Substring(index1);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            OnNewDesignForm?.Invoke(conModel);
        }

        /// <summary>
        /// sql查询窗口
        /// </summary>
        /// <param name="tableName"></param>
        public void ShowSQLForm(string tableName = "")
        {
            TreeNode node = Treeview.SelectedNode;

            ConnectionModel conModel = null;

            switch (node.Level)
            {
                case 3:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
                    conModel.Database = node.Parent.Text;
                    break;
                case 4:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
                    conModel.Database = node.Parent.Parent.Text;

                    break;
                default:
                    conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
                    conModel.Database = node.Text;
                    break;
            }

            #region mysql
            var index1 = conModel.ConnectionString.IndexOf("database=");

            if (index1 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index1);

                var str2 = conModel.ConnectionString.Substring(index1);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            OnNewSqlForm?.Invoke(conModel);
        }

        /// <summary>
        /// 数据编辑窗口
        /// </summary>
        /// <param name="node"></param>
        public void ShowDataForm(TreeNode node)
        {
            ConnectionModel conModel = null;

            conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
            conModel.Database = node.Parent.Parent.Text;

            #region mysql
            var index1 = conModel.ConnectionString.IndexOf("database=", StringComparison.OrdinalIgnoreCase);

            if (index1 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index1);

                var str2 = conModel.ConnectionString.Substring(index1);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            #region sqlserver
            var index12 = conModel.ConnectionString.IndexOf("Initial Catalog=", StringComparison.OrdinalIgnoreCase);

            if (index12 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index12);

                var str2 = conModel.ConnectionString.Substring(index12);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            conModel.TableName = node.Text;

            OnNewDataForm?.Invoke(conModel);
        }

        public void ShowSQLExportForm(TreeNode node)
        {
            ConnectionModel conModel = null;

            conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
            conModel.Database = node.Parent.Parent.Text;

            #region mysql
            var index1 = conModel.ConnectionString.IndexOf("database=", StringComparison.OrdinalIgnoreCase);

            if (index1 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index1);

                var str2 = conModel.ConnectionString.Substring(index1);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            #region sqlserver
            var index12 = conModel.ConnectionString.IndexOf("Initial Catalog=", StringComparison.OrdinalIgnoreCase);

            if (index12 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index12);

                var str2 = conModel.ConnectionString.Substring(index12);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            new SQLExportForm(conModel, node.Text).ShowDialog(this);
        }

        public void ShowSQLImportForm(TreeNode node)
        {
            ConnectionModel conModel = null;

            conModel = _connectList.Find(delegate (ConnectionModel con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
            conModel.Database = node.Parent.Parent.Text;

            #region mysql
            var index1 = conModel.ConnectionString.IndexOf("database=", StringComparison.OrdinalIgnoreCase);

            if (index1 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index1);

                var str2 = conModel.ConnectionString.Substring(index1);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            #region sqlserver
            var index12 = conModel.ConnectionString.IndexOf("Initial Catalog=", StringComparison.OrdinalIgnoreCase);

            if (index12 > 0)
            {
                var str1 = conModel.ConnectionString.Substring(0, index12);

                var str2 = conModel.ConnectionString.Substring(index12);

                var index2 = str2.IndexOf(";");

                str2 = str2.Substring(index2 + 1);

                conModel.ConnectionString = $"{str1}database={conModel.Database};{str2}";
            }

            #endregion

            new SQLImportForm(conModel, node.Text).ShowDialog(this);
        }

        /// <summary>
        /// 实现名称复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tview_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                {
                    if (Treeview.SelectedNode != null)
                    {
                        var txt = string.Empty;
                        if (Treeview.SelectedNode.Level == 1)
                        {
                            txt = _connectList.Find(b => b.ID.ToString() == Treeview.SelectedNode.Tag.ToString()).ConnectionString;
                        }
                        else
                        {
                            txt = Treeview.SelectedNode.Text;
                        }
                        Clipboard.SetText(txt, TextDataFormat.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("复制操作异常!ex:" + ex.Message, "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region mongodb快捷菜单

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            刷新ToolStripMenuItem2_Click(sender, e);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }



        #endregion



        #region 快速查询

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadForm.ShowLoading(this);                

                var text = searchTextBox.Text;

                var nodes = Treeview.Nodes;

                if (nodes != null && nodes.Count > 0)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        ChangeByKeyword(text, nodes[i]);
                    }
                }

                LoadForm.HideLoading(this);
            }
        }

        void ChangeByKeyword(string keyword, TreeNode treeNode)
        {
            List<TreeNode> list = new List<TreeNode>();

            if (treeNode.Nodes != null && treeNode.Nodes.Count > 0)
            {
                for (int i = 0; i < treeNode.Nodes.Count; i++)
                {
                    ChangeByKeyword(keyword, treeNode.Nodes[i]);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(keyword) || (string.IsNullOrEmpty(treeNode.Text) || treeNode.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) == -1))
                {
                    treeNode.BackColor = SystemColors.Window;
                    treeNode.ForeColor = SystemColors.WindowText;
                    //if (treeNode.Level == 4)
                    //    treeNode.Remove();
                }
                else
                {
                    treeNode.BackColor = Color.Red;
                    treeNode.ForeColor = Color.White;
                }
            }
        }

        #endregion

    }
}
