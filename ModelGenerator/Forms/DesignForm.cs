/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Forms
*文件名： DesignForm
*版本号： V1.0.0.0
*唯一标识：3cf87fb9-a481-426e-9f74-f69b189318dc
*当前的用户域：WALLE
*创建人： yswen
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/6/30 14:40:27
*描述：
*
*=================================================
*修改标记
*修改时间：2022/6/30 14:40:27
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    /// <summary>
    /// 设计表
    /// </summary>
    public partial class DesignForm : Form
    {
        DesignInfo _designInfo;

        /// <summary>
        /// 设计表
        /// </summary>
        public DesignForm()
        {
            InitializeComponent();

            _designInfo = new DesignInfo();
            _designInfo.ColumnInfos = new List<DBColumnInfo>();
            _designInfo.ColumnInfos.Add(new DBColumnInfo()
            {
                NameColumn = "ID",
                TypeColumn = DBDataType.Int,
                KeyColumn = true,
                NotNullColumn = true,
                AutoIncrementColumn = false,
                LengthColumn = 200,
                InfoColumn = "描述adfweafasfasfasfsadf"
            });

        }

        /// <summary>
        /// 设计表
        /// </summary>
        /// <param name="designInfo"></param>
        public DesignForm(DesignInfo designInfo) : this()
        {
            _designInfo = designInfo;
            textBox1.Text = _designInfo.TableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesignForm_Load(object sender, EventArgs e)
        {
            if (_designInfo != null
                && _designInfo.ColumnInfos != null
                && _designInfo.ColumnInfos.Count > 0)
            {
                foreach (var item in _designInfo.ColumnInfos)
                {
                    dataGridView1.Rows.Add(item.NameColumn,
                        item.TypeColumn.ToString(),
                        item.LengthColumn,
                        item.KeyColumn,
                        item.NotNullColumn,
                        item.AutoIncrementColumn,
                        item.InfoColumn);
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (dgv.CurrentCell.GetType().Name == "DataGridViewComboBoxCell" && dgv.CurrentCell.RowIndex != -1)
            {
                var cn = (e.Control as ComboBox);
                if (cn == null) return;
                cn.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            }
            if (dgv.CurrentCell.GetType().Name == "DataGridViewCheckBoxCell" && dgv.CurrentCell.RowIndex != -1)
            {
                var cn = (e.Control as CheckBox);
                if (cn == null) return;
                cn.CheckedChanged += DesignForm_CheckedChanged;
            }
        }


        public void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            if (combox == null) return;
            combox.Leave += new EventHandler(combox_Leave);

            try
            {
                var dgv = combox.Parent.Parent as DataGridView;
                if (dgv == null)
                {
                    return;
                }
                var selectCell = dgv.CurrentCell;
                var rowIndex = selectCell.RowIndex;
                var columnIndex = selectCell.ColumnIndex;

                var cell = dgv.Rows[rowIndex].Cells[columnIndex + 1];
                cell.ReadOnly = false;

                var dbDataType = selectCell.Value.ToString().ToDBDataType();
                switch (dbDataType)
                {
                    case DBDataType.Bit:
                        cell.Value = 1;
                        cell.ReadOnly = true;
                        break;
                    case DBDataType.NVarChar:
                    case DBDataType.Image:
                        cell.Value = 255;
                        break;
                    case DBDataType.Money:
                    case DBDataType.Int:
                    case DBDataType.DateTime:
                    case DBDataType.Float:
                    case DBDataType.Double:
                    default:
                        cell.Value = "";
                        break;
                }
            }
            catch { }
        }

        public void combox_Leave(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            if (combox == null) return;
            combox.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
            combox.Leave -= combox_Leave;
        }


        private void DesignForm_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckbox = sender as CheckBox;
            if (ckbox == null) return;
            ckbox.Leave += Ckbox_Leave;
        }

        private void Ckbox_Leave(object sender, EventArgs e)
        {
            CheckBox ckbox = sender as CheckBox;
            if (ckbox == null) return;
            ckbox.CheckedChanged -= DesignForm_CheckedChanged;
            ckbox.Leave -= Ckbox_Leave;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.Rows;
            if (rows == null || rows.Count < 1) return;
            //foreach (var item in collection)
            //{

            //}
        }
    }
}
