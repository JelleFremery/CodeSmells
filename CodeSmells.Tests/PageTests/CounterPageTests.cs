namespace CodeSmells.Tests.PageTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CounterPageTests : PlaywrightTest
    {       
        [Test]
        public async Task GivenCounterOfZero_WhenClickCounterTwice_ThenCounterIsTwo()
        {
            var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://localhost:7050/counter");
            
            await page.WaitForTimeoutAsync(2000);                        
            
            var button = page.Locator("button", new PageLocatorOptions { HasTextString = "Click me" });
            int numberOfClicks = 2;

            for (int i = 1; i <= numberOfClicks; i++)
            {
                await button.ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }

            var currentCount = page.Locator("p", new PageLocatorOptions { HasTextRegex = new Regex("Current count: *") });                   
            
            await Expect(currentCount).ToHaveTextAsync(new Regex($"Current count: {numberOfClicks}"));
            await page.WaitForTimeoutAsync(500);

            await context.CloseAsync();
            await browser.CloseAsync();
        }

        [Test]
        public async Task GivenCounterOfOne_WhenReload_ThenCounterIsZero()
        {
            var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://localhost:7050/counter");

            await page.WaitForTimeoutAsync(2000);

            var button = page.Locator("button", new PageLocatorOptions { HasTextString = "Click me" });
            int numberOfClicks = 1;

            for (int i = 1; i <= numberOfClicks; i++)
            {
                await button.ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }

            await page.ReloadAsync();
            await page.WaitForTimeoutAsync(2000);

            var currentCount = page.Locator("p", new PageLocatorOptions { HasTextRegex = new Regex("Current count: *") });
            await Expect(currentCount).ToHaveTextAsync(new Regex("Current count: 0"));

            await context.CloseAsync();
            await browser.CloseAsync();
        }
    }
}