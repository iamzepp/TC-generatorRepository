using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public class DefaultInput : IInput
    {
        public InputInfo GetInfo()
        {
            Random random = new Random();

            InputInfo input = new InputInfo();

            input.StudyCount = 14;
            input.ColdFlowCount = 100;
            input.HotFlowCount = 100;

            for (int i = 0; i < input.ColdFlowCount; i++)
            {
                input.CF_Tk.Add(random.Next(200, 250));
                input.CF_Tn.Add(random.Next(100, 150));
            }

            for (int i = 0; i < input.HotFlowCount; i++)
            {
                input.HF_Tk.Add(random.Next(300, 350));
                input.HF_Tn.Add(random.Next(150, 250));
            }

            return input;
        }
    }
}
