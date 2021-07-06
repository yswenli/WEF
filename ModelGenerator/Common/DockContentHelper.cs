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
using System.Collections.Concurrent;
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
    /// <summary>
    /// Dock内容工具类
    /// </summary>
    public static class DockContentHelper
    {
        static string _configFile = string.Empty;

        static string _tempFile = string.Empty;

        static List<ConnectionModel> _connections;

        static DockPanel _dockPanel = null;

        /// <summary>
        /// Dock内容工具类
        /// </summary>
        static DockContentHelper()
        {
            _configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Config", "wefdp.config");

            _tempFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Config", "wefTemp.config");

            _connections = new List<ConnectionModel>();
        }

        static IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(LeftPanelForm).ToString())
            {
                var lf = new LeftPanelForm();
                lf.OnNewSqlForm += Lf_newsqlForm;
                lf.OnNewContentForm += Lf_newcontentForm;
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


        static ConcurrentDictionary<string, ContentForm> _contentFormDic = new ConcurrentDictionary<string, ContentForm>();

        private static void Lf_newcontentForm(ConnectionModel conModel)
        {
            var key = $"({conModel.Database}){conModel.TableName}";

            try
            {
                ContentForm contentForm = _contentFormDic.GetOrAdd(key, (k) =>
                {
                    ContentForm s = new ContentForm();
                    s.Text = "(" + conModel.Database + ")" + conModel.TableName;
                    s.TableName = conModel.TableName;
                    s.DatabaseName = conModel.Database;
                    s.IsView = conModel.IsView;
                    s.ConnectionModel = conModel;
                    return s;
                });
                contentForm.Show(_dockPanel);
            }
            catch
            {
                _contentFormDic.TryRemove(key, out ContentForm _);
            }
        }

        static ConcurrentDictionary<string, SQLForm> _sqlFormDic = new ConcurrentDictionary<string, SQLForm>();


        private static void Lf_newsqlForm(ConnectionModel conModel)
        {
            var key = $"({conModel.Database}){conModel.TableName}";

            try
            {
                SQLForm sqlForm = _sqlFormDic.GetOrAdd(key, (k) =>
                {
                    SQLForm s = new SQLForm(conModel.TableName);
                    s.Text = "(" + conModel.Database + ")SQL查询窗口";
                    s.ConnectionModel = conModel;
                    return s;
                });
                sqlForm.Show(_dockPanel);
            }
            catch
            {
                _sqlFormDic.TryRemove(key, out SQLForm _);
            }
        }

        public static bool Load(DockPanel dockPanel)
        {
            try
            {
                _dockPanel = dockPanel;

                if (File.Exists(_tempFile))
                {
                    var json = File.ReadAllText(_tempFile);

                    _connections = WEFExtention.JsonDeserialize<List<ConnectionModel>>(json);
                }

                var content = new DeserializeDockContent(GetContentFromPersistString);

                if (File.Exists(_configFile))
                {
                    dockPanel.LoadFromXml(_configFile, content);


                    foreach (var item in _dockPanel.Contents)
                    {
                        var sf = item as SQLForm;
                        if (sf != null)
                        {
                            _sqlFormDic.TryAdd(sf.Text, sf);
                        }

                        var cf = item as ContentForm;
                        if (cf != null)
                        {
                            _contentFormDic.TryAdd(cf.Text, cf);
                        }
                    }

                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// 保存界面状态
        /// </summary>
        /// <param name="dockPanel"></param>
        public static void Save(DockPanel dockPanel)
        {
            try
            {
                _dockPanel = dockPanel;

                var contents = dockPanel.Contents;

                if (contents != null && contents.Any())
                {
                    List<ConnectionModel> connections = new List<ConnectionModel>();

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
            catch { }
        }
    }
}
