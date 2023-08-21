using System.ComponentModel.DataAnnotations;

namespace API.Cover.Entities
{
    public class Position
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid PositionID{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "")]
        public string? PositionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "")]
        public string? PositionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
