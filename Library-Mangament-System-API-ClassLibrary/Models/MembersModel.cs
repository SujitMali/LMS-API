using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class MembersModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int GenderId { get; set; }
        public int LocationId { get; set; }
        public int? DepartmentId { get; set; }
        public int CurrentStudyYear { get; set; }
        public int UserTypeId { get; set; }
        public int NoOfIssues { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int ModifiedBy { get; set; } = 2;
        public bool? IsActive { get; set; } = true;
    }
}
