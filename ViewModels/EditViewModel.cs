using EversisZadanieRekrutacyjne.Commands;
using EversisZadanieRekrutacyjne.Helpers;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using EversisZadanieRekrutacyjne.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EversisZadanieRekrutacyjne.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeService _employeeService;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICommand SaveCommand { get; }

        public EditViewModel(Employee employee, IEmployeeService employeeService)
        {
            _employeeService = employeeService;

            InitializeEmployee(employee);
            SaveCommand = new RelayCommandAsync(Save);
        }
        public bool CanCloseWindow()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zamknąć okno?", "Zamknij", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        private void InitializeEmployee(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            Surname = employee.Surename;
            Email = employee.Email;
            Phone = employee.Phone;
        }

        private bool CanSave(out string error)
        {
            error = string.Empty;
            bool isNameValid = !string.IsNullOrEmpty(Name);
            bool isSurnameValid = !string.IsNullOrEmpty(Surname);
            bool isEmailValid = !string.IsNullOrEmpty(Email) && EmailValidator.ValidateEmail(Email);
            bool isPhoneValid = !string.IsNullOrEmpty(Phone); //&& PhoneValidator.ValidatePhoneNumber(Phone); // numery telefonów w pliku Excel mają 10 cyfr

            if (!isNameValid)
                error = "Proszę podać imię.";

            if (!isSurnameValid)
                error = "Proszę podać nazwisko.";

            if (!isEmailValid)
                error = "Proszę podać poprawny adres e-mail.";

            if (!isPhoneValid)
                error = "Proszę podać poprawny numer telefonu.";

            return isNameValid && isSurnameValid && isEmailValid && isPhoneValid;
        }

        private async Task Save(object parameter)
        {
            try
            {
                if (CanSave(out string error))
                {
                    var employee = EmployeeFactory.CreateEmployee(Id, Name, Surname, Email, Phone);
                    await UpdateEmployee(employee);
                    RequestClose?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ShowErrorMessage("Nie można zapisać danych pracownika. " + error);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Wystąpił błąd podczas zapisu danych pracownika: " + ex.Message);
            }
        }

        private async Task UpdateEmployee(Employee employee)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd podczas aktualizacji pracownika: " + ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}