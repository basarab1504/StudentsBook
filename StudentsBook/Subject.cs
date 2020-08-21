using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsBook
{
    [XmlType("Subject")]
    public class Subject : NotifyPropertyChanged
    {
        [XmlElement("Student")]
        private Student student;
        [XmlElement("From")]
        private DateTime from;
        [XmlElement("To")]
        private DateTime to;
        [XmlElement("IsPaid")]
        public bool IsPaid { get; set; }
        [XmlElement("Mats")]
        private string materials;
        [XmlArray("Homeworks")]
        public List<Homework> Homework { get; set; }
        
        public string Materials
        {
            get { return materials; }
            set
            {
                materials = value;
                OnPropertyChanged("Mats");
            }
        }

        public Student Student
        {
            get { return student; }
            set
            {
                student = value;
                OnPropertyChanged("Student");
            }
        }

        public DateTime From
        {
            get { return from; }
            set
            {
                from = value;
                OnPropertyChanged("From");
            }
        }

        public DateTime To
        {
            get { return to; }
            set
            {
                to = value;
                OnPropertyChanged("To");
            }
        }

        public override string ToString()
        {
            return From.ToString();
        }
    }

    public class SubjectModel : Model<Subject>
    {
        public SubjectModel()
        {
            Items = FakeDB.Subjects;
        }

        public IEnumerable<Subject> GetAllSubjectsByDate(DateTime dateTime)
        {
            return Items.Where(x => x.From == dateTime);
        }

        public override void Save()
        {
        }
    }
}
