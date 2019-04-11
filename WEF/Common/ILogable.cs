/*****************************************************************************************************
 * �������Ȩ��Wenli���У�All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * ������WENLI-PC
 * ��¼�û���Administrator
 * CLR�汾��4.0.30319.17929
 * Ψһ��ʶ��fc2b3c60-82bd-4265-bf8c-051e512a1035
 * �������ƣ�WENLI-PC
 * ��ϵ�����䣺wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace WEF.Common
{
    /// <summary>
    /// A delegate used for log.
    /// </summary>
    /// <param name="logMsg">The msg to write to log.</param>
    public delegate void LogHandler(string logMsg);

    /// <summary>
    /// Mark a implementing class as loggable.
    /// </summary>
    interface ILogable
    {
        /// <summary>
        /// OnLog event.
        /// </summary>
        event LogHandler OnLog;
    }
}