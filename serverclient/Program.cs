using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
  
namespace HTTPServer
{
// Класс-обработчик клиента
class Client
 {
    
           // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
            public Client(TcpClient Client)
          {
            
            // Объявим строку, в которой будет хранится запрос клиента
            string Request = "";
                 // Буфер для хранения принятых от клиента данных
              byte[] Buffer = new byte[1024];
                 // Переменная для хранения количества байт, принятых от клиента
              int Count;
            // Читаем из потока клиента до тех пор, пока от него поступают данные
            string Headers = "1234";
           byte[] HeadersBuffer = Encoding.ASCII.GetBytes(Headers);
             Client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);
            Console.WriteLine(Headers);
            
            Console.WriteLine("Сообщение отправленно");

            
            Client.Close();
        }
    }
    class Server
      {
           TcpListener Listener; // Объект, принимающий TCP-клиентов
   
         // Запуск сервера
        public Server(int Port)
          {
            Listener = new TcpListener(IPAddress.Any, Port); // Создаем "слушателя" для указанного порта
                       Listener.Start(); // Запускаем его
            Console.WriteLine("Сервер запущен");

            // В бесконечном цикле
            while (true)
                              {
                
                                 // Принимаем новых клиентов. После того, как клиент был принят, он передается в новый поток (ClientThread)
                // с использованием пула потоков.
                 ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
                             
            }
        }
        
        static void ClientThread(Object StateInfo)
         {
            Thread.Sleep(2000);
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient)StateInfo);
            
        }
  
           
          
  
          static void Main(string[] args)
           {

            int MaxThreadsCount = 2;
                         // Установим максимальное количество рабочих потоков
           ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                          // Установим минимальное количество рабочих потоков
             ThreadPool.SetMinThreads(1, 1);

            // Создадим новый сервер на порту 80
            new Server(80);
              }
     }
    }
  