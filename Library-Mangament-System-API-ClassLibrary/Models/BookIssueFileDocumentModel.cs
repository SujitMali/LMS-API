using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class BookIssueFileDocumentModel
    {
        public int BookIssueFileDocumentId { get; set; }
        public int BookIssueId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int ModifiedBy { get; set; } = 2;
        public bool? IsActive { get; set; } = true;

    }
}
