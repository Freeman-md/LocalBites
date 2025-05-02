using System.ComponentModel.DataAnnotations;
using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using LocalBites.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocalBites.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRestaurantRepository _restaurantRepository;

    public List<Restaurant> Restaurants { get; set; } = new();
    public SelectList? Cuisines  { get; set; }
    public SelectList? Locations  { get; set; }

    [BindProperty(SupportsGet = true)]
    public FilterCriteria Filter { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger, IRestaurantRepository restaurantRepository)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
    }

    public async Task OnGet()
    {
        Cuisines = new SelectList(Enum.GetValues(typeof(Cuisine)));
        Locations = new SelectList(Enum.GetValues(typeof(Location)));

        if (Filter.Cuisine.HasValue && !Enum.IsDefined(typeof(Cuisine), Filter.Cuisine.Value))
            ModelState.AddModelError("Filter.Cuisine", "Invalid cuisine selection");

        if (Filter.Location.HasValue && !Enum.IsDefined(typeof(Location), Filter.Location.Value))
            ModelState.AddModelError("Filter.Location", "Invalid location selection");

        if (!ModelState.IsValid)
            return;
        
        Restaurants = await _restaurantRepository.FilterByPreferences(Filter.Cuisine, Filter.Location);
    }
}
