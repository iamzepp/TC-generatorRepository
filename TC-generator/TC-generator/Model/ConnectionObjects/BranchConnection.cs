using System;
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

        public Point InstallationPoint { get; set; }

        public ArrayList BranchConnectionLines
        {
            get
            {
                return CreateBranchConnectionLines();
            }
        }

        public BranchConnection()
        {
            IdBranchConnection++;
            Name = "Branch Connection №" + IdBranchConnection.ToString();

            this.InstallationPoint = InstallationPoint;
        }

        private ArrayList CreateBranchConnectionLines()
        {
            ArrayList list = new ArrayList();

            Line line1 = new Line()
            {

                Stroke = Brushes.Black
            };


            return list;

        }

    }
}
