using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StudentsBook
{
    [XmlType("Student")]
    public class Student : INotifyPropertyChanged
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Payment")]
        public string Payment { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class Subject
    {
        public Student Student { get; set; }
        public Homework Homework { get; set; }
        public bool IsPaid { get; set; }
    }

    public class Homework
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }

    public static class Formatter
    {
        public static IEnumerable<Student> G()
        {
            List<Student> s = new List<Student>();
            using (var reader = new StreamReader("students.xml"))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Student>),
                    new XmlRootAttribute("Students"));
                s = (List<Student>)deserializer.Deserialize(reader);
            }
            return s;
        }

        public static T DeserializeXMLFileToObject<T>(string XmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFilename)) return default(T);
            try
            {
                StreamReader xmlStream = new StreamReader(XmlFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                //ExceptionLogger.WriteExceptionToConsole(ex, DateTime.Now);
            }
            return returnObject;
        }
    }
}
