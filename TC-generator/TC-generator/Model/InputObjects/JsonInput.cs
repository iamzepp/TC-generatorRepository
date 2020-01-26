using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public class JsonInput : IInput
    {
        public InputInfo GetInfo(string name)
        {
            string json = File.ReadAllText("document.json");

            return JsonConvert.DeserializeObject<InputInfo>(json); ;
        }
    }
}
