﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TC_generator.Model.ConnectionObjects
{
    public class BranchConnection
    {
        public int IdBranchConnection { get; } = 0;
        public string Name { get; }

        public Point HFPoint { get; set; }
        public Point CFPoint { get; set; }

        public double Delta 
        { 
            get 
            { 
                return Math.Abs(HFPoint.Y - CFPoint.Y); 
            }
        }

        public Line BranchConnectionLine
        {
            get
            {
                return CreateBranchConnectionLine();
            }
        }

        public BranchConnection(Point HFPoint, Point CFPoint)
        {
            IdBranchConnection++;
            Name = "Branch Connection №" + IdBranchConnection.ToString();

            this.HFPoint = HFPoint;
            this.CFPoint = CFPoint;
        }

        private Line CreateBranchConnectionLine()
        {

            Line line = new Line()
            {
                X1 = HFPoint.X,
                Y1 = HFPoint.Y,
                X2 = HFPoint.X,
                Y2 = HFPoint.Y + Delta,

                Stroke = Brushes.Black
            };


            return line;

        }

    }
}
