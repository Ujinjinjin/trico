// See https://aka.ms/new-console-template for more information

using Comar.Configuration;
using Comar.Extensions;
using Microsoft.Extensions.DependencyInjection;

var sc = new ServiceCollection();

sc.AddConfiguration()
	.AddInMemoryProvider()
	.AddFileProvider()
	.AddEnvironmentVariableProvider();

var sb = sc.BuildServiceProvider();

sb.UseConfiguration();

var config = sb.GetRequiredService<IConfiguration>();
var options = new Dictionary<string, string>
{
	{ "config-filepath", "./config.json" },
	{ "prefix", "comar_" }
};

await config.LoadAsync(options, default);

var key = Guid.NewGuid().ToString("N");
var value = Guid.NewGuid().ToString("N");

config[key] = value;

await config.DumpAsync(default);

Console.WriteLine($"{key} = {value}");
