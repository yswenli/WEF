#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
#if NET20
using WEFInternal.Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;

#endif

namespace WEFInternal.Newtonsoft.Json.Utilities
{
    internal static class JavaScriptUtils
    {
        internal static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];
        internal static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];
        internal static readonly bool[] HtmlCharEscapeFlags = new bool[128];

        static JavaScriptUtils()
        {
            IList<char> escapeChars = new List<char>
            {
                '\n', '\r', '\t', '\\', '\f', '\b',
            };
            for (int i = 0; i < ' '; i++)
            {
                escapeChars.Add((char)i);
            }

            foreach (var escapeChar in escapeChars.Union(new[] { '\'' }))
            {
                SingleQuoteCharEscapeFlags[escapeChar] = true;
            }
            foreach (var escapeChar in escapeChars.Union(new[] { '"' }))
            {
                DoubleQuoteCharEscapeFlags[escapeChar] = true;
            }
            foreach (var escapeChar in escapeChars.Union(new[] { '"', '\'', '<', '>', '&' }))
            {
                HtmlCharEscapeFlags[escapeChar] = true;
            }
        }

        private const string EscapedUnicodeText = "!";

        public static bool[] GetCharEscapeFlags(StringEscapeHandling stringEscapeHandling, char quoteChar)
        {
            if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
                return HtmlCharEscapeFlags;

            if (quoteChar == '"')
                return DoubleQuoteCharEscapeFlags;

            return SingleQuoteCharEscapeFlags;
        }

        public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
        {
            if (s == null)
                return false;

            foreach (char c in s)
            {
                if (c >= charEscapeFlags.Length || charEscapeFlags[c])
                    return true;
            }

            return false;
        }

        public static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter, bool appendDelimiters,
            bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, ref char[] writeBuffer)
        {
            // leading delimiter
            if (appendDelimiters)
                writer.Write(delimiter);

            if (s != null)
            {
                int lastWritePosition = 0;

                for (int i = 0; i < s.Length; i++)
                {
                    var c = s[i];

                    if (c < charEscapeFlags.Length && !charEscapeFlags[c])
                        continue;

                    string escapedValue;

                    switch (c)
                    {
                        case '\t':
                            escapedValue = @"\t";
                            break;
                        case '\n':
                            escapedValue = @"\n";
                            break;
                        case '\r':
                            escapedValue = @"\r";
                            break;
                        case '\f':
                            escapedValue = @"\f";
                            break;
                        case '\b':
                            escapedValue = @"\b";
                            break;
                        case '\\':
                            escapedValue = @"\\";
                            break;
                        case '\u0085': // Next Line
                            escapedValue = @"\u0085";
                            break;
                        case '\u2028': // Line Separator
                            escapedValue = @"\u2028";
                            break;
                        case '\u2029': // Paragraph Separator
                            escapedValue = @"\u2029";
                            break;
                        default:
                            if (c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                            {
                                if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                {
                                    escapedValue = @"\'";
                                }
                                else if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                {
                                    escapedValue = @"\""";
                                }
                                else
                                {
                                    if (writeBuffer == null)
                                        writeBuffer = new char[6];

                                    StringUtils.ToCharAsUnicode(c, writeBuffer);

                                    // slightly hacky but it saves multiple conditions in if test
                                    escapedValue = EscapedUnicodeText;
                                }
                            }
                            else
                            {
                                escapedValue = null;
                            }
                            break;
                    }

                    if (escapedValue == null)
                        continue;

                    bool isEscapedUnicodeText = string.Equals(escapedValue, EscapedUnicodeText);

                    if (i > lastWritePosition)
                    {
                        int length = i - lastWritePosition + ((isEscapedUnicodeText) ? 6 : 0);
                        int start = (isEscapedUnicodeText) ? 6 : 0;

                        if (writeBuffer == null || writeBuffer.Length < length)
                        {
                            char[] newBuffer = new char[length];

                            // the unicode text is already in the buffer
                            // copy it over when creating new buffer
                            if (isEscapedUnicodeText)
                                Array.Copy(writeBuffer, newBuffer, 6);

                            writeBuffer = newBuffer;
                        }

                        s.CopyTo(lastWritePosition, writeBuffer, start, length - start);

                        // write unchanged chars before writing escaped text
                        writer.Write(writeBuffer, start, length - start);
                    }

                    lastWritePosition = i + 1;
                    if (!isEscapedUnicodeText)
                        writer.Write(escapedValue);
                    else
                        writer.Write(writeBuffer, 0, 6);
                }

                if (lastWritePosition == 0)
                {
                    // no escaped text, write entire string
                    writer.Write(s);
                }
                else
                {
                    int length = s.Length - lastWritePosition;

                    if (writeBuffer == null || writeBuffer.Length < length)
                        writeBuffer = new char[length];

                    s.CopyTo(lastWritePosition, writeBuffer, 0, length);

                    // write remaining text
                    writer.Write(writeBuffer, 0, length);
                }
            }

            // trailing delimiter
            if (appendDelimiters)
                writer.Write(delimiter);
        }

        public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
        {
            return ToEscapedJavaScriptString(value, delimiter, appendDelimiters, StringEscapeHandling.Default);
        }

        public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters, StringEscapeHandling stringEscapeHandling)
        {
            bool[] charEscapeFlags = GetCharEscapeFlags(stringEscapeHandling, delimiter);

            using (StringWriter w = StringUtils.CreateStringWriter(StringUtils.GetLength(value) ?? 16))
            {
                char[] buffer = null;
                WriteEscapedJavaScriptString(w, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, ref buffer);
                return w.ToString();
            }
        }
    }
}
