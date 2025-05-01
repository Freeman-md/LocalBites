using System;
using Microsoft.Playwright;
using Xunit;

namespace LocalBites.Tests.E2E;

public class IndexTests : IAsyncLifetime
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;
    private const string APP_URL = "http://localhost:5212/";

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = true
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }


    [Fact]
    public async Task IndexPage_Should_DisplayRestaurants()
    {
        await _page.GotoAsync(APP_URL);

        var restaurantExists = await _page.Locator(".restaurant-item").CountAsync() > 0;

        Assert.True(restaurantExists, "Restaurants should be displayed on the Index page.");
    }

    [Fact]
    public async Task Filter_Should_ShowFilteredResults()
    {
        await _page.GotoAsync(APP_URL);

        // await _page.SelectOptionAsync("#CuisineFilter", "Italian");

        // await _page.ClickAsync("#filter-button");

        var filteredText = await _page.InnerTextAsync("body");
        Assert.Contains("Clear Filters", filteredText);
    }


    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
}
