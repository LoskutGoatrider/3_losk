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
        /// Логика взаимодействия для Authenication.xaml
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
                        CreateTimer();// Создаем и настраиваем таймер для выполнения определенных задач
                        _user = user;
                        _positionAtWork = idPositionAtWork;
                        _email = user.Employee.Email;
                }

                private void CreateTimer() // Метод для создания и настройки таймера
                {
                        timer = new DispatcherTimer();// Инициализируем новый экземпляр таймера
                        timer.Interval = TimeSpan.FromSeconds(1);// Устанавливаем интервал таймера на 1 секунду
                        timer.Tick += Timer_Tick;// Подписываемся на событие Tick таймера для обработки
                }

                // Обработчик события Tick таймера
                private void Timer_Tick(object sender, EventArgs e)
                {
                        remainingTime--;// Уменьшаем оставшееся время на 1 секунду

                        // Проверяем, истекло ли время
                        if (remainingTime <= 0)
                        {
                                timer.Stop();// Останавливаем таймер
                                btnSend.IsEnabled = true;// Активируем кнопку отправки
                                txtbTimer.Visibility = Visibility.Hidden;// Скрываем текстовое поле с таймером
                                return;// Завершаем выполнение метода
                        }
                        // Обновляем текстовое поле с оставшимся временем до повторной отправки кода
                        txtbTimer.Text = $"Отправить код повторно \nчерез: {remainingTime} секунд";
                }

                private void btnConfirm_Click(object sender, RoutedEventArgs e)
                {
                        if (txtbConfirmCode.Text == _confirmationCode)
                        {
                                LoadPage(_user, _positionAtWork);
                        }
                }

                private void btnSend_Click(object sender, RoutedEventArgs e)
                {
                        // Проверяем, что адрес электронной почты сотрудника не равен null
                        if (_email != null)
                        {
                                // Создаем экземпляр класса ConfirmationCode для отправки кода подтверждения
                                ConfirmationCode confCode = new ConfirmationCode();
                                // Отправляем код подтверждения на электронную почту и сохраняем его в переменную
                                _confirmationCode = confCode.SendEmail(_email);
                                // Деактивируем кнопку отправки, чтобы предотвратить повторные нажатия
                                btnSend.IsEnabled = false;
                                remainingTime = 60;// Устанавливаем оставшееся время в 60 секунд
                                txtbTimer.Visibility = Visibility.Visible;// Показываем текстовое поле таймера
                                timer.Start();// Запускаем таймер
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
