
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Server
{
    public class WorkWithClient
    {
        static int count = 0;
        Socket socket;
        int id;
        string clientInfo;
        string answer;
        StringBuilder builder = new StringBuilder();
        byte[] data;
        int dataLength;
        public University students;
        public WorkWithClient(Socket socket,University students)
        {
            this.socket = socket;
            id = count;
            count++;
            clientInfo = socket.RemoteEndPoint.ToString();
            this. students = students;
            
        }
        public void Run()
        {

            Console.WriteLine($"Client №{id}. Информация: Установлено соединение с \"{clientInfo}\"");
            Interpretator interpretator = new Interpretator(this.students);
            try
            {
                do
                {
                    // Получение команды
                    data = new byte[256];
                    builder.Clear();
                    
                    do
                    {
                        dataLength = socket.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, dataLength));

                    } while (socket.Available > 0);
                    Console.WriteLine($"Client №{id}. Команда: {builder.ToString()}");
                    // Обработка команды для генерации ответа
                    
                    string[] command = builder.ToString().Split('|');

                    answer =(interpretator.Execute(command))+ "\n" + DateTime.Now.ToString();
                    //if (answer.Contains("Вход выполнен:"))
                    //{
                    //    data = Encoding.Unicode.GetBytes(answer);
                    //    socket.Send(data);
                    //    string xmlFilePath = @"F:\\Информатика\Методы\8\8lab\Server\bin\Debug\xmlFile.xml";
                    //    byte[] xmlData = File.ReadAllBytes(xmlFilePath);
                    //}
                    data = Encoding.Unicode.GetBytes(answer);
                    socket.Send(data);


                } while (builder.ToString() != "exit");
                Console.WriteLine($"Client №{id}. Информация: Клиент отключился");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Client №{id}. {e.Message}");
            }

        }
    }
}