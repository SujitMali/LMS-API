using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Mangament_System_API_ClassLibrary.Models
{
    public class UsersModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, ErrorMessage = "Phone number should not exceed 10 digits")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Joining Date is required")]
        public DateTime JoiningDate { get; set; }

        public DateTime? LeavingDate { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public int LocationId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        [Required(ErrorMessage = "Active status is required")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "User Type is required")]
        public int UserTypeId { get; set; }

        public string UserTypeName { get; set; }

        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        public List<GendersModel> GendersList { get; set; }
        public List<UserTypesModel> UserTypesList { get; set; }
        public List<LocationsModel> LocationsList { get; set; }
    }
}
