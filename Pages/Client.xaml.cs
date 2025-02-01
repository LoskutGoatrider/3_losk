﻿using System;
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
using losk_3.BasaSQL;
using losk_3.Services;

namespace losk_3.Pages
{
        /// <summary>
        /// Логика взаимодействия для Client.xaml
        /// </summary>
       
        
                public partial class Client : Page
                {
                private List<Employees> _employees;
                private List<Employees> _filteredEmployees;
                private List<string> _jobTitles;

                public Client(User user)
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

                        _filteredEmployees = _employees.Where(emp =>
                            (emp.LastName + " " + emp.FirstName + " " + emp.MiddleName).ToLower().Contains(searchText) &&
                            (selectedJobTitle == "Все должности" || emp.PositionAtWork == selectedJobTitle))
                            .ToList();

                        EmployeesListView.ItemsSource = null;
                        EmployeesListView.ItemsSource = _filteredEmployees;
                }

                private void RefreshButton_Click(object sender, RoutedEventArgs e)
                {
                        LoadEmployees();
                }
        
        }
        
}
