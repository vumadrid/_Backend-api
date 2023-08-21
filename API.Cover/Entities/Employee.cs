//using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using API.Cover.Enum;
using System.ComponentModel.DataAnnotations;
namespace API.Cover.Entities
{
    public class Employee
    {
        
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage ="e004")]
        public string? EmployeeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string? EmployeeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfBirth { get; set;}
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "e006")]
        public string? IdentityNumber { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string? IdentityIssuedPlace { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public DateTime? IdentityIssuedDate { get; set;}
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "e007")]
        [EmailAddress(ErrorMessage = "e009")]
        public string? Email { get; set;}
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "e008")]
        public string? PhoneNumber { get;set;}
        /// <summary>
        /// 
        /// </summary>
        public Guid? PositionID { get;set;}
        /// <summary>
        /// 
        /// </summary>
        public Guid? DepartmentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? TaxCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Double Salary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? JoiningDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WorkStatus? WorkStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ModifiedBy { get; set; }

    }
}
