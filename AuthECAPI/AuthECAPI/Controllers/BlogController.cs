using AuthECAPI.DTO;
using AuthECAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace AuthECAPI.Controllers
{
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {

        private readonly DBContext.AppDBContext _context;

        public BlogController(DBContext.AppDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _context.Blogs
                .Include(b => b.User)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Content,
                    Author = b.User.FirstName + " " + b.User.LastName,
                    b.CreatedAt
                })
                .ToListAsync();

            return Ok(blogs);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.User)
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Content,
                    Author = b.User.FirstName + " " + b.User.LastName,
                    b.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        [Authorize(Roles = "User")]
        [HttpGet("my")]
        public async Task<IActionResult> MyBlogs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blogs = await _context.Blogs
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(blogs);
        }

        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBlogById),
                new { id = blog.Id },
                blog
            );
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound();

            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (role != "Admin" && blog.UserId != userId)
                return Forbid();

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpPut()]
        public async Task<IActionResult> UpdateBlog(int id,[FromBody] UpdateBlogDto dto)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound();

            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (role != "Admin" && blog.UserId != userId)
                return Forbid();

            blog.Title = dto.Title;
            blog.Content = dto.Content;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Blog updated successfully" });
        }

    }
}
