using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Common;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MVVM;

namespace Launcher
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : MetroWindow
    {
        /// <summary>
        /// 对话框设置
        /// </summary>
        private MetroDialogSettings metroDialogSettings = null;

        /// <summary>
        /// DialogDictionary
        /// </summary>
        private ResourceDictionary DialogDictionary = new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml") };

        public Login()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;

            this.DataContext = new LoginViewModel();

            metroDialogSettings = new MetroDialogSettings
            {
                CustomResourceDictionary = DialogDictionary,
                AffirmativeButtonText = LangHelper.GetValue("Message", "确定", "ok"),
                NegativeButtonText = LangHelper.GetValue("Message", "取消", "cancel")
                //AffirmativeButtonText = LangHelper.GetValue("Message", "是", "yes"),
                //NegativeButtonText = LangHelper.GetValue("Message", "否", "no")
                //SuppressDefaultResources = true
            };

            Messager.Default.Register<NotificationMessage>(this, message =>
             {
                 if (message.Key == LoginViewModel.MESSAGE_LOGINFAIL)//登陆失败
                 {
                     this.ShowMessageAsync(LangHelper.GetValue("Message", "提示信息", "title"), message.Data.ToString(), MessageDialogStyle.Affirmative, metroDialogSettings);
                 }
                 else if (message.Key == LoginViewModel.MESSAGE_LOGINOK)//登陆成功
                 {
                     new MainWindow().Show(); //显示程序主窗口
                     this.Close();
                 }
             });

        }

        /// <summary>
        /// 关闭窗口时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            Messager.Default.Unregister(this);
            base.OnClosing(e);
        }
    }

    /// <summary>
    /// 用户登陆逻辑
    /// </summary>
    public class LoginViewModel : NotifyBaseModel
    {
        /// <summary>
        /// 登陆失败
        /// </summary>
        public static readonly string MESSAGE_LOGINFAIL = "Login_Fail";

        /// <summary>
        /// 登陆成功
        /// </summary>
        public static readonly string MESSAGE_LOGINOK = "LoginOK";

        /// <summary>
        /// 实体
        /// </summary>
        private LoginModel _LoginModel = null;
        /// <summary>
        /// 实体
        /// </summary>
        [Display(Name = "用户登陆", ResourceType = typeof(LanguageResource), ShortName = "Login_Title")]
        public LoginModel LoginModel
        {
            get
            {
                if (_LoginModel == null)
                {
                    _LoginModel = new LoginModel();
                }
                return _LoginModel;
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        public ICommand ThemeCommand
        {
            get
            {
                return new MVVM.Command.DelegateCommand(() =>
                {
                    Themes.PaletteSelector.Instance.ShowDialog();
                });
            }
        }

        /// <summary>
        /// 用户点击用户登陆按钮触发
        /// </summary>
        [Display(Name = "登  陆", ResourceType = typeof(LanguageResource), ShortName = "Login_LoginButton")]
        public ICommand LoginCommand
        {
            get
            {
                return new MVVM.Command.DelegateCommand(() =>
                {
                    //new Themes.PaletteSelector().ShowDialog();

                    if (String.IsNullOrWhiteSpace(LoginModel.UserName))
                    {
                        //没有输入用户名
                        Messager.Default.Send(new NotificationMessage()
                        {
                            Key = MESSAGE_LOGINFAIL,
                            Data = LangHelper.GetValue("Login_Fail_UserName", "请输入用户名！")
                        });
                        return;
                    }
                    if (String.IsNullOrWhiteSpace(LoginModel.Password))
                    {
                        //没有输入密码
                        Messager.Default.Send(new NotificationMessage()
                        {
                            Key = MESSAGE_LOGINFAIL,
                            Data = LangHelper.GetValue("Login_Fail_Password", "请输入密码！")
                        });
                        return;
                    }
                    try
                    {
                        //登陆数据验证
                        if (!LoginModel.Login())
                        {
                            //登陆失败
                            Messager.Default.Send(new NotificationMessage()
                            {
                                Key = MESSAGE_LOGINFAIL,
                                Data = LangHelper.GetValue("Login_Fail", "用户名或密码错误！")
                            });
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString().WriteToLog(log4net.Core.Level.Error);
                        //登陆失败
                        Messager.Default.Send(new NotificationMessage()
                        {
                            Key = MESSAGE_LOGINFAIL,
                            Data = LangHelper.GetValue("Login_Fail_Exception", "登陆时出现异常，请稍后重试！")
                        });
                        return;
                    }

                    //登陆成功
                    Messager.Default.Send(new NotificationMessage()
                    {
                        Key = MESSAGE_LOGINOK,
                    });
                });
            }
        }
    }

    /// <summary>
    /// 用户登陆实体类
    /// </summary>
    public class LoginModel : NotifyBaseModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名", ResourceType = typeof(LanguageResource), ShortName = "Login_UserName", Prompt = "请输入用户名")]
        public string UserName
        {
            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密　码", ResourceType = typeof(LanguageResource), ShortName = "Login_Password", Prompt = "请输入密码")]
        public string Password
        {
            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// login方法
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            return true;
        }
    }
}
