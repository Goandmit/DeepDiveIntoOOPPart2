using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeepDiveIntoOOPPart2
{
    internal static class Repository
    {
        private static readonly string idFilePath = "IDStorage.txt";        
        public static string ClientFilePath { get { return "ClientDataStorage.txt"; } }
        public static Worker CurrentUser { get; set; }
        public static string CurrentId { get; set; }

        public static string GenerateHistoryFilePath(string id)
        {
            string historyFilePath = $"HistoryOfChanges{id}.txt";

            return historyFilePath;
        }

        public static bool CheckBeforeReading(string filePath)
        {
            bool status = false;

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 6)
            {
                status = true;
            }

            return status;
        }

        public static int AssignId()
        {
            int id;
            string streamString;

            if (File.Exists(idFilePath) && new FileInfo(idFilePath).Length > 6)
            {
                using (StreamReader streamReader =
                    new StreamReader(idFilePath, Encoding.Unicode))
                {
                    streamString = $"{streamReader.ReadLine()}";
                }

                id = Convert.ToInt32(streamString) + 1;
                streamString = id.ToString();
            }
            else
            {
                streamString = "1";
                id = Convert.ToInt32(streamString);
            }

            using (StreamWriter streamWriter =
                new StreamWriter(idFilePath, false, Encoding.Unicode))
            {
                streamWriter.WriteLine(streamString);
            }

            return id;
        }

        public static string HideString(string normalString)
        {
            string hiddenString = String.Empty;

            for (int i = 0; i < normalString.Length; i++)
            {
                if (normalString[i] != ' ')
                {
                    hiddenString += "*";
                }
                else
                {
                    hiddenString += " ";
                }
            }

            return hiddenString;
        }

        public static List<Client> GetAllClientsFromFile()
        {
            List<Client> clients = new List<Client>();

            using (StreamReader streamReader =
                new StreamReader(ClientFilePath, Encoding.Unicode))
            {
                while (!streamReader.EndOfStream)
                {
                    string streamString = $"{streamReader.ReadLine()}";
                    string[] streamStringSplited = streamString.Split('#');

                    Client client = new Client(streamStringSplited[0],
                        streamStringSplited[1], streamStringSplited[2],
                        streamStringSplited[3], streamStringSplited[4],
                        streamStringSplited[5], streamStringSplited[6],
                        streamStringSplited[7]);

                    clients.Add(client);
                }
            }

            return clients;
        }        

        public static string ConvertClientToString(Client client)
        {
            string clientInString = $"{client.ClientId}#" +
                $"{client.Surname}#" +
                $"{client.Name}#" +
                $"{client.Patronymic}#" +
                $"{client.ClientName}#" +
                $"{client.PhoneNumber}#" +
                $"{client.PassportSeries}#" +
                $"{client.PassportNumber}";

            return clientInString;
        }

        public static void WriteClientsToFile(List<Client> clients)
        {
            using (StreamWriter streamWriter =
                new StreamWriter(ClientFilePath, false, Encoding.Unicode))
            {
                foreach (Client client in clients)
                {
                    string clientInString = ConvertClientToString(client);
                    streamWriter.WriteLine(clientInString);
                }
            }
        }

        public static Client FindClient(List<Client> clients, string id)
        {
            Client client = null;

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].ClientId == id)
                {
                    client = clients[i];
                    break;
                }
            }

            return client;
        }

        public static Client GetClientFromFile(string id)
        {
            List<Client> clients = GetAllClientsFromFile();

            Client client = FindClient(clients, id);

            return client;
        }       

        public static List<string> WriteHistory(string id, string changedFields, string operation)
        {
            List<string> history = new List<string>()
            {
                $"Операция: {operation}",
                $"Дата и время: {DateTime.Now}",
                $"Сотрудник: {CurrentUser.Position}",
            };

            if (operation == "изменение записи")
            {
                history.Add($"Изменены поля: {changedFields}");
            }

            history.Add(String.Empty);

            using (StreamWriter streamWriter = new StreamWriter
                ($"{GenerateHistoryFilePath(id)}", true, Encoding.Unicode))
            {
                foreach (string streamString in history)
                {
                    streamWriter.WriteLine(streamString);                    
                }
            }

            return history;
        }

        public static List<string> GetHistory(string id)
        {
            List<string> history = new List<string>();            

            string filePath = GenerateHistoryFilePath(id);

            bool status = CheckBeforeReading(filePath);

            if (status == true)
            {
                using (StreamReader streamReader = new StreamReader
                (filePath, Encoding.Unicode))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string streamString = $"{streamReader.ReadLine()}";
                        history.Add(streamString);
                    }
                }
            }

            return history;
        }

        public static List<string> OwerwriteOrWriteClient(Client client)
        {
            string changedFields = String.Empty;
            string operation = "создание записи";            

            bool status = CheckBeforeReading(ClientFilePath);

            if (status == true)
            {
                if (CurrentUser.GetType() == typeof(Consultant))
                {
                    changedFields = (CurrentUser as Consultant).OwerwriteClient(client);
                }

                if (CurrentUser.GetType() == typeof(Manager))
                {
                    changedFields = (CurrentUser as Manager).OwerwriteClient(client);
                }

                if (changedFields == "Идентификатор не найден")
                {
                    (CurrentUser as Manager).WriteClient(client);
                }
                else
                {
                    operation = "изменение записи";
                }
            }
            else
            {
                (CurrentUser as Manager).WriteClient(client);                
            }           

            return WriteHistory(client.ClientId, changedFields, operation);
        }

        public static List<ClientListItem> GetСlientListItems()
        {
            List<ClientListItem> list = new List<ClientListItem>();

            bool status = Repository.CheckBeforeReading(ClientFilePath);

            if (status == true)
            {
                List<Client> clients = Repository.GetAllClientsFromFile();               

                foreach (Client client in clients)
                {
                    ClientListItem clientListItem = new ClientListItem()
                    {
                        ClientID = client.ClientId,
                        ClientName = client.ClientName
                    };

                    list.Add(clientListItem);
                }
            }

            return list;
        }

        public static void DeleteClient()
        {
            if (CurrentUser.GetType() == typeof(Manager))
            {
                (CurrentUser as Manager).DeleteClient();
            }
        }
    }
}
