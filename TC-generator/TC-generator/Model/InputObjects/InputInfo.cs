using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public enum ConnectType 
    { 
        V,
        W,
        Y,
        Z
    }

    public class InputInfo
    {
        public int StudyCount { get; set; }
        public int ColdFlowCount { get; set; }
        public int HotFlowCount { get; set; }

        public List<int> CF_Tn { get; set; }
        public List<int> CF_Tk { get; set; }

        public List<int> HF_Tn { get; set; }
        public List<int> HF_Tk { get; set; }

        public ConnectType[,] Type { get; set; }
        public int[,] ConnectArray { get; set; }


        public InputInfo()
        {
            CF_Tn = new List<int>();
            CF_Tk = new List<int>();

            HF_Tn = new List<int>();
            HF_Tk = new List<int>();
        }

        public void StartInitial()
        {
            Type = new ConnectType[StudyCount * HotFlowCount, StudyCount * ColdFlowCount];
            ConnectArray = new int[StudyCount * HotFlowCount, StudyCount * ColdFlowCount];
        }

       

        public List<(int HotNumber, int HotStudy, int ColdNumber, int ColdStudy, ConnectType ConnectVariant)> GetBranches()
        {
            var s = new List<(int, int, int, int, ConnectType)>();

            for (int i = 0; i < (StudyCount * HotFlowCount); i++)
            {
                for (int j = 0; j < (StudyCount * ColdFlowCount); j++)
                {
                    if (ConnectArray[i, j] == 1)
                    {
                        int u_1 = i / StudyCount;
                        int u_2 = j / StudyCount;

                        int HS;
                        int HN;
                        if (u_1 == 0)
                        {
                            HN = 1;
                            HS = i + 1;
                        }
                        else
                        {
                            HN = u_1 + 1;
                            HS = i - u_1 * StudyCount + 2;
                        }

                        int CS;
                        int CN;
                        if (u_2 == 0)
                        {
                            CN = 1;
                            CS = j + 1;
                        }
                        else
                        {
                            CN = u_2 + 1;
                            CS = j - u_2 * StudyCount + 2;
                        }

                        s.Add((HN, HS, CN, CS, Type[i, j]));
                        break;
                    }
                }
            }

            return s;
        }

    }
}
