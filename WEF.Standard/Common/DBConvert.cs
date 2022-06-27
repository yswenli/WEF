/****************************************************************************
*项目名称：WEF.Common
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.Common
*类 名 称：DBConvert
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/5/5 10:37:09
*描述：
*=====================================================================
*修改时间：2019/5/5 10:37:09
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;

namespace WEF.Common
{
    internal static class DBConvert
    {
        public static bool IsDBNull(object value)
        {
            return object.Equals(DBNull.Value, value);
        }
        public static short ToInt16(object value)
        {
            if (value is short)
            {
                return (short)value;
            }
            try
            {
                return Convert.ToInt16(value);
            }
            catch
            {
                return 0;
            }
        }
        public static ushort ToUInt16(object value)
        {
            if (value is ushort)
            {
                return (ushort)value;
            }
            try
            {
                return Convert.ToUInt16(value);
            }
            catch
            {
                return 0;
            }
        }
        public static int ToInt32(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }
        public static uint ToUInt32(object value)
        {
            if (value is uint)
            {
                return (uint)value;
            }
            try
            {
                return Convert.ToUInt32(value);
            }
            catch
            {
                return 0;
            }
        }
        public static long ToInt64(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }
        public static ulong ToUInt64(object value)
        {
            if (value is long)
            {
                return (ulong)value;
            }
            try
            {
                return Convert.ToUInt64(value);
            }
            catch
            {
                return 0;
            }
        }
        public static bool ToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is bool)
            {
                return (bool)value;
            }
            if (value.Equals("1") || value.Equals("-1"))
            {
                value = "true";
            }
            else if (value.Equals("0"))
            {
                value = "false";
            }

            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static DateTime ToDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static decimal ToDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }
        public static double ToDouble(object value)
        {
            if (value is double)
            {
                return (double)value;
            }
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }
        //2015-09-22
        public static float ToFloat(object value)
        {
            if (value is Single || value is float)
            {
                return (float)value;
            }
            try
            {
                return Convert.ToSingle(value);
            }
            catch
            {
                return 0;
            }
        }
        public static Guid ToGuid(object value)
        {
            if (value is Guid)
            {
                return (Guid)value;
            }
            try
            {
                return Guid.Parse(value.ToString());
            }
            catch
            {
                return new Guid();
            }
        }
        public static byte[] ToByteArr(object value)
        {
            var arr = value as byte[];
            return arr;
        }

        public static Nullable<short> ToNInt16(object value)
        {
            if (value is short)
            {
                return new Nullable<short>((short)value);
            }
            try
            {
                return new Nullable<short>(Convert.ToInt16(value));
            }
            catch
            {
                return new Nullable<short>();
            }
        }
        public static Nullable<ushort> ToNUInt16(object value)
        {
            if (value is ushort)
            {
                return new Nullable<ushort>((ushort)value);
            }
            try
            {
                return new Nullable<ushort>(Convert.ToUInt16(value));
            }
            catch
            {
                return new Nullable<ushort>();
            }
        }
        public static Nullable<int> ToNInt32(object value)
        {
            if (value is int)
            {
                return new Nullable<int>((int)value);
            }
            try
            {
                return new Nullable<int>(Convert.ToInt32(value));
            }
            catch
            {
                return new Nullable<int>();
            }
        }
        public static Nullable<uint> ToNUInt32(object value)
        {
            if (value is uint)
            {
                return new Nullable<uint>((uint)value);
            }
            try
            {
                return new Nullable<uint>(Convert.ToUInt32(value));
            }
            catch
            {
                return new Nullable<uint>();
            }
        }
        public static Nullable<long> ToNInt64(object value)
        {
            if (value is long)
            {
                return new Nullable<long>((long)value);
            }
            try
            {
                return new Nullable<long>(Convert.ToInt64(value));
            }
            catch
            {
                return new Nullable<long>();
            }
        }
        public static Nullable<ulong> ToNUInt64(object value)
        {
            if (value is long)
            {
                return new Nullable<ulong>((ulong)value);
            }
            try
            {
                return new Nullable<ulong>(Convert.ToUInt64(value));
            }
            catch
            {
                return new Nullable<ulong>();
            }
        }
        public static Nullable<bool> ToNBoolean(object value)
        {
            if (value is bool)
            {
                return new Nullable<bool>((bool)value);
            }
            try
            {
                return new Nullable<bool>(Convert.ToBoolean(value));
            }
            catch
            {
                return new Nullable<bool>();
            }
        }
        public static Nullable<DateTime> ToNDateTime(object value)
        {
            if (value is DateTime)
            {
                return new Nullable<DateTime>((DateTime)value);
            }
            try
            {
                return new Nullable<DateTime>(Convert.ToDateTime(value));
            }
            catch
            {
                return new Nullable<DateTime>();
            }
        }
        public static Nullable<decimal> ToNDecimal(object value)
        {
            if (value is decimal)
            {
                return new Nullable<decimal>((decimal)value);
            }
            try
            {
                return new Nullable<decimal>(Convert.ToDecimal(value));
            }
            catch
            {
                return new Nullable<decimal>();
            }
        }
        public static Nullable<double> ToNDouble(object value)
        {
            if (value is double)
            {
                return new Nullable<double>((double)value);
            }
            try
            {
                return new Nullable<double>(Convert.ToDouble(value));
            }
            catch
            {
                return new Nullable<double>();
            }
        }
        public static Nullable<float> ToNFloat(object value)
        {
            if (value is Single || value is float)
            {
                return new Nullable<float>((float)value);
            }
            try
            {
                return new Nullable<float>(Convert.ToSingle(value));
            }
            catch
            {
                return new Nullable<float>();
            }
        }
        public static Nullable<Guid> ToNGuid(object value)
        {
            if (value is Guid)
            {
                return new Nullable<Guid>((Guid)value);
            }
            try
            {
                return new Nullable<Guid>(Guid.Parse(value.ToString()));
            }
            catch
            {
                return new Nullable<Guid>();
            }
        }
    }
}
