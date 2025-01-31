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

## Introduction

This is an assignment for a job application. The assignment was:  

- Write a WPF application that uses the [JSONPlaceholder API](http://jsonplaceholder.typicode.com/) to fetch 100 posts and display them in a 10x10 grid, where each post is represented by a square.
- By default, display the post ID on each square. When clicking on a square, replace all post IDs with their respective user IDs.  
- Clicking again toggles the display back to post IDs, and so on.  
- Upload the project to a private GitHub repository without any references to the company (in the name, description, or files). The repository should include a well-structured `README.md` explaining how to run the application, potential challenges, and the reasoning behind design choices.

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
