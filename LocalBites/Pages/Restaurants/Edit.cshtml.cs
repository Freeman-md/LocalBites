using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using LocalBites.Models.DTOs;
using LocalBites.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocalBites.Pages.Restaurants;

public class EditModel : PageModel
{
    private readonly IRestaurantRepository _restaurantRepository;

    public EditModel(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    [BindProperty]
    public CreateRestaurantDto EditRestaurantDto { get; set; } = new();

    public SelectList Locations { get; set; }
    public SelectList Cuisines { get; set; }

    public string? ImagePreview { get; set; }
    public string Id { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        Id = id;
        var restaurant = await _restaurantRepository.GetById(id);
        if (restaurant == null) return NotFound();

        EditRestaurantDto = new CreateRestaurantDto
        {
            Name = restaurant.Name,
            Location = restaurant.Location,
            Cuisine = restaurant.Cuisine,
            Description = restaurant.Description,
        };

        ImagePreview = restaurant.ImageUrl;

        Locations = new SelectList(Enum.GetValues(typeof(Location)));
        Cuisines = new SelectList(Enum.GetValues(typeof(Cuisine)));

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        Id = id;
        Locations = new SelectList(Enum.GetValues(typeof(Location)));
        Cuisines = new SelectList(Enum.GetValues(typeof(Cuisine)));

        if (!ModelState.IsValid) return Page();

        var restaurant = await _restaurantRepository.GetById(id);
        if (restaurant == null) return NotFound();

        restaurant.Name = EditRestaurantDto.Name;
        restaurant.Location = EditRestaurantDto.Location;
        restaurant.Cuisine = EditRestaurantDto.Cuisine;
        restaurant.Description = EditRestaurantDto.Description;

        if (EditRestaurantDto.Image != null)
        {
            restaurant.ImageUrl = await SaveImage(EditRestaurantDto.Image);
        }

        await _restaurantRepository.Update(id, restaurant);

        return RedirectToPage("/Restaurants/View", new { id = restaurant.Id });
    }

    private async Task<string> SaveImage(IFormFile file)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{fileName}";
    }
}
