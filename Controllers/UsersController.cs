using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly Dictionary<int, User> Users = new()
    {
        { 1, new User { Id = 1, Name = "Alice", Email = "alice@example.com" } },
        { 2, new User { Id = 2, Name = "Bob", Email = "bob@example.com" } },
        { 3, new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" } }
    };
    private static int _nextId = Users.Keys.Max() + 1;

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers(int pageNumber = 1, int pageSize = 10)
    {
        var users =  Users.Values
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetUser(int id)
    {
        if (Users.TryGetValue(id, out var user))
        {
            return Ok(user);
        }
        return NotFound();
    }

    [HttpPost]
    public ActionResult<User> AddUser(User user)
    {
        if (IsValidUser(user))
        {
            return BadRequest("Invalid user data.");
        }
        user.Id = _nextId++;
        Users[user.Id] = user;
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateUser(int id, User updatedUser)
    {
        if (!Users.ContainsKey(id)) return NotFound();
        if (IsValidUser(updatedUser))
        {
            return BadRequest("Invalid user data.");
        }
        updatedUser.Id = id;
        Users[id] = updatedUser;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteUser(int id)
    {
        if (Users.Remove(id))
        {
            return NoContent();
        }
        return NotFound();
    }

    private bool IsValidUser(User user) =>
        !string.IsNullOrEmpty(user.Name) && IsValidEmail(user.Email);
    
    private bool IsValidEmail(string email)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(email);
    }
}