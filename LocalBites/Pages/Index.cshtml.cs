using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocalBites.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRestaurantRepository _restaurantRepository;

    public List<Restaurant> Restaurants { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger, IRestaurantRepository restaurantRepository)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
    }

    public async Task OnGet()
    {
        Restaurants = await _restaurantRepository.GetAll();
    }

    public async Task OnPostFilter(Cuisine? cuisine, Location? location)
    {
        if (cuisine.HasValue && !Enum.IsDefined(typeof(Cuisine), cuisine.Value)) 
            throw new ArgumentException("Invalid cuisine selection");
        
        if (location.HasValue && !Enum.IsDefined(typeof(Location), location.Value)) 
            throw new ArgumentException("Invalid location selection");

        Restaurants = await _restaurantRepository.FilterByPreferences(cuisine, location);
    }
}
