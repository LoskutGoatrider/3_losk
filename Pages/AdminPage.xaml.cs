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
        /// Этот код  для страницы, отображающей список сотрудников и позволяющей их редактировать или добавлять новых.
        /// </summary>
        public partial class AdminPage : Page
        {
                private List<Employees> _employees;
                private List<Employees> _filteredEmployees;
                private List<string> _jobTitles;

                public AdminPage(User user)
                {
                        InitializeComponent();
                        LoadEmployees();
                        LoadJobTitles();
                }
                private void LoadEmployees()
                {
                        _employees = Helper.GetContext().Employee.Select(e => new Employees
                        {

                                ID = e.ID.ToString(),
                                FirstName = e.First_name,
                                LastName = e.Last_name,
                                MiddleName = e.Midle_name,
                                BornDate = e.Born_date,
                                Gender = e.Gender1.Name,
                                PositionAtWork = e.Job_title.Name,
                                Wages = e.Wages,
                                PassportSerial = e.Passport_serial.ToString(),
                                PassportNumber = e.Passport_number.ToString(),
                                Registration = e.Registration,
                                Email = e.Email,
                                PhoneNumber = e.Phone
                        }).ToList();
                        foreach (var employee in _employees)
                        {
                                employee.FullName = $"{employee.LastName} {employee.FirstName}";
                                employee.PhotoUrl = "C:\\Downloads\\avatarka.png";
                        }
                        EmployeesListView.ItemsSource = _employees;
                }

                private void LoadJobTitles()
                {
                        _jobTitles = Helper.GetContext().Job_title.Select(j => j.Name).Distinct().ToList();

                        _jobTitles.Insert(0, "Все должности");

                        cbJobTitle.ItemsSource = _jobTitles;

                        cbJobTitle.SelectedIndex = 0;
                }

                private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
                {
                        FilterEmployees();
                }

                private void cbJobTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
                        FilterEmployees();
                }

                private void FilterEmployees()
                {
                        string searchText = tbSearch.Text.ToLower();
                        string selectedJobTitle = cbJobTitle.SelectedItem as string;

                        // Фильтрация списка сотрудников на основе текста поиска и выбранной должности
                        _filteredEmployees = _employees.Where(emp =>
                            // Проверяем, содержится ли полное имя сотрудника (фамилия, имя и отчество) в тексте поиска
                            (emp.LastName + " " + emp.FirstName + " " + emp.MiddleName).ToLower().Contains(searchText) &&
                            (selectedJobTitle == "Все должности" || emp.PositionAtWork == selectedJobTitle))
                            .ToList();

                        EmployeesListView.ItemsSource = null;
                        EmployeesListView.ItemsSource = _filteredEmployees;
                }
                /// <summary>
                /// Обработчик события двойного щелчка мыши по элементу в списке сотрудников (EmployeesListView).
                /// Открывает страницу редактирования для выбранного сотрудника, если он существует в базе данных.
                /// </summary>
                /// <param name="sender">Объект, вызвавший событие (EmployeesListView).</param>
                /// <param name="e">Аргументы события, содержащие информацию о событии двойного щелчка мыши.</param>
                private void EmployeesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
                {
                        if (EmployeesListView.SelectedItem is Employees selectedEmployee)
                        {
                                try
                                {
                                        int employeeId = int.Parse(selectedEmployee.ID);

                                        telecom_loskEntities db = Helper.GetContext();

                                        var employeeExists = db.Employee.Find(employeeId) != null;

                                        if (employeeExists)
                                        {
                                                NavigationService.Navigate(new EditEmployee(employeeId));
                                        }
                                        else
                                        {
                                                MessageBox.Show($"Сотрудник с ID = {employeeId} не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }

                                }
                                catch (FormatException ex)
                                {
                                        MessageBox.Show("Ошибка формата ID: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                        }
                }

                private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
                {
                        NavigationService.Navigate(new AddEmployee());
                }

                private void RefreshButton_Click(object sender, RoutedEventArgs e)
                {
                        LoadEmployees();
                }
                private void PrintEmployee_Click(object sender, RoutedEventArgs e)
                {
                        // Создаем экземпляр стандартного диалогового окна печати Windows
                        PrintDialog printDialog = new PrintDialog();
                        // Отображаем диалоговое окно печати и проверяем, нажал ли пользователь кнопку 
                        if (printDialog.ShowDialog() == true)
                        {
                                FlowDocument docToPrint = new FlowDocument();
                                foreach (var employee in _employees)
                                {
                                        var employeeBlock = new Paragraph();
                                       //  объект Run для добавления текста в абзац
                                        employeeBlock.Inlines.Add(new Run($"ФИО: {employee.FullName}\n"));
                                        employeeBlock.Inlines.Add(new Run($"Должность: {employee.PositionAtWork}\n"));
                                        employeeBlock.Inlines.Add(new Run($"Телефон: {employee.PhoneNumber}\n"));
                                        docToPrint.Blocks.Add(employeeBlock);
                                }

                                IDocumentPaginatorSource idpSource = docToPrint;
                                printDialog.PrintDocument(idpSource.DocumentPaginator, "Список сотрудников");
                        }
                }
                private void PrintServices_Click(object sender, RoutedEventArgs e)
                {
                        telecom_loskEntities db = Helper.GetContext();
                        // _servicesItems - это коллекция, содержащая данные об услугах
                        var _servicesItems = db.Services.ToList();

                                PrintDialog printDialog = new PrintDialog();
                                if (printDialog.ShowDialog() == true)
                                {
                                        FlowDocument docToPrint = new FlowDocument();
                                        foreach (var services in _servicesItems)
                                        {
                                                var servicesBlock = new Paragraph();
                                                servicesBlock.Inlines.Add(new Run($"Название услуги: {services.ServiceName}\n"));
                                                servicesBlock.Inlines.Add(new Run($"цена: {services.Price}\n"));
                                                docToPrint.Blocks.Add(servicesBlock);
                                        }

                                        IDocumentPaginatorSource idpSource = docToPrint;
                                        printDialog.PrintDocument(idpSource.DocumentPaginator, "Список услуг");
                                }
                        
                }

        }
}
