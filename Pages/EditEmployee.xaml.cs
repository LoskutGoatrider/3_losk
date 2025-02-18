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

namespace losk_3.Pages
{
        /// <summary>
        /// Логика взаимодействия для EditEmployee.xaml
        /// </summary>
        public partial class EditEmployee : Page
        {
                private Employee _employee;

                public EditEmployee(long employeeId)
                {
                        InitializeComponent();
                        LoadComboBoxes();
                        LoadEmployeeData(employeeId);
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

                private void LoadEmployeeData(long employeeId)
                {
                        telecom_loskEntities db = Helper.GetContext();
                        
                                _employee = db.Employee.Find(employeeId);

                                if (_employee != null)
                                {
                                        tbFirstName.Text = _employee.First_name.ToString();
                                        tbLastName.Text = _employee.Last_name.ToString();
                                        tbMiddleName.Text = _employee.Midle_name.ToString();
                                        tbBornDate.Text = _employee.Born_date.ToString();
                                        cbGender.SelectedValue = _employee.Gender;
                                        cbPositionAtWork.SelectedValue = _employee.Position_at_work;
                                        tbWages.Text = _employee.Wages.ToString();
                                        tbPassportSerial.Text = _employee.Passport_serial.ToString();
                                        tbPassportNumber.Text = _employee.Passport_number.ToString();
                                        tbRegistration.Text = _employee.Registration.ToString();
                                        tbEmail.Text = _employee.Email.ToString();
                                        tbPhoneNumber.Text = _employee.Phone.ToString();
                                }
                        
                }

                private void SaveButton_Click(object sender, RoutedEventArgs e)
                {
                        telecom_loskEntities db = Helper.GetContext();
                        
                                var existingEmployee = db.Employee.Find(_employee.ID);

                                var selectedGender = cbGender.SelectedItem as Gender;
                                var selectedPosition = cbPositionAtWork.SelectedItem as Job_title;

                                if (selectedGender == null || selectedPosition == null)
                                {
                                        MessageBox.Show("Не удалось получить выбранные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                }

                                if (existingEmployee != null)
                                {
                                        existingEmployee.First_name = tbFirstName.Text;
                                        existingEmployee.Last_name = tbLastName.Text;
                                        existingEmployee.Midle_name = tbMiddleName.Text;
                                        existingEmployee.Born_date = DateTime.TryParse(tbBornDate.Text, out var bornDate) ? bornDate : DateTime.MinValue;
                                        existingEmployee.Gender = selectedGender.GenderID;
                                        existingEmployee.Position_at_work = selectedPosition.ID;
                                        existingEmployee.Wages = decimal.TryParse(tbWages.Text, out var wages) ? wages : 0;
                                        existingEmployee.Passport_serial = decimal.TryParse(tbPassportSerial.Text, out var passportSerial) ? passportSerial : 0;
                                        existingEmployee.Passport_number = decimal.TryParse(tbPassportNumber.Text, out var passportNumber) ? passportNumber : 0;
                                        existingEmployee.Registration = tbRegistration.Text;
                                        existingEmployee.Email = tbEmail.Text;
                                        existingEmployee.Phone = tbPhoneNumber.Text;

                                      ValidateEmployees validate = new ValidateEmployees();
                                        string validationMessage = validate.ValidateEmployee(existingEmployee);
                                        if (!string.IsNullOrEmpty(validationMessage))
                                        {
                                                MessageBox.Show(validationMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                return;
                                        }
                                        

                                try
                                        {
                                                db.SaveChanges();
                                                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        catch (Exception ex)
                                        {
                                                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                }
                                else
                                {
                                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                        
                }

                private void DeleteButton_Click(object sender, RoutedEventArgs e)
                {
                        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)

                        {
                                try
                                {
                                        telecom_loskEntities db = Helper.GetContext();
                                        
                                                var employeeToDelete = db.Employee.Find(_employee.ID);

                                                if (employeeToDelete != null)
                                                {
                                                        db.Employee.Remove(employeeToDelete);
                                                        db.SaveChanges();

                                                        MessageBox.Show("Сотрудник успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                                        NavigationService.GoBack();
                                                }
                                                else
                                                {
                                                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                                }
                                        
                                }
                                catch (Exception ex)
                                {
                                        MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                        }
                }
                private void GenerateContractt_Click(object sender, RoutedEventArgs e)
                {
                        ContractGenerator laborContractGenerator = new ContractGenerator();
                        laborContractGenerator.GenerateContract(_employee.ID);
                }
        }
        
}
