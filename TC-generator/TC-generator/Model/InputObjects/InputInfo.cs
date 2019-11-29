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
            CF_Tn = new List<int>();
            CF_Tk = new List<int>();

            HF_Tn = new List<int>();
            HF_Tk = new List<int>();
        }

    }
}
