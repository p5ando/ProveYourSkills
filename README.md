<h1 align="center"> Prove Your Skils </h1> <br>

<p align="center">
  <a href="https://itunes.apple.com/us/app/gitpoint/id1251245162?mt=8">
    <img alt="Download on the App Store" title="App Store" src="http://i.imgur.com/0n2zqHD.png" width="140">
  </a>

  <a href="https://play.google.com/store/apps/details?id=com.gitpoint">
    <img alt="Get it on Google Play" title="Google Play" src="http://i.imgur.com/mtGRPuM.png" width="140">
  </a>
</p>

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Feedback](#feedback)
- [Contributors](#contributors)
- [Build Process](#build-process)
- [Backers](#backers-)
- [Sponsors](#sponsors-)
- [Acknowledgments](#acknowledgments)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Introduction

[![Build Status](https://img.shields.io/travis/gitpoint/git-point.svg?style=flat-square)](https://travis-ci.org/gitpoint/git-point)
[![Coveralls](https://img.shields.io/coveralls/github/gitpoint/git-point.svg?style=flat-square)](https://coveralls.io/github/gitpoint/git-point)
[![All Contributors](https://img.shields.io/badge/all_contributors-73-orange.svg?style=flat-square)](./CONTRIBUTORS.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg?style=flat-square)](http://commitizen.github.io/cz-cli/)
[![Gitter chat](https://img.shields.io/badge/chat-on_gitter-008080.svg?style=flat-square)](https://gitter.im/git-point)

This is an assigment for the job application. The assigment was: 
- write an WPF application that uses [jsonplaceholder API](http://jsonplaceholder.typicode.com/) to fetch 100 posts and render them all where each post is a separate square, 10 rows x 10
columns.
- By default, display the id on each square. When clicking on a square, the id should be replaced with the user id on all squares.
- When clicking again, show the ids on all squares and so on and on and on...
- A private Github repo without any references to Company (on the name, description or files), with a nice
README.md file with how to run things, gotchas and some motivation behind choices if you feel the
need of sharing

## Solution
Solution contains 2 projects;
- ProveYourSkills - the app source code
- ProveYourSkills.Tests - the Unit tests

ProveYourSkills is divaded in 3 folders:
- Core - contains Models and Services that were used
- Infrastructure - contains infrastructure elements for DI, Configuration, Http
- UI - that contains Views and View models


Application is initilized in App.xml and App.xaml.cs using HostBuild from Microsoft.Extensions.Hosting NuGetpackage. Through the builder, a logger, a configuration file and Dependency Injection are set up.
While building the application MVVM pattern was followed. As pattern defines, application contains Model, View, and ViewModels.
There is only one Model called Post, that represents an individual post that is retrieved from the remote API.
Only one View called MainWindow.xaml can be find in a solution, along with its code-behide C# class called MainWindow.xaml.cs. It defines a visual elements that are shown in the app.
Also, there are 2 View Models:
- PostViewModel.cs - defines the logic and data for each individual squre (Post) in the app
- PostGridViewModel.cs - defines the logic and data for the main screen. In some way, this view encapsulates PostViewModel in the form of ObservableCollection

### Core folder
This folder Contains Model class that is described above and 3 more services:
- PostApiClient - It is responsible for retrieveing the posts from [jsonplaceholder API](http://jsonplaceholder.typicode.com/). It relies on RestApiClient that will be mentioned in Infrastructure segment
- GridCellBuilder - Handles the creation of the one squere that represents one post. It relies on UiComponentFactory
- UiComponentFactory - Centralizes a creation of the the certain UI elements that are utilized in MainWindow.xaml.cs code-behide.

### Infrastructure
The folder contains:
- Configuration class - AppSettings.cs
- Dependency Injection in form of DiConfiguration.cs
- Http infrastructure - RestApiClient.cs for handling HTTP request and HttpUtilities.cs for common operation over HTTP requests 


### UI
Contains already mentioned View and ViewModel componenets, along with App.xaml and App.xaml.cs classes which are the starting point of the app

## Application workflow
As soon as the MainWindow is loaded, the <b>Loaded</b> event is fired. In the constructor of the MainWindow.xaml.cs class, the <b>InitializeGridContentAsync</b> method is subscribed for the event.
It will initiate the call towards the [jsonplaceholder API](http://jsonplaceholder.typicode.com/) through the PostGridViewModel and as soon as the response is 
retrieved, the appropriate UI elements are created.
After the grid of 10x10 squares is created, you will be able to interact with the grid, by clicking on it which will result in the change of the cells' content (instead of Post Id, UserId will be presented).
Click is registered using the functionalities of MouseBinding and the ToggleContentCommand that is defined in the PostGridViewModel. Triggering the command will result in interating through ObservableCollection and each PostViewModel
and calls SwitchContent methon within PostViewModel. PostViewModel contains the individual instance of the Post retrieved from the remote API along with the Content property that is binded the Text of the squere (in the UI sense, the squre is nothing else then TextBlock).
The SwitchContent model will operate over Content property and it will change the its value. By changing the value (through the set property) it also invokes INotifyPropertyChange.ProperyChanged event which 
initiate the change of the UI representation.

## Contributors

[Predrag Sando](https://github.com/p5ando/ProveYourSkills) 
<br>
sandopredrag95@gmail.com 

## Starting the app
### Prerequirements
It is required to have .NET 8 installed on your machine
### 