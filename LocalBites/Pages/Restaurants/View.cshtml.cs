using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocalBites.Pages.Restaurants;

public class ViewModel : PageModel {
    private readonly IRestaurantRepository _restaurantRepository;

    public Restaurant? Restaurant { get; set; }

    public ViewModel(IRestaurantRepository restaurantRepository) {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<IActionResult> OnGetAsync(string id) {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        Restaurant = await _restaurantRepository.GetById(id);

        if (Restaurant == null)
            return NotFound();

        return Page();
    }
}