set var=1.0.0

c:\LocalNugetPackages\nuget pack Iris.Core.nuspec -Version %var%
c:\LocalNugetPackages\nuget pack Iris.Messaging.nuspec -Version %var%
c:\LocalNugetPackages\nuget pack Iris.EntityFramework.nuspec -Version %var%
c:\LocalNugetPackages\nuget pack Iris.ServiceHost.nuspec -Version %var%
c:\LocalNugetPackages\nuget pack Iris.Autofac.nuspec -Version %var%

move *.nupkg c:\\LocalNugetPackages

pause