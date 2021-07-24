/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： SqlWord
*版本号： V1.0.0.0
*唯一标识：b8d23148-cfc1-4afc-b8e3-5f67dec79264
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/7/19 13:38:04
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/19 13:38:04
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.ModelGenerator.Model
{
    public class SqlWord
    {
        public SqlWordTokenType Type { get; set; }
        public string Text { get; set; }
        public object Source { get; set; }
    }

    [Flags]
    public enum SqlWordTokenType : int
    {
        None = 0,
        Keyword = 2,
        BuiltinFunction = 4,
        Owner = 8,
        Function = 16,
        Table = 32,
        View = 64,
        TableColumn = 128,
        String = 256,
        Comment = 512
    }

    public class SqlWordToken
    {
        public SqlWordTokenType Type { get; set; }
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
        public string Text { get; set; }
    }
}
