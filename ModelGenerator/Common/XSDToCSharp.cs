/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Common
*文件名： XSDToCSharp
*版本号： V1.0.0.0
*唯一标识：201fe12a-cff3-4a68-acb6-23c6a53ffe5d
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/12/2 14:48:53
*描述：xsd 转换成C#
*
*=====================================================================
*修改标记
*修改时间：2021/12/2 14:48:53
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：xsd 转换成C#
*
*****************************************************************************/
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// xsd 转换成C#
    /// </summary>
    public static class XSDToCSharp
    {
        static CodeNamespace Process(Stream stream, string targetNamespace)
        {
            StreamReader sr = new StreamReader(stream);

            XmlSchema xsd = XmlSchema.Read(sr, null);

            xsd.Compile(null);

            XmlSchemas schemas = new XmlSchemas();

            schemas.Add(xsd);

            XmlSchemaImporter importer = new XmlSchemaImporter(schemas);

            CodeNamespace ns = new CodeNamespace(targetNamespace);

            XmlCodeExporter exporter = new XmlCodeExporter(ns);

            foreach (XmlSchemaElement element in xsd.Elements.Values)
            {
                XmlTypeMapping mapping = importer.ImportTypeMapping(element.QualifiedName);
                exporter.ExportTypeMapping(mapping);
            }
            return ns;

        }

        /// <summary>
        /// 生成C#
        /// </summary>
        /// <param name="xsdTxt"></param>
        /// <param name="targetNamespace"></param>
        /// <returns></returns>
        public static string GeneratedCode(string xsdTxt, string targetNamespace)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xsdTxt)))
            {
                CodeNamespace ns = Process(stream, targetNamespace);

                CodeDomProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(ms))
                    {
                        provider.CreateGenerator().GenerateCodeFromNamespace(ns, sw, new CodeGeneratorOptions());
                        ms.Position = 0;
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }                    
                }
            }
        }

        /// <summary>
        /// 生成C#
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="targetNamespace"></param>
        /// <returns></returns>
        public static string GeneratedCodeFromFile(string filePath, string targetNamespace)
        {
            var xsdTxt = Encoding.UTF8.GetString(FileHelper.Read(filePath));
            return GeneratedCode(xsdTxt, targetNamespace);
        }
    }
}
