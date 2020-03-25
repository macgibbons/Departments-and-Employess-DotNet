using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using DepartmentEmployeesDotNet.Models;
using Microsoft.AspNetCore.Http;

namespace DepartmentEmployeesDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // Get all Departments from the database
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, DeptName FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Department> departments = new List<Department>();

                    while (reader.Read())
                    {
                        Department department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            DeptName = reader.GetString(reader.GetOrdinal("DeptName")),
                        };

                        departments.Add(department);
                    }
                    reader.Close();

                    return Ok(departments);
                }
            }
        }

    }
}




   

    //    // Get a single coffee from Id
    //    [HttpGet("{id}", Name = "GetCoffee")]
    //    public async Task<IActionResult> Get([FromRoute] int id)
    //    {
    //        using (SqlConnection conn = Connection)
    //        {
    //            conn.Open();
    //            using (SqlCommand cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = @"
    //                    SELECT
    //                        Id, Title, BeanType
    //                    FROM Coffee
    //                    WHERE Id = @id";
    //                cmd.Parameters.Add(new SqlParameter("@id", id));
    //                SqlDataReader reader = cmd.ExecuteReader();

    //                Coffee coffee = null;

    //                if (reader.Read())
    //                {
    //                    coffee = new Coffee
    //                    {
    //                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
    //                        Title = reader.GetString(reader.GetOrdinal("Title")),
    //                        BeanType = reader.GetString(reader.GetOrdinal("BeanType"))
    //                    };
    //                    reader.Close();

    //                    return Ok(coffee);
    //                }
    //                else
    //                {
    //                    return NotFound();
    //                }
    //            }
    //        }
    //    }

    //    private bool CoffeeExists(int id)
    //    {
    //        using (SqlConnection conn = Connection)
    //        {
    //            conn.Open();
    //            using (SqlCommand cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = @"
    //                    SELECT Id, Title, BeanType
    //                    FROM Coffee
    //                    WHERE Id = @id";
    //                cmd.Parameters.Add(new SqlParameter("@id", id));

    //                SqlDataReader reader = cmd.ExecuteReader();
    //                return reader.Read();
    //            }
    //        }
    //    }
    //}