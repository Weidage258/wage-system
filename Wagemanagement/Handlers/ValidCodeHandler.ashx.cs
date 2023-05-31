using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Wagemanagement.Handlers
{
    /// <summary>
    /// ValidCodeHandler 的摘要说明
    /// </summary>
    public class ValidCodeHandler : IHttpHandler, IRequiresSessionState
    {

        //public void ProcessRequest(HttpContext context)
        //{
        //    Color[] colors = new Color[]
        //   {
        //        Color.Red,Color.Green,Color.Yellow,Color.Blue,Color.AliceBlue,Color.Aqua,Color.Bisque,Color.Brown
        //   };
        //    Image img = new Bitmap(100, 36);
        //    Graphics graphics = Graphics.FromImage(img);
        //    Random random = new Random(DateTime.Now.Millisecond);
        //    int charNum1 = random.Next(1, 9);
        //    char charNum2 = '*';
        //    int charNum3 = random.Next(1, 9);
        //    char charNum4 = '=';
        //    //保存验证码
        //    context.Session["sn"] = charNum1 * charNum3;

        //    Font font = new Font("宋体", 24);
        //    Brush brush1 = new SolidBrush(colors[random.Next(0, colors.Length - 1)]);
        //    graphics.DrawString((charNum1).ToString(), font, brush1, 7, -3);
        //    Brush brush2 = new SolidBrush(colors[random.Next(0, colors.Length - 1)]);
        //    graphics.DrawString((charNum2).ToString(), font, brush2, 26, 0);
        //    Brush brush3 = new SolidBrush(colors[random.Next(0, colors.Length - 1)]);
        //    graphics.DrawString((charNum3).ToString(), font, brush3, 50, 3);
        //    Brush brush4 = new SolidBrush(colors[random.Next(0, colors.Length - 1)]);
        //    graphics.DrawString((charNum4).ToString(), font, brush4, 70, 0);
        //    context.Response.ContentType = "image/jpeg";
        //    img.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        //    img.Dispose();
        //}
        private Random RandomSeed = new Random();
        public void ProcessRequest(HttpContext context)
        {
            string strWord = "23456789QWERTYUPASDFGHKXCVBNM";
            string NumStr = null;
            for (int i = 0; i < 4; i++)
            {
                NumStr += strWord[RandomSeed.Next(0, strWord.Length)];
            }
            context.Session["sn"] = NumStr.ToLower();
            CreateImages(context, NumStr);
        }
        private void CreateImages(HttpContext context, string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 17);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 32);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);

            }
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(font[findex], 20, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 2 + (i * 12), ii);

            }
            g.DrawRectangle(new Pen(ColorTranslator.FromHtml("#cccccc"), 0), 0, 0, image.Width - 1, image.Height - 1);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            context.Response.ClearContent();
            context.Response.ContentType = "image/gif";
            context.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}