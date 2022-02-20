using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace TSGS_CS_Login_Client
{
    public static class TSGS_CS_Utilities
    {

        public static Bitmap RotateImage(System.Drawing.Image image, float angle)
        {
            int OldWidth = image.Width;
            int OldHeight = image.Height;
            double AngleInRadians = angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(AngleInRadians));
            double sin = Math.Abs(Math.Sin(AngleInRadians));
/*            int NewWidth = (int)(OldWidth * cos + OldHeight * sin);
            int NewHeight = (int)(OldWidth * sin + OldHeight * cos); */
            int NewWidth = Math.Max(OldWidth, OldHeight);
            int NewHeight = Math.Max(OldWidth, OldHeight);

            var newBitmap = new Bitmap(NewWidth, NewHeight);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)(NewWidth - OldWidth) / 2, (float)(NewHeight - OldHeight) / 2);
            graphics.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
            graphics.DrawImage(image, new Point(0, 0));
            return newBitmap;
            //return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2),angle);
        }

        public static Bitmap RotateImage(System.Drawing.Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }
    }


}