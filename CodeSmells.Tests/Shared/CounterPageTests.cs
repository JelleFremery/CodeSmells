namespace CodeSmells.Tests.Shared
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class OtherTests : PlaywrightTest
    {
        private IBrowser Browser;
        private IBrowserContext BrowserContext;
        private IPage Page;               

        [Test]
        public async Task GivenCounterOfZero_WhenClickCounterTwice_ThenCounterIsTwo()
        {
            var page = new CounterPage(Page);
            
            int numberOfClicks = 2;
            await page.ClickCounterButtonAsync(numberOfClicks);

            var isMatch = await page.StatusEqualsAsync(numberOfClicks);
            Assert.That(isMatch, $"{numberOfClicks}");
        }

        [Test]
        public async Task GivenCounterOfOne_WhenReload_ThenCounterIsZero()
        {
            var page = new CounterPage(Page);
                        
            await page.ClickCounterButtonAsync(1);
            await Page.ReloadAsync();
            
            var isMatch = await page.StatusEqualsAsync(0);
            Assert.That(isMatch, "Counter did not read zero after reload.");
        }

        [SetUp]
        public async Task SetUpAsync()
        {
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            BrowserContext = await Browser.NewContextAsync();
            Page = await BrowserContext.NewPageAsync();
            await Page.GotoAsync("https://localhost:7050/counter");
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await BrowserContext.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}