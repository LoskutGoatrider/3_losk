using losk_3.Pages;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace losk_3
{
        /// <summary>
        /// Логика взаимодействия для MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
                public MainWindow()
                {
                        InitializeComponent();
                        FrmMain.Navigate(new Autho());
                }

                private void FrmMain_Navigated(object sender, NavigationEventArgs e)
                {

        }

                private void Button_Click(object sender, RoutedEventArgs e)
                {

                }

                private void BtnBack_Click(object sender, RoutedEventArgs e)
                {
                        FrmMain.GoBack();
                }

                private void FrmMain_Navigated_1(object sender, NavigationEventArgs e)
                {

                }

                private void FrmMain_ContentRendered(object sender, EventArgs e)
                {
                        if (FrmMain.CanGoBack)
                                BtnBack.Visibility= Visibility.Visible;
                        else
                                BtnBack.Visibility = Visibility.Hidden;


                }
        }
}