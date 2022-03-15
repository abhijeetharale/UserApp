using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserApp.Models;

namespace UserApp.Pages
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public UserModel Users { get; set; }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                Users = UserRegistrationModel.AddedUsers.Find(a => a.Id == id);
            }
        }
        public IActionResult OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel model = UserRegistrationModel.AddedUsers.Find(a => a.Id == Users.Id);
                    model.Name = Users.Name;
                    model.Email = Users.Email;
                    model.PhoneNumber = Users.PhoneNumber;
                    model.Address = Users.Address;
                    model.City = Users.City;
                }
            }
            catch (Exception ex)
            { }
            return RedirectToPage("UserRegistration");
        }
    }
}