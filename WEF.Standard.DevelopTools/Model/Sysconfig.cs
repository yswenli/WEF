using System;

namespace WEF.Standard.DevelopTools.Model
{
    public class Sysconfig
    {
        /// <summary>
        /// 针对高dpi的显示器系统屏幕缩放比例
        /// </summary>
        public float DpiScale { get; set; } = 2.0f;


        private string _namespace = "WEF.Models";

        /// <summary>
        /// 记住的命名空间
        /// </summary>
        public string Namespace
        {
            get
            {
                return _namespace;
            }
            set
            {
                _namespace = value;
            }
        }

        private string batchDirectoryPath;
        /// <summary>
        /// 记住的批量生成的路径
        /// </summary>
        public string BatchDirectoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(batchDirectoryPath))
                {
                    batchDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GenerateCS";
                }
                return batchDirectoryPath;
            }
            set
            {
                batchDirectoryPath = value;
            }
        }
    }
}
