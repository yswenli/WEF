/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： FileHelper
*版本号： V1.0.0.0
*唯一标识：42d16536-5c19-4987-88fe-2f3cbe883546
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/11/8 13:21:12
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/11/8 13:21:12
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.Standard.DevelopTools.Common
{
    public static class FileHelper
    {
        public static bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static long GetLenth(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// 读取文件字节
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] Read(string filePath)
        {
            try
            {
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Write(string filePath, byte[] data)
        {
            try
            {
                var path = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var ms = new MemoryStream(data))
                {
                    ms.Position = 0;
                    using (var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        ms.CopyTo(fs);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 获取目录下全部文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filters"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public static List<string> GetFiles(string path, string filters = "*.*", bool all = true)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists) return null;

            List<string> result = new List<string>();

            var filterArr = filters.Split(new string[] { ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> files = new List<string>();

            Parallel.ForEach(filterArr, filter =>
            {
                try
                {
                    var data = dirInfo.GetFiles(filter).Select(q => q.FullName);
                    if (data != null) files.AddRange(data);
                }
                catch { }
            });

            if (files != null && files.Any())
            {
                result.AddRange(files);
            }
            if (all)
            {
                try
                {
                    var dirs = dirInfo.GetDirectories();

                    if (dirs != null && dirs.Length > 0)
                    {
                        Parallel.ForEach(dirs, dir =>
                        {
                            try
                            {
                                var sfiles = GetFiles(dir.FullName, filters, all);
                                if ( sfiles != null && sfiles.Count > 0)
                                {
                                    result.AddRange(sfiles);
                                }
                            }
                            catch { }
                        });
                    }
                }
                catch
                {

                }
            }
            return result;
        }

        /// <summary>
        /// 读取全部内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadTxt(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch
            {

            }
            return "";
        }

        /// <summary>
        /// 读取行
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] ReadLines(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="txt"></param>
        public static void WriteTxt(string filePath, string txt)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt))
                    File.WriteAllText(filePath, txt, Encoding.UTF8);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="str"></param>
        /// <param name="path"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static List<string> Find(string str, string path, string filters = "*.*")
        {
            ConcurrentBag<string> concurrentBag = new ConcurrentBag<string>();
            var files = GetFiles(path, filters);
            if (files != null && files.Count > 0)
            {
                if (string.IsNullOrEmpty(str))
                {
                    foreach (var file in files)
                    {
                        concurrentBag.Add(file);
                    }
                }
                else
                {
                    Parallel.ForEach(files, (file) =>
                    {
                        var txt = ReadTxt(file);
                        if (!string.IsNullOrEmpty(txt))
                        {
                            if (txt.IndexOf(str, StringComparison.InvariantCultureIgnoreCase) > -1)
                            {
                                concurrentBag.Add(file);
                            }
                        }
                    });
                }

                return concurrentBag.ToList();
            }
            return null;
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> Replace(string source, string target, List<string> files)
        {
            if (files == null || files.Count < 1) return null;

            ConcurrentBag<string> concurrentBag = new ConcurrentBag<string>();

            Parallel.ForEach(files, (file) =>
            {
                var txt = ReadTxt(file);
                if (!string.IsNullOrEmpty(txt))
                {
                    WriteTxt(file, txt.Replace(source, target));
                    concurrentBag.Add(file);
                }
            });

            return concurrentBag.ToList();
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="appendTxt"></param>
        /// <param name="files"></param>
        /// <param name="appendStatus"></param>
        /// <returns></returns>
        public static List<bool> AppendText(string appendTxt, List<string> files, int appendStatus = 1)
        {
            ConcurrentBag<bool> result = new ConcurrentBag<bool>();

            Parallel.ForEach(files, (file) =>
            {
                var txt = ReadTxt(file);
                if (appendStatus == 1)
                {
                    WriteTxt(file, appendTxt + txt);
                    result.Add(true);
                }
                else if (appendStatus == 2)
                {
                    WriteTxt(file, txt + appendTxt);
                    result.Add(true);
                }
                else if (appendStatus == 3)
                {
                    WriteTxt(file, appendTxt + txt + appendTxt);
                    result.Add(true);
                }
                else
                {
                    result.Add(false);
                }
            });

            return result.ToList();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void Delete(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
