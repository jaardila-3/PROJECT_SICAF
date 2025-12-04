using Microsoft.AspNetCore.Mvc;

using SICAF.Web.Interfaces.Identity;

namespace SICAF.Web.ViewComponents;

public class ProfileDropdownViewComponent(ICurrentUserService currentUserService) : ViewComponent
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync();
        return View(currentUser);
    }
}