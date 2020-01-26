using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TC_generator.Model.SaveObjects
{
    public class SaveXML : ISave
    {
        public void Save(object obj)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(SaveInfo));

            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj as SaveInfo);
            }
        }
    }
}
