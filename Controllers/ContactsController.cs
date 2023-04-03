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
using System.Threading.Channels;
using System;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        public ContactsController(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetContacts")]
        public async Task<IActionResult> GetContacts()
        { 
            return Ok(await dbContext.Contacts.ToListAsync());
        }
        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact(string first_name, string last_name, string email,int phone_number)
        {
                var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(u => u.email == email || u.phone_number == phone_number);
                if (existingContact != null)
                {
                    return BadRequest("Contact with the same phone number or email already exists");
                }
            var contact = new Contacts()
            {
                first_name = first_name,
                last_name = last_name,
                email = email,
                phone_number = phone_number,
                };
                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
        [HttpPatch]
        [Route("EditContact/{contact_id}")]
        public async Task<IActionResult> EditContact(int contact_id, string first_name, string last_name, string email, int phone_number)
        {
            var contact = await dbContext.Contacts.FirstOrDefaultAsync(u => u.contact_id == contact_id);
            if (contact == null)
            {
                return NotFound();
            }
            contact.first_name = first_name;
            contact.last_name = last_name;
            contact.email = email;
            contact.phone_number = phone_number;
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }
        [HttpPost]
        [Route("DeleteContact")]
        public async Task<IActionResult> DeleteContact(string email, int phone_number)
        {
            var delcontact = await dbContext.Contacts.FirstOrDefaultAsync(u => u.email == email && u.phone_number == phone_number);
            if (delcontact == null)
            {
                return NotFound();
            }

            // Remove user from database
            dbContext.Contacts.Remove(delcontact);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
