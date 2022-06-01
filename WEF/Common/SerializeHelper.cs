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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using WEFInternal.Newtonsoft.Json;
using WEFInternal.Newtonsoft.Json.Serialization;

namespace WEF.Common
{
    /// <summary>
    /// newtonsoft.json 序列化
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// newton.json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indented"></param>
        /// <param name="defalutVal"></param>
        /// <param name="nullValue"></param>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public static string Serialize(object obj, bool indented = false, bool defalutVal = true, bool nullValue = false, bool camelCase = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DefaultValueHandling = defalutVal ? DefaultValueHandling.Include : DefaultValueHandling.Ignore;
            settings.NullValueHandling = nullValue ? NullValueHandling.Ignore : NullValueHandling.Include;
            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None, settings);
        }

        /// <summary>
        /// newton.json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="defalutVal"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json, bool defalutVal = true, bool nullValue = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DefaultValueHandling = defalutVal ? DefaultValueHandling.Include : DefaultValueHandling.Ignore;
            settings.NullValueHandling = nullValue ? NullValueHandling.Ignore : NullValueHandling.Include;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// newton.json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="defalutVal"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static dynamic Deserialize(string json, Type type, bool defalutVal = true, bool nullValue = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DefaultValueHandling = defalutVal ? DefaultValueHandling.Include : DefaultValueHandling.Ignore;
            settings.NullValueHandling = nullValue ? NullValueHandling.Ignore : NullValueHandling.Include;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.DeserializeObject(json, type, settings);
        }

        /// <summary>
        /// newton.json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="defalutVal"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static dynamic Deserialize(string json, bool defalutVal = true, bool nullValue = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DefaultValueHandling = defalutVal ? DefaultValueHandling.Include : DefaultValueHandling.Ignore;
            settings.NullValueHandling = nullValue ? NullValueHandling.Ignore : NullValueHandling.Include;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.DeserializeObject(json, settings);
        }


        /// <summary>
        /// 深复制当前对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCloneForDynamic<T>(this T obj)
        {
            var json = Serialize(obj);
            if (!string.IsNullOrEmpty(json))
                return Deserialize<T>(json);
            return default(T);
        }

        /// <summary>
        /// 深复制当前对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCloneForDynamic<T>(this object obj)
        {
            var json = Serialize(obj);
            if (!string.IsNullOrEmpty(json))
                return Deserialize<T>(json);
            return default(T);
        }

        /// <summary>
        /// 转换成josn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return ConvertJsonString(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// 转json格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }


        #region stuct

        /// <summary>
        /// The serialize delegate.
        /// </summary>
        /// <param name="obj">obj to be serialized.</param>
        /// <returns></returns>
        public delegate string TypeSerializeHandler(object obj);
        /// <summary>
        /// The deserialize delegate.
        /// </summary>
        /// <param name="data">the data to be deserialied.</param>
        /// <returns></returns>
        public delegate object TypeDeserializeHandler(string data);


        private static Dictionary<Type, KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>> handlers = new Dictionary<Type, KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>>();

        /// <summary>
        /// Deserializes the specified return type.
        /// </summary>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static object XmlDeserialize(Type returnType, string data)
        {
            if (data == null)
            {
                return null;
            }

            if (handlers.ContainsKey(returnType))
            {
                return handlers[returnType].Value(data);
            }
            else
            {
                StringReader sr = new StringReader(data);
                XmlSerializer serializer = new XmlSerializer(returnType);
                object obj = serializer.Deserialize(sr);
                sr.Close();
                return obj;
            }
        }

        /// <summary>
        /// 返序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string data) where T : class
        {
            if (data == null)
            {
                return null;
            }
            var type = typeof(T);

            if (handlers.ContainsKey(type))
            {
                return handlers[type].Value(data) as T;
            }
            else
            {
                using (StringReader sr = new StringReader(data))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(sr) as T;
                }
            }
        }


        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (handlers.ContainsKey(obj.GetType()))
            {
                return handlers[obj.GetType()].Key(obj);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sb.ToString();
            }
        }
        #endregion

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
