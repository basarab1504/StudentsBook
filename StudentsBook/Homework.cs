using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsBook
{
    [XmlType("Homework")]
    public class Homework : NotifyPropertyChanged
    {
        [XmlElement("Title")]
        private string title;
        [XmlElement("Description")]
        private string description;
        [XmlElement("Code")]
        private string code;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
    public class HomeworkModel : Model<Homework>
    {
        public HomeworkModel()
        {
            Items = FakeDB.Homeworks;
        }

        public override void Save()
        {
        }
    }
}
