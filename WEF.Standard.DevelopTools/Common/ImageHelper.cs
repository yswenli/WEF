using System;
using System.Drawing;

namespace WEF.Standard.DevelopTools.Common
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 根据图片exif信息来旋转调整图片
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image OrientationImage(Image image)
        {
            if (image == null || image.PropertyIdList == null || image.PropertyIdList.Length < 1) return image;

            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {
                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                image.RemovePropertyItem(274);

            }
            return image;
        }
    }
}
