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

        public enum ActivityStatus
        {
            Active,
            Inactive
        }

        public enum RequestStatus
        {
            New,
            Approve,
            Reject
        }
        public enum AbsenceReason
        {
            Illness,
            FamilyReason
        }

        public enum ProjectType
        {
            Internal,
            External,
            Research,
            Development,
            Maintenance
        }
    }
}
