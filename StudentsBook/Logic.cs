using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    public static class FakeDB
    {
        private static ObservableCollection<Student> students = new ObservableCollection<Student>(Formatter.GetData<Student>("students.xml", "ArrayOfStudent"));
        private static ObservableCollection<Subject> subjects = new ObservableCollection<Subject>(Formatter.GetData<Subject>("subjects.xml", "ArrayOfSubject"));
        private static ObservableCollection<Homework> homeworks = new ObservableCollection<Homework>(Formatter.GetData<Homework>("homeworks.xml", "ArrayOfHomework"));
        private static ObservableCollection<string> languages = new ObservableCollection<string>(Formatter.GetData<string>("languages.xml", "ArrayOfLanguages"));

        public static ObservableCollection<Student> Students
        {
            get => students;
            set { students = value; }
        }
        public static ObservableCollection<Subject> Subjects
        {
            get => subjects;
            set { subjects = value; }
        }
        public static ObservableCollection<Homework> Homeworks
        {
            get => homeworks;
            set { homeworks = value; }
        }
        public static ObservableCollection<string> Langugages
        {
            get => languages;
            set { languages = value; }
        }

        public static void Load()
        {

        }

        public static void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Student>));
            using (FileStream fs = new FileStream("students.xml", FileMode.Create))
            {
                formatter.Serialize(fs, Students);
            }
            XmlSerializer formatter1 = new XmlSerializer(typeof(ObservableCollection<Subject>));
            using (FileStream fs = new FileStream("subjects.xml", FileMode.Create))
            {
                formatter1.Serialize(fs, Subjects);
            }
            XmlSerializer formatter2 = new XmlSerializer(typeof(ObservableCollection<Homework>));
            using (FileStream fs = new FileStream("homeworks.xml", FileMode.Create))
            {
                formatter2.Serialize(fs, Homeworks);
            }
        }
    }


    public abstract class Model<T> : NotifyPropertyChanged
    {
        public ObservableCollection<T> Items { get; set; }

        public void Add(T e)
        {
            Items.Add(e);
            OnPropertyChanged("Items");
            Save();
        }

        public void Remove(T e)
        {
            Items.Remove(e);
            OnPropertyChanged("Items");
            Save();
        }

        public void Change(T e)
        {
            T s = Items.FirstOrDefault(x => e.Equals(x));
            s = e;
            OnPropertyChanged("Items");
            Save();
        }

        public abstract void Save();
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

    public static class GoogleCalendar
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        public static IEnumerable<Subject> GetSubjects(DateTime min, DateTime max)
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
            request.TimeMin = min;
            request.TimeMax = max;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 1000;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            List<Subject> subjects = new List<Subject>();
            Events events = request.Execute();
            foreach (var eventItem in events.Items)
            {
                Subject s = EventToSubject(eventItem);
                if (s != null)
                    subjects.Add(EventToSubject(eventItem));
            }
            return subjects;
        }

        public static Subject EventToSubject(Event e)
        {
            Subject s = null;

            Student student = FakeDB.Students.FirstOrDefault(x => e.Summary.Contains(x.Name));

            if(student != null)
            {
                s = new Subject();
                s.Student = student;
                s.From = e.Start.DateTime ?? DateTime.Now;
                s.To = e.End.DateTime ?? DateTime.Now;
            }
            return s;
        }
    }
}
