using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class BookIssueModel
    {
        public int IssueId { get; set; }
        public string MemberName { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsActive { get; set; }
        public int? MemberId { get; set; }
        public DateTime IssueDate { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int ModifiedBy { get; set; } = 2;
        public DateTime? ModifiedOn { get; set; }


        public List<BookIssueDetailViewModel> BookDetails { get; set; }
        public List<BookIssueFileDocumentModel> fileListForBookIssueId { get; set; }

        [Required(ErrorMessage = "Please upload at least one file.")]
        public List<HttpPostedFileBase> UploadFiles { get; set; }
        public List<BookIssueModel> issues { get; set; }


        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalRecords { get; set; }
        public int? SelectedMemberId { get; set; } = 2;




    }

    public class BookIssueDetailViewModel
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
    }
}











//public string SelectedBookIds { get; set; }
//public BookIssueModel Issue { get; set; }
//public int Quantity { get; set; }
//public List<BookIssueDetailViewModel> IssuedBookDetails { get; set; }
//public BookIssueModel IssuesList { get; set; }
//public List<MembersModel> MembersList { get; set; }
