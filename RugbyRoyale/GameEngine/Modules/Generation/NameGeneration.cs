using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RugbyRoyale.GameEngine.Modules
{
    internal class NameGeneration
    {
        private static Dictionary<Nationality, string[]> nationalityCodes = new Dictionary<Nationality, string[]>()
        {
            { Nationality.American, new string[] { "usa" } },
            { Nationality.Argentine, new string[] { "spa" } },
            { Nationality.Australian, new string[] { "eng", "iri" } },
            { Nationality.Canadian, new string[] { "eng", "fre" } },
            { Nationality.English, new string[] { "eng" } },
            //{ Nationality.Fijian, new string[] {} },
            { Nationality.French, new string[] { "spa" } },
            { Nationality.Georgian, new string[] { "geo" } },
            { Nationality.Irish, new string[] { "iri" } },
            { Nationality.Italian, new string[] { "ita" } },
            { Nationality.Japanese, new string[] { "jap" } },
            { Nationality.NewZealander, new string[] { "eng", "sco", "mao" } },
            { Nationality.Romanian, new string[] { "rmn" } },
            { Nationality.Russian, new string[] { "rus" } },
            //{ Nationality.Samoan, new string[] {} },
            { Nationality.Scottish, new string[] { "sco" } },
            { Nationality.SouthAfrican, new string[] { "afk", "eng", "nde", "sot", "swz", "tsw", "xho", "zul" } },
            //{ Nationality.Tongan, new string[] {} },
            { Nationality.Uruguayan, new string[] { "spa" } },
            { Nationality.Welsh, new string[] { "wel" } },
        };

        public static async Task<Player> GenerateRandomName(Player player, Nationality nationality)
        {
            // TODO
            // Temporary code. Previously used API seems to be gone
            player.FirstName = "John";
            player.LastName = "Smith";
            return player;
            //

            string[] codes = nationalityCodes[nationality];
            string code;
            if (codes.Length == 1)
            {
                code = codes[0];
            }
            else if (codes.Length > 1)
            {
                code = codes[new Random().Next(0, codes.Length)];
            }
            else
            {
                code = "eng";
            }

            WebRequest request = WebRequest.Create($"https://www.behindthename.com/api/random.xml?gender=m&number=1&randomsurname=yes&usage={code}&key={ConfigurationManager.AppSettings["behindTheNameAPIKey"]}");
            WebResponse response = await request.GetResponseAsync();

            /*
                <response>
                  <names>
                    <name>Jaylen</name>
                    <name>Langley</name>
                  </names>
                </response>
             */

            var streamReader = new StreamReader(response.GetResponseStream());
            string result = await streamReader.ReadToEndAsync();

            var xmlResult = XElement.Parse(result);
            IEnumerable<XElement> names = xmlResult.Element("names").Elements("name");
            if (names.Count() == 2)
            {
                player.FirstName = names.First().Value;
                player.LastName = names.Last().Value;
            }
            else
            {
                // TODO: error
            }

            return player;
        }
    }
}