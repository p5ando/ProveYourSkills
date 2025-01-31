# Prove Your Skills

## Table of Contents

- [Introduction](#introduction)
- [Solution](#solution)
    - [Core Folder](#core-folder)
    - [Infrastructure](#infrastructure)
    - [UI](#ui)
- [Application Workflow](#application-workflow)
- [Starting the App](#starting-the-app)
    - [Prerequisites](#prerequisites)
    - [Getting the Repository](#getting-the-repository)
    - [Building the Executables](#building-the-executables)
    - [Running the App](#running-the-app)
- [Contributors](#contributors)
- [Bonus Question and Answers](#bonus-question-and-answers)

## Introduction

This is an assignment for a job application. The assignment was:  

- Write a WPF application that uses the [JSONPlaceholder API](http://jsonplaceholder.typicode.com/) to fetch 100 posts and display them in a 10x10 grid, where each post is represented by a square.
- By default, display the post ID on each square. When clicking on a square, replace all post IDs with their respective user IDs.  
- Clicking again toggles the display back to post IDs, and so on.  
- Upload the project to a private GitHub repository without any references to the company (in the name, description, or files). The repository should include a well-structured `README.md` explaining how to run the application, potential challenges, and the reasoning behind design choices.


At the end of the this document you can find the answers to the general questions that were part of this assigment.

## Solution

The solution contains two projects:  

- `ProveYourSkills` - The application source code.  
- `ProveYourSkills.Tests` - Unit tests.  

The `ProveYourSkills` project is divided into three main folders:  

- **Core** - Contains Models and Services.  
- **Infrastructure** - Contains configuration, dependency injection, and HTTP utilities.  
- **UI** - Contains Views and ViewModels.  

The application is initialized in `App.xaml` and `App.xaml.cs` using `HostBuilder` from the `Microsoft.Extensions.Hosting` NuGet package. This setup configures logging, configuration files, and dependency injection.  

The app follows the **MVVM (Model-View-ViewModel)** pattern:  

- **Model:** A `Post` class represents an individual post retrieved from the API.  
- **View:** A single view, `MainWindow.xaml`, defines the UI layout.  
- **ViewModel:**  
  - `PostViewModel.cs` - Handles logic and data for each square (post).  
  - `PostGridViewModel.cs` - Manages the main screen and contains an `ObservableCollection<PostViewModel>`.  

### Core Folder

This folder contains the `Post` model and three services:  

- **PostApiClient** - Retrieves posts from the [JSONPlaceholder API](http://jsonplaceholder.typicode.com/), relying on `RestApiClient` from the `Infrastructure` folder.  
- **GridCellBuilder** - Handles the creation of a single square representing a post, using `UiComponentFactory`.  
- **UiComponentFactory** - Centralizes the creation of UI elements used in `MainWindow.xaml.cs`.  

### Infrastructure

This folder includes:  

- **AppSettings.cs** - Manages configuration settings.  
- **DiConfiguration.cs** - Handles dependency injection setup.  
- **HTTP utilities:**  
  - `RestApiClient.cs` - Manages HTTP requests.  
  - `HttpUtilities.cs` - Contains common HTTP operations.  

### UI

Contains the main view (`MainWindow.xaml`), view models, and application entry points (`App.xaml` and `App.xaml.cs`).

## Application Workflow

When `MainWindow` is loaded, the `Loaded` event is triggered. In the constructor of `MainWindow.xaml.cs`, the `InitializeGridContentAsync` method subscribes to this event.

1. **Fetching Data:**  
   - `InitializeGridContentAsync` calls the API through `PostGridViewModel`.  
   - Once the response is received, UI elements are dynamically created.  

2. **Grid Display:**  
   - A 10x10 grid of squares is created, each showing a post ID by default.  

3. **Interaction:**  
   - Clicking a square triggers the `ToggleContentCommand` in `PostGridViewModel`.  
   - The command iterates through the `ObservableCollection<PostViewModel>` and calls `SwitchContent()`.  
   - `SwitchContent()` toggles the displayed value between `Post.Id` and `Post.UserId`.  
   - The UI updates automatically using `INotifyPropertyChanged`.  

## Starting the App

### Prerequisites

.NET 8 is required. Download it from:  
[https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Getting the Repository

#### Option 1: Download the ZIP file  

1. Open [https://github.com/p5ando/ProveYourSkills](https://github.com/p5ando/ProveYourSkills).  
2. Click the **Code** button.  
3. Select **Download ZIP** and extract the file.  

#### Option 2: Clone the Repository  

Run the following commands in the terminal:

```sh
git clone https://github.com/p5ando/ProveYourSkills.git
cd ProveYourSkills
```

### Building the Executables

1. Navigate to the project root (where the `.sln` file is located).  
2. Run the following command to build the application:

```sh
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=false
```

3. The `.exe` file will be generated in:  

```
ProveYourSkills\bin\Release\net8.0-windows\win-x64\publish
```

### Running the App

Run the `.exe` file from the generated folder.

## Contributors

[Predrag Sando](https://github.com/p5ando/ProveYourSkills)  
📧 sandopredrag95@gmail.com 

## Bonus Question and Answers
1. In C# there are several ways to make code run in multiple threads. To make things easier, the await keyword was introduced; what does this do?
```
Running code in multiple threads can be achieved in several ways, including using the Thread class, ThreadPool, or Task.
async/await is used for asynchronous programming, primarily with Task but also with ValueTask<T> and custom awaitables.
When a method returning a Task is awaited, execution is suspended at that point without blocking the calling thread.
If running on a ThreadPool thread, the thread can return to the pool for other work. However, if running on a UI thread, execution will resume on the same UI thread after the task completes.
Once the awaited Task finishes, execution resumes from the point where it was awaited.
```
2. If you make http requests to a remote API directly from a UI component, the UI will freeze for a while, how can you use await to avoid this and
how does this work?
```
If there is a button, and we define the BulttonClick handler like this:

public void ButtonClick(object s, EventArgs e)
{ 
   //...
   var result = httpClient.GetString("some_random_url");
   //...
}

It will block the UI thread. To avoid that we should use async methodm like following:

public async void ButtonClick(object s, EventArgs e)
{
   //...
   var resultTask = httpClient.GetStringAsync("some_random_url");
   var result = await resultTask;
   //...
}

By using Async/Await we ensure the UI elements remains responsive, which was not the case in the first example.
Note that ButtonClick method has void as return type instead of Task, which would be the case if it wasn't the WPF application, since delegates do not support Task.
```

3. Imagine that you have to process a large unsorted CSV file with two columns: ProductId (int) and AvailableIn (ISO2 String, e.g. "US", "NL"). The goal is
to group the file sorted by ProductId together with a list where the product is available. Example: 1, "DE" 2, "NL" 1, "US" 3, "US" Becomes: 1 -> ["DE", "US"]
2 -> ["NL"] 3 -> ["US"]
   1. How would you do this using LINQ syntax (write a short example)?
   ```
   csvLines
      .Select(line => line.Split(","))
      .Select(columnValues => new { id = columnValues[0], region = columnValues[1] })
      .GroupBy(item => item.id)
      .OrderBy(group => group.Key)
   ```
   2. The program crashes with an OutOfMemoryError after processing approx. 80%. What would you do to succeed?
   ```
   - Reading file as a stream
   - IAsyncEnumerable and yield return
   ```
4. In C# there is an interface IDisposable.
   1. Give an example of where and why to implement this interface.
   ```
   Should be used whenever we encounter the usage of resources that are not managed by .NET garbage collector such as database connections, file handles, sockets... 
   IDisposable makes us the define a logic for releasing these resources.
   ```
   2. We can use disposable objects in a using block. What is the purpose of doing this?
   ```
   As soon as the end of the using block is reached the IDisposable.Dispose() method is called automatically and the resource will be released 
   ```
5. When a user logs in on our API, a JWT token is issued and our Outlook plugin uses this token for every request for authentication. Why and when is it (or isn't it) safe to use this?
```
It's safe:
- if it's used over secure network such as HTTPS
- expiration is defined
- if token signature is validated/verified for each request on server side

Not safe:
- if you don't do safe things from above :)
- if you store it indefinitely