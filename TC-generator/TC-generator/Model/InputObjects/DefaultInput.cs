using System;
using System.Collections.Generic;
using System.IO;
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

            input.StudyCount = 4;
            input.ColdFlowCount = 4;
            input.HotFlowCount = 4;

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

            input.ConnectArray = new int[,]
             { //1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //1
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,}, //2
                {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //3
                {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //4
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,}, //5
                {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //6
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,}, //7
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //8
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,}, //9
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,}, //10
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,}, //11
                {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,}, //12
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,}, //13
                {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //14
                {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //15
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,}  //16
                };

            input.IntTypeConnect = new int[,]
             { //1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16
                {3, 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //1
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,}, //2
                {3, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //3
                {4, 4, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //4
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,}, //5
                {4, 4, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //6
                {4, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,}, //7
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //8
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,}, //9
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,}, //10
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,}, //11
                {4, 4, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,}, //12
                {3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,}, //13
                {4, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //14
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,}, //15
                {4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,}  //16
                };

            //Для случаного заполнения
            //List<int> tab = new List<int>();
            //for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
            //{
            //    Link:

            //    int j = random.Next(0, input.StudyCount * input.ColdFlowCount);

            //    if (!tab.Contains(j))
            //    {
            //        input.ConnectArray[i, j] = 1;
            //        tab.Add(j);
            //    }
            //    else
            //    {
            //        goto Link;
            //    }
            //}

            //Записывает лог
            using (StreamWriter w = new StreamWriter("LOG.txt", false, Encoding.GetEncoding(1251)))
            {
                w.WriteLine("{");
                for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
                {
                    w.Write("{");
                    for (int j = 0; j < (input.StudyCount * input.ColdFlowCount); j++)
                    {
                        w.Write(input.ConnectArray[i, j].ToString() + ",");

                    }
                    w.Write("},");
                    w.WriteLine();
                }
                w.Write("};");
            }

            //Для случайного заполнения
            //for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
            //{
            //    for (int j = 0; j < (input.StudyCount * input.ColdFlowCount); j++)
            //    {
            //        input.Type[i, j] = T[random.Next(0, 4)];
            //    }
            //}

            for (int i = 0; i < (input.StudyCount * input.HotFlowCount); i++)
            {
                for (int j = 0; j < (input.StudyCount * input.ColdFlowCount); j++)
                {
                    switch(input.IntTypeConnect[i,j])
                    {
                        case 1:
                            input.Type[i, j] = ConnectType.V1;
                            break;
                        case 2:
                            input.Type[i, j] = ConnectType.V2;
                            break;
                        case 3:
                            input.Type[i, j] = ConnectType.V3;
                            break;
                        case 4:
                            input.Type[i, j] = ConnectType.V4;
                            break;
                    }
                }
            }

            return input;
        }

    }

}
