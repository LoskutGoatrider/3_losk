using losk_3.BasaSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using losk_3.Services;

namespace losk_3.Pages
{
        /// <summary>
        /// Этот код реализует страницу аутентификации, которая требует от пользователя подтверждения email-адреса. 
        /// </summary>
        public partial class Authenication : Page
        {
                private User _user;
                private string _positionAtWork;
                private string _email;
                private string _confirmationCode;
                private DispatcherTimer timer;
                private int remainingTime;

                public Authenication(User user, string idPositionAtWork)
                {
                        InitializeComponent();
                        CreateTimer();
                        _user = user;
                        _positionAtWork = idPositionAtWork;
                        _email = user.Employee.Email;
                }

                private void CreateTimer() 
                {
                        timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                }
         
                private void Timer_Tick(object sender, EventArgs e)
                {
                        remainingTime--;


                        if (remainingTime <= 0)
                        {
                                timer.Stop();
                                btnSend.IsEnabled = true;
                                txtbTimer.Visibility = Visibility.Hidden;
                                return;
                        }
                        txtbTimer.Text = $"Отправить код повторно \nчерез: {remainingTime} секунд";
                }
                /// <summary>
                /// Обработчик события нажатия кнопки "Подтвердить", проверяет код подтверждения, введенный пользователем.
                /// В случае успеха перенаправляет пользователя на соответствующую страницу в зависимости от его роли.
                /// </summary>
                /// <param name="sender">Объект, вызвавший событие (кнопка).</param>
                /// <param name="e">Аргументы события, содержащие информацию о событии нажатия кнопки.</param>
                private void btnConfirm_Click(object sender, RoutedEventArgs e)
                {
                        if (txtbConfirmCode.Text == _confirmationCode)
                        {
                                LoadPage(_user, _positionAtWork);
                        }
                }

                private void btnSend_Click(object sender, RoutedEventArgs e)
                {
                        if (_email != null)
                        {
                                ConfirmationCode confCode = new ConfirmationCode();
                                _confirmationCode = confCode.SendEmail(_email);
                                btnSend.IsEnabled = false;
                                remainingTime = 60;
                                txtbTimer.Visibility = Visibility.Visible;
                                timer.Start();
                        }
                        else
                        {
                                MessageBox.Show("Email сотрудника не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }

                private void LoadPage(User user, string idPositionAtWork)
                {
                        switch (idPositionAtWork)
                        {
                                case "1":
                                        NavigationService.Navigate(new AdminPage(user));
                                        break;
                                default:
                                        NavigationService.Navigate(new Client(null));
                                        break;
                        }
                }
        }
}
