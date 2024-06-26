﻿// JSON 实体类生成器
// Power By Leigh.Wen @2015
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WEF.ModelGenerator.JsonTools
{
    public interface ICodeWriter
    {
        string FileExtension { get; }
        string DisplayName { get; }
        string GetTypeName(JsonType type, IJsonClassGeneratorConfig config);
        void WriteClass(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type);
        void WriteFileStart(IJsonClassGeneratorConfig config, TextWriter sw);
        void WriteFileEnd(IJsonClassGeneratorConfig config, TextWriter sw);
        void WriteNamespaceStart(IJsonClassGeneratorConfig config, TextWriter sw, bool root);
        void WriteNamespaceEnd(IJsonClassGeneratorConfig config, TextWriter sw, bool root);
    }
}
