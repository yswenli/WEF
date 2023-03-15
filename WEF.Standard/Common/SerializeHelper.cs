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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace WEF.Common
{
    /// <summary>
    /// newtonsoft.json 序列化
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// 序列化到XmlNode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="node"></param>
        public static void XmlSerialize<T>(T t, out XmlNode node)
        {
            DataContractSerializer datacontractSerializer = new DataContractSerializer(typeof(T));
            XmlDocument doc = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                datacontractSerializer.WriteObject(ms, t);
                ms.Position = 0;
                doc.Load(ms);
                node = doc.LastChild;
            }
        }
        /// <summary>
        /// 反序列化XmlNode中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="t"></param>
        public static void XmlDeserialize<T>(XmlNode node, out T t)
        {
            DataContractSerializer datacontractSerializer = new DataContractSerializer(typeof(T));
            using (XmlReader reader = new XmlNodeReader(node))
            {
                t = (T)datacontractSerializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic Deserialize(Type type, string json)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                return serializer.ReadObject(ms);
            }
        }
    }
}
