using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Core
{
    public class Enums
    {
        public enum Subdivision
        {
            HR,
            IT,
            Sales,
            Marketing,
            Finance
        }

        public enum Position
        {
            Manager,
            Developer,
            Analyst,
            Consultant,
            Intern
        }

        public enum Status
        {
            Active,
            Inactive,
            New
        }
    }
}
