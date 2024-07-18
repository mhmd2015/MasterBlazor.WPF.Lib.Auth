

# MasterBlazor.WPF.Lib.Auth

A library to manage ADFS and SAML authentication and Accessing site's REST API for WPF applications using WebView2.
It used by **MasterBlazor.WPF.UC.LogingPage** to give a full experience to access ADFS sites.

## Installation

### NuGet Package

You can install the library via NuGet Package Manager:

``` ps
Install-Package MasterBlazor.WPF.Lib.Auth
```

Or you can download to your project directly.

### GitHub
Clone the repository:

``` bash
git clone https://github.com/yourusername/MasterBlazor.WPF.Lib.git
```

## Usage
### AuthManager
AuthManager is used to manage authentication cookies for a specified site.

#### Initialize AuthManager
``` csharp

using MasterBlazor.WPF.Lib.Auth;

// Initialize with the site URL
var authManager = new AuthManager("https://example.com");
```

#### Save Cookies to File
``` csharp

authManager.Save();
```

#### Load Cookies from File
``` csharp

var cookieContainer = authManager.Load();
```

#### Reset Cookies
``` csharp

authManager.Reset();
```

#### Get Specific Cookie Value
``` csharp

string cookieValue = authManager.GetCookie("FedAuth");
```

#### Initialize Cookies from WebView2

``` csharp

using Microsoft.Web.WebView2.Wpf;
using System.Collections.Generic;

// Assume webView is your WebView2 control
WebView2 webView = new WebView2();
List<CoreWebView2Cookie> cookies = await authManager.Initialize(webView);
```

### RestManager
RestManager is used to make REST API calls using the cookies managed by AuthManager.

#### Initialize RestManager
``` csharp

using MasterBlazor.WPF.Lib.Auth;
using System.Net;

// Using AuthManager
var restManager = new RestManager(authManager);

// Using CookieContainer directly
var cookieContainer = new CookieContainer();
var restManagerWithCookies = new RestManager(cookieContainer, "https://example.com");
```

#### Make GET Request
``` csharp

string apiEndpoint = "/api/endpoint";
string response = await restManager.Get(apiEndpoint);
```
#### Dispose RestManager
``` csharp
restManager.Dispose();
```