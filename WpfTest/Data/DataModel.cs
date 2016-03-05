using System.Collections.Generic;
using System.Text;

namespace WpfTest.Data
{
    enum StatusCheck
    {
        Ok = 0,
        FuckedUp = 1
    }

    class DataModel
    {
        private static DataModel s_instance;
        private PersonsData _personsData;

        private DataModel()
        {
            _personsData = new PersonsData();
        }

        public static DataModel Instance
        {
            get { return s_instance ?? (s_instance = new DataModel()); }
        }

        public StatusCheck AddNewPerson(Person p)
        {
            _personsData.AllPersons.Add(p);
            return StatusCheck.Ok;
        }

        public string GetUpdatedList()
        {
            if (_personsData.AllPersons.Count == 0)
            {
                return "List is empty";
            }

            StringBuilder personsList = new StringBuilder();
            foreach (var person in _personsData.AllPersons)
            {
                personsList.Append("Name:"+person.Name+", Age:");
                personsList.AppendLine(person.Age);
            }
            return personsList.ToString();
        }
    }

    class PersonsData
    {
        public List<Person> AllPersons;

        public PersonsData()
        {
            AllPersons = new List<Person>();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
}
