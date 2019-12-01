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
            ConnectType[] T = new ConnectType[4] {ConnectType.V,
                                                  ConnectType.W,
                                                  ConnectType.Y,
                                                  ConnectType.Z};

            input.StudyCount = 30;
            input.ColdFlowCount = 1000;
            input.HotFlowCount = 1000;

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

            input.StartInitial();

            //input.ConnectArray = new int[,] { { 0,1,0,0,0,0,0,0 },
            //                                  { 0,0,1,0,0,0,0,0 },
            //                                  { 1,0,0,0,0,0,0,0 },
            //                                  { 0,0,0,1,0,0,0,0 },
            //                                  { 0,0,0,0,1,0,0,0 },
            //                                  { 0,0,0,0,0,1,0,0 },
            //                                  { 0,0,0,0,0,0,0,1 },
            //                                  { 0,0,0,0,0,0,1,0 }};

            
            for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
            {
                List<int> tab = new List<int>();

                while (true)
                {
                    int j = random.Next(0, input.StudyCount * input.ColdFlowCount);

                    if (!tab.Contains(j))
                    {
                        input.ConnectArray[i, j] = 1;
                        tab.Add(j);
                    }
                    else
                    {
                        break;
                    }
                }    
            }
            

            for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
            {
                for (int j = 0; j < (input.StudyCount * input.ColdFlowCount); j++)
                {
                    input.Type[i, j] = T[random.Next(0, 4)];
                }
            }

            return input;
        }

    }
  
}
