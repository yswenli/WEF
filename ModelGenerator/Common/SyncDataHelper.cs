/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Common
*文件名： SyncDataHelper
*版本号： V1.0.0.0
*唯一标识：94b4a9e0-cddc-4aaa-b26f-e6dd679efe2d
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/7/20 10:46:08
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/20 10:46:08
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Common
{
    public delegate void OnLogHandler(string logTxt);

    public static class SyncDataHelper
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static event OnLogHandler OnLog;

        static void RaiseOnLog(string txt)
        {
            OnLog?.BeginInvoke($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {txt}", null, null);
        }

        static CancellationToken _stopToken;

        static CancellationTokenSource _cancellationTokenSource;

        static DateTime _startedTime;

        public static void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _stopToken = _cancellationTokenSource.Token;
            _startedTime = DateTime.Now;
            Task.Run(Run, _stopToken);
        }

        public static void Stop()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        static async void Run()
        {
            await Task.Yield();

            if (_stopToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                var configs = DataSyncConfig.Read();

                if (configs == null || configs.Count < 1)
                {
                    RaiseOnLog("执行同步失败，无任何同步配置");

                    return;
                }

                var cs = configs.Where(q => q.IsEnabled).ToList();

                if (cs == null || cs.Count < 1)
                {
                    RaiseOnLog("执行同步失败，无任何可用的同步配置");

                    return;
                }

                foreach (var item in cs)
                {
                    SyncData(_stopToken, item);
                }
            }
            catch (Exception ex)
            {
                OnLog?.BeginInvoke(ex.Message, null, null);
            }
        }

        static async void SyncData(CancellationToken token, DataSyncConfig dataSyncConfig)
        {
            await Task.Yield();

            var runSpanTime = DateTime.FromFileTime(_startedTime.AddMinutes(dataSyncConfig.RunSpan).ToFileTime());

            var recordTime = dataSyncConfig.Started;

            while (true)
            {
                await Task.Delay(1000);

                if (token.IsCancellationRequested)
                {
                    RaiseOnLog($"同步任务【{dataSyncConfig.Name}】已取消");
                    return;
                }

                if (runSpanTime <= DateTime.Now)
                {
                    RaiseOnLog($"同步任务【{dataSyncConfig.Name}】开始处理");

                    await Task.Run(async () =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            RaiseOnLog($"同步任务【{dataSyncConfig.Name}】已取消");
                            return;
                        }

                        var sql = dataSyncConfig.Source.Sql.Replace("{datetime}", $"'{recordTime:yyyy-MM-dd HH:mm:ss.fff}'");

                        runSpanTime = runSpanTime.AddMinutes(dataSyncConfig.RunSpan);

                        DataTable dt = null;
                        Stopwatch stopwatch = null;
                        try
                        {
                            stopwatch = Stopwatch.StartNew();

                            var source = DBObjectHelper.GetDBObject(dataSyncConfig.Source);

                            recordTime = DateTime.Now;

                            var sourceData = source.Query(dataSyncConfig.Source.Database, sql);

                            if (sourceData == null || sourceData.Tables == null || sourceData.Tables.Count < 1 || sourceData.Tables[0].Rows == null)
                            {
                                RaiseOnLog($"同步任务【{dataSyncConfig.Name}】未获取到数据，用时:{stopwatch.ElapsedMilliseconds} ms，任务已退出");

                                return;
                            }
                            else
                            {
                                RaiseOnLog($"同步任务【{dataSyncConfig.Name}】已获取数据{sourceData.Tables[0].Rows.Count}条，用时:{stopwatch.ElapsedMilliseconds} ms");
                            }

                            dt = sourceData.Tables[0];
                        }
                        catch (Exception ex)
                        {
                            RaiseOnLog($"同步任务【{dataSyncConfig.Name}】获取源数据失败,任务已退出，Error:" + ex.Message);
                        }
                        finally
                        {
                            stopwatch.Stop();
                        }

                        await Task.Yield();

                        if (token.IsCancellationRequested)
                        {
                            RaiseOnLog($"同步任务【{dataSyncConfig.Name}】已取消");
                            return;
                        }

                        try
                        {
                            stopwatch = Stopwatch.StartNew();

                            if (dt != null)
                            {
                                var target = DBObjectHelper.GetDBContext(dataSyncConfig.Target);

                                target.BulkInsert(dataSyncConfig.Target.TableName, dt);

                                RaiseOnLog($"同步任务【{dataSyncConfig.Name}】已处理完成，用时:{stopwatch.ElapsedMilliseconds} ms");
                            }
                        }
                        catch (Exception ex)
                        {
                            RaiseOnLog($"同步任务【{dataSyncConfig.Name}】获取源数据失败,任务已退出，Error:" + ex.Message);
                        }
                        finally
                        {
                            stopwatch.Stop();
                        }
                    });
                }

            }



        }
    }
}
