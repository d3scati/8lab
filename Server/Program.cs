using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace Server
{
    public class Program
    {
        public University students;
        static void Main(string[] args)
        {
            University students = DeserializeXML("xmlFile.xml");

            try
            {
                Console.Write("Введите порт данного сервера: ");
                int port = int.Parse(Console.ReadLine());
                //int port = 8005; // порт для приема входящих запросов

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);

                IPHostEntry myhost = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress ip in myhost.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    Console.WriteLine("IP-адрес сервера: " + ip.ToString());
                }
                Console.WriteLine("Текущий номер порта " + port.ToString());



                // создаем сокет
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);
                // начинаем прослушивание
                listenSocket.Listen(10);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                do
                {
                    //Socket handler = listenSocket.Accept();
                    //Console.WriteLine($"Входящее подключение от {handler.RemoteEndPoint}");
                    //WorkWithClient workWithClient = new WorkWithClient(handler);
                    //Processing(handler);

                    Socket handler = listenSocket.Accept();
                    

                    // Создание объекта для работы с соединением в отдельном потоке
                    WorkWithClient client = new WorkWithClient(handler,students);
                    ThreadStart threadStart = new ThreadStart(client.Run);
                    Thread thread = new Thread(threadStart);
                    thread.Start();



                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //static void Processing(Socket handler)
        //{
        //    int bytes = 0; // количество полученных байтов
        //    byte[] data = new byte[256]; ; // буфер для получаемых данных
        //    StringBuilder builder = new StringBuilder();
        //    Console.WriteLine("Клиент подключен");
        //    try
        //    {
        //        do
        //        {
        //            // получение сообщения
        //            builder.Clear();
        //            do
        //            {
        //                bytes = handler.Receive(data);
        //                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
        //            } while (handler.Available > 0);
        //            Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + builder.ToString());
        //            // отправка ответа
        //            data = Encoding.Unicode.GetBytes("Ваше сообщение доставлено");
        //            handler.Send(data);
        //        } while (builder.ToString() != "exit");
        //        // закрытие сокета
        //        Console.WriteLine("Клиент отключен");
        //        handler.Shutdown(SocketShutdown.Both);
        //        handler.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        public static University DeserializeXML(string fileName)
        {
            XmlSerializer xml = new XmlSerializer(typeof(University));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return (University)xml.Deserialize(fs);
            }
        }
    }
    
}
