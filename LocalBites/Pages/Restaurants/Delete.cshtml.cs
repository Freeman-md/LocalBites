using LocalBites.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocalBites.Pages.Restaurants;

public class DeleteModel : PageModel
{
    private readonly IRestaurantRepository _restaurantRepository;

    public DeleteModel(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        await _restaurantRepository.Delete(id);

        return RedirectToPage("/Index");
    }
}
