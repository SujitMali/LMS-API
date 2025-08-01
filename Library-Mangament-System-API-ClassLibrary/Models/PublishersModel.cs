using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class PublishersModel
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string ContactNumber { get; set; }
        public string LanguageName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
