using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Server
{
    public class Interpretator
    {
        University students;// = new University();
        StringBuilder answer = new StringBuilder();

        public  User user = null;

        static Users users = new Users(); 


        public Interpretator(University students)
        {
            //students = new University();
            this.students = students;
        }
        public string Execute(string[] command)
        {
            Thread.Sleep(1500);
            if (user == null)
            {
                
                if (command[0] == "login" && command.Length == 3)
                {
                    AuthenticateUser(command[1], command[2]);
                    if (user==null)
                    {
                        return "Ошибка ввода данных";
                    }
                    else
                    {
                        //students = DeserializeXML("xmlFile.xml");
                        return "Вход выполнен: " + user.Login +" в качестве " + user.Role+".";
                    }
                }
                else
                {
                    return "Пройдите аутентификацию:\nlogin|имя|пароль";
                }
            }
            else
            {
                if (user.Role == "Administrator")
                {
                    switch (command[0])
                    {
                        case "help":
                            return Help(command);
                        case "list":
                            return List();
                        case "addStudent":
                            return AddStudent(command);
                        case "delStudent":
                            return DelStudent(command);
                        case "setGroup":
                            return SetGroup(command);
                        case "setSpecialty":
                            return SetSpecialty(command);
                        case "serializeBoth":
                            return SerializeBoth(command);
                        case "serializeXML":
                            return SerializeOnlyXML(command);
                        case "deserializeXML":
                            return DeserializeOnlyXML(command);
                        case "serializeJSON":
                            return SerializeOnlyJSON(command);
                        case "deserializeJSON":
                            return DeserializeOnlyJSON(command);
                        case "backUpXML":
                            return BackUpXML(command);
                        case "backUpJson":
                            return BackUpJSON(command);
                        case "exit":
                            return Exit(command);
                        default:
                            return Error(command);
                    }
                }
                else
                {
                    switch (command[0])
                    {
                        case "help":
                            return HelpForDefault(command);
                        case "list":
                            return List();
                        case "addStudent":
                            return AddStudent(command);
                        case "delStudent":
                            return DelStudent(command);
                        case "setGroup":
                            return SetGroup(command);
                        case "setSpecialty":
                            return SetSpecialty(command);
                        case "exit":
                            return Exit(command);
                        default:
                            return Error(command);
                    }
                }
            }

        }
        public User AuthenticateUser(string login, string password)
        {
            users = DeserializeXMLDataBase("dataBase.xml");

            User userForAuthentication = users.UsersList.Find(u => u.Login == login && u.Password == password);

            if (userForAuthentication != null)
            {
                user = userForAuthentication;
                
                return user;
            }
            else
            {
                return user;
            }
        }
        private string List()
        {
            answer.Clear();
            foreach (Student student in students.StudentsList)
            {
                Type studentType = typeof(Student);
                PropertyInfo[] properties = studentType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(student);
                    answer.Append($"{property.Name}: {value}\n");
                }
                answer.Append('\n');
            }
            return answer.ToString();
        }
        private string AddStudent(string[] command)
        {
            answer.Clear();
            if (command.Length < 8)
            {
                return("Не достаточно аргументов");
            }
            else
            {
                if (DateTime.TryParse(command[4], out DateTime dateOfBirth))
                {
                    if (int.TryParse(command[7], out int missing))
                    {
                        Student student  = new Student(command[1], command[2], command[3], dateOfBirth, command[5], command[6], missing);
                        students.StudentsList.Add(student);
                        int i = 0;
                        foreach (Student studentOne in students.StudentsList)
                        {
                            
                            studentOne.Id = i;
                            i++;

                        }
                        return("Студент добавлен");
                    }
                }
                else
                {
                    return("Некорректный формат даты рождения. Используйте формат dd.MM.yyyy.");
                }
                return "";
            }
        }
        private string Help(string[] command)
        {
            answer.Clear();
            answer.AppendLine("Список доступных команд:");
            answer.AppendLine("list - вывод списка студентов");
            answer.AppendLine("addStudent|Фамилия|Имя|Отчество|Дата рождения|Специальность|Группа|Количество пропусков - добавить нового студента");
            answer.AppendLine("delStudent|ID - удалить студента по ID");
            answer.AppendLine("setGroup|ID|Группа - установить группу студента");
            answer.AppendLine("setSpecialty|ID|Специальность - установить специальность студента");
            answer.AppendLine("serializeBoth|ИмяФайлаXML|ИмяФайлаJSON - сериализация в XML и JSON форматы");
            answer.AppendLine("serializeXML|ИмяФайла - сериализация в XML");
            answer.AppendLine("serializeJSON|ИмяФайла - сериализация в JSON");
            answer.AppendLine("deserializeXML|ИмяФайла - десериализация XML");
            answer.AppendLine("deserializeJSON|ИмяФайла - десериализация JSON");
            answer.AppendLine("backUpXML|НомерФайла - десериализация прошлого XML файла(1-3)");
            answer.AppendLine("backUpJSON|НомерФайла - десериализация  JSON файла(1-3)");
            answer.AppendLine("exit - выход из программы");
            return answer.ToString();

        }
        private string HelpForDefault(string[] command)
        {
            answer.Clear();
            answer.AppendLine("Список доступных команд:");
            answer.AppendLine("list - вывод списка студентов");
            answer.AppendLine("addStudent|Фамилия|Имя|Отчество|Дата рождения|Специальность|Группа|Количество пропусков - добавить нового студента");
            answer.AppendLine("delStudent|ID - удалить студента по ID");
            answer.AppendLine("setGroup|ID|Группа - установить группу студента");
            answer.AppendLine("setSpecialty|ID|Специальность - установить специальность студента");
            answer.AppendLine("exit - выход из программы");
            return answer.ToString();

        }

        string DelStudent(string[] command)
        {
            answer.Clear();
            int studentId;
            if (command.Length < 2)
            {
                return("Не достаточно аргументов");
            }
            else if (int.TryParse(command[1], out studentId))
            {
                Student deletedStudent = FindStudent(students.StudentsList, studentId);
                if (deletedStudent != null)
                {
                    students.StudentsList.Remove(deletedStudent);
                    int i = 0;
                    foreach (Student studentOne in students.StudentsList)
                    {

                        studentOne.Id = i;
                        i++;

                    }
                    return ("Студент удален");
                }
                else
                {
                    return("Данный студент не обнаружен");
                }
            }
            else
            {
                return ("Аргумент должен быть числом");
            }
        }

        Student FindStudent(List<Student> students, int id)
        {
            answer.Clear();
            foreach (Student student in students)
                if (student.Id == id)
                    return student;
            return null;
        }

        string SetGroup(string[] command)
        {
            answer.Clear();
            if (command.Length < 3)
            {
                return ("Не достаточно аргументов");
            }
            else
            {
                if (int.TryParse(command[1], out int studentId))
                {
                    Student student = FindStudent(students.StudentsList, studentId);
                    if (student != null)
                    {
                        student.Group = command[2];
                        return($"Группа студента с ID {studentId} установлена: {command[2]}");
                    }
                    else
                    {
                        return ("Данный студент не обнаружен");
                    }
                }
                else
                {
                    return ("Аргумент должен быть числом (ID студента)");
                }
            }
        }

        string SetSpecialty(string[] command)
        {
            answer.Clear();
            if (command.Length < 3)
            {
                return ("Не достаточно аргументов");
            }
            else
            {
                if (int.TryParse(command[1], out int studentId))
                {
                    Student student = FindStudent(students.StudentsList, studentId);
                    if (student != null)
                    {
                        student.Specialty = command[2];
                        return ($"Специальность студента с ID {studentId} установлена: {command[2]}");
                    }
                    else
                    {
                        return ("Данный студент не обнаружен");
                    }
                }
                else
                {
                    return ("Аргумент должен быть числом (ID студента)");
                }
            }
        }
        static void SerializeXML(string fileName, University students)
        {
            XmlSerializer xml = new XmlSerializer(typeof(University));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xml.Serialize(fs, students);
            }
        }
        static void SerializeXMLDataBase(string fileName, Users users)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Users));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xml.Serialize(fs, users);
            }
        }
        public static University DeserializeXML(string fileName)
        {
            XmlSerializer xml = new XmlSerializer(typeof(University));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return (University)xml.Deserialize(fs);
            }
        }
        static Users DeserializeXMLDataBase(string fileName)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Users));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return (Users)xml.Deserialize(fs);
            }
        }
        static void SerializeJSON(string fileName, University students)
        {
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
            });
            File.WriteAllText(fileName, json);
        }

        University DeserializeJSON(string fileName)
        {
            
            string json = File.ReadAllText(fileName);
            University university = JsonSerializer.Deserialize<University>(json);
            return university;
        }
        string SerializeBoth(string[] command)
        {
            answer.Clear();
            SerializeXML(command[1] + ".xml", students);
            SerializeJSON(command[2] + ".json", students);
            return("Сереализация выполнена");
        }
        string SerializeOnlyXML(string[] command)
        {
            answer.Clear();
            SerializeXML(command[1] + ".xml", students);
            return ("Сереализация выполнена");
        }
        string SerializeOnlyJSON(string[] command)
        {
            answer.Clear();
            SerializeJSON(command[1] + ".json", students);
            return ("Сереализация выполнена");
        }
        string DeserializeOnlyXML(string[] command)
        {
            answer.Clear();
            CreateBackupXML(command[1], "backupDirectoryXML");
            students.StudentsList.Clear();
            students = DeserializeXML(command[1] + ".xml");
            Thread.Sleep(1500);
            return ("Десереализация выполнена");
        }
        string DeserializeOnlyJSON(string[] command)
        {
            answer.Clear();
            CreateBackupJSON(command[1], "backupDirectoryJOSN");
            students.StudentsList.Clear();
            students = DeserializeJSON(command[1] + ".json");
            Thread.Sleep(1500);
            return ("Десереализация выполнена");
        }


        private string Error(string[] command)
        {
            answer.Clear();
            return "Ошибка в написании команды.";
        }
        private string Exit(string[] command)
        {
            answer.Clear();
            return "Отключение от сервера.";
        }
        private void CreateBackupDirectory(string backupDirectory)
        {
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }
        }
        private void CreateBackupXML(string fileName, string backupDirectory)
        {
            CreateBackupDirectory(backupDirectory);
            FileInfo[] backupFiles = new DirectoryInfo(backupDirectory).GetFiles("*.xml");

            if (backupFiles.Length >= 3)
            {
                Array.Sort(backupFiles, (a, b) => a.CreationTime.CompareTo(b.CreationTime));
                File.Delete(backupFiles[0].FullName);
            }
            fileName = $"{DateTime.Now:yyyyMMddHHmmss}.xml";
            string backupFileName = Path.Combine(backupDirectory, fileName);
            SerializeXML(backupFileName, students);

        }
        private void CreateBackupJSON(string fileName, string backupDirectory)
        {
            CreateBackupDirectory(backupDirectory);
            FileInfo[] backupFiles = new DirectoryInfo(backupDirectory).GetFiles("*.json");

            if (backupFiles.Length >= 3)
            {
                Array.Sort(backupFiles, (a, b) => a.CreationTime.CompareTo(b.CreationTime));
                File.Delete(backupFiles[0].FullName);
            }
            fileName = $"{DateTime.Now:yyyyMMddHHmmss}.json";
            string backupFileName = Path.Combine(backupDirectory, fileName);
            SerializeJSON(backupFileName, students);

        }
        string BackUpXML(string []command)
        {
            int i = int.Parse(command[1]) - 1;
            FileInfo[] backupFiles = new DirectoryInfo("backupDirectoryXML").GetFiles("*.xml");
            Array.Sort(backupFiles, (a, b) => a.CreationTime.CompareTo(b.CreationTime));

            if (i >= 0 && i < backupFiles.Length)
            {
                string filePath = backupFiles[i].FullName; // Получаем путь к файлу в виде строки
                students = DeserializeXML(filePath);
                return "Восстановлено!";
            }
            else
            {
                return "Индекс вне диапазона допустимых значений.";
            }
        }
        string BackUpJSON(string[] command)
        {

            int i = int.Parse(command[1]) - 1;
            FileInfo[] backupFiles = new DirectoryInfo("backupDirectoryJSON").GetFiles("*.json");
            Array.Sort(backupFiles, (a, b) => a.CreationTime.CompareTo(b.CreationTime));

            if (i >= 0 && i < backupFiles.Length)
            {
                string filePath = backupFiles[i].FullName; // Получаем путь к файлу в виде строки
                students = DeserializeJSON(filePath);
                return "Восстановлено!";
            }
            else
            {
                return "Индекс вне диапазона допустимых значений.";
            }
        }
    }

}
