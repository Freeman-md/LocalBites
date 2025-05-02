using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using LocalBites.Models.DTOs;
using LocalBites.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LocalBites.Pages.Restaurants;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IRestaurantRepository _restaurantRepository;

    [BindProperty]
    public CreateRestaurantDto CreateRestaurantDto { get; set; } = new();

    public SelectList Locations { get; set; }

    public SelectList Cuisines { get; set; }

    public CreateModel(ILogger<CreateModel> logger, IRestaurantRepository restaurantRepository)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
    }

    public void OnGet()
    {
        Locations = new SelectList(Enum.GetValues(typeof(Location)));
        Cuisines = new SelectList(Enum.GetValues(typeof(Cuisine)));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Locations = new SelectList(Enum.GetValues(typeof(Location)));
        Cuisines = new SelectList(Enum.GetValues(typeof(Cuisine)));

        if (!ModelState.IsValid)
        {
            return Page();
        }

        string imageUrl = await SaveImage(CreateRestaurantDto.Image);

        var restaurant = new Restaurant(CreateRestaurantDto.Name, CreateRestaurantDto.Location, CreateRestaurantDto.Cuisine, CreateRestaurantDto.Description)
        {
            Rating = new Random().Next(1, 6),
            ImageUrl = imageUrl
        };

        Restaurant newRestaurant = await _restaurantRepository.Add(restaurant);

        TempData["Success"] = "Restaurant added successfully!";
        TempData["RestaurantId"] = newRestaurant.Id;

        return RedirectToPage("Create");
    }

    public async Task<string> SaveImage(IFormFile formFile)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CreateRestaurantDto.Image.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        return $"/uploads/{fileName}";
    }
}