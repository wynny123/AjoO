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
    public class AccountController : ControllerBase
    {
        string constr = "Data Source=.;Initial Catalog=AjoO;Integrated Security=True";
        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccount()
        {
            List<Account> account = new List<Account>();
            string query = "SELECT * FROM Account";
         await using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    await using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            account.Add(new Account
                            {
                                Date = Convert.ToDateTime (sdr["Date"]),
                                Id = Convert.ToInt32(sdr["Id"]),
                                Contribution = Convert.ToInt32(sdr["Contribution"]),
                                Loan = Convert.ToInt32(sdr["Loan"]),
                                Balance = Convert.ToInt32(sdr["Balance"]),
                                MemberId = Convert.ToInt32(sdr["MemberId"])

                            });
                        }
                    }
                    con.Close();
                }
            }

            return account;
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(long id)
        {

            Account accountObj = new Account();
            string query = "SELECT * FROM Account where Id=" + id;
            await using (SqlConnection con = new SqlConnection(constr))
            {
                await using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            accountObj = new Account
                            {
                                Date = Convert.ToDateTime(sdr["Date"]),
                                Id = Convert.ToInt32(sdr["Id"]),
                                Contribution = Convert.ToInt32(sdr["Contribution"]),
                                Loan = Convert.ToInt32(sdr["Loan"]),
                                Balance = Convert.ToInt32(sdr["Balance"]),
                                MemberId = Convert.ToInt32(sdr["MemberId"])
                            };
                        }
                    }
                    con.Close();
                }
            }
            if (accountObj == null)
            {
                return NotFound();
            }
            return accountObj;
        }
        // PUT: api/Account/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(long id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }
            Account accountobj = new Account();
            if (ModelState.IsValid)
            {
                string query = "UPDATE Account SET Date = @Date, Contribution = @Contribution," +
                    "ContactNo=@ContactNo," +
                    "Loan=@Loan, Balance=@Balance, MemberId=@MemberId Where Id =@Id";
                await using (SqlConnection con = new SqlConnection(constr))
                {
                    await using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Date", account.Date);
                        cmd.Parameters.AddWithValue("@Contribution", account.Contribution);
                        cmd.Parameters.AddWithValue("@Loan", account.Loan);
                        cmd.Parameters.AddWithValue("@Balance", account.Balance);
                        cmd.Parameters.AddWithValue("@MemberId", account.MemberId);
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

        // POST: api/Account
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await using (SqlConnection con = new SqlConnection(constr))
            {
                //inserting account data into database
                string query = "insert into Account values (@Date, @Contribution, @Loan, @Balance, @MemberId)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Date", account.Date);
                    cmd.Parameters.AddWithValue("@Contribution", account.Contribution);
                    cmd.Parameters.AddWithValue("@Loan", account.Loan);
                    cmd.Parameters.AddWithValue("@Balance", account.Balance);
                    cmd.Parameters.AddWithValue("@MemberId", account.MemberId);

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

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {

            await using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "Delete FROM Account where Id='" + id + "'";
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

