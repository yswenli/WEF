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
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEF.DbDAL;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator
{
    public partial class LeftPanel : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        string conConnectionString;

        public LeftPanel()
        {
            InitializeComponent();
            Treeview.ExpandAll();
        }

        public delegate void NewContentForm(Connection conModel, string tableName, string databaseName, bool isView);

        public event NewContentForm newcontentForm;

        public event Action<Connection> newsqlForm;

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
                        DbSelect.DBSqlServer dbsqlserver = new WEF.ModelGenerator.DbSelect.DBSqlServer();
                        dia = dbsqlserver.ShowDialog();
                        break;
                    case DatabaseType.MsAccess:
                        DbSelect.DBMsAccess dbMsAccess = new WEF.ModelGenerator.DbSelect.DBMsAccess();
                        dia = dbMsAccess.ShowDialog();
                        break;
                    case DatabaseType.Oracle:
                        DbSelect.DBOracle dbOracle = new WEF.ModelGenerator.DbSelect.DBOracle();
                        dia = dbOracle.ShowDialog();
                        break;
                    case DatabaseType.Sqlite3:
                        DbSelect.DbSqlite dbSqlite = new WEF.ModelGenerator.DbSelect.DbSqlite();
                        dia = dbSqlite.ShowDialog();
                        break;
                    case DatabaseType.MySql:
                        DbSelect.DBMySql dbMySql = new WEF.ModelGenerator.DbSelect.DBMySql();
                        dia = dbMySql.ShowDialog();
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
        List<Connection> _ConnectList;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftPanel_Load(object sender, EventArgs e)
        {
            this.CloseButtonVisible = false;
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

            _ConnectList = UtilsHelper.GetConnectionList();

            TreeNode node = Treeview.Nodes[0];

            node.ContextMenuStrip = contextMenuStripTop;
            foreach (Connection connection in _ConnectList)
            {
                TreeNode nnode = new TreeNode(connection.Name, 0, 0);
                nnode.ContextMenuStrip = contextMenuStripDatabase;
                nnode.Tag = connection.ID.ToString();
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

            if (node.Level == 0)
            {
                node.Nodes.Clear();
                getServers();
                Treeview.ExpandAll();
            }
            else if (node.Level == 1)
            {
                node.Nodes.Clear();
                getDatabaseinfo();
            }
            else if (node.Level == 4)
            {
                if (null != newcontentForm)
                {
                    Connection conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(Treeview.SelectedNode.Parent.Parent.Parent.Tag.ToString()); });
                    conConnectionString = Treeview.SelectedNode.Parent.Parent.Tag.ToString();
                    try
                    {
                        newcontentForm(conModel, Treeview.SelectedNode.Text, Treeview.SelectedNode.Parent.Parent.Text, Treeview.SelectedNode.Tag.ToString().Equals("V"));
                    }
                    catch { }
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
        /// 
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
            List<Connection> connList = UtilsHelper.GetConnectionList();

            foreach (Connection conn in connList)
            {
                Connection tempconn = _ConnectList.Find(delegate (Connection connin) { return conn.ID.ToString().Equals(connin.ID.ToString()); });
                if (null == tempconn)
                {
                    TreeNode nnode = new TreeNode(conn.Name, 0, 0);
                    nnode.ContextMenuStrip = contextMenuStripDatabase;
                    nnode.Tag = conn.ID.ToString();
                    Treeview.Nodes[0].Nodes.Add(nnode);
                }
            }

            _ConnectList = connList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshConnectionList();
            Treeview.ExpandAll();
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stringid = Treeview.SelectedNode.Tag.ToString();
            UtilsHelper.DeleteConnection(stringid);
            Connection tempconn = _ConnectList.Find(delegate (Connection conn) { return conn.ID.ToString().Equals(stringid); });
            if (null != tempconn)
                _ConnectList.Remove(tempconn);
            Treeview.Nodes.Remove(Treeview.SelectedNode);
        }

        #endregion

        #region database

        private void 连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            node.Nodes.Clear();
            getDatabaseinfo();
        }



        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            node.Nodes.Clear();
            getDatabaseinfo();
            Treeview.ExpandAll();
        }

        /// <summary>
        /// 获取数据库服务器
        /// </summary>
        private void getDatabaseinfo()
        {
            LoadForm.ShowLoading(this);

            TreeNode node = Treeview.SelectedNode;

            Task.Factory.StartNew(() =>
            {
                try
                {

                    Connection conModel = null;

                    this.Invoke(new Action(() =>
                    {
                        conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Tag.ToString()); });
                    }));


                    conConnectionString = conModel.ConnectionString;

                    IDbObject dbObject;

                    if (conModel.DbType.Equals(DatabaseType.MsAccess.ToString()))
                    {
                        dbObject = new WEF.DbDAL.OleDb.DbObject(conConnectionString);

                        TreeNode tnode = null;

                        this.Invoke(new Action(() =>
                        {
                            tnode = new TreeNode(conModel.Database, 1, 1);
                            tnode.Tag = conConnectionString;
                            tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                            node.Nodes.Add(tnode);
                        }));

                        var tables = dbObject.GetTables("");

                        var views = dbObject.GetVIEWs("");


                        this.Invoke(new Action(() =>
                        {
                            gettables(tnode, tables, views);
                        }));

                    }
                    else if (conModel.DbType.Equals(DatabaseType.Sqlite3.ToString()))
                    {
                        dbObject = new WEF.DbDAL.SQLite.DbObject(conConnectionString);

                        TreeNode tnode = null;

                        this.Invoke(new Action(() =>
                        {
                            tnode = new TreeNode(conModel.Database, 1, 1);
                            tnode.Tag = conConnectionString;
                            tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                            node.Nodes.Add(tnode);
                        }));

                        var tables = dbObject.GetTables("");

                        var views = dbObject.GetVIEWs("");


                        this.Invoke(new Action(() =>
                        {
                            gettables(tnode, tables, views);
                        }));

                    }
                    else if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()) || conModel.DbType.Equals(DatabaseType.SqlServer9.ToString()))
                    {
                        if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()))
                            dbObject = new WEF.DbDAL.SQL2000.DbObject(conConnectionString);
                        else
                            dbObject = new WEF.DbDAL.SQL2005.DbObject(conConnectionString);

                        if (conModel.Database.Equals("all"))
                        {
                            DataTable dt = dbObject.GetDBList();

                            foreach (DataRow dr in dt.Rows)
                            {
                                TreeNode tnode = null;

                                this.Invoke(new Action(() =>
                                {
                                    tnode = new TreeNode(dr[0].ToString(), 1, 1);
                                    tnode.Tag = conConnectionString.Replace("master", dr[0].ToString());
                                    tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                                    node.Nodes.Add(tnode);
                                }));

                                var tables = dbObject.GetTables(tnode.Text);

                                var views = dbObject.GetVIEWs(tnode.Text);

                                this.Invoke(new Action(() =>
                                {
                                    gettables(tnode, tables, views);
                                }));
                            }
                        }
                        else
                        {
                            TreeNode tnode = null;

                            this.Invoke(new Action(() =>
                            {
                                tnode = new TreeNode(conModel.Database, 1, 1);
                                tnode.Tag = conConnectionString;
                                tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                                node.Nodes.Add(tnode);
                            }));

                            var tables = dbObject.GetTables(tnode.Text);

                            var views = dbObject.GetVIEWs(tnode.Text);

                            this.Invoke(new Action(() =>
                            {
                                gettables(tnode, tables, views);
                            }));
                        }
                    }
                    else if (conModel.DbType.Equals(DatabaseType.Oracle.ToString()))
                    {
                        dbObject = new WEF.DbDAL.Oracle.DbObject(conConnectionString);

                        TreeNode tnode = null;

                        this.Invoke(new Action(() =>
                        {
                            tnode = new TreeNode(conModel.Database, 1, 1);
                            tnode.Tag = conConnectionString;
                            tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                            node.Nodes.Add(tnode);
                        }));

                        var tables = dbObject.GetTables(tnode.Text);

                        var views = dbObject.GetVIEWs(tnode.Text);

                        this.Invoke(new Action(() =>
                        {
                            gettables(tnode, tables, views);
                        }));
                    }
                    else if (conModel.DbType.Equals(DatabaseType.MySql.ToString()))
                    {
                        dbObject = new WEF.DbDAL.MySql.DbObject(conConnectionString);

                        if (conModel.Database.Equals("all"))
                        {
                            DataTable dt = dbObject.GetDBList();

                            foreach (DataRow dr in dt.Rows)
                            {
                                TreeNode tnode = null;

                                this.Invoke(new Action(() =>
                                {
                                    tnode = new TreeNode(dr[0].ToString(), 1, 1);
                                    tnode.Tag = conConnectionString.Replace("master", dr[0].ToString());
                                    tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                                    node.Nodes.Add(tnode);
                                }));

                                var tables = dbObject.GetTables(tnode.Text);

                                var views = dbObject.GetVIEWs(tnode.Text);

                                this.Invoke(new Action(() =>
                                {
                                    gettables(tnode, tables, views);
                                }));
                            }

                        }
                        else
                        {
                            TreeNode tnode = null;

                            this.Invoke(new Action(() =>
                            {
                                tnode = new TreeNode(conModel.Database, 1, 1);
                                tnode.Tag = conConnectionString;
                                tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                                node.Nodes.Add(tnode);
                            }));

                            var tables = dbObject.GetTables(tnode.Text);

                            var views = dbObject.GetVIEWs(tnode.Text);

                            this.Invoke(new Action(() =>
                            {
                                gettables(tnode, tables, views);
                            }));
                        }
                    }
                    LoadForm.HideLoading();
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading();
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
        /// <summary>
        /// 获取数据表信息
        /// </summary>
        /// <param name="databaseNodel"></param>
        /// <param name="tables"></param>
        /// <param name="views"></param>
        private void gettables(TreeNode databaseNodel, DataTable tables, DataTable views)
        {
            TreeNode tableNode = new TreeNode("数据表", 2, 3);
            if (null != tables && tables.Rows.Count > 0)
            {
                DataRow[] tablesdrs = tables.Select("", "name asc");
                foreach (DataRow tablesDR in tablesdrs)
                {
                    TreeNode tnode = new TreeNode(tablesDR[0].ToString(), 4, 4);
                    tnode.Tag = "T";
                    tnode.ContextMenuStrip = contextMenuStripTable;
                    tableNode.Nodes.Add(tnode);
                }
            }
            databaseNodel.Nodes.Add(tableNode);

            TreeNode viewNode = new TreeNode("视图", 2, 3);
            DataRow[] viewsdrs = views.Select("", "name asc");

            foreach (DataRow viewsDR in viewsdrs)
            {
                TreeNode tnode = new TreeNode(viewsDR[0].ToString(), 4, 4);
                tnode.Tag = "V";
                tnode.ContextMenuStrip = contextMenuStripTable;
                viewNode.Nodes.Add(tnode);
            }
            databaseNodel.Nodes.Add(viewNode);
        }
        #endregion



        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 生成代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != newcontentForm)
            {
                Connection conModel = _ConnectList.Find(
                    delegate (Connection con)
                    {
                        return con.ID.ToString().Equals(Treeview.SelectedNode.Parent.Parent.Parent.Tag.ToString());
                    });
                conConnectionString = Treeview.SelectedNode.Parent.Parent.Tag.ToString();
                newcontentForm(conModel, Treeview.SelectedNode.Text, Treeview.SelectedNode.Parent.Parent.Text, Treeview.SelectedNode.Tag.ToString().Equals("V"));
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

            node.Nodes.Clear();

            Connection conModel = null;
            try
            {
                conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            }
            catch { }
            try
            {
                if (conModel == null) conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
            }
            catch { }

            IDbObject dbObject;

            if (conModel.DbType.Equals(DatabaseType.MsAccess.ToString()))
            {
                dbObject = new WEF.DbDAL.OleDb.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(""), dbObject.GetVIEWs(""));
            }
            else if (conModel.DbType.Equals(DatabaseType.SqlServer.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2000.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
            }
            else if (conModel.DbType.Equals(DatabaseType.SqlServer9.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2005.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
            }
            else if (conModel.DbType.Equals(DatabaseType.Oracle.ToString()))
            {
                dbObject = new WEF.DbDAL.Oracle.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
            }
            else if (conModel.DbType.Equals(DatabaseType.MySql.ToString()))
            {
                dbObject = new WEF.DbDAL.MySql.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
            }
            else if (conModel.DbType.Equals(DatabaseType.Sqlite3.ToString()))
            {
                dbObject = new WEF.DbDAL.SQLite.DbObject(conConnectionString);
                gettables(node, dbObject.GetTables(node.Text), dbObject.GetVIEWs(node.Text));
            }
            node.ExpandAll();
        }


        /// <summary>
        /// 批量生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 批量生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = Treeview.SelectedNode;
            Connection conModel = null;
            try
            {
                conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            }
            catch { }
            try
            {
                if (conModel == null)
                {
                    conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
                }
            }
            catch { }
            BatchForm bf = new BatchForm(node.Text, conModel);
            bf.ShowDialog();
        }

        private void 执行SQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSQLForm();
        }
        private void sQL查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSQLForm();
        }

        public void ShowSQLForm()
        {
            TreeNode node = Treeview.SelectedNode;

            Connection conModel = null;

            switch (node.Level)
            {
                case 3:
                    conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Parent.Tag.ToString()); });
                    break;
                case 4:
                    conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Parent.Parent.Tag.ToString()); });
                    break;
                default:
                    conModel = _ConnectList.Find(delegate (Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
                    break;
            }
            conModel.Database = node.Text;

            newsqlForm?.Invoke(conModel);
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
                            txt = _ConnectList.Find(b => b.ID.ToString() == Treeview.SelectedNode.Tag.ToString()).ConnectionString;
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
                MessageBox.Show("复制操作异常!ex:" + ex.Message);
            }
        }

        
    }
}
