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

namespace EversisZadanieRekrutacyjne.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged
    {
        public event EventHandler RequestClose;
        private readonly IEmployeeRepository _employeeRepository;
        public bool CanCloseWindow()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zamknąć okno?", "Zamknij", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
        private bool CanSave()
        {
            bool isNameValid = !string.IsNullOrEmpty(Name);
            bool isSurnameValid = !string.IsNullOrEmpty(Surname);
            bool isEmailValid = !string.IsNullOrEmpty(Email);
            bool isPhoneValid = !string.IsNullOrEmpty(Phone);

            return isNameValid && isSurnameValid && isEmailValid && isPhoneValid;
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditViewModel(Employee employee, IEmployeeRepository employeeRepository)
        {
            Id = employee.Id;
            Name = employee.Name;
            Surname = employee.Surename;
            Email = employee.Email;
            Phone = employee.Phone;

            _employeeRepository = employeeRepository;
            SaveCommand = new RelayCommand(Save);
        }

        private void Save(object parameter)
        {
            if (CanSave())
            {
                _employeeRepository.Update(EmployeeFactory.CreateEmployee(Id, Name, Surname, Email, Phone));
                _employeeRepository.Save();
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            else
            { 
                MessageBox.Show("Nie można zapisać danych pracownika. Należy sprawdzić czy wszystkie pola są wypełnione poprawnie."); 
            }
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}