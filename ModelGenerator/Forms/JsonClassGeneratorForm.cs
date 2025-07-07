using CCWin;
using CCWin.SkinControl;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WEF.Common;
using WEF.ModelGenerator.JsonTools;
using WEF.ModelGenerator.JsonTools.CodeWriters;

namespace WEF.ModelGenerator.Forms
{
    public partial class JsonClassGeneratorForm : Form
    {

        public string targetPath;


        public JsonClassGeneratorForm()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void frmCSharpClassGeneration_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.UseProperties = radProperties.Checked;
            settings.InternalVisibility = radInternal.Checked;
            settings.SecondaryNamespace = edtSecondaryNamespace.Text;

            if (radNestedClasses.Checked) settings.NamespaceStrategy = 0;
            else if (radSameNamespace.Checked) settings.NamespaceStrategy = 1;
            else settings.NamespaceStrategy = 2;

            if (!settings.UseSeparateNamespace)
            {
                settings.SecondaryNamespace = string.Empty;
            }
            settings.SingleFile = chkSingleFile.Checked;
            settings.DocumentationExamples = chkDocumentationExamples.Checked;
            settings.Save();
        }

        private void frmCSharpClassGeneration_Load(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            (settings.UseProperties ? radProperties : radFields).Checked = true;
            (settings.InternalVisibility ? radInternal : radPublic).Checked = true;
            edtSecondaryNamespace.Text = settings.SecondaryNamespace;
            if (settings.NamespaceStrategy == 0) radNestedClasses.Checked = true;
            else if (settings.NamespaceStrategy == 1) radSameNamespace.Checked = true;
            else radDifferentNamespace.Checked = true;
            chkSingleFile.Checked = settings.SingleFile;
            chkDocumentationExamples.Checked = settings.DocumentationExamples;
            UpdateStatus();
        }

        private readonly static ICodeWriter[] CodeWriters = new ICodeWriter[] {
            new CSharpCodeWriter(),
            new VisualBasicCodeWriter(),
            new TypeScriptCodeWriter(),
            new JavaCodeWriter()
        };

        private void edtNamespace_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            HideCompletionMessage();

