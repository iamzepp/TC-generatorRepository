using System;
using System.Collections.Generic;
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
    public class ChartBilder : IChartBilder
    {
        public InputInfo input;
        public ItemsControl Canvasss;

        public bool IsMoved;
        public bool IsMovedUtility;

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


        public Rectangle CurrentUtilityRectangel;
        public Line CurrentUtilityLine_1;
        public Line CurrentUtilityLine_2;
        public Line CurrentUtilityLine_3;
        public Label CurrentUtilityHeatLabel;
        public Rectangle CurrentUtilityDRectange;

        List<EnergyFlowBase> flows = new List<EnergyFlowBase>();

        public ChartBilder(InputInfo input, ItemsControl Canvasss)
        {
            this.input = input;
            this.Canvasss = Canvasss;
        }

        public void Bild()
        {
            InitialFlows();
            DrawFlows();
            DrawConnects();
        }

        public void InitialFlows()
        {

            int XforCF = 190 + 300 * input.StudyCount;

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

        }

        public void DrawFlows()
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
        }

        private void DrawConnects()
        {
            foreach (var branch in input.GetBranches())
            {

                var HF = (flows.Where(flow => flow.Name == "H" + (branch.HotNumber + 1).ToString())).ToList().Single();
                var CF = (flows.Where(flow => flow.Name == "C" + (branch.ColdNumber + 1).ToString())).ToList().Single();

                switch (branch.ConnectVariant)
                {
                    case ConnectType.V1:

                        if (Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Hot);

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Cold);

                        break;

                    case ConnectType.V2:

                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                            CreateBranchElements(branch, HF, CF);

                        if (Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Hot);

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Cold);

                        break;

                    case ConnectType.V3:

                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                            CreateBranchElements(branch, HF, CF);

                        if (Tools.CheckQandF(input.F_HeaterArray[branch.i, branch.j], input.Q_HeaterArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Cold);

                        break;

                    case ConnectType.V4:

                        if (Tools.CheckQandF(input.F_RecuperatorArray[branch.i, branch.j], input.Q_RecuperatorArray[branch.i, branch.j]))
                            CreateBranchElements(branch, HF, CF);

                        if (Tools.CheckQandF(input.F_CoolerArray[branch.i, branch.j], input.Q_CoolerArray[branch.i, branch.j]))
                            CreateUtilityElements(branch, HF, CF, FlowType.Hot);

                        break;
                }
            }
        }

        private void RectangelBranch_MouseButtonDown(object sender, MouseButtonEventArgs e)
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

        private void RectangelBranch_MouseButtonMove(object sender, MouseEventArgs e)
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

        private void RectangelBranch_MouseButtonUp(object sender, MouseButtonEventArgs e)
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

        private void RectangelBranch_MouseButtonLeave(object sender, MouseEventArgs e)
        {
            if (!IsMovedUtility)
            {
                Rectangle rectangle = (sender as Rectangle);
                rectangle.Opacity = 0;

                string name = rectangle.Name;
                string IdOfElement = name.Substring(2);

                Rectangle hot_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("UrcH", IdOfElement, Canvasss);
                Rectangle cold_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("UrcC", IdOfElement, Canvasss);

                if (cold_rectangel != null)
                    cold_rectangel.Opacity = 0;

                if (hot_rectangel != null)
                    hot_rectangel.Opacity = 0;
            }
        }

        private void RectangelBranch_MouseButtonEnter(object sender, MouseEventArgs e)
        {
            if (!IsMovedUtility)
            {
                Rectangle rectangle = (sender as Rectangle);
                rectangle.Opacity = 0.15;

                string name = rectangle.Name;
                string IdOfElement = name.Substring(2);

                Rectangle hot_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("UrcH", IdOfElement, Canvasss);
                Rectangle cold_rectangel = Tools.FindUidObjectFromItemsControlUI<Rectangle>("UrcC", IdOfElement, Canvasss);

                if (cold_rectangel != null)
                    cold_rectangel.Opacity = 0.15;

                if (hot_rectangel != null)
                    hot_rectangel.Opacity = 0.15;
            }
        }

        private void RectangelUtility_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMovedUtility = true;
                CurrentUtilityRectangel = (sender as Rectangle);

                string name = CurrentUtilityRectangel.Name;
                string IdOfElement = name.Substring(4);
                string UtlityName = name[3].ToString();

                switch (UtlityName)
                {
                    case "C":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;

                    case "H":
                        FindUtilityObjects(UtlityName, IdOfElement);
                        break;
                }

            }
        }

        private void RectangelUtility_MouseButtonMove(object sender, MouseEventArgs e)
        {
            if (IsMovedUtility & e.LeftButton == MouseButtonState.Pressed)
            {
                CurrentUtilityRectangel.Opacity = 0.15;
                double X = e.GetPosition(Canvasss).X;

                CurrentUtilityRectangel.Margin = new Thickness(X - 10, CurrentUtilityRectangel.Margin.Top, 0, 0);
                CurrentUtilityHeatLabel.Margin = new Thickness(X - 18, CurrentUtilityHeatLabel.Margin.Top, 0, 0);
                CurrentUtilityDRectange.Margin = new Thickness(X - 10, CurrentUtilityDRectange.Margin.Top, 0, 0);

                CurrentUtilityLine_1.X1 = X;
                CurrentUtilityLine_1.X2 = X;



                CurrentUtilityLine_2.X1 = X;
                CurrentUtilityLine_2.X2 = X - 5;

                CurrentUtilityLine_3.X1 = X;
                CurrentUtilityLine_3.X2 = X + 5;

            }
        }

        private void RectangelUtility_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMovedUtility = false;

            CurrentUtilityRectangel = null;
            CurrentUtilityLine_1 = null;
            CurrentUtilityLine_2 = null;
            CurrentUtilityLine_3 = null;
            CurrentUtilityHeatLabel = null;
            CurrentUtilityDRectange = null;
        }

        private void RectangelUtility_MouseButtonLeave(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Opacity = 0;
        }

        private void RectangelUtility_MouseButtonEnter(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Opacity = 0.15;
        }

        private void FindUtilityObjects(string UtlityName, string IdOfElement)
        {
            CurrentUtilityLine_1 = Tools.FindUidObjectFromItemsControlUI<Line>("0Line" + UtlityName, IdOfElement, Canvasss);
            CurrentUtilityLine_2 = Tools.FindUidObjectFromItemsControlUI<Line>("1Line" + UtlityName, IdOfElement, Canvasss);
            CurrentUtilityLine_3 = Tools.FindUidObjectFromItemsControlUI<Line>("2Line" + UtlityName, IdOfElement, Canvasss);
            CurrentUtilityHeatLabel = Tools.FindUidObjectFromItemsControlUI<Label>("Q_U" + UtlityName, IdOfElement, Canvasss);
            CurrentUtilityDRectange = Tools.FindUidObjectFromItemsControlUI<Rectangle>("kv_U" + UtlityName, IdOfElement, Canvasss);
        }

        private List<UIElement> CreateBranchElements((int HotNumber, int HotStudy, int ColdNumber, int ColdStudy, ConnectType ConnectVariant, int i, int j) branch, EnergyFlowBase HF, EnergyFlowBase CF)
        {
            //Коллекция возвращаемых UI объектов
            List<UIElement> uIElements = new List<UIElement>();

            //Id элемента
            string ElementId = branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();

            //Точка на горячем потоке
            Point point1 = new Point();
            point1.X = ((HF.Lines[branch.HotStudy].X1 + HF.Lines[branch.HotStudy].X2) / 2);
            point1.Y = HF.Lines[branch.HotStudy].Y1;

            //Точка на холодном потоке
            Point point2 = new Point();
            point2.X = ((CF.Lines[branch.ColdStudy].X1 + CF.Lines[branch.ColdStudy].X2) / 2);
            point2.Y = CF.Lines[branch.ColdStudy].Y1;

            //Линия связи из point1 и point2
            Line line = (new BranchConnection(point1, point2)).BranchConnectionLine;
            line.Uid = "Connect" + ElementId;

            //Область выделения линии
            Rectangle rc = new Rectangle();
            rc.Width = Tools.RectangelBranchWidth;
            rc.Height = Math.Abs(line.Y1 - line.Y2);
            rc.Fill = Tools.RectangelBranchColor;
            rc.Opacity = 0;
            rc.Name = "rc" + ElementId;
            rc.Margin = new Thickness(line.X1 - (Tools.RectangelBranchWidth - 10), line.Y1, 0, 0);
            rc.MouseDown += new MouseButtonEventHandler(RectangelBranch_MouseButtonDown);
            rc.MouseMove += new MouseEventHandler(RectangelBranch_MouseButtonMove);
            rc.MouseUp += new MouseButtonEventHandler(RectangelBranch_MouseButtonUp);
            rc.MouseEnter += new MouseEventHandler(RectangelBranch_MouseButtonEnter);
            rc.MouseLeave += new MouseEventHandler(RectangelBranch_MouseButtonLeave);

            Ellipse ELL_H = new Ellipse();
            ELL_H.Uid = "ELL_H" + ElementId;
            ELL_H.Fill = Brushes.Black;
            ELL_H.Margin = new Thickness(line.X1 - 2.5, line.Y1 - 2.5, 0, 0);
            ELL_H.Width = 5;
            ELL_H.Height = 5;

            Ellipse ELL_C = new Ellipse();
            ELL_C.Uid = "ELL_C" + ElementId;
            ELL_C.Fill = Brushes.Black;
            ELL_C.Margin = new Thickness(line.X2 - 2.5, line.Y2 - 2.5, 0, 0);
            ELL_C.Width = 5;
            ELL_C.Height = 5;

            Label E = new Label
            {
                Margin = new Thickness(point1.X, point1.Y, 0, 0),
                Uid = "E" + ElementId,
                Content = "E " + (branch.HotNumber + 1).ToString() + (branch.HotStudy + 1).ToString() + (branch.ColdNumber + 1).ToString() + (branch.ColdStudy + 1).ToString()
            };

            Label Q = new Label
            {
                Margin = new Thickness(point1.X - 20, point1.Y - 25, 0, 0),
                Uid = "Q" + ElementId,
                Content = input.Q_RecuperatorArray[branch.i, branch.j].ToString() + " " + Tools.UnitOfQ
            };

            Label F = new Label
            {
                Margin = new Thickness(point1.X - 25, point2.Y, 0, 0),
                Uid = "F" + ElementId,
                Content = (Math.Round(input.F_RecuperatorArray[branch.i, branch.j], Tools.Round)).ToString() + " " + Tools.UnitOfF
            };

            Canvasss.Items.Add(line);
            Canvasss.Items.Add(rc);
            Canvasss.Items.Add(E);
            Canvasss.Items.Add(Q);
            Canvasss.Items.Add(F);
            Canvasss.Items.Add(ELL_H);
            Canvasss.Items.Add(ELL_C);


            return uIElements;
        }

        private List<UIElement> CreateUtilityElements((int HotNumber, int HotStudy, int ColdNumber, int ColdStudy, ConnectType ConnectVariant, int i, int j) branch, EnergyFlowBase HF, EnergyFlowBase CF, FlowType type)
        {
            //Коллекция возвращаемых UI объектов
            List<UIElement> uIElements = new List<UIElement>();

            //Id элемента
            string ElementId = branch.HotNumber.ToString() + branch.ColdNumber.ToString() + branch.HotStudy.ToString() + branch.ColdStudy.ToString();
            string TypeString = default;
            double A = default;
            string Q_content = default;
            SolidColorBrush solidColor = default;

            Point point = new Point();
            if (type == FlowType.Hot)
            {
                point.X = ((HF.Lines[branch.HotStudy].X1 + HF.Lines[branch.HotStudy].X2) / 2) + 60;
                point.Y = HF.Lines[branch.HotStudy].Y1;
                TypeString = "H";
                A = 0;
                Q_content = input.Q_CoolerArray[branch.i, branch.j].ToString();
                solidColor = Brushes.Blue;
            }
            else
            {
                if (branch.ConnectVariant != ConnectType.V2 && branch.ConnectVariant != ConnectType.V3)
                {
                    point.X = ((CF.Lines[branch.ColdStudy].X1 + CF.Lines[branch.ColdStudy].X2) / 2) - 60;
                    point.Y = CF.Lines[branch.ColdStudy].Y1;
                }
                else
                {
                    point.X = ((HF.Lines[branch.HotStudy].X1 + HF.Lines[branch.HotStudy].X2) / 2) - 60;
                    point.Y = CF.Lines[branch.ColdStudy].Y1;
                }

                TypeString = "C";
                A = 25;
                Q_content = input.Q_HeaterArray[branch.i, branch.j].ToString();
                solidColor = Brushes.Red;
            }

            UtilityConnection utility = new UtilityConnection(point, type);


            for (int i = 0; i < utility.UtilityLines.Length; i++)
            {
                Line line = utility.UtilityLines[i];
                line.Uid = i.ToString() + "Line" + TypeString + ElementId;
                Canvasss.Items.Add(line);
            }

            Label Q = new Label
            {
                Uid = "Q_U" + TypeString + ElementId,
                Margin = new Thickness(utility.UtilityLines[1].X1 - 18, utility.UtilityLines[1].Y1 - A, 0, 0),
                Content = Q_content + " " + Tools.UnitOfQ
            };

            Rectangle rc = new Rectangle();
            rc.Width = 20;
            rc.Height = 46;
            rc.Fill = Tools.RectangelBranchColor;
            rc.Opacity = 0;
            rc.Name = "Urc" + TypeString + ElementId;
            rc.Uid = "Urc" + TypeString + ElementId;
            rc.Margin = new Thickness(point.X - 10, point.Y - 23, 0, 0);
            rc.MouseDown += new MouseButtonEventHandler(RectangelUtility_MouseButtonDown);
            rc.MouseMove += new MouseEventHandler(RectangelUtility_MouseButtonMove);
            rc.MouseUp += new MouseButtonEventHandler(RectangelUtility_MouseButtonUp);
            rc.MouseEnter += new MouseEventHandler(RectangelUtility_MouseButtonEnter);
            rc.MouseLeave += new MouseEventHandler(RectangelUtility_MouseButtonLeave);

            Rectangle kv = new Rectangle();
            kv.Width = 20;
            kv.Height = 10;
            kv.Fill = Brushes.Transparent;
            kv.Uid = "kv_U" + TypeString + ElementId;
            kv.Opacity = 1;
            kv.Stroke = solidColor;
            kv.Margin = new Thickness(point.X - 10, point.Y - 5, 0, 0);


            Canvasss.Items.Add(kv);
            Canvasss.Items.Add(rc);
            Canvasss.Items.Add(Q);

            return uIElements;
        }

    }
}
