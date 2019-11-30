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

        public List<string> GetBranches()
        {
            List<string> s = new List<string>();

            for (int i = 0; i < (StudyCount * HotFlowCount); i++)
            {
                for (int j = 0; j < (StudyCount * ColdFlowCount); j++)
                {
                    if (ConnectArray[i, j] == 1)
                    {
                        int HN = 0;
                        int HS = 0;

                        int CN = 0;
                        int CS = 0;

                        int u_1 = i / StudyCount;
                        int u_2 = j / StudyCount;

                        if (u_1 == 0)
                        {
                            HN = 0;
                            HS = i;
                        }
                        else
                        {
                            HN = u_1;
                            HS = i - u_1 * StudyCount;
                        }

                        if (u_2 == 0)
                        {
                            CN = 0;
                            CS = j;
                        }
                        else
                        {
                            CN = u_2;
                            CS = j - u_2 * StudyCount;
                        }

                        s.Add("H" + HN.ToString() + "S" + HS.ToString() + " cоединяется " + "C" + CN.ToString() + "S" + CS.ToString() + " --- тип " + Type[i, j].ToString());
                        break;
                    }
                }
            }

            return s;
        }

    }
}
