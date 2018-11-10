using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Light_Control_Test
{
   static  class NormolProcess
    {

        public static string[] imgInfo=null;
        public static int imgHasSet = 0;

        public static void LightClass(string name)
        {

        }


       //static Thread thread = new Thread(ImgInfoHasSet);


        //public static void ImgInfoHasSet()    //模仿MessageBox产生的窗体返回值
        //{
        //    imgHasSet = 0;
        //    ImgSet imgset = new ImgSet();
        //    imgset.ShowDialog       
        //    while (imgHasSet==0)
        //    {
        //        if (imgHasSet>0)
        //        {
        //            imgHasSet = 0;
        //            break;
        //        }
        //        if(imgHasSet<0)
        //        {
        //            imgHasSet = 0;
        //            break;
        //        }
        //    }
        //    imgHasSet = 0;
          
        //}



        public static string SelectPic(string flag)
        {
            string pic = "pack://application:,,,/Resources/配电箱黄.png";
            switch (flag)    //根据高位划分灯具类型并添加不同图片显示
            {
                //case "1": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }
                case "1": { pic = "pack://application:,,,/Resources/照明灯绿.png"; break; }
                case "1全亮": { pic = "pack://application:,,,/Resources/照明灯绿.png"; break; }
                case "1全灭": { pic = "pack://application:,,,/Resources/照明灯灰.png"; break; }
                case "1左亮": { pic = "pack://application:,,,/Resources/照明灯黄.png.png"; break; }

                case "2": { pic = "pack://application:,,,/Resources/标志灯双向绿.png"; break; }
                case "2全亮": { pic = "pack://application:,,,/Resources/标志灯双向绿.png"; break; }
                case "2全灭": { pic = "pack://application:,,,/Resources/标志灯双向灭.png"; break; }
                case "2左亮": { pic = "pack://application:,,,/Resources/标志灯左绿.png"; break; }
                case "2右亮": { pic = "pack://application:,,,/Resources/标志灯右绿.png"; break; }

                case "3": { pic = "pack://application:,,,/Resources/双头灯绿.png"; break; }
                case "3全亮": { pic = "pack://application:,,,/Resources/双头灯绿.png"; break; }
                case "3全灭": { pic = "pack://application:,,,/Resources/双头灯灰.png"; break; }
                case "3左亮": { pic = "pack://application:,,,/Resources/双头灯黄.png"; break; }

                //case "3": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/双头灯绿.png")), Stretch = Stretch.Uniform }; break; }
                //case "4": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/地埋灯双向绿.png")), Stretch = Stretch.Uniform }; break; }
                //case "5":
                //    {
                //        if (code[0].Split('_')[1].ToCharArray()[4].ToString() == "0")
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        break;
                //    }
                //case "7":
                //    {
                //        if (code[0].Split('_')[1].ToCharArray()[4].ToString() == "0")
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        break;
                //    }
                //case "8": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }

                default:
                    break;
            }
            return pic;
        }
    }
}
