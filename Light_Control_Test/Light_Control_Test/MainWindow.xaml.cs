using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Data;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Light_Control_Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Tick += Dt_Tick;
            dt.Start();
            timerDis.Interval = new TimeSpan(0, 0, 5);
            timerDis.Tick += TimerDis_Tick;
            timerDis.Start();
            //KeyBorder keyBorder = new KeyBorder();
            //keyBorder.Show();
            addBtn = addPanel.FindName("addNew") as Button;
            Thread checkThread = new Thread(new ThreadStart(Check));  //自检线程
            checkThread.Start();
            PortHelper.OpenPort();

            Thread hwControlThread = new Thread(new ThreadStart(HCThread));    //硬件控制线程
            hwControlThread.IsBackground = true;
            hwControlThread.Start();
        }


        private void TimerDis_Tick(object sender, EventArgs e)
        {
            Task tase = new Task(ThreadUpdate);
            tase.Start();

        }

        private object locker = new object();
        private int PeiCount = 0;    //记录正在检测数据库中配电箱的次序


        public void ThreadUpdate()    //故障UI更新
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from Pei");
            byte[] dataPro = null;

            if (dt.Rows.Count!=0)
            {
                //if (Instruction.P_FindpeiCode(dt.Rows[PeiCount]["Name"].ToString().Split('_')[1]) == true)
                //{
                //    dataPro = Instruction.backByte;
                //}
                //if (Instruction.P_AskLightCodeAndStatus(dt.Rows[PeiCount]["Name"].ToString().Split('_')[1]) == true)
                //{
                //    // dataPro = Instruction.backByte;
                //    int i = 3;
                //    while (dataPro[i] != 0x4F)
                //    {

                //    }
                //}
                //this.Dispatcher.BeginInvoke(new Action(() =>
                //{                                   //异步更新UI
                //    DisErrorPei(dataPro, dt.Rows[PeiCount]["Name"].ToString().Split('_')[1]);
                //}));
                //PeiCount++;                              //下一个需要检测的配电箱的序号
                //if (PeiCount >= dt.Rows.Count)            //判断是否已经全部检测完毕
                //{
                //    PeiCount = 0;
                //}
            }
        }



        public void DisErrorPei(byte[] dataPro, string peiName)
        {

            if (dataPro != null)
            {
                if (GetbitValue(dataPro[3], 0) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "灯珠故障");
                }
                if (GetbitValue(dataPro[3], 1) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "灯通信故障");
                }
                if (GetbitValue(dataPro[3], 2) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "无应急电池");
                }
                if (GetbitValue(dataPro[3], 3) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "主电故障");
                }
                if (GetbitValue(dataPro[3], 4) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "电池充电");
                }
                if (GetbitValue(dataPro[3], 5) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "36V故障");
                }
                if (GetbitValue(dataPro[3], 6) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "配电箱应急");
                }
                if (GetbitValue(dataPro[3], 7) == 1)
                {
                    ErrorDisList.Items.Add(dateDisplay.Content + "     " + peiName + "备电故障");
                }
            }
        }

        private int GetbitValue(int input, int index)
        {
            return (input & ((uint)1 << index)) > 0 ? 1 : 0;
        }





      
        public void Check()
        {
            //string[] date = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\selfCheck.txt");
            //while (true)
            //{
            //    this.Dispatcher.BeginInvoke(new Action(() => { date = new string[] { dateCheck.Content.ToString() }; }));
            //    if (date[0]==" ")
            //    {
            //     date = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\selfCheck.txt");
            //    }
            //    if (date[0] == DateTime.Now.ToString())
            //    {
            //        Instruction.C_ControlMonthCheck();
            //        //this.Dispatcher.BeginInvoke(new Action(() => { MessageBox.Show("asa"); }));
            //        DispatcherTimer checkTimew = new DispatcherTimer();
            //        checkTimew.Interval = TimeSpan.FromSeconds(30);
            //        checkTimew.Tick += CheckTimew_Tick;
            //        break;
            //    }
            //}
        }

        private void CheckTimew_Tick(object sender, EventArgs e)
        {
            Instruction.C_ControlYearCheck();
        }

        public string addStatus = "配电箱";    //界面状态字符串
        public bool disSetFlag = false;          //配电箱或者灯下面显示内容的设置
        Button addBtn;
        private DispatcherTimer dt = new DispatcherTimer();
        private DispatcherTimer timerDis = new DispatcherTimer();
        public bool manager = false;


        private void Dt_Tick(object sender, EventArgs e)
        {
            string dateNow = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "  " + DateTime.Now.ToLongTimeString();
            dateDisplay.Content = dateNow;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 后台改背景  MainGrid.Background = new ImageBrush
            //{
            //    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/主界面.png"))
            //};

            FireWorkTogether fwt = new FireWorkTogether();
            fwt.Show();
        }


        #region 菜单显示以及点击空白处、二级菜单以及显示逻辑
        private void Menu_Click(object sender, RoutedEventArgs e)  //菜单的显示
        {
            if (manager)
            {
                menuImage.Visibility = Visibility.Visible;
                update.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("请先登录!", "提示");
            }
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Control)sender).Name != "menuImage" && ((Control)sender).Name != "update")
            {
                update.Visibility = Visibility.Collapsed;
                menuImage.Visibility = Visibility.Collapsed;
                SecondMenuDisplay(-1);
            }
        }



        private void SecondMenuDisplay(int i)   //二级菜单显示，只能同时显示一个或者零个
        {
            System.Windows.Controls.Primitives.Popup[] popue = new System.Windows.Controls.Primitives.Popup[] { SecondMenuPop, SecondMenuPop2, SecondMenuPop3 };
            foreach (System.Windows.Controls.Primitives.Popup item in popue)
            {
                item.IsOpen = false;
            }
            if (i >= 0)
                popue[i].IsOpen = true;

        }

        private void Window_Deactivated(object sender, EventArgs e)   //窗体失去焦点，即不活跃时，将popup子窗隐藏
        {
            SecondMenuDisplay(-1);
            menuImage.Visibility = Visibility.Collapsed;
        }


        private void menuImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  //结束路由事件的传递，防止menuImage上的点击传递给床体
        {
            e.Handled = true;

        }

        private void addPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  //点击AddPanel其余菜单消失
        {
            update.Visibility = Visibility.Collapsed;
            menuImage.Visibility = Visibility.Collapsed;
            SecondMenuDisplay(-1);
        }


        #endregion


        private void FireWorkBack_Click(object sender, RoutedEventArgs e)  //灯具返回至配电箱按键
        {
            FireWork.Visibility = Visibility.Collapsed;
            addStatus = "配电箱";  //更新状态字符串
            _addName.Text = addStatus;
            EleBoxDis();           //显示全部配电箱
        }

        public void EleBoxDis()    //从数据库调取数据进行显示配电箱
        {
            addPanel.Children.Clear();
            DataTable dt_Table = new DataTable();
            dt_Table = SqlHelper.ExecuteDataTable("select * from Pei");
            foreach (DataRow item in dt_Table.Rows)
            {
                Add(item["Name"].ToString() + "+" + item["Position"].ToString());
            }
            AddNewBtnDis();
        }

        public void LightDis(string hostName)  //从数据库调取数据显示灯具,在指定配电箱下调取
        {
            addPanel.Children.Clear();
            DataTable dt_Table = new DataTable();
            dt_Table = SqlHelper.ExecuteDataTable("select * from " + hostName + " ");
            foreach (DataRow item in dt_Table.Rows)
            {
                Add(item["Name"].ToString() + "+" + item["Position"].ToString());
            }
        }


        public void Add(string _addName)//新增数据，添加图标，绑定数据
        {

            string[] disString = _addName.Split('+');
            _addName = disString[0];
            if (addStatus == "配电箱")
            {
                Button bt = new Button();
                bt.Style = this.Resources["testStyle"] as Style;
                bt.Width = 90;
                bt.Height = 120;
                bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")) };
                if (disSetFlag)   //切换配电箱和灯下面的显示
                {
                    try
                    {
                        if (String.IsNullOrEmpty(disString[1]))
                        {
                            bt.Content = new TextBlock { Text = "未设置" + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                        }
                        else
                        {
                            bt.Content = new TextBlock { Text = disString[1] + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                        }
                    }
                    catch
                    {
                        bt.Content = new TextBlock { Text = "未设置" + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                    }
                }
                else
                {
                    bt.Content = new TextBlock { Text = disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                }
                bt.Margin = new Thickness(0, 0, 10, 10);
                bt.Name = _addName;
                bt.MouseRightButtonUp += Bt_MouseRightButtonUp;
                bt.Click += SwitchToLight_Click;
                addPanel.Children.Add(bt);
                //addPanel.RegisterName(_addName, bt);
                AddNewBtnDis();
            }
            else
            {
                Button bt = new Button();
                bt.Style = this.Resources["testStyle"] as Style;
                bt.Width = 90;
                bt.Height = 120;
                switch (_addName.Split('_')[1].ToCharArray()[0].ToString())    //根据高位划分灯具类型并添加不同图片显示
                {
                    case "1": { bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }
                    case "2": { bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/标志灯双向绿.png")), Stretch = Stretch.Uniform }; break; }
                    case "3": { bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/双头灯绿.png")), Stretch = Stretch.Uniform }; break; }
                    case "4": { bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/地埋灯双向绿.png")), Stretch = Stretch.Uniform }; break; }
                    case "5":
                        {
                            if (_addName.Split('_')[1].ToCharArray()[4].ToString() == "0")
                            {
                                bt.Background = new ImageBrush
                                {
                                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                                    Stretch = Stretch.Uniform
                                };
                            }
                            else
                            {
                                bt.Background = new ImageBrush
                                {
                                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                                    Stretch = Stretch.Uniform
                                };
                            }
                            break;
                        }
                    case "7":
                        {
                            if (_addName.Split('_')[1].ToCharArray()[4].ToString() == "0")
                            {
                                bt.Background = new ImageBrush
                                {
                                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                                    Stretch = Stretch.Uniform
                                };
                            }
                            else
                            {
                                bt.Background = new ImageBrush
                                {
                                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                                    Stretch = Stretch.Uniform
                                };
                            }
                            break;
                        }
                    case "8": { bt.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }

                    default:
                        break;
                }

                if (disSetFlag)   //切换配电箱和灯下面的显示
                {
                    try
                    {
                        if (String.IsNullOrEmpty(disString[1]))
                        {
                            bt.Content = new TextBlock { Text = "未设置" + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                        }
                        else
                        {
                            bt.Content = new TextBlock { Text = disString[1] + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                        }
                    }
                    catch
                    {

                        bt.Content = new TextBlock { Text = "未设置" + disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };

                    }
                }
                else
                {
                    bt.Content = new TextBlock { Text = disString[0], TextWrapping = TextWrapping.Wrap, LineHeight = 2, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                }
                bt.Margin = new Thickness(0, 0, 10, 10);
                bt.Name = _addName;
                bt.MouseRightButtonUp += Bt_MouseRightButtonUp;
                bt.Click += InfoDis_Click;
                addPanel.Children.Add(bt);

                AddNewBtnDis();
            }
        }


        public void AddNewBtnDis()   //添加按钮最后显示,分为三种不同的情况
        {
            try
            {
                addBtn.Content = "     添加" + _addName.Text;

                addPanel.Children.Remove(addBtn);
                addPanel.UnregisterName("addNew");
                addPanel.Children.Add(addBtn);
                addPanel.RegisterName("addNew", addBtn);
            }
            catch
            {
                try
                {
                    addBtn.Content = "     添加" + _addName.Text;
                    addPanel.UnregisterName("addNew");
                    addPanel.Children.Add(addBtn);
                    addPanel.RegisterName("addNew", addBtn);
                }
                catch
                {
                    addBtn.Content = "     添加" + _addName.Text;
                    addPanel.Children.Add(addBtn);
                    addPanel.RegisterName("addNew", addBtn);
                }
            }
        }


        private void Bt_MouseRightButtonUp(object sender, MouseButtonEventArgs e) //配电箱右键按下事件
        {
            Point p = new Point();
            p = Mouse.GetPosition(e.Source as FrameworkElement);
            p = (e.Source as FrameworkElement).PointToScreen(p);
            if (addStatus == "配电箱")
            {
                Right_Popup rp = new Right_Popup(((Button)sender).Name, _addName.Text);
                rp.Left = p.X;
                rp.Top = p.Y;
                rp.Owner = this;
                rp.Show();
            }
            else
            {
                Right_Popup rp = new Right_Popup(((Button)sender).Name + "+" + _lightHost.Text, _addName.Text);
                rp.Left = p.X;
                rp.Top = p.Y;
                rp.Owner = this;
                rp.Show();
            }
        }



        private void SwitchToLight_Click(object sender, RoutedEventArgs e)  //灯具配电箱切换事件，就是配电箱单击事件
        {
            _lightHost.Text = ((Button)sender).Name;
            addStatus = "灯具";
            _addName.Text = addStatus;
            //保留AddNewBtn并清除所有子元素，为下一次添加做准备
            addPanel.Children.Clear();
            LightDis(((Button)sender).Name);
            AddNewBtnDis();
            FireWork.Visibility = Visibility.Visible;
        }
        private void Bt_Click(object sender, RoutedEventArgs e)  //添加配电箱按钮事件，新增
        {
            AddItems addItems = new AddItems();
            addItems.Owner = this;
            addItems.Show();
        }

        private void InfoDis_Click(object sender, RoutedEventArgs e)   //详情显示页面显示单击事件
        {
            Point p = new Point();
            p = Mouse.GetPosition(e.Source as FrameworkElement);
            p = (e.Source as FrameworkElement).PointToScreen(p);
            Right_Popup rp = new Right_Popup(((Button)sender).Name + "+" + _lightHost.Text, _addName.Text);
            rp.btnGrid.Visibility = Visibility.Collapsed;
            rp.Info.Visibility = Visibility.Visible;
            DataTable dt = SqlHelper.ExecuteDataTable("select * from " + _lightHost.Text + " where Name='" + ((Button)sender).Name + "'");
            rp.inWhichPei.Content = _lightHost.Text;
            if (String.IsNullOrEmpty(dt.Rows[0]["Position"].ToString()))
                rp.location.Content = "未设置";
            else
                rp.location.Content = dt.Rows[0]["Position"];
            rp.lightCode.Content = ((Button)sender).Name;
            if (String.IsNullOrEmpty(dt.Rows[0]["IniStatus"].ToString()))
                rp.iniStatus.Content = "未设置";
            else
                rp.iniStatus.Content = dt.Rows[0]["IniStatus"];
            rp.lightCode.Content = ((Button)sender).Name;
            rp.Left = p.X;
            rp.Top = p.Y;
            rp.Owner = this;
            rp.Show();
        }

        private void MainMenuSet_Click(object sender, RoutedEventArgs e)  //设置二级菜单
        {
            SecondMenuDisplay(0);
        }

        private void DataCenter_Click(object sender, RoutedEventArgs e)   //数据中心二级菜单
        {
            SecondMenuDisplay(1);
        }

        private void DataLoad_Click(object sender, RoutedEventArgs e)    //数据载入二级菜单
        {
            SecondMenuDisplay(2);
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            update.Visibility = Visibility.Visible;
            menuImage.Visibility = Visibility.Collapsed;
            SecondMenuDisplay(-1);
        }





        private void buttonPlan_Click(object sender, RoutedEventArgs e)   //疏散预案进入
        {
            if (((Button)sender).Name == "buttonPlan")
            {
                PrePlanPei.Items.Clear();
                DataTable dt = SqlHelper.ExecuteDataTable("select Name,Position from Pei");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PrePlanPei.Items.Add(dt.Rows[i][0]);
                }
                PrePlanPei.SelectedIndex = 0;
                try
                {
                    PeiPosition.Content = dt.Rows[0][1];
                }
                catch
                {
                    PeiPosition.Content = "空";
                }
                PrePlanDis(" ");
                LightInPeiDis();
                planGrid.Visibility = Visibility.Visible;

            }
            if (((Button)sender).Name == "back")
            {
                planGrid.Visibility = Visibility.Collapsed;
            }
        }

        public void PrePlanDis(string restrict)
        {
            bool notNull = true;
            bool PeiOrLight = true;
            DataTable dt = null;
            if (restrict == " ")
            {
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select * from " + PrePlanPei.SelectedItem + "");
                }
                catch
                {
                    notNull = false;
                }
            }
            else if (restrict == "A")
            {
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select * from " + PrePlanPei.SelectedItem + " where PrePlanLeft='未设置-未设置-未设置-未设置-未设置'");
                }
                catch
                {
                    notNull = false;
                }
            }
            else if (restrict == "PeiInsLocation")
            {
                PeiOrLight = false;
                dt = SqlHelper.ExecuteDataTable("select * from Pei");
            }
            else if (restrict == "LocationPei")
            {
                PeiOrLight = false;
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select * from " + LocationPei.SelectedItem + "");
                }
                catch
                {
                    notNull = false;
                }
            }
            else
            {
                try
                {
                    dt = SqlHelper.ExecuteDataTable("select * from " + PrePlanPei.SelectedItem + " where Name='" + LightInPei.Text + "'");
                }
                catch
                {
                    notNull = false;
                }
            }
            if (notNull)
            {
                List<PrePlanSetClass> list = new List<PrePlanSetClass>();
                foreach (DataRow rowNew in dt.Rows)
                {
                    PrePlanSetClass prePlanClass = new PrePlanSetClass();
                    prePlanClass.Name = rowNew["Name"].ToString();
                    prePlanClass.IdCode = rowNew["IdCode"].ToString();
                    prePlanClass.Position = rowNew["Position"].ToString();
                    if (PeiOrLight)
                    {
                        string[] Lplan = rowNew["PrePlanLeft"].ToString().Split('-');
                        string[] Rplan = rowNew["PrePlanRight"].ToString().Split('-');
                        if (Lplan.Length < 5)
                        {
                            prePlanClass.LPlan_1 = "未设置";
                            prePlanClass.LPlan_2 = "未设置";
                            prePlanClass.LPlan_3 = "未设置";
                            prePlanClass.LPlan_4 = "未设置";
                            prePlanClass.LPlan_5 = "未设置";
                        }
                        else
                        {
                            prePlanClass.LPlan_1 = Lplan[0];
                            prePlanClass.LPlan_2 = Lplan[1];
                            prePlanClass.LPlan_3 = Lplan[2];
                            prePlanClass.LPlan_4 = Lplan[3];
                            prePlanClass.LPlan_5 = Lplan[4];
                        }
                        if (Rplan.Length < 5)
                        {
                            prePlanClass.RPlan_1 = "未设置";
                            prePlanClass.RPlan_2 = "未设置";
                            prePlanClass.RPlan_3 = "未设置";
                            prePlanClass.RPlan_4 = "未设置";
                            prePlanClass.RPlan_5 = "未设置";
                        }
                        else
                        {
                            prePlanClass.RPlan_1 = Rplan[0];
                            prePlanClass.RPlan_2 = Rplan[1];
                            prePlanClass.RPlan_3 = Rplan[2];
                            prePlanClass.RPlan_4 = Rplan[3];
                            prePlanClass.RPlan_5 = Rplan[4];
                        }
                    }
                    else
                    {
                        string[] Location = rowNew["Position"].ToString().Split('层', '区', '栋', '号');
                        if (Location.Length < 4)
                        {
                            prePlanClass.Area = "未设置";
                            prePlanClass.Building = "未设置";
                            prePlanClass.Layer = "未设置";
                            prePlanClass.Room = "未设置";
                        }
                        else
                        {
                            prePlanClass.Area = Location[0];
                            prePlanClass.Building = Location[1];
                            prePlanClass.Layer = Location[2];
                            prePlanClass.Room = Location[3];
                        }

                        if (restrict == "PeiInsLocation")
                        {
                            dt = SqlHelper.ExecuteDataTable("select * from " + prePlanClass.Name + "");
                            prePlanClass.LightCount = dt.Rows.Count.ToString();
                        }

                    }
                    list.Add(prePlanClass);
                }
                if (restrict == "PeiInsLocation")
                {
                    PeiPositionSet.ItemsSource = list;
                }
                else if (restrict == "LocationPei")
                {
                    LightLocationSet.ItemsSource = list;
                }
                else
                {
                    LDis.ItemsSource = list;
                }
            }
        }

        public void LightInPeiDis()
        {
            LightInPei.Items.Clear();
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select Name from " + PrePlanPei.SelectedItem + "");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LightInPei.Items.Add(dt.Rows[i][0]);
                }
            }
            catch
            {
            }
        }

        private void selfCheck_Click(object sender, RoutedEventArgs e)
        {
            SelfCheck selfCheck = new SelfCheck();
            selfCheck.Owner = this;
            selfCheck.Show();
        }

        private void errorBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            errorDis.Visibility = Visibility.Collapsed;
        }

        private void error_Click(object sender, RoutedEventArgs e)  //故障按钮按下，显示故障显示界面
        {
            errorDis.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EleBoxDis();           //显示全部配电箱
        }

        private void PrePlanPei_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (planGrid.Visibility == Visibility.Visible)
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from Pei where Name='" + PrePlanPei.SelectedItem + "'");
                PeiPosition.Content = dt.Rows[0]["Position"];
                PrePlanDis(" ");
                LightInPeiDis();
            }
        }

        private void ConnectionSet_Click(object sender, RoutedEventArgs e)
        {
            AddItems addItems = new AddItems("A");
            addItems.Owner = this;
            addItems.Show();
        }

        private void disSetBt_Click(object sender, RoutedEventArgs e)
        {
            DisplaySet ds = new DisplaySet();
            ds.Owner = this;
            ds.Show();
            SecondMenuDisplay(-1);
        }

        private void VoiceCancelSet_Click(object sender, RoutedEventArgs e)
        {
            VoiceCancel vc = new VoiceCancel();
            vc.Owner = this;
            vc.Show();
            SecondMenuDisplay(-1);
        }

        private void prePlanConfirmBt_Click(object sender, RoutedEventArgs e)    //预案确定
        {
            bool planMode = true;
            try
            {
                foreach (var item in LDis.Items)
                {
                    PrePlanSetClass prePlanSetClass = item as PrePlanSetClass;

                    string LPlan = prePlanSetClass.LPlan_1 + "-" + prePlanSetClass.LPlan_2 + "-" + prePlanSetClass.LPlan_3 + "-" + prePlanSetClass.LPlan_4 + "-" + prePlanSetClass.LPlan_5;
                    string RPlan = prePlanSetClass.RPlan_1 + "-" + prePlanSetClass.RPlan_2 + "-" + prePlanSetClass.RPlan_3 + "-" + prePlanSetClass.RPlan_4 + "-" + prePlanSetClass.RPlan_5;
                    if ((LPlan.Contains("未设置") && LPlan != "未设置-未设置-未设置-未设置-未设置") || (RPlan.Contains("未设置") && RPlan != "未设置-未设置-未设置-未设置-未设置"))
                    {
                        MessageBox.Show(prePlanSetClass.Name + "请提供完整的预案信息!", "提示");
                        planMode = false;
                        break;
                    }
                    else
                    {
                        string command = "update " + PrePlanPei.Text + " set PrePlanLeft='" + LPlan + "' ,PrePlanRight='" + RPlan + "' where Name='" + prePlanSetClass.Name + "'";
                        SqlHelper.ExecuteNonQuery(command);
                    }

                }
                if (planMode)
                    MessageBox.Show("修改成功!", "提示");
            }
            catch
            {
                MessageBox.Show("修改失败!", "提示");
            }
        }

        private void disNoSet_Checked(object sender, RoutedEventArgs e)
        {
            PrePlanDis("A");
        }

        private void disNoSet_Unchecked(object sender, RoutedEventArgs e)
        {
            PrePlanDis(" ");
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PrePlanDis("B");
        }


        private void TabReturn_button_Click(object sender, RoutedEventArgs e)
        {
            Positiondisplay.Visibility = Visibility.Hidden;
        }

        private void CorrectingBt_Click(object sender, RoutedEventArgs e)
        {
            Correcting correcting = new Correcting();
            correcting.Show();
        }

        private void insLocation_Click(object sender, RoutedEventArgs e)
        {
            SecondMenuDisplay(-1);
            PrePlanDis("PeiInsLocation");
            LocationPei.Items.Clear();
            DataTable dt = SqlHelper.ExecuteDataTable("select Name,Position from Pei");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LocationPei.Items.Add(dt.Rows[i][0]);
            }
            LocationPei.SelectedIndex = 0;
            try
            {
                PeiLocation.Content = dt.Rows[0][1];
            }
            catch
            {

                PeiLocation.Content = "空";
            }
            PrePlanDis("LocationPei");
            Positiondisplay.Visibility = Visibility.Visible;
        }

        private void LocationConfirmBt_Click(object sender, RoutedEventArgs e)
        {
            bool locationMode = true;
            string position = null;
            PrePlanSetClass ppsc = null;
            if (((Button)sender).Name == "LocationConfirmBt_1")
            {
                foreach (var item in PeiPositionSet.Items)
                {
                    ppsc = item as PrePlanSetClass;
                    position = ppsc.Area + "区" + ppsc.Building + "栋" + ppsc.Layer + "层" + ppsc.Room + "号";
                    if (position.Contains("未设置") && position != "未设置区未设置栋未设置层未设置号")
                    {
                        MessageBox.Show(ppsc.Name + "请提供完整的位置信息!", "提示");
                        locationMode = false;
                        break;
                    }
                    else
                    {
                        string command = "update Pei set Position='" + position + "' where Name='" + ppsc.Name + "'";
                        SqlHelper.ExecuteNonQuery(command);
                    }
                }
            }
            else
            {
                foreach (var item in LightLocationSet.Items)
                {
                    ppsc = item as PrePlanSetClass;
                    position = ppsc.Area + "区" + ppsc.Building + "栋" + ppsc.Layer + "层" + ppsc.Room + "号";
                    if (position.Contains("未设置") && position != "未设置区未设置栋未设置层未设置号")
                    {
                        MessageBox.Show(ppsc.Name + "请提供完整的位置信息!", "提示");
                        locationMode = false;
                        break;
                    }
                    else
                    {
                        string command = "update " + LocationPei.Text + " set Position='" + position + "' where Name='" + ppsc.Name + "'";
                        SqlHelper.ExecuteNonQuery(command);
                    }
                }
            }
            if (locationMode)
                MessageBox.Show("修改成功！", "提示");
        }

        private void LocationPei_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Positiondisplay.Visibility == Visibility.Visible)
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from Pei where Name='" + LocationPei.SelectedItem + "'");
                PeiPosition.Content = dt.Rows[0]["Position"];
                PrePlanDis("LocationPei");
            }
        }

        private void ModePassword_Click(object sender, RoutedEventArgs e)
        {
            Changepd changepd = new Changepd();
            changepd.Show();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (manager)
            {
                MessageBox.Show("注销成功！", "提示");
                Login.Content = "登录";
                manager = false;
            }
            else
            {
                trueMore trueMore = new trueMore();
                trueMore.Owner = this;
                trueMore.Show();
            }
        }

        private void DisExitL_Click(object sender, RoutedEventArgs e)  //不同的灯具显示从数据库调取的部分
        {
            DataTable disDt = null;
            switch (((Button)sender).Name)
            {
                case "DisExitL":
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + " where Name like '_5%'");
                        break;
                    }
                case "DisBrightL":
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + " where Name like '_5%'");
                        break;
                    }
                case "DisDoubleL":
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + " where Name like '_3%'");
                        break;
                    }
                case "DisFlagL":
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + " where Name like '_2%'");
                        break;
                    }
                case "DisBuryL":
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + " where Name like '_4%'");
                        break;
                    }
                default:
                    {
                        disDt = SqlHelper.ExecuteDataTable("select *  from " + _lightHost.Text + "");
                        break;
                    }
            }
            addPanel.Children.Clear();
            bool nullFlag = true;
            foreach (DataRow item in disDt.Rows)
            {
                nullFlag = false;
                Add(item["Name"].ToString() + "+" + item["Position"].ToString());
            }
            if (nullFlag)
            {
                AddNewBtnDis();
            }

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            SqlHelper.ExecuteNonQuery("delete Pei;Drop table _60000002");
            EleBoxDis();
           
        }



        public void HCThread()    //硬件控制线程
        {
            while(true)
            {
                byte[] getBackByte = null ;
                if (Instruction.backByte!=null)
                {
                    getBackByte = Instruction.backByte;
                    if (getBackByte[0]==0x66)    //放应急指令
                    {
                        Instruction.C_ControlMonthCheck();
                    }
                    if (getBackByte[0]==0x13)   //主电指令
                    {
                        Instruction.C_ControlYearCheck();
                    }
                }
            }
        }


    }


}
