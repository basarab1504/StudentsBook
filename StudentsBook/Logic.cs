using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StudentsBook
{
    public interface IRepository<T>
    {
        event Action<T> Added;
        event Action<T> Removed;
        event Action<T> Edited;

        void Add(T e);
        void Remove(T e);
        void Edit(T e);
        void Save();
    }

    public abstract class BaseRepository<T> : IRepository<T>
    {
        public event Action<T> Added;
        public event Action<T> Removed;
        public event Action<T> Edited;

        protected List<T> items;

        public void Add(T e)
        {
            items.Add(e);
            Added(e);
        }
        public virtual void Remove(T e)
        {
            items.Remove(e);
            Removed(e);
        }
        public virtual void Edit(T e)
        {
            items[items.FindIndex(x => x.Equals(e))] = e;
            Edited(e);
        }

        public abstract void Save();
    }

    public class HomeworkRepository : BaseRepository<Homework>
    {
        public HomeworkRepository()
        {
            items = FakeDB.Homeworks;
        }

        public IEnumerable<Homework> GetAllHomeworks()
        {
            return items;
        }

        public override void Save()
        {
            FakeDB.Homeworks = items;
        }
    }

    public class SubjectRepository : BaseRepository<Subject>
    {
        public SubjectRepository()
        {
            items = FakeDB.Subjects;
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return items;
        }

        public IEnumerable<Subject> GetAllSubjectsByDate(DateTime date)
        {
            return items.Where(x => x.DateTime == date);
        }

        public override void Save()
        {
            FakeDB.Subjects = items;
        }
    }

    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository()
        {
            items = FakeDB.Students;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return items;
        }

        public override void Save()
        {
            FakeDB.Students = items;
        }
    }

    public static class FakeDB
    {
        private static List<Student> students = new List<Student>(Formatter.GetData<Student>("students.xml", "Students"));
        private static List<Subject> subjects = new List<Subject>(Formatter.GetData<Subject>("subjects.xml", "Subjects"));
        private static List<Homework> homeworks = new List<Homework>(Formatter.GetData<Homework>("homeworks.xml", "Homeworks"));

        public static List<Student> Students
        {
            get => students;
            set { students = value; }
        }
        public static List<Subject> Subjects
        {
            get => subjects;
            set { subjects = value; }
        }
        public static List<Homework> Homeworks
        {
            get => homeworks;
            set { homeworks = value; }
        }
    }


    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    
    [XmlType("Student")]
    public class Student : NotifyPropertyChanged
    {
        [XmlElement("Name")]
        private string name;
        [XmlElement("Payment")]
        private string payment;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Payment
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

    [XmlType("Subject")]
    public class Subject : NotifyPropertyChanged
    {
        [XmlElement("Student")]
        public Student Student { get; set; }
        [XmlElement("DateTime")]
        public DateTime DateTime { get; set; }
        [XmlElement("IsPaid")]
        public bool IsPaid { get; set; }
        [XmlArray("Homeworks")]
        public Homework[] Homework { get; set; }

        public override string ToString()
        {
            return DateTime.ToString();
        }
    }

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

    public static class Formatter
    {
        public static IEnumerable<T> GetData<T>(string filename, string rootAttribute)
        {
            List<T> s = new List<T>();
            using (var reader = new StreamReader(filename))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<T>),
                    new XmlRootAttribute(rootAttribute));
                s = (List<T>)deserializer.Deserialize(reader);
            }
            return s;
        }
    }

    public static class Calendar
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        public static IList<Event> GetEvents(DateTime date)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = date;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            return request.Execute().Items;
        }
    }
}
