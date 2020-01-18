using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TC_generator.Model.ConnectionObjects;
using TC_generator.Model.InputObjects;
using TC_generator.Model.Objects;

namespace TC_generator.Model.BuilderObjects
{


    public class Director
    {
        public InputInfo input;
        public ItemsControl Canvasss;
        public bool IsMoved;
        public int k = 0;
        public int Y = 120;
        public int XforHF = 190;

        public Rectangle CurrentPlaceRectangel;
        public Line CurrentBranchLine;
        public Label CurrentNameOfBranchLabel;
        public Label CurrentHeatOfBranchLabel;
        public Label CurrentAreaOfBranchLabel;
        public Ellipse CurrentHotEllipse;
        public Ellipse CurrentColdEllipse;

        public Line UC_0;
        public Line UC_1;
        public Line UC_2;

        public Director(InputInfo input, ItemsControl Canvasss)
        {
            this.input = input;
            this.Canvasss = Canvasss;   
        }

        public void StartDraw()
        {
           
            int XforCF = 190 + 300 * input.StudyCount;

            List<EnergyFlowBase> flows = new List<EnergyFlowBase>();

            for (int i = 0; i < input.HotFlowCount; i++)
            {
                flows.Add(new HotFlow(input.StudyCount, new Point(XforHF, Y), input.HF_Tn[i], input.HF_Tk[i]));
                Y += 100;
            }

            for (int i = 0; i < input.ColdFlowCount; i++)
            {
                flows.Add(new ColdFlow(input.StudyCount, new Point(XforCF, Y), input.CF_Tn[i], input.CF_Tk[i]));
                Y += 100;
            }

            Canvasss.Height = Y + 120;
            Canvasss.Width = 300 * input.StudyCount + 2 * XforHF;

            Draw(flows);
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
                        Margin = new Thickness(flow.IdTextPoint[i].X, flow.IdTextPoint[i].Y - 10, 0, 0),
                        Content = (i + 1).ToString(),
                        Uid = "Study_label" + k.ToString()

                    };

                    k++;

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

                switch (branch.ConnectVariant)
                {
                    case ConnectType.V1:

                        if(Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2) + 60;
                            point.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Hot);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {

                                utility.UtilityLines[i].Uid = i.ToString() + "LineH" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1-18, utility.UtilityLines[1].Y1, 0, 0),
                                Content = input.Q_CoolerArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Name = "V1H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 20, point.Y -23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Fill = Brushes.Transparent;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Blue;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);


