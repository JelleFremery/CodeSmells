using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeSmells.Tests.Shared
{
    public class CounterPage
    {
        private readonly IPage Page;

        private const string CounterPrefix = "Current count:";

        public CounterPage(IPage page)
        {
            Page = page;
        }

        public async Task ClickCounterButtonAsync(int numberOfClicks)
        {
            var button = Page.Locator("button", new PageLocatorOptions { HasTextString = "Click me" });

            for (int i = 1; i <= numberOfClicks; i++)
            {
                await button.ClickAsync();                
            }
        }

        public async Task<bool> StatusEqualsAsync(int numberOfClicks)
        {
            var expectedText = $"{CounterPrefix} {numberOfClicks}"; 
            var currentCount = Page.Locator("p", new PageLocatorOptions { HasTextRegex = new Regex($"{CounterPrefix} *") });
            var actualText = await currentCount.TextContentAsync();
            return string.Equals(expectedText, actualText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
