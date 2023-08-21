using System.ComponentModel.DataAnnotations;

namespace API.Cover.Entities
{
    public class Department
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DepartmentID {get; set;}

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "")]
        public string? DepartmentCode { get; set;}

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "")]
        public string? DepartmentName { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string? CreatedBy { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDate { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string? ModifiedBy { get;set;}

    }
}
