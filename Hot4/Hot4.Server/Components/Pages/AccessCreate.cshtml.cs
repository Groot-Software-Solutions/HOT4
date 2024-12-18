using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hot4.Server.Components.Pages
{
    public class AccessCreateModel : PageModel
    {
        private IAccessRepository _accessRepository;
        public AccessCreateModel(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }
        [BindProperty]
        public Access access { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var mdl = new Access() { AccessCode = "7009373728", AccessPassword = "5555", AccountId = 10001002, ChannelId = 1 };
            await _accessRepository.AddAccess(access);


            return RedirectToPage("./Index");
        }
    }
}
