/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Common
*文件名： InvokeHelper
*版本号： V1.0.0.0
*唯一标识：fb5126a6-0c23-4d19-bc5d-68d0f9d51375
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/6/3 15:51:33
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/6/3 15:51:33
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 线程同步ui线程
    /// </summary>
    public static class InvokeHelper
    {
        /// <summary>
        /// 线程同步ui线程
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="action"></param>
        public static void Invoke(this Form owner, Action action)
        {
            if (!owner.IsHandleCreated) throw new InvalidOperationException("父对象必须是从UI线程上创建的");
            if (owner.InvokeRequired)
            {
                owner.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// 异步线程同步ui线程
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="action"></param>
        public static void InvokeAsync(this Form owner, Action action)
        {
            if (!owner.IsHandleCreated) throw new InvalidOperationException("父对象必须是从UI线程上创建的");
            Task.Run(() => owner.Invoke(action));
        }



        /// <summary>
        /// 安全弹出模态窗体
        /// </summary>
        /// <param name="form"></param>
        /// <param name="owner"></param>
        public static void ShowDialogAsync(this Form form, Form owner)
        {
            owner.InvokeAsync(() => form.ShowDialog(owner));
        }

        /// <summary>
        /// 安全关闭弹出模态窗体
        /// </summary>
        /// <param name="form"></param>
        /// <param name="owner"></param>
        public static void HideDialogAsync(this Form form, Form owner)
        {
            owner.InvokeAsync(() => form.Hide());
        }


        /// <summary>
        /// 安全弹出模态窗体
        /// </summary>
        /// <param name="form"></param>
        /// <param name="owner"></param>
        /// <param name="action"></param>
        /// <param name="time"></param>
        public static void ShowDialogWithLoopAsync(this Form form, Form owner, Action<string> action, int time = 1000)
        {
            Task.Run(() =>
            {
                owner.Invoke(() =>
                {
                    Task.Run(() =>
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();

                        while (true)
                        {
                            var visiable = true;

                            owner.Invoke(() =>
                            {
                                visiable = form.Visible;
                            });

                            if (!visiable)
                            {
                                break;
                            }

                            var seconds = (stopwatch.ElapsedMilliseconds / 1000).ToString("N");

                            owner.Invoke(() => action.Invoke(seconds));

                            Thread.Sleep(time);
                        }
                    });
                    if (!form.Visible)
                        form.ShowDialog(owner);
                });
            });
        }

    }
}
