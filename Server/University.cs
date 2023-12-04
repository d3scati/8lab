using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class University
    {
        
        public List<Student> StudentsList { get; set; } = new List<Student>();
        public University()
        {

            //StudentsList.Add(new Student("Селезнев", "Никита", "Дмитриевич", new DateTime(1998, 5, 15), "Информационная безопасность автоматизированных систем", "СО222КОБ", 5));
            //StudentsList.Add(new Student("Иванов", "Иван", "Иванович", new DateTime(1999, 3, 10), "Программная инженерия", "СО222КОБ", 3));
            //StudentsList.Add(new Student("Сидоров", "Алексей", "Алексеевич", new DateTime(2000, 8, 22), "Информационаня безопасность", "СО222КОБ", 8));
            //StudentsList.Add(new Student("Иванова", "Елена", "Ивановна", new DateTime(1997, 11, 5), "Информационаня безопасность", "СО222КОБ", 11));
            //StudentsList.Add(new Student("Семенов", "Павел", "Павлович", new DateTime(2001, 2, 18), "Информационная безопасность автоматизированных систем", "СО222КОБ", 2));

        }
    }
}
