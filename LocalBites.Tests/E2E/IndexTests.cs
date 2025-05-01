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
    private readonly LocalBitesWebAppFactory<Program> _factory;
    private const string APP_URL = "http://localhost:5212/";
    private string _appUrl;

    public IndexTests()
    {
        _factory = new LocalBitesWebAppFactory<Program>();
        _appUrl = _factory.Server.BaseAddress.ToString();
    }


    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = false
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    // [Fact]
    // public async Task IndexPage_Should_DisplayRestaurants()
    // {
    //     await _page.GotoAsync(_appUrl);

    //     var restaurantExists = await _page.Locator(".restaurant-item").CountAsync() > 0;

    //     Assert.True(restaurantExists, "Restaurants should be displayed on the Index page.");
    // }

    // [Fact]
    // public async Task Filter_Should_ShowFilteredResults()
    // {
    //     await _page.GotoAsync(_appUrl);

    //     // await _page.SelectOptionAsync("#CuisineFilter", "Italian");

    //     // await _page.ClickAsync("#filter-button");

    //     var filteredText = await _page.InnerTextAsync("body");
    //     Assert.Contains("Clear Filters", filteredText);
    // }


    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
        _factory.Dispose();
    }
}
