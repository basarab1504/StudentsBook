using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentsBook
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

    class StudentsViewModel : INotifyPropertyChanged
    {
        private Student selectedStudent;

        public ObservableCollection<Student> Students { get; set; }

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ??
                  (add = new RelayCommand(obj =>
                  {
                      Student student = new Student() { Name = "Новичок" };
                      Students.Add(student);
                      SelectedStudent = student;
                  }));
            }
        }

    public Student SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }

        public StudentsViewModel()
        {
            Students = new ObservableCollection<Student>(Formatter.G());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
