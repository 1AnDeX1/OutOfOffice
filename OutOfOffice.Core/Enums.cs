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
            None,
            IT,
            Sales,
            Marketing,
            Finance
        }

        public enum Position
        {
            Employee,
            HRManager,
            ProjectManager,
            Administrator
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
