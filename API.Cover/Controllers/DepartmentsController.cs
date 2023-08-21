using Dapper;
using API.Cover.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Cover.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type:typeof(Department))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllDepartment()
        {
            try
            {
                //ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);
                //chuan bi cau lenh select
                string selectCommand = "Select * from departments";

                //Thuc hien goi vao DB va thuc hien truy van cau lenh
                var departments = mySqlConnection.Query<Department>(selectCommand);
                //tra ve ket qua
                return StatusCode(200, departments);
            }
            catch(Exception) 
            {
                return StatusCode(400, "e001");
            }
        }
    }
}
