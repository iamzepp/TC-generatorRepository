using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TC_generator.Model.ConnectionObjects;
using TC_generator.Model.InputObjects;
using TC_generator.Model.Objects;

namespace TC_generator.Model.BuilderObjects
{
    public class Director
    {
        readonly InputInfo input;
        readonly ItemsControl Canvasss;

        public Director(InputInfo input, ItemsControl Canvasss)
        {
            this.input = input;
            this.Canvasss = Canvasss;   
        }

        public async void StartDrawAsync()
        {
            await Task.Run(() => StartDraw());
        }

        public void StartDraw()
        {
            int XforHF = 100;
            int XforCF = 100 + 100 * input.StudyCount;
            int Y = 100;

            List<EnergyFlowBase> flows = new List<EnergyFlowBase>();

            for (int i = 0; i < input.HotFlowCount; i++)
            {
                flows.Add(new HotFlow(input.StudyCount, new Point(XforHF, Y), input.HF_Tn[i], input.HF_Tk[i]));
                Y += 50;
            }

            for (int i = 0; i < input.ColdFlowCount; i++)
            {
                flows.Add(new ColdFlow(input.StudyCount, new Point(XforCF, Y), input.CF_Tn[i], input.CF_Tk[i]));
                Y += 50;
            }

            Canvasss.Height = Y + 100;
            Canvasss.Width = 100 * input.StudyCount + 2 * XforCF;

            Draw(flows);
            //DrawParallel(flows);
        }

        public void DrawParallel(List<EnergyFlowBase> flows)
        {
            Parallel.ForEach( flows, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, DrawP);
        }

        public void Draw(List<EnergyFlowBase> flows)
        {

            foreach (var flow in flows)
            {
                for (int i = 0; i < flow.Lines.Count; i++)
                {
                    Canvasss.Items.Add(flow.Lines[i]);
                }

                for (int i = 0; i < flow.ArrowLines.Length; i++)
                {
                    Canvasss.Items.Add(flow.ArrowLines[i]);
                }

                Label Tn_L1 = new Label
                {
                    Margin = new Thickness(flow.TnPoint.X, flow.TnPoint.Y, 0, 0),
                    Content = flow.Tn.ToString() + " C"
                };

                Canvasss.Items.Add(Tn_L1);

                Label Tk_L1 = new Label
                {
                    Margin = new Thickness(flow.TkPoint.X, flow.TkPoint.Y, 0, 0),
                    Content = flow.Tk.ToString() + " C"
                };
                Canvasss.Items.Add(Tk_L1);

                for (int i = 0; i < flow.IdTextPoint.Count; i++)
                {
                    Label p = new Label
                    {
                        Foreground = Brushes.DarkRed,
                        Margin = new Thickness(flow.IdTextPoint[i].X, flow.IdTextPoint[i].Y, 0, 0),
                        Content = (i + 1).ToString()
                    };

                    Canvasss.Items.Add(p);
                }

            }

           DrawConnect(flows);

        }

        private void DrawConnect(List<EnergyFlowBase> flows)
        {
            foreach(var branch in input.GetBranches())
            {
                var HF = (from flow in flows
                          where flow.Name == "H" + (branch.HotNumber + 1).ToString()
                          select flow).ToList();

                var CF = (from flow in flows
                          where flow.Name == "C" + (branch.ColdNumber + 1).ToString()
                          select flow).ToList();

                int[] n = new int[] { 10, 15, 20, 25, 30 };

                switch (branch.ConnectVariant)
                {
                    case ConnectType.V1:

                        if(CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2) + 10;
                            point.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Hot);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }
                            
                        }

                        if (CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 10;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                        }

                        break;

                    case ConnectType.V2:


                        if (CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            Canvasss.Items.Add((new BranchConnection(point1, point2)).BranchConnectionLine);
                        }

                        if (CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2) + 10;
                            point.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Hot);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                        }

                        if (CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 10;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                        }

                        break;

                    case ConnectType.V3:

                        if (CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            Canvasss.Items.Add((new BranchConnection(point1, point2)).BranchConnectionLine);
                        }

                        if (CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 10;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                        }

                        break;

                    case ConnectType.V4:

                        if (CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            Canvasss.Items.Add((new BranchConnection(point1, point2)).BranchConnectionLine);
                        }

                        if (CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 10;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                        }


                        break;
                }
            }
        }

        public bool CheckQandF( double F, double Q)
        {
            if (Q > 0.1 && F > 0.1)
                return true;
            else
                return false;
        }

        public void DrawP(EnergyFlowBase flow)
        {

            for (int i = 0; i < flow.Lines.Count; i++)
            {
                Canvasss.Items.Add(flow.Lines[i]);
            }

            for (int i = 0; i < flow.ArrowLines.Length; i++)
            {
                Canvasss.Items.Add(flow.ArrowLines[i]);
            }

            Label Tn_L1 = new Label
            {
                Margin = new Thickness(flow.TnPoint.X, flow.TnPoint.Y, 0, 0),
                Content = flow.Tn.ToString() + " C"
            };

            Canvasss.Items.Add(Tn_L1);

            Label Tk_L1 = new Label
            {
                Margin = new Thickness(flow.TkPoint.X, flow.TkPoint.Y, 0, 0),
                Content = flow.Tk.ToString() + " C"
            };
            Canvasss.Items.Add(Tk_L1);

            for (int i = 0; i < flow.IdTextPoint.Count; i++)
            {
                Label p = new Label
                {
                    Foreground = Brushes.DarkRed,
                    Margin = new Thickness(flow.IdTextPoint[i].X, flow.IdTextPoint[i].Y, 0, 0),
                    Content = i.ToString()
                };

                Canvasss.Items.Add(p);
            }

        }
    }
}
