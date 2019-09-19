/****************************************************************************
*项目名称：WEF.ModelGenerator.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.ModelGenerator.Common
*类 名 称：DockContentHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2019/9/19 10:12:19
*描述：
*=====================================================================
*修改时间：2019/9/19 10:12:19
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WEF.ModelGenerator.Forms;
using WEF.ModelGenerator.Model;
using WeifenLuo.WinFormsUI.Docking;

namespace WEF.ModelGenerator.Common
{
    public static class DockContentHelper
    {
        static string _configFile = string.Empty;

        static string _tempFile = string.Empty;

        static List<Connection> _connections;

        static DockPanel _dockPanel = null;

        static DockContentHelper()
        {
            _configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "wefdp.config");

            _tempFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "wefTemp.config");

            _connections = new List<Connection>();
        }

        static IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(LeftPanelForm).ToString())
            {
                var lf = new LeftPanelForm();
                lf.newsqlForm += Lf_newsqlForm;
                lf.newcontentForm += Lf_newcontentForm;
                return lf;
            }
            else if (persistString == typeof(SQLForm).ToString())
            {
                var cm = _connections.First();
                _connections.Remove(cm);
                var sf = new SQLForm();
                sf.Text = $"({cm.Database})SQL查询窗口";
                sf.ConnectionModel = cm;
                sf.AutoTextBox.TextBox.Text = cm.Sql;
                return sf;
            }

            else if (persistString == typeof(ContentForm).ToString())
            {
                var cm = _connections.First();
                _connections.Remove(cm);
                var cf = new ContentForm();
                cf.Text = $"({cm.Database}){cm.TableName}";
                cf.ConnectionModel = cm;
                return cf;
            }
            else if (persistString == typeof(SQLTemplateForm).ToString())
                return new SQLTemplateForm();
            else
                return null;
        }

        private static void Lf_newcontentForm(Connection conModel)
        {
            ContentForm s = new ContentForm();
            s.Text = "(" + conModel.Database + ")" + conModel.TableName;
            s.TableName = conModel.TableName;
            s.DatabaseName = conModel.Database;
            s.IsView = conModel.IsView;
            s.ConnectionModel = conModel;
            s.Show(_dockPanel);
        }

        private static void Lf_newsqlForm(Connection conModel)
        {
            SQLForm s = new SQLForm();
            s.Text = "(" + conModel.Database + ")SQL查询窗口";
            s.ConnectionModel = conModel;
            s.Show(_dockPanel);
        }

        public static bool Load(DockPanel dockPanel)
        {
            _dockPanel = dockPanel;

            if (File.Exists(_tempFile))
            {
                var json = File.ReadAllText(_tempFile);

                _connections = WEFExtention.JsonDeserialize<List<Connection>>(json);
            }

            var content = new DeserializeDockContent(GetContentFromPersistString);

            if (File.Exists(_configFile))
            {
                dockPanel.LoadFromXml(_configFile, content);

                return true;
            }

            return false;
        }

        public static void Save(DockPanel dockPanel)
        {
            _dockPanel = dockPanel;

            var contents = dockPanel.Contents;

            if (contents != null && contents.Any())
            {
                List<Connection> connections = new List<Connection>();

                foreach (var content in contents)
                {
                    if (content is SQLForm)
                    {
                        var sqlForm = (SQLForm)content;

                        sqlForm.ConnectionModel.Sql = sqlForm.AutoTextBox.TextBox.Text;

                        connections.Add(sqlForm.ConnectionModel);
                    }
                    if (content is ContentForm)
                    {
                        var contentForm = (ContentForm)content;

                        connections.Add(contentForm.ConnectionModel);
                    }
                }

                var json = WEFExtention.JsonSerialize(connections);

                File.WriteAllText(_tempFile, json, Encoding.UTF8);
            }

            dockPanel.SaveAsXml(_configFile);
        }
    }
}
