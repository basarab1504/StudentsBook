using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentsBook
{
    class LanguagesViewModel : NotifyPropertyChanged
    {
        private Student student;
        private string selectedPickedLanguages;
        private string selectedSettedLanguages;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;

        public LanguagesViewModel(Student student)
        {
            this.student = student;
            AllLanguages = FakeDB.Langugages;
            SettedLanguages = new ObservableCollection<string>(student.Languages);
            //AllLanguages = new HomeworkModel().Items;
            //if (student.Homework == null)
            //    student.Homework = new List<object>();
            //SettedLanguages = new ObservableCollection<object>(student.Homework);
        }

        public IEnumerable<string> AllLanguages { get; set; }
        public ObservableCollection<string> SettedLanguages { get; set; }

        public string SelectedPickedLanguage
        {
            get { return selectedPickedLanguages; }
            set
            {
                selectedPickedLanguages = value;
                OnPropertyChanged("SelectedPickedLanguage");
            }
        }
        public string SelectedSettedLanguage
        {
            get { return selectedSettedLanguages; }
            set
            {
                selectedSettedLanguages = value;
                OnPropertyChanged("SelectedSettedLanguage");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      string language = obj as string;
                      if (language != null)
                      {
                          SettedLanguages.Add(language);
                          student.Languages.Add(language);
                      }
                  }/*,
                  (obj) => (obj as Homework)?.Count() > 0*/));
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      string language = obj as string;
                      if (language != null)
                      {
                          SettedLanguages.Remove(language);
                          student.Languages.Remove(language);
                      }
                  }/*,
                  (obj) => (obj as Homework)?.Count() > 0*/));
            }
        }
    }
}
