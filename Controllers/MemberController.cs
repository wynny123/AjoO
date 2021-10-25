using AjoO.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AjoO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        string constr = "Data Source=.;Initial Catalog=AjoO;Integrated Security=True";
        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMember()
        {
            List<Member> members = new List<Member>();
            string query =  "SELECT * FROM Member";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                  await using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                           members.Add(new Member
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = Convert.ToString(sdr["Name"]),
                                Email = Convert.ToString(sdr["Email"]),
                                ContactNo = Convert.ToString(sdr["ContactNo"]),
                               Department = Convert.ToString(sdr["Department"]),
                                Address = Convert.ToString(sdr["Address"])
                               
                            });
                        }
                    }
                    con.Close();
                }
            }

            return members;
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(long id)
        {

            Member memberObj = new Member();
            string query = "SELECT * FROM Member where Id=" + id;
          await  using (SqlConnection con = new SqlConnection(constr))
            {
              await  using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            memberObj = new Member
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = Convert.ToString(sdr["Name"]),
                                Email = Convert.ToString(sdr["Email"]),
                                ContactNo = Convert.ToString(sdr["ContactNo"]),
                                Department = Convert.ToString(sdr["Department"]),
                                Address = Convert.ToString(sdr["Address"])
                               
                            };
                        }
                    }
                    con.Close();
                }
            }
            if (memberObj == null)
            {
                return NotFound();
            }
            return memberObj;
        }
        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Member(long id, Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }
            Member memberobj = new Member();
            if (ModelState.IsValid)
            {
                string query = "UPDATE Member SET Name = @Name, Email = @Email," +
                    "ContactNo=@ContactNo," +
                    " Department=@Department, Address=@Address Where Id =@Id";
             await   using (SqlConnection con = new SqlConnection(constr))
                {
                 await   using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Name", member.Name);
                        cmd.Parameters.AddWithValue("@Email", member.Email);
                        cmd.Parameters.AddWithValue("@ContactNo", member.ContactNo);
                        cmd.Parameters.AddWithValue("@Department", member.Department);
                        cmd.Parameters.AddWithValue("@Address", member.Address);
                        
                        cmd.Parameters.AddWithValue("@Id", member.Id);
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            return NoContent();
                        }
                        con.Close();
                    }
                }

            }
            return BadRequest(ModelState);
        }

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         await   using (SqlConnection con = new SqlConnection(constr))
            {
                //inserting Member data into database
                string query = "insert into Member values (@Name, @Email, @ContactNo,@Department,@Address)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Name", member.Name);
                    cmd.Parameters.AddWithValue("@Email", member.Email);
                    cmd.Parameters.AddWithValue("@ContactNo", member.ContactNo);
                    cmd.Parameters.AddWithValue("@Department", member.Department);
                    cmd.Parameters.AddWithValue("@Address", member.Address);
                    
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        return Ok();
                    }
                    con.Close();
                }
            }
            return BadRequest();

        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(long id)
        {

        await using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "Delete FROM Member where Id='" + id + "'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        return NoContent();
                    }
                    con.Close();
                }
            }
            return BadRequest();
        }

    }
}

