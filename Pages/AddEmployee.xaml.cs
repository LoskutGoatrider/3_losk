using losk_3.BasaSQL;
using losk_3.Services;
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
using Xceed.Words.NET;

namespace losk_3.Pages
{
        /// <summary>
        ///  Реализует добавление нового сотрудника (newEmployee) в базу данных и обработку возможных ошибок.
        /// </summary>
        public partial class AddEmployee : Page
        {
                public AddEmployee()
                {
                        InitializeComponent();
                        LoadComboBoxes();
                }
                private void LoadComboBoxes()
                {
                        telecom_loskEntities db = Helper.GetContext();
                        
                                var genders = db.Gender.ToList();
                                if (genders.Any())
                                {
                                        cbGender.ItemsSource = genders;
                                        cbGender.DisplayMemberPath = "Name";
                                        cbGender.SelectedValuePath = "ID";
                                }
                                else
                                {
                                        MessageBox.Show("Полы не найдены");
                                }

                                var jobs = db.Job_title.ToList();
                                if (jobs.Any())
                                {
                                        cbPositionAtWork.ItemsSource = jobs;
                                        cbPositionAtWork.DisplayMemberPath = "Name";
                                        cbPositionAtWork.SelectedValuePath = "ID";
                                }
                                else
                                {
                                        MessageBox.Show("Должности не найдены");
                                }
                        
                }
                /// <summary>
                /// Обработчик события нажатия кнопки "Добавить", добавляет нового сотрудника в базу данных.
                /// </summary>
                /// <param name="sender">Объект, вызвавший событие (кнопка).</param>
                /// <param name="e">Аргументы события, содержащие информацию о событии.</param>
                private void AddButton_Click(object sender, RoutedEventArgs e)
                {

                        var selectedGender = cbGender.SelectedItem as Gender;
                        var selectedPosition = cbPositionAtWork.SelectedItem as Job_title;

                        if (selectedGender == null || selectedPosition == null)
                        {
                                MessageBox.Show("Не удалось получить выбранные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                        }

                        var newEmployee = new Employee
                        {
                                First_name = tbFirstName.Text,
                                Last_name = tbLastName.Text,
                                Midle_name = tbMiddleName.Text,
                                Born_date = DateTime.TryParse(tbBornDate.Text, out var bornDate) ? bornDate : DateTime.MinValue,
                                Gender = selectedGender.GenderID,
                                Position_at_work = selectedPosition.ID,
                                Wages = decimal.TryParse(tbWages.Text, out var wages) ? wages : 0,
                                Passport_serial = decimal.TryParse(tbPassportSerial.Text, out var passportSerial) ? passportSerial : 0,
                                Passport_number = decimal.TryParse(tbPassportNumber.Text, out var passportNumber) ? passportNumber : 0,
                                Registration = tbRegistration.Text,
                                Email = tbEmail.Text,
                                Phone = tbPhoneNumber.Text
                        };
                        
                        ValidateEmployees validate = new ValidateEmployees();
                        string validationMessage = validate.ValidateEmployee(newEmployee);
                        if (!string.IsNullOrEmpty(validationMessage))
                        {
                                MessageBox.Show(validationMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                        }
                        
                        try
                        {
                                telecom_loskEntities db = Helper.GetContext();


                                db.Employee.Add(newEmployee);
                                db.SaveChanges();
                                

                                MessageBox.Show("Сотрудник успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                NavigationService.GoBack();


                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }           
        }
}
