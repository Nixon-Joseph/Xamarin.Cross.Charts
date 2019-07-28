# Xamarin.Cross.Charts

Create and display simple charts in Xamarin.Android, Xamarin.iOS, Xamarin.UWP, and Xamarin.Forms projects.

## TODO: Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### TODO: Prerequisites

If you are using the `.Forms` variant, you will need at least `Xamarin.Forms v3.3.0.912540`
Both the `.Forms` and the native variants depend on `SkiaSharp v1.68.0`

### TODO: Installing

Available on NuGet

**NET Standard 2.0, Xamarin.iOS, Xamarin.Android, UWP**

[![NuGet](https://img.shields.io/nuget/v/Xamarin.Cross.Charts.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Cross.Charts/)

**Xamarin.Forms (.NET Standard 2.0)**

[![NuGet](https://img.shields.io/nuget/v/Xamarin.Cross.Charts.Forms.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Cross.Charts.Forms/)

## Built With

* [Xamarin](https://docs.microsoft.com/en-us/xamarin/) - The mobile framework
* [SkiaSharp](https://github.com/mono/SkiaSharp) - Dependancy

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Nixon-Joseph/Xamarin.Cross.Charts/tags). 

## Authors

* **Joseph Nixon** - *Initial work* - [Microcharts](https://github.com/dotnet-ad/Microcharts/)

TODO: See also the list of [contributors](https://github.com/Nixon-Joseph/Xamarin.Cross.Charts/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* This project is 'forked' directly off of [Microcharts](https://github.com/dotnet-ad/Microcharts/).

## Huge shoutout to [Microcharts](https://github.com/dotnet-ad/Microcharts/)

I was a contributor on [Microcharts](https://github.com/dotnet-ad/Microcharts/) since stumpling upon it after the original author had moved on to other things, but it was a little too much to deal with limited permissions in the project and it was in the middle of a Gitflow release/development. Also I wanted to make wholesale changes to syntax, coding structure, variable naming, etc, without screwing with the original author's vision of the project. This project started out as a direct pull off of the develop branch, so it has all the latest things.

I attempted to contribute directly to his project, but stepping into a project with many unresolved 'issues' from more than a year ago, it was hard to catch up, not to mention, if the project is my own, I feel like I have more responsibility to keep it alive, and functional. [Microcharts](https://github.com/dotnet-ad/Microcharts/) also was originally created to provide simple charts, with purposfully limited functionality. Alois didn't intend people to be displaying complex data (40+ datapoints) in the charts, which is fine, but I indend to make it more flexable for all scenarios. Alois also didn't intend on making it interactable, but one of the first features I plan to implement is a touch gesture on the chart elements/labels, as that is one of the most widely requeested features in his repo.
