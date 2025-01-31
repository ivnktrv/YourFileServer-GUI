﻿using System.Net.Sockets;
using yfs_security;
using System.Text;
using yfs_net;
using yfs_io;

namespace yfs_client;

public class YFSclient
{
    YFSio  io  = new();
    YFSnet net = new();
    YFSsec sec = new();

    public void run()
    {
        string ip = Console.ReadLine();
        int port = int.Parse(Console.ReadLine());

        try
        {
            Socket socket = net.createClient(ip, port);
            //sec.sendAuthData(socket);

            byte[] getAnswer = new byte[1];
            socket.Receive(getAnswer);

            if (getAnswer[0] == 1) { }
            else
            {
                Console.WriteLine("\n[-] Логин или пароль неверный. Возможно, сервер изменил данные для входа");
                socket.Close();
                return;
            }

            string currentDir = "/";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.WriteLine($"""
                ----------------------------------------------
                """);

                net.sendData(socket, "list");

                while (true)
                {
                    byte[] b = new byte[512];
                    socket.Receive(b);
                    string s = Encoding.UTF8.GetString(b);
                    Console.Write(s);
                    if (s.Contains(":END_OF_LIST"))
                        break;
                }

                Console.Write($"""


                ----------------------------------------------
                 ДИРЕКТОРИЯ: {currentDir}
                
                 [1] Скачать файл       [2] Загрузить файл
                 [3] Удалить файл       [4] Перейти в каталог
                 [5] Отключиться        [F] Информация о файле

                -> 
                """);

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.NumPad1 || key.Key == ConsoleKey.D1)
                {
                    net.sendData(socket, "download");

                    Console.Write("\nКакой файл скачать?: ");
                    string downloadFile = Console.ReadLine();

                    net.sendData(socket, downloadFile);

                    Console.Write("В какую папку сохранить?: ");
                    string getPath = Console.ReadLine();

                    Console.WriteLine("\n[...] Идёт скачивание");
                    //io.downloadFile(socket, getPath);

                }

                else if (key.Key == ConsoleKey.NumPad2 || key.Key == ConsoleKey.D2)
                {
                    net.sendData(socket, "upload");

                    Console.Write("\nКакой файл загрузить на сервер?: ");
                    string sendFile = Console.ReadLine();

                    Console.Write("Зашифровать файл перед отправкой? [y/n]: ");
                    ConsoleKeyInfo k = Console.ReadKey();
                    /*if (k.Key == ConsoleKey.Y)
                        io.uploadFile(socket, sendFile, encryptFile: true);
                    else
                        io.uploadFile(socket, sendFile);*/
                }

                else if (key.Key == ConsoleKey.NumPad3 || key.Key == ConsoleKey.D3)
                {
                    net.sendData(socket, "delete");

                    Console.Write("\nКакой файл удалить?: ");
                    string delFile = Console.ReadLine();

                    net.sendData(socket, delFile);
                }

                else if (key.Key == ConsoleKey.NumPad4 || key.Key == ConsoleKey.D5)
                {
                    net.sendData(socket, "cd");

                    Console.Write("\nВ какой каталог перейти?: ");
                    string changeDir = Console.ReadLine();

                    net.sendData(socket, changeDir);
                    if (changeDir == "..")
                        currentDir = "/";
                    else
                        currentDir = changeDir;
                }

                else if (key.Key == ConsoleKey.NumPad5 || key.Key == ConsoleKey.D6)
                {
                    net.sendData(socket, "closeconn");
                    socket.Close();
                    break;
                }

                else if (key.Key == ConsoleKey.F)
                {
                    net.sendData(socket, "fileinfo");
                    Console.Write("\nФайл: ");
                    net.sendData(socket, Console.ReadLine());

                    byte[] data = new byte[1024];
                    socket.Receive(data);

                    Console.WriteLine('\n'+Encoding.UTF8.GetString(data));
                    Console.ReadKey();
                }
            }
        }
         catch (SocketException)
        {
            Console.WriteLine("[-] Сервер отвёрг запрос на подключение или же он не в сети");
        }
    }
}
