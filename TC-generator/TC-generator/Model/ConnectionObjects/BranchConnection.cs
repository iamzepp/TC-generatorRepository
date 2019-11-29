using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TC_generator.Model.ConnectionObjects
{
    public class BranchConnection
    {
        public int IdBranchConnection { get; } = 0;
        public string Name { get; }

        public Point InstallationPoint { get; set; }

        public BranchConnection(Point InstallationPoint)
        {
            IdBranchConnection++;
            Name = "Branch Connection №" + IdBranchConnection.ToString();

            this.InstallationPoint = InstallationPoint;
        }

    }
}
