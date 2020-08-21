using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsBook
{

    [XmlType("Student")]
    public class Student : NotifyPropertyChanged
    {
        [XmlElement("Name")]
        private string name;
        [XmlElement("Payment")]
        private int payment;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                OnPropertyChanged("Payment");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class StudentModel : Model<Student>
    {
        public StudentModel()
        {
            Items = FakeDB.Students;
        }

        public override void Save()
        {

        }
    }
}
