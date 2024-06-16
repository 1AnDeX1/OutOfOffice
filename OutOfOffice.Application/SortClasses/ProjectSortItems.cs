using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OutOfOffice.Core.Enums;

namespace OutOfOffice.Application.SortClasses
{
    public class ProjectSortItems
    {
        public int? ID { get; set; }
        public ProjectType? ProjectType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProjectManagerId { get; set; }
        public ActivityStatus? Status { get; set; }
    }

}
