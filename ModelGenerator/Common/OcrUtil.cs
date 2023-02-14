/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Common
*文件名： OcrUtil
*版本号： V1.0.0.0
*唯一标识：a10e9b61-ed76-4c55-89c7-f321e65e0cbe
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/31 16:42:48
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/31 16:42:48
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;

using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Online;

namespace WEF.ModelGenerator.Common
{

    /// <summary>
    /// orc引擎
    /// </summary>
    public class OCREngine
    {
        BlockingQueue<OCRData> _queue;

        Dictionary<int, FullOcrModel> _modelDic = null;

        /// <summary>
        /// orc引擎
        /// </summary>
        /// <param name="done"></param>
        async void DownloadModelAsync(Action done = null)
        {
            _modelDic = new Dictionary<int, FullOcrModel>();
            _modelDic.Add(0, await OnlineFullModels.ChineseServerV2.DownloadAsync());
            _modelDic.Add(1, await OnlineFullModels.ChineseV2.DownloadAsync());
            _modelDic.Add(2, await OnlineFullModels.ChineseV3.DownloadAsync());
            done?.Invoke();
        }

        /// <summary>
        /// 识别事件
        /// </summary>
        public event OnRecognizeHandler OnRecognize;

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; private set; } = false;

        /// <summary>
        /// 引擎类型
        /// </summary>
        public EngineType EngineType { get; private set; }

        bool _desc = true;

        /// <summary>
        /// orc引擎
        /// </summary>
        public OCREngine()
        {
            _queue = new BlockingQueue<OCRData>();
        }

