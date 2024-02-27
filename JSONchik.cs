using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Prakticheskaya_2
{
    internal class JSONchik
    {
        public static void JsonSerialize<T>(T notebook)
        {
            string PathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Notebooks.json";
            string json = JsonConvert.SerializeObject(notebook);
            File.WriteAllText(PathToDesktop, json);
        }
        public static T JsonDeserialize<T>()
        {
            string PathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Notebooks.json";
            string json = File.ReadAllText(PathToDesktop);
            T notebookdeser = JsonConvert.DeserializeObject<T>(json);
            return notebookdeser;
        }
    }
}
