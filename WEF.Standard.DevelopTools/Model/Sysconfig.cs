using System;
using System.Collections.Generic;
using System.Text;

namespace WEF.Standard.DevelopTools.Model
{
    public class Sysconfig
    {
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
                    batchDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\GenerateCS";
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
