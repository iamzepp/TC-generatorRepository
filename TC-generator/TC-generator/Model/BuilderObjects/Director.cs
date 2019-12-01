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
            int X = 100;
            int Y = 100;

            List<EnergyFlowBase> flows = new List<EnergyFlowBase>();

            for (int i = 0; i < input.HotFlowCount; i++)
            {
                flows.Add(new HotFlow(input.StudyCount, new Point(X, Y), input.HF_Tn[i], input.HF_Tk[i]));
                Y += 50;
            }

            for (int i = 0; i < input.ColdFlowCount; i++)
            {
                flows.Add(new ColdFlow(input.StudyCount, new Point(X, Y), input.CF_Tn[i], input.CF_Tk[i]));
                Y += 50;
            }

            Canvasss.Height = Y + 100;
            Canvasss.Width = 100 * input.StudyCount + 2 * X;

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
                        Content = i.ToString()
                    };

                    Canvasss.Items.Add(p);
                }

            }

            DrawConnect(flows);

        }

        private void DrawConnect(List<EnergyFlowBase> flows)
        {
            var Branches = input.GetBranches();

            foreach(var branch in Branches)
            {
                switch (branch.ConnectVariant)
                {
                    case ConnectType.V:
                        Point point1 = new Point();
                        Point point2 = new Point();

                        Canvasss.Items.Add((new BranchConnection(point1, point2)).BranchConnectionLine);
                        break;

                    case ConnectType.W:
                        Point point3 = new Point();
                        Point point4 = new Point();

                        Canvasss.Items.Add((new BranchConnection(point3, point4)).BranchConnectionLine);
                        break;

                    case ConnectType.Y:
                        Point point5 = new Point();
                        Point point6 = new Point();

                        Canvasss.Items.Add((new BranchConnection(point5, point6)).BranchConnectionLine);
                        break;

                    case ConnectType.Z:
                        Point point7 = new Point();
                        Point point8 = new Point();

                        Canvasss.Items.Add((new BranchConnection(point7, point8)).BranchConnectionLine);
                        break;
                }         
            }
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
