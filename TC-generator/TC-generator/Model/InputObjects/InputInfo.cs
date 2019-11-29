using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public class InputInfo
    {
        public int StudyCount { get; set; }
        public int ColdFlowCount { get; set; }
        public int HotFlowCount { get; set; }

        public List<int> CF_Tn { get; set; }
        public List<int> CF_Tk { get; set; }

        public List<int> HF_Tn { get; set; }
        public List<int> HF_Tk { get; set; }

        

        public InputInfo()
        {
           
            CreateDefault();
        }

  

        public void CreateDefault()
        {
            StudyCount = 14;
            ColdFlowCount = 100;
            HotFlowCount = 100;

            CF_Tn = new List<int>();
            CF_Tk = new List<int>();

            HF_Tn = new List<int>();
            HF_Tk = new List<int>();

            Random random = new Random();

            for (int i = 0; i < ColdFlowCount; i++)
            {
                CF_Tk.Add(random.Next(200, 250));
                CF_Tn.Add(random.Next(100, 150));
            }

            for (int i = 0; i < HotFlowCount; i++)
            {
                HF_Tk.Add(random.Next(300, 350));
                HF_Tn.Add(random.Next(150, 250));
            }
        }

    }
}