            using (var b = new FolderBrowserDialog())
            {
                b.ShowNewFolderButton = true;
                b.SelectedPath = targetPath;
                b.Description = "请选择文件输出目录";
                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    targetPath = b.SelectedPath + "/JsonClassies";
                }
            }
            if (string.IsNullOrEmpty(targetPath))
            {
                MessageBox.Show(this, "请指定输出目录。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (edtNamespace.Text == string.Empty)
            {
                MessageBox.Show(this, "请指定一个命名空间。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var gen = Prepare();
            if (gen == null) return;
            try
            {
                gen.GenerateClasses();
                messageTimer.Stop();
                lblDone.Visible = true;
                lnkOpenFolder.Visible = true;
                messageTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "无法生成代码： " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string lastGeneratedString;

        private void PasteAndGo()
        {
            HideCompletionMessage();
            var jsonClipboard = Clipboard.GetText(TextDataFormat.UnicodeText | TextDataFormat.Text);
            var gen = Prepare();
            if (gen == null) return;
            try
            {
                gen.TargetFolder = null;
                gen.SingleFile = true;
                using (var sw = new StringWriter())
                {
                    gen.OutputStream = sw;
                    gen.GenerateClasses();
                    sw.Flush();
                    lastGeneratedString = sw.ToString();
                    Clipboard.SetText(lastGeneratedString);
                    codeTxt.Text = lastGeneratedString;
                }
                messageTimer.Stop();
                lblDoneClipboard.Visible = true;
                messageTimer.Start();
                btnGenerate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Json校验失败，json字符串" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private JsonClassGenerator Prepare()
        {
            if (edtJson.Text == string.Empty)
            {
                MessageBox.Show(this, "请插入一些示例JSON", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                edtJson.Focus();
                return null;
            }

            if (edtMainClass.Text == string.Empty)
            {
                MessageBox.Show(this, "请指定一个主类名。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var gen = new JsonClassGenerator();
            gen.Example = edtJson.Text;
            gen.InternalVisibility = radInternal.Checked;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = chkExplicitDeserialization.Checked && gen.CodeWriter is CSharpCodeWriter;
            gen.Namespace = string.IsNullOrEmpty(edtNamespace.Text) ? null : edtNamespace.Text;
            gen.NoHelperClass = chkNoHelper.Checked;
            gen.SecondaryNamespace = radDifferentNamespace.Checked && !string.IsNullOrEmpty(edtSecondaryNamespace.Text) ? edtSecondaryNamespace.Text : null;
            gen.TargetFolder = targetPath;
            gen.UseProperties = radProperties.Checked;
            gen.MainClass = edtMainClass.Text;
            gen.UsePascalCase = chkPascalCase.Checked;
            gen.UseNestedClasses = radNestedClasses.Checked;
            gen.ApplyObfuscationAttributes = chkApplyObfuscationAttributes.Checked;
            gen.SingleFile = chkSingleFile.Checked;
            gen.ExamplesInDocumentation = chkDocumentationExamples.Checked;
            return gen;
        }

        private void chkExplicitDeserialization_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {

            if (edtSecondaryNamespace.Text.Contains("JsonTypes") || edtSecondaryNamespace.Text == string.Empty)
            {
                edtSecondaryNamespace.Text = edtNamespace.Text == string.Empty ? string.Empty : edtNamespace.Text + "." + edtMainClass.Text + "JsonTypes";
            }
            edtSecondaryNamespace.Enabled = radDifferentNamespace.Checked;
            var writer = new CSharpCodeWriter();
            chkPascalCase.Enabled = !(writer is TypeScriptCodeWriter);
            chkExplicitDeserialization.Enabled = writer is CSharpCodeWriter;
            chkNoHelper.Enabled = chkExplicitDeserialization.Enabled && chkExplicitDeserialization.Checked;
        }

        private void radNestedClasses_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void radSameNamespace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void radDifferentNamespace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void edtMainClass_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                PasteAndGo();
            }
            if (e.KeyCode == Keys.F6 && btnGenerate.Enabled)
            {
                btnGenerate_Click(null, null);
            }
            base.OnKeyDown(e);
        }

        private void messageTimer_Tick(object sender, EventArgs e)
        {
            messageTimer.Stop();
            HideCompletionMessage();
        }

        private void HideCompletionMessage()
        {
            lnkOpenFolder.Visible = false;
            lblDone.Visible = false;
            lblDoneClipboard.Visible = false;
        }

        private void lnkOpenFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(targetPath);
            }
            catch (Exception)
            {

            }
        }

        private void edtJson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                edtJson.SelectionStart = 0;
                edtJson.SelectionLength = edtJson.TextLength;
                e.Handled = true;
            }
        }

        private void btnPasteAndGo_Click(object sender, EventArgs e)
        {
            PasteAndGo();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:wenguoli_520@qq.com?subject=JsonClassGenerator消息反馈&body=Dear li.wen");
        }

        private void edtTargetFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        #region json

        private string Operation(string txt, byte op = 0)
        {
            var str = string.Empty;
            if (!string.IsNullOrEmpty(txt))
                try
                {
                    switch (op)
                    {
                        case 0:
                            str = SerializeHelper.ExpandJson(txt);
                            break;
                        case 1:
                            str = SerializeHelper.ContractJson(txt);
                            break;
                        case 2:
                            str = SerializeHelper.EscapeJson(txt);
                            break;
                        case 3:
                            str = SerializeHelper.UnEscapeJson(txt);
                            break;
                        default:
                            str = txt;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"转换失败:{ex.Message}");
                }
            return str;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            edtJson.Text = Operation(edtJson.Text, 0);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            edtJson.Text = Operation(edtJson.Text, 1);
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            edtJson.Text = Operation(edtJson.Text, 2);
        }
        private void skinButton4_Click(object sender, EventArgs e)
        {
            edtJson.Text = Operation(edtJson.Text, 3);
        }

        #endregion




    }
}
