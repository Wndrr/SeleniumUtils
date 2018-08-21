# Selenium utils
A C#.NET Standard 2.0 collection of utility methods for Selenium Webdriver

## Compatibility

This library targets .NET Standard 2.0 which makes it compatible with both .NET Core 2.0 and above aswell as .NET Framework 4.6.1 and above. See the [compatibility page](http://immo.landwerth.net/netstandard-versions/#) for a more exhaustive list of compatible frameworks.

## NuGet package

The NuGet package can be found on [NuGet.org](https://www.nuget.org/packages/Wndrr.Selenium.Utils/) or by searching `Wndrr.Selenium.Utils` in the NuGet explorer.

## Usage

### The `ScrollTo` extension method

Tries to put an element in the center of the screen using a javascript call to `window.scrollTo`.
Usage example
```csharp
// Instanciate an IWebDriver
using (var driver = new ChromeDriver())
{
    // Find an element in the DOM (isn't "dummyElement" a very cute name for an ID ?)
    var dummyElement = _driver.FindElement(By.Id("dummyElement"));
    // Scroll the element to the center of the window (more or less) 
    driver.ScrollTo(dummyElement);
}
```

### The `SwitchToTab` method

Changes the focus of the `IWebDriver` to target the tab/window, finding it by its id.
The return value indicates if the tab could be found. When it is `false` no tab change occured.
```csharp
// Instanciate an IWebDriver
using (var driver = new ChromeDriver())
{
    // Focus to the 2nd tab
    bool isSwitchSuccess = driver.SwitchToTab(1);
}
```
