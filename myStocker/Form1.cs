using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myStocker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private System.Timers.Timer timer1;
        private HttpCodeLib.XJHTTP xj;
        private string market1_url = "http://webquotepic.eastmoney.com/GetPic.aspx?id=0000011&imageType=rm&token=44c9d251add88e27b65ed86506f6e5da";
        private string market2_url = "http://webquotepic.eastmoney.com/GetPic.aspx?id=3990012&imageType=rm&token=44c9d251add88e27b65ed86506f6e5da";
        private string market3_url = "http://webquotepic.eastmoney.com/GetPic.aspx?id=3990062&imageType=rm&token=44c9d251add88e27b65ed86506f6e5da";
        private void Form1_Load(object sender, EventArgs e)
        {
            
            timer1 = new System.Timers.Timer(60*100);
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_TimesUp);
        }
        private void Timer1_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            getMarketPic();
        }

        public delegate void ShowMessage();//创建一个代理 
        public void ShowTextBox(PictureBox pb, Image img)
        {
            if (pb.InvokeRequired)
            {
                ShowMessage msg;
                msg = () =>
                {
                    pb.Image = img;
                };
                pb.Invoke(msg);
                return;
            }
            else
            {
                pb.Image = img;
            }
        }

        private void getMarketPic()
        {
            xj = new HttpCodeLib.XJHTTP();
            var rtn1 = xj.GetImageByImage(market1_url, "", null);
            var rtn2 = xj.GetImageByImage(market2_url, "", null);
            var rtn3 = xj.GetImageByImage(market3_url, "", null);
            ShowTextBox(pictureBox1, MakeGrayscale((Bitmap)rtn1));
            ShowTextBox(pictureBox2, MakeGrayscale((Bitmap)rtn2));
            ShowTextBox(pictureBox3, MakeGrayscale((Bitmap)rtn3));
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = true;
            userControl11.timer1.Start();
            userControl12.timer1.Start();
            userControl13.timer1.Start();
            timer1.Start();
            toolStripButton1.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = true;
            userControl11.timer1.Stop();
            userControl12.timer1.Stop();
            userControl13.timer1.Stop();
            timer1.Stop();
            toolStripButton2.Enabled = false;
        }
        private static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                                    new float[][]
                        {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                        });
            //create some image attributes
            System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }
    }
}
