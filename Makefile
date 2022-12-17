nuget:
	dotnet build Trico.sln -c Release
	dotnet test Trico.Tests/Trico.Tests.csproj
	dotnet pack Trico/Trico.csproj -o .
