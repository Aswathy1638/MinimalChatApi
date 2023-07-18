using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalChatApplication.Data;
using System.ComponentModel.DataAnnotations;

namespace MinimalChatApplication.Controllers
{
           [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public UserController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpPost("register")]
            public IActionResult Register(User model)
            {
                // Validate the input
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the email is already registered
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    return Conflict(new { error = "Email is already registered." });
                }

                // Create a new user entity
                var user = new User
                {
                    Email = model.Email,
                    Name = model.Name,
                    Password = model.Password
                };

                // Add the user to the database
                _context.Users.Add(user);
                _context.SaveChanges();

                // Return the registered user's information
                return Ok(new
                {
                    userId = user.Id,
                    name = user.Name,
                    email = user.Email
                });
            }


        }

        
 }

