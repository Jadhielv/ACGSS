using ACGSS.Domain.DTOs;
using ACGSS.Domain.Enums;
using ACGSS.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACGSS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDto> _validator;

        public UserController(IUserService userService, IValidator<UserDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();

            return users != null ?
                        View(users) :
                        Problem("Entity 'Users' is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userService.GetUser((int)id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,PhoneNumber,Email")] UserDto userDto)
        {
            userDto.CreatedDate = DateTime.Now;
            userDto.ModifiedDate = DateTime.Now;
            userDto.IsActive = UserStatus.Active;

            await AddValidationErrorsAsync(userDto);

            if (ModelState.IsValid)
            {
                await _userService.AddUser(userDto);
                return RedirectToAction(nameof(Index));
            }
            return View(userDto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userService.GetUser((int)id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,PhoneNumber,Email,CreatedDate,ModifiedDate,IsActive")] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return NotFound();
            }

            userDto.ModifiedDate = DateTime.Now;

            await AddValidationErrorsAsync(userDto);

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUser(userDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userDto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userService.GetUser((int)id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _userService.GetUsers();
            if (users == null)
            {
                return Problem("Entity 'Users' is null.");
            }

            var user = await _userService.GetUser(id);
            user.ModifiedDate = DateTime.Now;
            user.IsActive = UserStatus.Inactive;

            if (user != null)
            {
                await _userService.UpdateUser(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task AddValidationErrorsAsync(UserDto userDto)
        {
            var result = await _validator.ValidateAsync(userDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }

        private bool UserExists(int id)
        {
            var user = _userService.GetUser(id);
            return user != null ? true : false;
        }
    }
}
