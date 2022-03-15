using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using UserApp.Models;

namespace UserApp.Pages
{
    public class UserRegistrationModel : PageModel
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHostingEnvironment _httpContextAccessor;
        public static List<UserModel> AddedUsers;

        static UserRegistrationModel()
        {
            AddedUsers = new List<UserModel>();
        }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public string ReturnUrl { get; set; }
        [BindProperty]
        public UserModel Users { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }

        public UserRegistrationModel(IHostingEnvironment hostingEnvironment, IHostingEnvironment httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public static List<UserModel> Data { get; set; }

        public async Task OnGetAsync()
        {
            Count = await GetCount();
            Data = await GetPaginatedResult(CurrentPage, PageSize);
        }
        public async Task<List<UserModel>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            return AddedUsers.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<int> GetCount()
        {
            return AddedUsers.Count;
        }
        public async Task OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int NewId = 1;
                    if (AddedUsers.Count > 0)
                    {
                        UserModel model = AddedUsers.OrderByDescending(u => u.Id).FirstOrDefault();
                        NewId = model.Id + 1;
                    }
                    Users.Id = NewId;
                    AddedUsers.Add(Users);

                    Count = await GetCount();
                    Data = await GetPaginatedResult(CurrentPage, PageSize);

                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            { }
        }
        public IActionResult OnPostDelete(int Id)
        {
            try
            {
                UserModel userModel = AddedUsers.Find(a => a.Id == Id);
                AddedUsers.Remove(userModel);
            }
            catch (Exception ex)
            { }
            return RedirectToPage("UserRegistration");
        }
    }
}