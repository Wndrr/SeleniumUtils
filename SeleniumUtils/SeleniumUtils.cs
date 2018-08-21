using OpenQA.Selenium;

namespace SeleniumUtils
{
    public static class SeleniumUtils
    {
        /// <summary>
        /// Scrolls an element to the middle of the screen
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        public static void ScrollTo(this IWebDriver driver, IWebElement element)
        {
            if (element is ILocatable locatable)
            {
                var position = locatable.LocationOnScreenOnceScrolledIntoView;
                var js = (IJavaScriptExecutor)driver;
                var windowHeight = driver.Manage().Window.Size.Height;
                js.ExecuteScript($"window.scrollTo({position.X},{(windowHeight / 2) + position.Y});");
            }
        }

        /// <summary>
        /// Changes the currently focused tab
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="index">Target tab</param>
        /// <returns>
        /// false - when the requested index was outside of the possible range
        /// true - the tab was found, all clear 
        /// </returns>
        public static bool SwitchToTab(this IWebDriver driver, int index)
        {
            var switchAction = driver.SwitchTo();
            // If the index is out of range
            if (driver.WindowHandles.Count <= index)
                return false;

            var handle = driver.WindowHandles[index];
            switchAction.Window(handle);

            return true;
        }
    }
}
