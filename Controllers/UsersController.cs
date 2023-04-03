using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using testapi.Data;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Azure;
using Microsoft.AspNetCore.JsonPatch;


namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApiDbContext dbContext;
        public UsersController(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await dbContext.Users.ToListAsync());
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(string login, string email, string password)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.login == login || u.email == email);
            if (existingUser != null)
            {
                return BadRequest("User with the same login or email already exists");
            }
            var user = new Users()
            {
                login = login,
                email = email,
                password = password,
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string login, string password)
        {
            var deluser = await dbContext.Users.FirstOrDefaultAsync(u => u.login == login && u.password == password);
            if (deluser == null)
            {
                return NotFound();
            }

            // Remove user from database
            dbContext.Users.Remove(deluser);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("CheckUser")]
        public async Task <IActionResult> CheckUser(string login, string password)
        {
            var checkuser = await dbContext.Users.FirstOrDefaultAsync(u => u.login == login && u.password == password);
            if (checkuser == null)
            {
                return NotFound();
            }
            else
            {
               return Ok(checkuser);
            }
        }

    }
}
