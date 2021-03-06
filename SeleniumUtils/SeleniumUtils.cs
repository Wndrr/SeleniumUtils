﻿using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;

namespace Wndrr.Selenium
{
    public static class SeleniumUtils
    {

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

        #region SCROLL
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

        #endregion

        #region WAIT

        #region URL CHANGE
        
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

        #endregion

        #region URL IS

        /// <summary>
        /// Polls the driver URL until it changes to the targetUrl or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetUrl">The URL to look for</param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        /// <param name="trimTrailingSlash">Remove trailing slashes from the internet call to <paramref name="driver"/>.Url</param>
        public static void WaitUntilUrlIs(this IWebDriver driver, string targetUrl, TimeSpan timeout, int pollInterval = 500, bool trimTrailingSlash = true)
        {
            var timer = new Stopwatch();
            timer.Start();

            while (true)
            {
                var driverUrl = driver.Url;

                if (trimTrailingSlash)
                    driverUrl = driverUrl.TrimEnd('/');

                if (driverUrl == targetUrl)
                    return;

                var isTimeout = TimeSpan.Compare(timer.Elapsed, timeout) > 0;
                if (isTimeout)
                    throw new TimeoutException($"The URL '{driverUrl}' has not changed to '{targetUrl}' within the allowed {timeout.TotalSeconds} seconds");

                Thread.Sleep(pollInterval);
            }
        }

        /// <summary>
        /// Polls the driver URL until it changes to the targetUrl or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetUrl">The URL to look for</param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        /// <param name="trimTrailingSlash">Remove trailing slashes from the internet call to <paramref name="driver"/>.Url</param>
        public static void WaitUntilUrlIs(this IWebDriver driver, string targetUrl, int timeout = 10000, int pollInterval = 500, bool trimTrailingSlash = true)
        {

            var timeoutSpan = new TimeSpan(0, 0, 0, 0, timeout);
            WaitUntilUrlIs(driver, targetUrl, timeoutSpan, pollInterval, trimTrailingSlash);
        }

        #endregion

        #region URL CONTAINS

        /// <summary>
        /// Polls the driver URL until it contains the fragment of url or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetUrlPart">The URL fragment to look for</param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        /// <param name="trimTrailingSlash">Remove trailing slashes from the internet call to <paramref name="driver"/>.Url</param>
        public static void WaitUntilUrlContains(this IWebDriver driver, string targetUrlPart, TimeSpan timeout, int pollInterval = 500, bool trimTrailingSlash = true)
        {
            var timer = new Stopwatch();
            timer.Start();

            while (true)
            {
                var driverUrl = driver.Url;

                if (trimTrailingSlash)
                    driverUrl = driverUrl.TrimEnd('/');

                if (driverUrl.Contains(targetUrlPart))
                    return;

                var isTimeout = TimeSpan.Compare(timer.Elapsed, timeout) > 0;
                if (isTimeout)
                    throw new TimeoutException($"The URL '{driverUrl}' has not changed to contain '{targetUrlPart}' within the allowed {timeout.TotalSeconds} seconds");

                Thread.Sleep(pollInterval);
            }
        }

        /// <summary>
        /// Polls the driver URL until it contains the fragment of url or until the timeout expires
        /// <exception cref="TimeoutException"></exception>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetUrlPart">The URL fragment to look for</param>
        /// <param name="timeout">Throws if the URL has not changed before the specified timeout period is elapsed</param>
        /// <param name="pollInterval">The time to wait between two polls</param>
        /// <param name="trimTrailingSlash">Remove trailing slashes from the internet call to <paramref name="driver"/>.Url</param>
        public static void WaitUntilUrlContains(this IWebDriver driver, string targetUrlPart, int timeout = 10000, int pollInterval = 500, bool trimTrailingSlash = true)
        {

            var timeoutSpan = new TimeSpan(0, 0, 0, 0, timeout);
            WaitUntilUrlContains(driver, targetUrlPart, timeoutSpan, pollInterval, trimTrailingSlash);
        }

        #endregion

        #endregion
    }
}