        /// <summary>
        /// 初始化,此模型需要下载，首次初始化需要较长时间
        /// </summary>
        /// <param name="engineType"></param>
        /// <param name="desc"></param>
        public void Init(EngineType engineType = EngineType.V3, bool desc = true)
        {
            EngineType = engineType;

            _desc = desc;

            DownloadModelAsync(() =>
            {
                FullOcrModel model;

                switch (engineType)
                {
                    case EngineType.ServerV2:
                        model = _modelDic[0];
                        break;
                    case EngineType.V2:
                    default:
                        model = _modelDic[1];
                        break;
                    case EngineType.V3:
                        model = _modelDic[2];
                        break;
                }

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (!State)
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                        var data = _queue.Dequeue(1000);
                        if (data == null || string.IsNullOrEmpty(data.Key) || string.IsNullOrEmpty(data.ImageBase64Str))
                        {
                            Thread.Sleep(1);
                        }
                        else
                        {
                            Execute(model, data);
                        }
                    }
                }, TaskCreationOptions.LongRunning);

            });
        }

        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeOut">超时，单位秒</param>
        /// <param name="score">正确率</param>
        public void Recognize(string key, string data, int timeOut = 10, float score = 0.6f) => Recognize(new OCRData(key, data, timeOut, score));


        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="ocrData"></param>
        public void Recognize(OCRData ocrData)
        {
            _queue.Enqueue(ocrData);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ocrData"></param>
        void Execute(FullOcrModel model, OCRData ocrData)
        {
            var sw = Stopwatch.StartNew();

            var ocrResult = new OCRResult(ocrData.Key);

            try
            {
                using (var cts = new CancellationTokenSource(ocrData.TimeOut * 1000))
                {
                    var expired = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(ocrData.ImageBase64Str);

                            using (PaddleOcrAll all = new PaddleOcrAll(model)
                            {
                                AllowRotateDetection = true, /* 允许识别有角度的文字 */
                                Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
                            })
                            {
                                using (Mat src = Cv2.ImDecode(bytes, ImreadModes.Color))
                                {
                                    PaddleOcrResult result = all.Run(src);

                                    if (result == null || string.IsNullOrEmpty(result.Text)) throw new NullReferenceException("识别失败");

                                    PaddleOcrResultRegion[] regions;
                                    if (!_desc)
                                    {
                                        regions = result.Regions.Reverse().ToArray();
                                    }
                                    else
                                    {
                                        regions = result.Regions;
                                    }

                                    int index = 0;
                                    ocrResult.Data = new List<OcrResultItem>();
                                    foreach (PaddleOcrResultRegion region in result.Regions)
                                    {
                                        if (region.Score >= ocrData.Score)
                                        {
                                            index++;
                                            if (region.Rect.Angle >= 80 && region.Rect.Angle <= 110)
                                            {
                                                ocrResult.Data.Add(new OcrResultItem()
                                                {
                                                    Index = index,
                                                    Text = region.Text,
                                                    Score = region.Score,
                                                    Region = new OcrRegion()
                                                    {
                                                        Left = region.Rect.Center.X - region.Rect.Size.Height / 2,
                                                        Top = region.Rect.Center.Y - region.Rect.Size.Width / 2,
                                                        Width = region.Rect.Size.Width,
                                                        Height = region.Rect.Size.Height
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                ocrResult.Data.Add(new OcrResultItem()
                                                {
                                                    Index = index,
                                                    Text = region.Text,
                                                    Score = region.Score,
                                                    Region = new OcrRegion()
                                                    {
                                                        Left = region.Rect.Center.X - region.Rect.Size.Width / 2,
                                                        Top = region.Rect.Center.Y - region.Rect.Size.Height / 2,
                                                        Width = region.Rect.Size.Width,
                                                        Height = region.Rect.Size.Height
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    ocrResult.Status = true;
                                    ocrResult.Message = "操作成功";
                                    sw.Stop();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            ocrResult.Status = false;
                            ocrResult.Message = "操作失败：" + e.Message;
                        }
                    }, cts.Token).Wait(ocrData.TimeOut * 1000);
                    if (!expired)
                    {
                        ocrResult.Status = false;
                        ocrResult.Message = "操作失败：操作已超时";
                    }
                }
            }
            catch (Exception ex)
            {
                ocrResult.Status = false;
                ocrResult.Message = "操作失败：" + ex.Message;
            }
            ocrResult.Cost = sw.ElapsedMilliseconds;
            OnRecognize?.Invoke(ocrResult);
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            State = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            State = false;
        }
    }


    /// <summary>
    /// 识别事件委托
    /// </summary>
    /// <param name="ocrResult"></param>
    public delegate void OnRecognizeHandler(OCRResult ocrResult);

    /// <summary>
    /// 图处处理类
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// 获取识别后合成的缩略图
        /// </summary>
        /// <param name="request"></param>
        public static byte[] GetComposeImage(ComposeImageRequest request)
        {
            try
            {
                var filePath1 = Path.Combine(Application.StartupPath, Guid.NewGuid().ToString("N") + ".jpg");

                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(request.Url, filePath1);
                }

                var filePath2 = Path.Combine(Application.StartupPath, Guid.NewGuid().ToString("N") + ".jpg");

                using (var image = Bitmap.FromFile(filePath1))
                {
                    var pen = new Pen(request.BorderColor, request.BorderSize);

                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    using (var g = Graphics.FromImage(image))
                    {
                        foreach (var item in request.OcrResult.Data)
                        {
                            if (item.Region.Height >= item.Region.Width * 1.5)
                            {
                                item.Region.Height = item.Region.Height + item.Region.Width;
                                item.Region.Width = item.Region.Height - item.Region.Width;
                                item.Region.Height = item.Region.Height - item.Region.Width;
                            }
                            g.DrawRectangle(pen, item.Region.Left, item.Region.Top, item.Region.Width, item.Region.Height);
                            g.DrawString(item.Index.ToString(),
                                new Font("微软雅黑", request.FontSize, FontStyle.Regular),
                                Brushes.Red,
                                item.Region.Left + 3, item.Region.Top + 3);
                        }

                        g.Save();
                    };

                    image.Save(filePath2, ImageFormat.Jpeg);
                }

                var data = File.ReadAllBytes(filePath2);

                File.Delete(filePath1);
                File.Delete(filePath2);

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
    }
    #region models

    /// <summary>
    /// 合成请求参数
    /// </summary>
    public class ComposeImageRequest
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; } = 10;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Brush FontColor { get; set; } = Brushes.Red;
        /// <summary>
        /// 边框大小
        /// </summary>
        public int BorderSize { get; set; } = 1;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor { get; set; } = Color.Red;
        /// <summary>
        /// ocr识别结果
        /// </summary>
        public OCRResult OcrResult { get; set; }
    }

    /// <summary>
    /// 引擎类型
    /// </summary>
    public enum EngineType : byte
    {
        /// <summary>
        /// PPOcrServerV2
        /// </summary>
        [Description("PPOcrServerV2")]
        ServerV2 = 1,
        /// <summary>
        /// PP-OCRv2
        /// </summary>
        [Description("PP-OCRv2")]
        V2 = 2,
        /// <summary>
        /// PP-OCRv3
        /// </summary>
        [Description("PP-OCRv3")]
        V3 = 3
    }

    /// <summary>
    /// 数据
    /// </summary>
    public class OCRData
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 图片base64值
        /// </summary>
        public string ImageBase64Str { get; set; }
        /// <summary>
        /// 识别超时时间，秒
        /// </summary>
        public int TimeOut { get; set; } = 10;
        /// <summary>
        /// 正确率
        /// </summary>
        public float Score { get; set; } = 0.6F;

        /// <summary>
        /// 数据
        /// </summary>
        public OCRData()
        {

        }
        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <param name="score"></param>
        public OCRData(string key, string data, int timeOut, float score)
        {
            Key = key;
            ImageBase64Str = data;
            TimeOut = timeOut;
            Score = score;
        }
    }
    /// <summary>
    /// 执行结果
    /// </summary>
    public class OCRResult
    {
        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 执行结果状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 用时
        /// </summary>
        public long Cost { get; set; }
        /// <summary>
        /// 识别的数据
        /// </summary>
        public List<OcrResultItem> Data { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public OCRResult()
        {
            Status = false;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="key"></param>
        public OCRResult(string key) : this()
        {
            this.Key = key;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="cost"></param>
        /// <param name="data"></param>
        public OCRResult(string key, bool status, string message, long cost, List<OcrResultItem> data)
        {
            Key = key;
            Status = status;
            Message = message;
            Cost = cost;
            Data = data;
        }
    }
    /// <summary>
    /// 执行结果
    /// </summary>
    public class OcrResultItem
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public OcrRegion Region { get; set; }
    }

    /// <summary>
    /// ocr识别区域
    /// </summary>
    public class OcrRegion
    {
        public float Left { get; set; }

        public float Top { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }
    }

    #endregion
}
