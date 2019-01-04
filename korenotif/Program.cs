using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace korenotif
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Este é o KoreNotif desenvolvido por Edilson Pereira");
            inicio:
            Console.WriteLine("Neste momento efetuaremos a leitura dos arquivos de configuração");
            try
            {
                string[] linesConf = System.IO.File.ReadAllLines(@"conf.txt");
                string[] linesConfTelegram = System.IO.File.ReadAllLines(@"confTelegram.txt");
                List<String> sNotificado = new List<string>();
                //dados do conf
                Console.WriteLine("Lendo conf.txt");
                Console.WriteLine("Nome do mercador: " + linesConf[3]);
                Console.WriteLine("Local do log de venda: " + linesConf[1]);
                Console.WriteLine("Se estiver incorreto verificar conf.txt");
                Console.WriteLine("Local do log de chat: " + linesConf[5]);
                //dados do confTelegram
                Console.WriteLine("Lendo confTelegram.txt");
                Console.WriteLine("api_token: " + linesConfTelegram[1]);
                Console.WriteLine("chat_id: " + linesConfTelegram[3]);
                Console.WriteLine("Se estiver incorreto verificar confTelegram.txt");
                string urlString2 = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
                string apiToken = linesConfTelegram[1];
                string chatId = linesConfTelegram[3];


                //limpeza do arquivos de log
                Console.WriteLine("Agora iremos limpar os arquivos de log");
                System.IO.File.WriteAllText(@"" + linesConf[1], "");
                System.IO.File.WriteAllText(@"" + linesConf[5], "");
                Console.WriteLine("Arquivo de logs limpo..");
                //inicio do processamento
                Console.WriteLine("Agora iremos monitor esse arquivo e assim que algum item for vendido você sera notificado no telegram!!");
                while (true)
                {
                    string[] linesLogShop = System.IO.File.ReadAllLines(@"" + linesConf[1]);

                    foreach (string s in linesLogShop)
                    {
                        if (!sNotificado.Contains(s))
                        {
                            Console.WriteLine(s);
                            sendMsg(urlString2,apiToken,chatId,linesConf[3] + ": "+s);
                            sNotificado.Add(s);
                        }
                    }

                    string[] linesLogChat = System.IO.File.ReadAllLines(@"" + linesConf[5]);

                    foreach (string s in linesLogChat)
                    {
                        if (!sNotificado.Contains(s))
                        {
                            if (s.Contains("[PM]"))
                            {
                                Console.WriteLine(s);
                                sendMsg(urlString2, apiToken, chatId, linesConf[3] + ": " + s);
                                sNotificado.Add(s);
                            }
                            
                        }
                    }



                    Thread.Sleep(5000);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("esperaremos 60 segundos e tentaremos novemente...");
                Thread.Sleep(60000);
                goto inicio;
            }

            Console.ReadKey();
        }

        private static void sendMsg(string urlString2, string apiToken, string chatId, string s)
        {
            string urlString = String.Format(urlString2, apiToken, chatId, s);
            WebRequest request = WebRequest.Create(urlString);
            Console.WriteLine("Enviando dados");
            Stream rs = request.GetResponse().GetResponseStream();
            urlString = String.Format("");
            request.Abort();
        }


    }
}
