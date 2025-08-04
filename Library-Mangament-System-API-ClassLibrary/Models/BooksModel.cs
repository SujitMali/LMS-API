using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class BooksModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Can't be longer than 200 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]*$", ErrorMessage = "Only alphanumeric characters & spaces are allowed.")]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Number of Pages is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Pages must be a positive number.")]
        [Display(Name = "No. of Pages")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0.")]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Quantity must be at least 1.")]
        [Display(Name = "Total Quantity")]
        public int TotalQuantity { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Available Quantity must be 0 or more.")]
        [Display(Name = "Available Quantity")]
        public int AvailableQuantity { get; set; }

        [Display(Name = "Language")]
        public string LanguageName { get; set; }

        [Display(Name = "Publisher Name")]
        public string PublisherName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int ModifiedBy { get; set; } = 2;
        public bool? IsActive { get; set; } = true;

        public List<BooksModel> Books { get; set; }
        public List<PublishersModel> Publishers { get; set; }
        public List<LanguagesModel> Languages { get; set; }


        //public List<bool> PublisherIds { get; set; }
        public string PublisherIds { get; set; }

        public int PublisherId { get; set; } = 0;
        public int LanguageId { get; set; } = 0;
        //public int? LanguageId { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public string SearchBookName { get; set; }

        //-----------------------------------------------------------------
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        //-----------------------------------------------------------------


        public static int SearchPublisherId { get; set; }
        public static int SearchLangaugeId { get; set; }
        public static int SearchPageSize { get; set; }
        public static int SearchIsActive { get; set; }



        //---------------------------------------------------------------

        public List<MembersModel> MembersList { get; set; }
        public int? SelectedMemberId { get; set; } = 2;
        public string SelectedBookIds { get; set; }
        public int IssueId { get; set; }
        public List<BookIssueDetailViewModel> IssuedBookDetails { get; set; }
        public BookIssueModel IssuesList { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Quantity { get; set; }
        public List<BookIssueFileDocumentModel> fileListForBookIssueId { get; set; }
        //---------------------------------------------------------------

        [Required(ErrorMessage = "Please upload at least one file.")]
        [FileExtensions(Extensions = "jpg,jpeg,png,pdf,doc,docx", ErrorMessage = "Only image/doc files are allowed.")]
        public List<HttpPostedFileBase> UploadFiles { get; set; }

    }
}
