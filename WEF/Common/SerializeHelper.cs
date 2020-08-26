/****************************************************************************
*项目名称：WEF.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Common
*类 名 称：SerializeHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/26 11:00:40
*描述：
*=====================================================================
*修改时间：2020/8/26 11:00:40
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.IO;
using WEF.Common.Newtonsoft.Json;

namespace WEF.Common
{
    /// <summary>
    /// newtonsoft.json 序列化
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 展开josn
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ExpandJson(string str)
        {
            try
            {
                var serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                var jtr = new JsonTextReader(tr);
                var obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    var textWriter = new StringWriter();
                    var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
                return str;
            }
            catch (JsonReaderException ex)
            {
                return str;
            }
        }

        /// <summary>
        /// 收缩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ContractJson(string str)
        {
            try
            {
                var serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                var jtr = new JsonTextReader(tr);
                var obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    var textWriter = new StringWriter();
                    var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.None
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
                return str;
            }
            catch (JsonReaderException ex)
            {
                return str;
            }
        }

        /// <summary>
        /// 转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeJson(string str)
        {
            return str.Replace("\"", "\\\"");
        }

        /// <summary>
        /// 去掉转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscapeJson(string str)
        {
            return str.Replace("\\\"", "\"");
        }
    }
}
