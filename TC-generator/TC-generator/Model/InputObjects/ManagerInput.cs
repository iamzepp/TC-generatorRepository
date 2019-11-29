using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_generator.Model.InputObjects
{
    public class ManagerInput
    {
        private IInput input;

        public ManagerInput(IInput input)
        {
            this.input = input;
        }

        public InputInfo GetInput()
        {
            return input.GetInfo();
        }

        public void ChangeInputType(IInput input)
        {
            this.input = input;
        }

    }
}
