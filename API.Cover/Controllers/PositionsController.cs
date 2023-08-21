using API.Cover.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Cover.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách vị trí
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type:typeof(Position))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]

        public IActionResult GetAllPosition()
        {
            try
            {
                //tao ket noi
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);
                //chuan bi cau lenh select
                string selectCommand = "Select * from positions";
                //Thuc hien goi DB va truy van cau lenh
                var positions = mySqlConnection.Query<Position>(selectCommand);
                //Tra ve ket qua
                return StatusCode(200, positions);

            }
            catch(Exception )
            {
                return StatusCode(400, "e001");
            }
        }
    }
}
