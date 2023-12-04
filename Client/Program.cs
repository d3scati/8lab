using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlTypes;
using System.Xml;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Client
{
    
    public class Program
    {

        static void Main(string[] args)
        {

            // адрес и порт сервера, к которому будем подключаться
            try
            { 
                int port;
                string address;
                Console.Write("Введите порт сервера для подключения: ");
                port = int.Parse(Console.ReadLine());
                Console.Write("Введите ip-адрес сервера: ");
                address = Console.ReadLine();
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                socket.Connect(ipPoint);
                {

                    Processing(socket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        static void Processing(Socket socket)
        {
            Console.WriteLine("Пройдите аутентификацию:\nlogin|имя|пароль");
            int bytes = 0;
            string message;
            StringBuilder builder = new StringBuilder();



            do
            {
                Console.Write("\n> ");
                message = Console.ReadLine();
                if (message == null || message.Length == 0)
                {
                    message = " ";
                }
                
                byte[] data = Encoding.Unicode.GetBytes(message); 
                socket.Send(data);

                data = new byte[256];
                builder.Clear();
                do
                {
                    bytes = socket.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);
                Console.WriteLine(builder.ToString());
                //if (builder.ToString().Contains("Вход выполнен:"))
                //{
                //    byte[] dataFirst;
                //    dataFirst = new byte[256];
                //    builder.Clear();
                //    do
                //    {
                //        bytes = socket.Receive(dataFirst);
                //        builder.Append(Encoding.Unicode.GetString(dataFirst, 0, bytes));
                //    } while (socket.Available > 0); 
                //}
            } while (message != "exit");
            // закрытие сокета
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        

    }
}
