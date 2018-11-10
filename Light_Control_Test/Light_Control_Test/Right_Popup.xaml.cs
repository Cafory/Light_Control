using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Light_Control_Test
{
    /// <summary>
    /// Right_Popup.xaml 的交互逻辑
    /// </summary>
    public partial class Right_Popup : Window
    {
        public Right_Popup(string _addName, string status) //_addName里面有灯码和配电箱码，                                                        
        {                                                    //  如果是配电箱的右键只有配电箱码，灯的右键第一个码是灯，第二个是配电箱，status才是状态标识符
            InitializeComponent();
            IniMethod(_addName, status);
        }

        public Right_Popup(string _addName, string status,bool imgFlag) //_addName里面有灯码和配电箱码，                                                        
        {                                                    //  如果是配电箱的右键只有配电箱码，灯的右键第一个码是灯，第二个是配电箱，status才是状态标识符
            InitializeComponent();
            _imgFlag = imgFlag;
            IniMethod(_addName, status);
        }

        public void IniMethod(string _addName, string status)
        {
            _status = status;
            code = _addName.Split('A');
            windowTitle.Content = status + code[0];
            replace_BeforeCode.Content = code[0];
            replace_BeforeTip.Content = "原" + status + "码:";
            replace_NowTip.Content = status + "码";
            if (_status == "配电箱")
            {
                set.Content = "安装位置";
            }
            else
            {
                set.Content = "设置";
            }

        }
        bool _imgFlag = false;
        string _status;
        string[] code;
        private void Window_Deactivated(object sender, EventArgs e) //窗口失去焦点直接关闭配电箱右键窗体
        {
            this.Close();
        }


        private void control_Click(object sender, RoutedEventArgs e)
        {
            btnGrid.Visibility = Visibility.Collapsed;
            controlGrid.Visibility = Visibility.Visible;
        }

        private void controlGrid_Back_Click(object sender, RoutedEventArgs e)
        {
            controlGrid.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void replace_Click(object sender, RoutedEventArgs e)
        {
            btnGrid.Visibility = Visibility.Collapsed;
            lightReplace.Visibility = Visibility.Visible;
        }

        private void lightReplace_Back_Click(object sender, RoutedEventArgs e)
        {
            lightReplace.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void set_Click(object sender, RoutedEventArgs e)
        {
            if (_status == "配电箱")    //这里是因为配电箱是安装位置，灯具是设置稍有不同
            {
                btnGrid.Visibility = Visibility.Collapsed;
                setGrid_LightLocation.Visibility = Visibility.Visible;
                this.setGrid_LightLocation_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Pos_Tip.Content = "  配电箱位置";
            }
            else
            {
                btnGrid.Visibility = Visibility.Collapsed;
                setGrid.Visibility = Visibility.Visible;
                Pos_Tip.Content = "  灯具位置";
            }
        }

        private void setGrid_IniStatus_btn_Click(object sender, RoutedEventArgs e)
        {
            setGrid.Visibility = Visibility.Collapsed;
            setGrid_IniStatus.Visibility = Visibility.Visible;
        }

        private void setGrid_PrePlnModify_btn_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable("select * from " + code[1] + " where Name='" + code[0] + "'");

            string [] LPlan = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-');
            string [] RPlan = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-');

            if (LPlan.Length<5)
            {
                L1.Text = "未设置";
                L2.Text = "未设置";
                L3.Text = "未设置";
                L4.Text = "未设置";
                L5.Text = "未设置";
            }
            else
            {
                L1.Text = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-')[0];
                L2.Text = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-')[1];
                L3.Text = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-')[2];
                L4.Text = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-')[3];
                L5.Text = dataTable.Rows[0]["PrePlanLeft"].ToString().Split('-')[4];
            }

            if (RPlan.Length < 5)
            {
                R1.Text = "未设置";
                R2.Text = "未设置";
                R3.Text = "未设置";
                R4.Text = "未设置";
                R5.Text = "未设置";
            }
            else
            {
                R1.Text = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-')[0];
                R2.Text = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-')[1];
                R3.Text = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-')[2];
                R4.Text = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-')[3];
                R5.Text = dataTable.Rows[0]["PrePlanRight"].ToString().Split('-')[4];
            }


            setGrid.Visibility = Visibility.Collapsed;
            setGrid_prePlanModify.Visibility = Visibility.Visible;
        }

        private void setGrid_LightLocation_btn_Click(object sender, RoutedEventArgs e)   //设置界面的位置修改按钮
        {
            if (_status == "配电箱")
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select Position from Pei where Name='" + code[0] + "'");
                if (String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    IniPosition.Content = "原位置：未设置";
                }
                else
                    IniPosition.Content = "原位置：" + dt.Rows[0][0].ToString();
            }
            else
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select Position from  " + code[1] + "  where Name='" + code[0] + "'");
                if (String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    IniPosition.Content = "原位置：未设置";
                }
                else
                    IniPosition.Content = "原位置：" + dt.Rows[0][0].ToString();
            }
            setGrid.Visibility = Visibility.Collapsed;
            setGrid_LightLocation.Visibility = Visibility.Visible;
        }


        private void setGrid_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void IniStatus_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_IniStatus.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void IniStatus_Confirm_Click(object sender, RoutedEventArgs e)
        {
            //写入方法
            if (IniAllBright.IsChecked == true)
                IniAllBrightChecked();
            if (IniAllOff.IsChecked == true)
                IniAllOffChecked();
            if (IniLeftBright.IsChecked == true)
                IniLeftBrightChecked();
            if (IniRightBright.IsChecked == true)
                IniRightBrightChecked();
        }

        private void prePlanModify_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_prePlanModify.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void prePlanModify_Confirm_Click(object sender, RoutedEventArgs e)
        {
            //写入方法

            SqlHelper.ExecuteNonQuery("update " + code[1] + " set PrePlanLeft='" + L1.Text + "-" + L2.Text + "-" + L3.Text + "-" + L4.Text + "-" + L5.Text + "',PrePlanRight='" + R1.Text + "-" + R2.Text + "-" + R3.Text + "-" + R4.Text + "-" + R5.Text + "' where Name='"+code[0]+"'");
            setGrid_prePlanModify.Visibility = Visibility.Collapsed;   //返回界面
            setGrid.Visibility = Visibility.Visible;
        }

        private void lightLocation_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_LightLocation.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void lightLocation_Confirm_Click(object sender, RoutedEventArgs e)   //修改位置的方法
        {
            //写入方法
            if (_status == "配电箱")
            {
                string command = "update Pei set Position='" + Area.Text + "区" + Building.Text + "栋" + Layer.Text + "层" + Room.Text + "号" + "'  where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
            else
            {
                string command = "update " + code[1] + " set Position='" + Area.Text + "区" + Building.Text + "栋" + Layer.Text + "层" + Room.Text + "号" + "' where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
            // MessageBox.Show("修改成功！");
            this.setGrid_LightLocation_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void All_Shine_Click(object sender, RoutedEventArgs e)  //全亮
        {
            if (_status == "配电箱")
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x77 });  //配电箱下所有灯
            else
              if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x77 }))  //指定灯码
              {
                ChangeLNowStatus(SelectPic("全亮"), "全亮");
              }
        }

        private void All_Off_Click(object sender, RoutedEventArgs e)  //全灭
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x70 });
            }
            else
            {
                if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x70 }))
                {
                    ChangeLNowStatus(SelectPic("全灭"), "全灭");
                }
            }
        }

        private void Left_Shine_Click(object sender, RoutedEventArgs e)    //左亮
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x73 });
            }
            else
            {
                //if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x73 }))
                //{
                    ChangeLNowStatus(SelectPic("左亮"), "左亮");
                //}
            }
        }

        private void Right_Shine_Click(object sender, RoutedEventArgs e)    //右亮
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x76 });
            }
            else
            {
                if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x76 }))
                {
                    ChangeLNowStatus(SelectPic("右亮"), "右亮");
                }
            }
        }

        private void Shan_Click(object sender, RoutedEventArgs e)   //闪
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x07, 0x00 });
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x02, 0x01 });
            }
        }

        private void Shan_Before_Click(object sender, RoutedEventArgs e)  //闪前
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x08, 0x00 });
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x02, 0x00 });
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)   //替换灯码/配电箱码确认按钮
        {
            if (_status == "配电箱")
            {
                string command = "update Pei set Name='_" + nowCode.Text + "' where Name='" + code[0] + "'";  //更新数据库
                SqlHelper.ExecuteNonQuery(command);
                string content = "Id int identity(1,1) primary key,Name varchar(50),IdCode varchar(50),LogicCode varchar(50),LStatus varchar(10),Bright varchar(10),Position varchar(50),PrePlanLeft varchar(50),PrePlanRight varchar(50)";  //为每一个配电箱创建一个专用的表
                SqlHelper.CreateTable("LightControl", content, "_" + nowCode.Text);
                ((Button)((MainWindow)this.Owner).MainGrid.FindName("FireWorkBack")).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                command = "insert into  _" + nowCode.Text + "  " + "(Name,IdCode,LogicCode,LStatus,Bright,Position,PrePlanLeft,PrePlanRight) select Name,IdCode,LogicCode,LStatus,Bright,Position,PrePlanLeft,PrePlanRight from " + code[0] + " ;";
                SqlHelper.ExecuteNonQuery(command);
                command = "drop table " + code[0] + "";
                SqlHelper.ExecuteNonQuery(command);
                lightReplace.Visibility = Visibility.Collapsed;
                btnGrid.Visibility = Visibility.Visible;
            }
            else
            {
                string command = "update " + code[1] + " " + "set Name='" + nowCode.Text + "' where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
        }

        private void IniAllBrightChecked()  //修改指定灯状态为全亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x77 });
            }
        }

        private void IniAllOffChecked()//全灭
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x70 });
            }
        }

        private void IniRightBrightChecked()  //右亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x11 });
            }
        }

        private void IniLeftBrightChecked()   //左亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x44 });
            }
        }

        private void full_Click(object sender, RoutedEventArgs e)  //右键按钮界面后的 详情界面
        {
            btnGrid.Visibility = Visibility.Collapsed;
            //Info.Visibility = Visibility.Visible;
            if (_status=="配电箱")
            {
                InfoPei.Visibility = Visibility.Visible;
                DataTable dt = SqlHelper.ExecuteDataTable("select * from Pei");
                fullPeiLC.Content = dt.Rows.Count.ToString();
                dt = SqlHelper.ExecuteDataTable("select * from Pei where Name='" + code[0] + "'");
                fullPeiCodeDis.Content = code[0];
                if (String.IsNullOrEmpty(dt.Rows[0]["Position"].ToString()))
                    fullPeiLocation.Content = "未设置";
                else
                    fullPeiLocation.Content = dt.Rows[0]["Position"];
                if (String.IsNullOrEmpty(dt.Rows[0]["Status"].ToString()))
                    fullPeiStatus.Content = "未获取到";
                else
                    fullPeiStatus.Content = dt.Rows[0]["Status"].ToString();
            }
            else
            {
                Info.Visibility = Visibility.Visible;
                DataTable dt = SqlHelper.ExecuteDataTable("select * from " + code[1] + " where Name='" + code[0] + "'");
                inWhichPei.Content = code[1];
                lightCode.Content = code[0];

                if (String.IsNullOrEmpty(dt.Rows[0]["Position"].ToString()))
                    location.Content = "未设置";
                else
                    location.Content = dt.Rows[0]["Position"];
                if (String.IsNullOrEmpty(dt.Rows[0]["IniStatus"].ToString()))
                    iniStatus.Content = "未设置";
                else
                    iniStatus.Content = dt.Rows[0]["IniStatus"];
                if (String.IsNullOrEmpty(dt.Rows[0]["Status"].ToString()))
                    nowStatus.Content = "未设置";
                else
                    nowStatus.Content = dt.Rows[0]["Status"].ToString();

            }

        }

        private void emergency_Click(object sender, RoutedEventArgs e)
        {
            Instruction.C_ControlYearCheck();
        }


        public void ChangeLNowStatus(string path, string lStatus)  //改变灯的状态
        {
            if (_imgFlag)    //改变画布灯的状态
            {
                foreach (Image item in ((MainWindow)this.Owner).ImgShowCanvas.Children.OfType<Image>())
                {
                  
                        if (item.Name.Split('A')[0] == code[0])
                        {
                            item.Source =  new BitmapImage(new Uri(path));
                            SqlHelper.ExecuteNonQuery("update " + code[1] + " set Status='" + lStatus + "' where Name='" + code[0] + "' ");
                        }
              
                }
            }
            else       //改变灯显示界面的状态图片
            {
                foreach (Button item in ((MainWindow)this.Owner).addPanel.Children.OfType<Button>())
                {
                    if (!item.Content.ToString().Contains("添加灯具"))
                    {
                        if (((TextBlock)item.Content).Text == code[0])
                        {
                            item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(path)), Stretch = Stretch.Uniform };
                            SqlHelper.ExecuteNonQuery("update " + code[1] + " set Status='" + lStatus + "' where Name='" + code[0] + "' ");
                        }
                    }
                }
            }
        }



        public string SelectPic(string status)
        {
            string flag = code[0].Split('_')[1].ToCharArray()[0].ToString() + status;   //1 全亮 ，2 全灭，3 左亮 ，4 右亮，
            return NormolProcess.SelectPic(flag);
        }
    }
}
