using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public enum ConnectType 
    { 
        V1,
        V2,
        V3,
        V4
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
        public int[,] IntTypeConnect { get; set; }
        public int[,] ConnectArray { get; set; }

        public double[,] Q_RecuperatorArray { get; set; }
        public double[,] Q_CoolerArray { get; set; }
        public double[,] Q_HeaterArray { get; set; }

        public double[,] F_RecuperatorArray { get; set; }
        public double[,] F_CoolerArray { get; set; }
        public double[,] F_HeaterArray { get; set; }



        public InputInfo()
        {
            CF_Tn = new List<int>();
            CF_Tk = new List<int>();

            HF_Tn = new List<int>();
            HF_Tk = new List<int>();
        }

        public void StartInitial()
        {
            int x = StudyCount * HotFlowCount;
            int y = StudyCount * ColdFlowCount;

            Type = new ConnectType[x, y];
            IntTypeConnect = new int[x, y];
            ConnectArray = new int[x, y];

            Q_RecuperatorArray = new double[x, y];
            Q_CoolerArray = new double[x, y];
            Q_HeaterArray = new double[x, y];

            F_RecuperatorArray = new double[x, y];
            F_CoolerArray = new double[x, y];
            F_HeaterArray = new double[x, y];
        }

       

        public List<(int HotNumber, int HotStudy, int ColdNumber, int ColdStudy, ConnectType ConnectVariant, int i , int j)> GetBranches()
        {
            var s = new List<(int, int, int, int, ConnectType, int i, int j)>();

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
                            HN = 0;
                            HS = i;
                        }
                        else
                        {
                            HN = u_1;
                            HS = i - u_1 * StudyCount;
                        }

                        int CS;
                        int CN;
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



                        s.Add((HN, HS, CN, CS, Type[i, j], i, j));
                        break;
                    }
                }
            }

            return s;
        }

    }
}
