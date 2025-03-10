using System.ComponentModel.DataAnnotations;
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

    [BindProperty(SupportsGet = true)]
    public FilterCriteria Filter { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger, IRestaurantRepository restaurantRepository)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
    }

    public async Task OnGet()
    {
        Restaurants = await _restaurantRepository.GetAll();
    }

    public async Task OnGetFilterByPreferences()
    {
        if (Filter.Cuisine.HasValue && !Enum.IsDefined(typeof(Cuisine), Filter.Cuisine.Value))
            ModelState.AddModelError("Filter.Cuisine", "Invalid cuisine selection");

        if (Filter.Location.HasValue && !Enum.IsDefined(typeof(Location), Filter.Location.Value))
            ModelState.AddModelError("Filter.Location", "Invalid location selection");

        if (!ModelState.IsValid)
            return;

        Restaurants = await _restaurantRepository.FilterByPreferences(Filter.Cuisine, Filter.Location);
    }

    public async Task OnPostFilter(Cuisine? cuisine, Location? location)
    {
        if (cuisine.HasValue && !Enum.IsDefined(typeof(Cuisine), cuisine.Value))
            ModelState.AddModelError("cuisine", "Invalid cuisine selection");

        if (location.HasValue && !Enum.IsDefined(typeof(Location), location.Value))
            ModelState.AddModelError("location", "Invalid location selection");

        if (!ModelState.IsValid)
            return;


        Restaurants = await _restaurantRepository.FilterByPreferences(cuisine, location);
    }
}
