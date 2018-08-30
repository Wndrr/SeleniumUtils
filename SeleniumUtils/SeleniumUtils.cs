using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;

namespace Wndrr.Selenium
{
    public static class SeleniumUtils
    {
        /// <summary>
        /// Vertically scrolls an element to the middle of the screen using JavaScript
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element">The lement to scroll into view</param>
        public static void VerticalScrollTo(this IWebDriver driver, IWebElement element)
        {
            if (element is ILocatable locatableCalendar)
            {
                var halfWindowHeight = driver.Manage().Window.Size.Height / 2;
                var targetElementVerticalPosition = locatableCalendar.Coordinates.LocationInDom.Y;
                var yCoordinateForElementCenterScreen = targetElementVerticalPosition - halfWindowHeight;

                ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {yCoordinateForElementCenterScreen});");
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
