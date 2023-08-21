using Dapper;
using API.Cover.Entities;
using API.Cover.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using MySql.Data.MySqlClient;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Cover.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách nhân viên và phân trang
        /// </summary>
        /// <param name="search"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Employee>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees([FromQuery] string? search, [FromQuery] Guid? positionId, [FromQuery] Guid? departmentId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh tên Stored procedure
                string storedProcName = "Proc_Employee_GetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$skip", (pageNumber - 1) * pageSize);
                parameters.Add("@$Take", pageSize);
                parameters.Add("@$Sort", "modifiedDate DESC");

                string whereClause = "";
                if (search != null || positionId != null || departmentId != null)
                {
                    var whereConditions = new List<string>();
                    string searchClause = "";
                    if (search != null)
                    {
                        var searchConditions = new List<string>();

                        searchConditions.Add($"EmployeeCode LIKE \'%{search}%\'");

                        searchConditions.Add($"EmployeeName LIKE \'%{search}%\'");

                        searchConditions.Add($"PhoneNumber LIKE \'%{search}%\'");

                        searchClause = "(" + string.Join(" OR ", searchConditions) + ")";

                        whereConditions.Add(searchClause);
                    }

                    if (positionId != null)
                    {
                        whereConditions.Add($"PositionID = '{positionId}'");
                    }

                    if (departmentId != null)
                    {
                        whereConditions.Add($"DepartmentID = '{departmentId}'");
                    }
                    whereClause = "WHERE " + string.Join(" AND ", whereConditions);
                }

                parameters.Add("@$Where", whereClause);
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipResult = mySqlConnection.QueryMultiple(storedProcName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (multipResult != null)
                {
                    var employee = multipResult.Read<Employee>();
                    var totalCount = multipResult.Read<long>().Single();
                    return StatusCode(200, new PagingData<Employee>()
                    {
                        Data = employee.ToList(),
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
                //return StatusCode(200, whereClause);
            }
            catch (Exception)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// API thêm nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                //create connect
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Command insert employee
                string insertCommand = "INSERT INTO employees (EmployeeID, EmployeeCode, EmployeeName, Gender, DateOfBirth, IdentityNumber, IdentityIssuedPlace, IdentityIssuedDate, Email, PhoneNumber, PositionID, DepartmentID, TaxCode, Salary, JoiningDate, WorkStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                            "VALUES(@EmployeeID, @EmployeeCode, @EmployeeName, @Gender, @DateOfBirth, @IdentityNumber, @IdentityIssuedPlace, @IdentityIssuedDate, @Email, @PhoneNumber, @PositionID, @DepartmentID, @TaxCode, @Salary, @JoiningDate, @WorkStatus, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); ";
                //input parameters command insert
                var dateTimeNow = DateTime.Now;
                var EmployeeId = Guid.NewGuid();
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeID", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@CreatedDate", dateTimeNow);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);
                //Make a call to the DB to run the insert statement with the above input parameter
                var affectedRows = mySqlConnection.Execute(insertCommand, parameters);

                //Process the results returned from the DB
                if (affectedRows > 0)
                {
                    return StatusCode(201, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(400, "e003");
                }
                return StatusCode(400, "e001");
            }
            catch (Exception)
            {
                return StatusCode(400, "e001");
                throw;
            }
        }
        
        /// <summary>
        /// API sửa nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateEmployee([FromRoute] Guid EmployeeId, [FromBody] Employee employee)
        {
            try
            {
                // kết nối tới DB
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // chuẩn bị câu lệnh update
                string updateCommand = "update employees " +
                                        "SET EmployeeCode = @EmployeeCode, " +
                                        "EmployeeName = @EmployeeName, " +
                                        "Gender = @Gender, " +
                                        "DateOfBirth = @DateOfBirth, " +
                                        "IdentityNumber = @IdentityNumber, " +
                                        "IdentityIssuedPlace = @IdentityIssuedPlace, " +
                                        "IdentityIssuedDate = @IdentityIssuedDate, " +
                                        "Email = @Email, " +
                                        "PhoneNumber = @PhoneNumber, " +
                                        "PositionID = @PositionID, " +
                                        "DepartmentID = @DepartmentID, " +
                                        "TaxCode = @TaxCode, " +
                                        "Salary = @Salary, " +
                                        "JoiningDate = @JoiningDate, " +
                                        "WorkStatus = @WorkStatus, " +
                                        "ModifiedDate = @ModifiedDate, " +
                                        "ModifiedBy = @ModifiedBy " +
                                        "WHERE EmployeeID = @EmployeeID;";
                // chuẩn bị tham số đầu vào cho lệnh update
                var dateTimeNow = DateTime.Now;
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeID", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // gọi db với tham số đầu vào
                int affectedRows = mySqlConnection.Execute(updateCommand, parameters);

                // trả về kết quả
                if (affectedRows > 0)
                {
                    return StatusCode(200, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
            }
            catch (MySqlException mySqlEx)
            {
                // nếu như employeeCode bị trùng lặp
                if (mySqlEx.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(400, "e003");
                }
                return StatusCode(400, "e001");
            }
            catch (Exception)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// API xóa nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpDelete("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteEmployee([FromRoute] Guid EmployeeId)
        {
            try
            {
                //ket noi den DB
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //chuan bi cau lenh delete
                string deleteCommand = "delete from employees where EmployeeID = @EmployeeId";
                //chuan bi tham so cho lenh delete
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", EmployeeId);
                // gọi DB và chạy câu lệnh delete
                int affectedRows = mySqlConnection.Execute(deleteCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(200, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
            }
            catch (MySqlException)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// API lấy nhân viên theo ID
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpGet("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeById([FromRoute] Guid EmployeeId)
        {
            try
            {
                //tạo kết nối 
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);
                //chuẩn bị câu lệnh store procedures
                string storeProcedureName = "Proc_Employee_GetByEmployeeID";
                //chuẩn bị tham số cho procedures
                var parameters = new DynamicParameters();
                parameters.Add("@$EmployeeID", EmployeeId);
                //Gọi vào DB chạy procedures
                var employee = mySqlConnection.QueryFirstOrDefault(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                //Trả kết quả
                if (employee != null)
                {
                    return StatusCode(200, employee);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch(Exception)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// API lấy mã code lớn nhất
        /// </summary>
        /// <returns></returns>
        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(NewEmployeeCode))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                //tạo kết nối
                string connectionString = "Server=localhost;Port=3306;Database=hust.21h.2022.ltvu;Uid=root;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);
                //câu lệnh store procedures
                string callFunctionCommand = "Func_Get_Auto_EmployeeCode";
                //Gọi tới DB để truy vấn câu lệnh
                string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(callFunctionCommand, commandType: System.Data.CommandType.StoredProcedure);
                //Kết quả trả về
                return StatusCode(200, new NewEmployeeCode
                {
                    EmployeeCode = newEmployeeCode
                });
            }
            catch(Exception)
            {
                return StatusCode(400, "e001");
            }
        }
    }
}
