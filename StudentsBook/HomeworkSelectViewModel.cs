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
    class HomeworksSelectViewModel : NotifyPropertyChanged
    {
        private Subject subject;
        private Homework selectedPickedHomework;
        private Homework selectedSettedHomework;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;

        public HomeworksSelectViewModel(Subject _subject)
        {
            this.subject = _subject;
            AllHomeworks = new HomeworkModel().Items;
            if (_subject.Homework == null)
                _subject.Homework = new List<Homework>();
            SettedHomeworks = new ObservableCollection<Homework>(_subject.Homework);
        }

        public IEnumerable<Homework> AllHomeworks { get; set; }
        public ObservableCollection<Homework> SettedHomeworks { get; set; }

        public Homework SelectedPickedHomework
        {
            get { return selectedPickedHomework; }
            set
            {
                selectedPickedHomework = value;
                OnPropertyChanged("SelectedPickedHomework");
            }
        }
        public Homework SelectedSettedHomework
        {
            get { return selectedSettedHomework; }
            set
            {
                selectedSettedHomework = value;
                OnPropertyChanged("SelectedSettedHomework");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Homework homework = obj as Homework;
                      if (homework != null)
                      {
                          SettedHomeworks.Add(homework);
                          subject.Homework.Add(homework);
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
                      Homework homework = obj as Homework;
                      if (homework != null)
                      {
                          SettedHomeworks.Remove(homework);
                          subject.Homework.Remove(homework);
                      }
                  }/*,
                  (obj) => (obj as Homework)?.Count() > 0*/));
            }
        }
    }
}
