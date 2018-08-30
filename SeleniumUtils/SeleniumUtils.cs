using System;
using System.Diagnostics;
using System.Threading;
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
        /// Vertically scrolls an element to the middle of the screen using JavaScript
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element">The lement to scroll into view</param>
        [Obsolete("Please use VerticalScrollTo instead")]
        public static void ScrollTo(this IWebDriver driver, IWebElement element)
        {
            VerticalScrollTo(driver, element);
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

        /// <summary>
        /// Polls the driver URL until it changes or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        public static void WaitUntilCurrentUrlChange(this IWebDriver driver, TimeSpan timeout, int pollInterval = 500)
        {
            var timer = new Stopwatch();
            timer.Start();
            var startUrl = driver.Url;

            while (true)
            {
                if (startUrl != driver.Url)
                    break;

                var isTimeout = TimeSpan.Compare(timer.Elapsed, timeout) > 0;
                if (isTimeout)
                    throw new TimeoutException($"The URL {driver.Url} has not changed within the allowed {timeout} milliseconds");

                Thread.Sleep(pollInterval);
            }
        }
        
        /// <summary>
        /// Polls the driver URL until it changes or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        public static void WaitUntilCurrentUrlChange(this IWebDriver driver, int timeout = 10000, int pollInterval = 500)
        {
            var timeoutSpan = new TimeSpan(0, 0, 0, 0, timeout);
            WaitUntilCurrentUrlChange(driver, timeoutSpan, pollInterval);
        }
    }
}