                            Canvasss.Items.Add(kv);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);
                        }

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                           
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 60;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                string str = i.ToString() + "LineC" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                utility.UtilityLines[i].Uid = str;
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1-25, 0, 0),
                                Content = input.Q_HeaterArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Name = "V1C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 10, point.Y-23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Fill = Brushes.Transparent;
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Red;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);

                            Canvasss.Items.Add(kv);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);

                        }

                        break;

                    case ConnectType.V2:


                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            Line line = (new BranchConnection(point1, point2)).BranchConnectionLine;
                            line.Uid = "Connect" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = Math.Abs(line.Y1 - line.Y2);
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Name = "rc" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(line.X1 - 10, line.Y1, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(Line_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(Line_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(Line_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(Line_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(Line_MouseButtonLeave);

                            Ellipse ELL_H = new Ellipse();
                            ELL_H.Uid = "ELL_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_H.Fill = Brushes.Black;
                            ELL_H.Margin = new Thickness(line.X1 - 2.5, line.Y1 - 2.5, 0, 0);
                            ELL_H.Width = 5;
                            ELL_H.Height = 5;

                            Ellipse ELL_C = new Ellipse();
                            ELL_C.Uid = "ELL_C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_C.Fill = Brushes.Black;
                            ELL_C.Margin = new Thickness(line.X2 - 2.5, line.Y2 - 2.5, 0, 0);
                            ELL_C.Width = 5;
                            ELL_C.Height = 5;

                            Label E = new Label
                            {
                                Margin = new Thickness(point1.X, point1.Y, 0, 0),
                                Uid = "E" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = "E " + (branch.HotNumber + 1).ToString() + (branch.HotStudy + 1).ToString() + (branch.ColdNumber + 1).ToString() + (branch.ColdStudy + 1).ToString()
                            };

                            Label Q = new Label
                            {
                                Margin = new Thickness(point1.X - 20, point1.Y - 25, 0, 0),
                                Uid = "Q" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.Q_RecuperatorArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Label F = new Label
                            {
                                Margin = new Thickness(point1.X - 25, point2.Y, 0, 0),
                                Uid = "F" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.F_RecuperatorArray[branch.i, branch.j].ToString() + " м2"
                            };

                            Canvasss.Items.Add(line);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(E);
                            Canvasss.Items.Add(Q);
                            Canvasss.Items.Add(F);
                            Canvasss.Items.Add(ELL_H);
                            Canvasss.Items.Add(ELL_C);

                        }

                        if (Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2) + 60;
                            point.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Hot);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                utility.UtilityLines[i].Uid = i.ToString() + "LineH" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1, 0, 0),
                                Content = input.Q_CoolerArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Uid = "V2H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 10, point.Y - 23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Fill = Brushes.Transparent;
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Blue;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);


                            Canvasss.Items.Add(kv);

                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);

                        }

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 60;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                utility.UtilityLines[i].Uid = i.ToString() + "LineC" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1 - 25, 0, 0),
                                Content = input.Q_HeaterArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Uid = "V2C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 10, point.Y - 23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Fill = Brushes.Transparent;
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Red;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);

                            Canvasss.Items.Add(kv);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);

                        }

                        break;

                    case ConnectType.V3:

                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            Line line = (new BranchConnection(point1, point2)).BranchConnectionLine;
                            line.Uid = "Connect" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = Math.Abs(line.Y1 - line.Y2);
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Name = "rc" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(line.X1 - 10, line.Y1, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(Line_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(Line_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(Line_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(Line_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(Line_MouseButtonLeave);

                            Ellipse ELL_H = new Ellipse();
                            ELL_H.Uid = "ELL_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_H.Fill = Brushes.Black;
                            ELL_H.Margin = new Thickness(line.X1 - 2.5, line.Y1 - 2.5, 0, 0);
                            ELL_H.Width = 5;
                            ELL_H.Height = 5;

                            Ellipse ELL_C = new Ellipse();
                            ELL_C.Uid = "ELL_C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_C.Fill = Brushes.Black;
                            ELL_C.Margin = new Thickness(line.X2 - 2.5, line.Y2 - 2.5, 0, 0);
                            ELL_C.Width = 5;
                            ELL_C.Height = 5;

                            Label E = new Label
                            {
                                Margin = new Thickness(point1.X, point1.Y, 0, 0),
                                Uid = "E" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = "E " + (branch.HotNumber + 1).ToString() + (branch.HotStudy + 1).ToString() + (branch.ColdNumber + 1).ToString() + (branch.ColdStudy + 1).ToString()
                            };

                            Label Q = new Label
                            {
                                Margin = new Thickness(point1.X - 20, point1.Y - 25, 0, 0),
                                Uid = "Q" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.Q_RecuperatorArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Label F = new Label
                            {
                                Margin = new Thickness(point1.X - 25, point2.Y, 0, 0),
                                Uid = "F" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.F_RecuperatorArray[branch.i, branch.j].ToString() + " м2"
                            };

                            Canvasss.Items.Add(line);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(E);
                            Canvasss.Items.Add(Q);
                            Canvasss.Items.Add(F);
                            Canvasss.Items.Add(ELL_H);
                            Canvasss.Items.Add(ELL_C);

                        }

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2) - 60;
                            point.Y = CF.Single().Lines[branch.ColdStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Cold);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                utility.UtilityLines[i].Uid = i.ToString() + "LineC" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1 - 25, 0, 0),
                                Content = input.Q_HeaterArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Uid = "V3C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 10, point.Y - 23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Fill = Brushes.Transparent;
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Red;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);

                            Canvasss.Items.Add(kv);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);

                        }

                        break;

                    case ConnectType.V4:

                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                        {
                            Point point1 = new Point();
                            point1.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2);
                            point1.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            Point point2 = new Point();
                            point2.X = ((CF.Single().Lines[branch.ColdStudy].X1 + CF.Single().Lines[branch.ColdStudy].X2) / 2);
                            point2.Y = CF.Single().Lines[branch.ColdStudy].Y1;


                            Line line = (new BranchConnection(point1, point2)).BranchConnectionLine;
                            line.Uid = "Connect" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = Math.Abs(line.Y1 - line.Y2);
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Name = "rc" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(line.X1 - 10, line.Y1, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(Line_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(Line_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(Line_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(Line_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(Line_MouseButtonLeave);

                            Ellipse ELL_H = new Ellipse();
                            ELL_H.Uid = "ELL_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_H.Fill = Brushes.Black;
                            ELL_H.Margin = new Thickness(line.X1 - 2.5, line.Y1 - 2.5, 0, 0);
                            ELL_H.Width = 5;
                            ELL_H.Height = 5;

                            Ellipse ELL_C = new Ellipse();
                            ELL_C.Uid = "ELL_C" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            ELL_C.Fill = Brushes.Black;
                            ELL_C.Margin = new Thickness(line.X2 - 2.5, line.Y2 - 2.5, 0, 0);
                            ELL_C.Width = 5;
                            ELL_C.Height = 5;

                            Label E = new Label
                            {
                                Margin = new Thickness(point1.X, point1.Y, 0, 0),
                                Uid = "E" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = "E " + (branch.HotNumber + 1).ToString() + (branch.HotStudy + 1).ToString() + (branch.ColdNumber + 1).ToString() + (branch.ColdStudy + 1).ToString()
                            };

                            Label Q = new Label
                            {
                                Margin = new Thickness(point1.X - 20, point1.Y - 25, 0, 0),
                                Uid = "Q" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.Q_RecuperatorArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Label F = new Label
                            {
                                Margin = new Thickness(point1.X - 25, point2.Y, 0, 0),
                                Uid = "F" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Content = input.F_RecuperatorArray[branch.i, branch.j].ToString() + " м2"
                            };

                            Canvasss.Items.Add(line);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(E);
                            Canvasss.Items.Add(Q);
                            Canvasss.Items.Add(F);
                            Canvasss.Items.Add(ELL_H);
                            Canvasss.Items.Add(ELL_C);

                        }

                        if (Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                        {
                            Point point = new Point();
                            point.X = ((HF.Single().Lines[branch.HotStudy].X1 + HF.Single().Lines[branch.HotStudy].X2) / 2) + 60;
                            point.Y = HF.Single().Lines[branch.HotStudy].Y1;

                            UtilityConnection utility = new UtilityConnection(point, FlowType.Hot);

                            for (int i = 0; i < utility.UtilityLines.Length; i++)
                            {
                                utility.UtilityLines[i].Uid = i.ToString() + "LineH" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                                Canvasss.Items.Add(utility.UtilityLines[i]);
                            }  

                            Label Q = new Label
                            {
                                Uid = "Q_H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString(),
                                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1, 0, 0),
                                Content = input.Q_CoolerArray[branch.i, branch.j].ToString() + " кВт"
                            };

                            Rectangle rc = new Rectangle();
                            rc.Width = 20;
                            rc.Height = 46;
                            rc.Fill = Brushes.DeepPink;
                            rc.Opacity = 0;
                            rc.Uid = "V4H" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            rc.Margin = new Thickness(point.X - 10, point.Y - 23, 0, 0);
                            rc.Stroke = Brushes.Pink;
                            rc.MouseDown += new MouseButtonEventHandler(U_MouseButtonDown);
                            rc.MouseMove += new MouseEventHandler(U_MouseButtonMove);
                            rc.MouseUp += new MouseButtonEventHandler(U_MouseButtonUp);
                            rc.MouseEnter += new MouseEventHandler(U_MouseButtonEnter);
                            rc.MouseLeave += new MouseEventHandler(U_MouseButtonLeave);

                            Rectangle kv = new Rectangle();
                            kv.Width = 20;
                            kv.Height = 10;
                            kv.Uid = "kv_U" + branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
                            kv.Fill = Brushes.Transparent;
                            kv.Opacity = 1;
                            kv.Stroke = Brushes.Blue;
                            kv.Margin = new Thickness(point.X-10, point.Y-5, 0, 0);

                            Canvasss.Items.Add(kv);
                            Canvasss.Items.Add(rc);
                            Canvasss.Items.Add(Q);

                        }


                        break;
                }
            }
        }

        private void Line_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMoved = true;
                CurrentPlaceRectangel = (sender as Rectangle);

                string name = CurrentPlaceRectangel.Name;
                string IdOfElement = name.Substring(2);

                CurrentBranchLine = Tools.FindUidObjectFromItemsControlUI<Line>("Connect", IdOfElement, Canvasss);
                CurrentNameOfBranchLabel = Tools.FindUidObjectFromItemsControlUI<Label>("E", IdOfElement, Canvasss);
                CurrentHeatOfBranchLabel = Tools.FindUidObjectFromItemsControlUI<Label>("Q", IdOfElement, Canvasss);
                CurrentAreaOfBranchLabel = Tools.FindUidObjectFromItemsControlUI<Label>("F", IdOfElement, Canvasss);
                CurrentHotEllipse = Tools.FindUidObjectFromItemsControlUI<Ellipse>("ELL_H", IdOfElement, Canvasss);
                CurrentColdEllipse = Tools.FindUidObjectFromItemsControlUI<Ellipse>("ELL_C", IdOfElement, Canvasss);
            }
        }

        private void Line_MouseButtonMove(object sender, MouseEventArgs e)
        {
            if (IsMoved & e.LeftButton == MouseButtonState.Pressed)
            {
                CurrentPlaceRectangel.Opacity = 0.15;

                double X = e.GetPosition(Canvasss).X;

                CurrentPlaceRectangel.Margin = new Thickness(X - 10, CurrentPlaceRectangel.Margin.Top, 0, 0);
                CurrentNameOfBranchLabel.Margin = new Thickness(X, CurrentNameOfBranchLabel.Margin.Top, 0, 0);
                CurrentHeatOfBranchLabel.Margin = new Thickness(X - 18, CurrentHeatOfBranchLabel.Margin.Top, 0, 0);
                CurrentAreaOfBranchLabel.Margin = new Thickness(X - 18, CurrentAreaOfBranchLabel.Margin.Top, 0, 0);

                CurrentHotEllipse.Margin = new Thickness(X - 2.5, CurrentHotEllipse.Margin.Top, 0, 0);
                CurrentColdEllipse.Margin = new Thickness(X - 2.5, CurrentColdEllipse.Margin.Top, 0, 0);

                CurrentBranchLine.X1 = X;
                CurrentBranchLine.X2 = X;
            }
        }

        private void Line_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMoved = false;

            CurrentPlaceRectangel = null;
            CurrentBranchLine = null;
            CurrentNameOfBranchLabel = null;
            CurrentHeatOfBranchLabel = null;
            CurrentAreaOfBranchLabel = null;
            CurrentHotEllipse = null;
            CurrentHotEllipse = null;
        }

        private void Line_MouseButtonLeave(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = (sender as Rectangle);
            rectangle.Opacity = 0;

            string name = rectangle.Name;
            string IdOfElement = name.Substring(2);

            Rectangle hot_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("Hot_U", IdOfElement, Canvasss);
            Rectangle cold_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("Cold_U", IdOfElement, Canvasss);

            if (cold_rectangel != null)
               cold_rectangel.Opacity = 0;

            if (hot_rectangel != null)
               hot_rectangel.Opacity = 0;
        }

        private void Line_MouseButtonEnter(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = (sender as Rectangle);
            rectangle.Opacity = 0.15;

            string name = rectangle.Name;
            string IdOfElement = name.Substring(2);

            Rectangle hot_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("Hot_U", IdOfElement, Canvasss);
            Rectangle cold_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("Cold_U", IdOfElement, Canvasss);

            if (cold_rectangel != null)
                cold_rectangel.Opacity = 0.15;

            if (hot_rectangel != null)
                hot_rectangel.Opacity = 0.15;
        }

        /////////////////////////////////////////////////

        public Rectangle UtilityRectangel;
        public Line UtilityLine_1;
        public Line UtilityLine_2;
        public Line UtilityLine_3;
        public Label UtilityHeatLabel;
        public Rectangle UtilityDRectange;


        private void U_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMoved = true;
                UtilityRectangel = (sender as Rectangle);

                string name = UtilityRectangel.Name;
                string IdOfElement = name.Substring(3);
                string UtlityName = name.Substring(0, 3);

                switch (UtlityName)
                {
                    case "V1C":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "V1H":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "V2C":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "V2H":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "V3H":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "V4C":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;
                }

            }
        }

        private void U_MouseButtonMove(object sender, MouseEventArgs e)
        {
            if (IsMoved & e.LeftButton == MouseButtonState.Pressed)
            {
                UtilityRectangel.Opacity = 0.15;
                double X = e.GetPosition(Canvasss).X;

                UtilityRectangel.Margin = new Thickness(X - 10, UtilityRectangel.Margin.Top, 0, 0);
                UtilityHeatLabel.Margin = new Thickness(X - 18, UtilityHeatLabel.Margin.Top, 0, 0);
                UtilityDRectange.Margin = new Thickness(X - 10, UtilityDRectange.Margin.Top, 0, 0);

                UtilityLine_1.X1 = X;
                UtilityLine_1.X2 = X;

                UtilityLine_2.X1 = X;
                UtilityLine_2.X2 = X;

                UtilityLine_3.X1 = X;
                UtilityLine_3.X2 = X;

            }
        }

        private void U_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMoved = false;

            UtilityRectangel = null;
            UtilityLine_1 = null;
            UtilityLine_2 = null;
            UtilityLine_3 = null;
            UtilityHeatLabel = null;
            UtilityDRectange = null;
    }

        private void U_MouseButtonLeave(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Opacity = 0;
        }

        private void U_MouseButtonEnter(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Opacity = 0.15;
        }

        private void FindUtilityObjects(string UtlityName, string IdOfElement)
        {
            UtilityLine_1 = Tools.FindUidObjectFromItemsControlUI<Line>("1LineC", IdOfElement, Canvasss);
            UtilityLine_2 = Tools.FindUidObjectFromItemsControlUI<Line>("2LineC", IdOfElement, Canvasss);
            UtilityLine_3 = Tools.FindUidObjectFromItemsControlUI<Line>("3LineC", IdOfElement, Canvasss);
            UtilityHeatLabel = Tools.FindUidObjectFromItemsControlUI<Label>("Q_H", IdOfElement, Canvasss);
            UtilityDRectange = Tools.FindUidObjectFromItemsControlUI<Rectangle>("kv_U", IdOfElement, Canvasss);
        }

    }
}
