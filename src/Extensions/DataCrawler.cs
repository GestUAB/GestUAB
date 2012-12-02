using System;
using System.IO;
using System.Net;
using Fizzler;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using GestUAB.Models;

namespace GestUAB
{
    public static class DataCrawler
    {
        //public string url;

       // public Craw ()
       // {
       //     url = "../../test_data/101752.html"; 
       // }

        public static List<Scholarship> CrawData(string htmlString)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);
            IEnumerable<HtmlNode> nodes = htmlDocument.DocumentNode.QuerySelectorAll(".tab_listagem3 tr");
            List<Scholarship> scholarships = new List<Scholarship>();
            foreach (var node in nodes)
            {
                HtmlNodeCollection row = node.ChildNodes;
                
                if (row.Count >= 11)
                {
                    //Caso esteja pegando a linha com o titulo das colunas nao incluir na lista
                    //Arrumar uma forma melhor para fazer isso
                    if(row[1].InnerText.Replace("\n", "").Trim() != "CPF")
                    {
                        string cpf      = row[1].InnerText.Replace("\n", "").Trim();
                        string name     = row[3].InnerText.Trim().Replace("\n", "");
                        string function = row[5].InnerText.Trim().Replace("\n", "");
                        string lot      = row[7].InnerText.Trim().Replace("\n", "");
                        string state    = row[9].InnerText.Trim().Replace("\n", "");
                        string value    = row[11].InnerText.Trim().Replace("\n", "");                        

                        scholarships.Add(new Scholarship {CPF       = cpf     ,
                            Name      = name    ,
                            Function  = function,
                            Lots      = lot     ,
                            State     = state   ,
                            Value     = value   });
                    }
                }
            }
            return scholarships;
        }


        //Usa um arquivo de teste
        public static string GetTestText (string filepath)
        {
            Console.WriteLine(filepath);
            if (!File.Exists(filepath))
            {
                Console.WriteLine("Caminho do arquivo nao encontrado!");
            }
            return File.ReadAllText(filepath);
        }

        //Usa uma url para fazer o webcraw
        public static string GetWebText (string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (url);
            request.UserAgent = "A .NET Web Crawler";
            WebResponse response = request.GetResponse ();
            Stream stream = response.GetResponseStream ();
            StreamReader reader = new StreamReader (stream);
            string htmlText = reader.ReadToEnd ();
            return htmlText;
        }
    }
}

