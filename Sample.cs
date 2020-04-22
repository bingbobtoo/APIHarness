using System.Collections.Generic;
using System.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NewtonSoft
{
    public class Sample
    {
        static HttpClient client = new HttpClient();
        static Dictionary<String, Uri> linkHeadersRel = new Dictionary<string, Uri>();

        // https://briancaos.wordpress.com/2017/11/03/using-c-httpclient-from-sync-and-async-code/

        /// <summary>
        /// 
        /// </summary>
        public async void DoSomething()
        {
            linkHeadersRel.Add("first",new Uri("https://api.github.com/search/code?q=extension:css+user:octocat&page=1&per_page=2"));
            linkHeadersRel.Add("next", new Uri("https://api.github.com/search/code?q=extension:css+user:octocat&page=1&per_page=2"));

            var token = TOKEN
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            bool flag = true;
            string linkHeader = "";
            HttpResponseMessage response = null;
            while (flag == true)
            {
                response = await client.GetAsync(linkHeadersRel["next"]);

                // raise exception if failure
                if (!response.IsSuccessStatusCode) {
                    throw new System.ArgumentException("URI couldnt be reached");
                }

                var t = await response.Content.ReadAsStringAsync();
                linkHeader = ((string[])response.Headers.GetValues("Link"))[0];

                updateLinkHeadersRels(linkHeader);

                // Get JSON payload - and do what with it
                var y = JObject.Parse(t);
                string count = (string)y["total_count"];
                //Console.WriteLine(count);
                string name = (string)y.SelectToken("items[0].name");
                Console.WriteLine(name);
                //IEnumerable<JToken> items = y.SelectTokens("$.items[*].name");
                IEnumerable<JToken> items = y.SelectTokens("$.items[*].repository.id");

                foreach (JToken item in items)
                {
                    Console.WriteLine(item);
                    // Build data structure
                }
                // https://developer.github.com/v3/repos/contents/
                // https://raw.githubusercontent.com/octokit/octokit.rb/master/README.md

                if (linkHeadersRel["next"] == null) { flag = false; }
                
                // do counter for max loops 
                // do counter for loops based on total / page size round up.

                // Add to data structure
            } 

            // Write to File ??? 
            Console.ReadKey();
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file
            // https://www.pluralsight.com/guides/understand-control-flow-async-await
        }
        /// <summary>
        ///  Helper method to parse HTTP Link Header - first, prev, last, next
        ///  Format:
        ///  <url>; rel="next|last|prev|first"
        ///  Example: <https://api.github.com/search/code?q=extension%3Acss+user%3Aoctocat&page=2&per_page=1>; //rel=\"next\",  
        /// </summary>
        void updateLinkHeadersRels(string linkHeader) {

            string[] tmp = linkHeader.Split(',');
            string tmpKey = "";
            string tmpValue = "";
            string tmpKey01 = "";
            string tmpKey02 = "";
            string regEscape01 = @"rel=""";
            string regEscape02 = @"""";
            linkHeadersRel["prev"]  = null;
            linkHeadersRel["next"]  = null;
            linkHeadersRel["first"] = null;
            linkHeadersRel["last"]  = null;

            // Escape the input.
            foreach (string linkItem in tmp)
            {
                tmpKey = (linkItem.Split(";"))[1];
                tmpKey01 = Regex.Replace(tmpKey,   regEscape01, "").Trim();
                tmpKey02 = Regex.Replace(tmpKey01, regEscape02, "").Trim();

                tmpValue = ((linkItem.Split(";"))[0]).Trim().Replace("%3A", ":").Replace(">","").Replace("<","");
                
                if (tmpKey02.Equals("prev"))  { linkHeadersRel["prev"] = new Uri(tmpValue); }
                if (tmpKey02.Equals("next"))  { linkHeadersRel["next"] = new Uri(tmpValue); }
                if (tmpKey02.Equals("first")) { linkHeadersRel["first"]= new Uri(tmpValue); }
                if (tmpKey02.Equals("last"))  { linkHeadersRel["last"] = new Uri(tmpValue); }
            }                
        }

        public void WriteToFile() {
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
            {
                outputFile.WriteLine("Fourth Line");
            }
        }
    }
}


