/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：508dafaf-8d9d-445b-94a8-1ab08bb7a9f0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：HttpTool
 * 类名称：MyTextBox
 * 创建时间：2016/11/14 20:26:34
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/

using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Controls
{
    partial class MyTextBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyTextBox));
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // txtRow
            // 
            resources.ApplyResources(this.txtRow, "txtRow");
            this.txtRow.AllowDrop = true;
            this.txtRow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRow.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtRow.ForeColor = System.Drawing.Color.Gray;
            this.txtRow.Name = "txtRow";
            this.txtRow.ReadOnly = true;
            this.txtRow.ShortcutsEnabled = false;
            this.txtRow.TabStop = false;
            this.txtRow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtRow_MouseDown);
            // 
            // txtContent
            // 
            this.txtContent.AcceptsReturn = true;
            this.txtContent.AcceptsTab = true;
            resources.ApplyResources(this.txtContent, "txtContent");
            this.txtContent.AllowDrop = true;
            this.txtContent.BackColor = System.Drawing.Color.Honeydew;
            this.txtContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContent.Name = "txtContent";
            this.txtContent.SizeChanged += new System.EventHandler(this.txtContent_SizeChanged);
            // 
            // MyTextBox
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.txtRow);
            this.Controls.Add(this.txtContent);
            this.Name = "MyTextBox";
            this.Load += new System.EventHandler(this.MyTextBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtContent;
        //private TextBoxE txtContent;
    }
}
