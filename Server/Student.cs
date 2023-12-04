using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    [Serializable]
    public class Student
    {
        string secondName;
        string name;
        string surname;
        string group;
        string speciality;
        DateTime dateOfBirth;
        static int count;
        int missing;
        int id;
        public int Id { get { return id; } set => id = value; }
        public string SecondName { get => secondName; set => secondName = value; }
        [XmlElement("StudentName")]
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Group { get => group; set => group = value; }
        public string Specialty { get => speciality; set => speciality = value; }
        public int Missing { get => missing; set => missing = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }

        public Student(string secondName, string name, string surname, DateTime dateOfBirth, string speciality, string group, int missing)
        {
            this.dateOfBirth = dateOfBirth;
            this.speciality = speciality;
            this.group = group;
            this.secondName = secondName;
            this.name = name;
            this.surname = surname;
            this.missing = missing;
            id = count;
            count++;
        }
        public Student() { }

    }
}
