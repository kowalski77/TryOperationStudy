using Microsoft.Extensions.Logging;
using RealWorldExample.Results;
using RealWorldExample.Tokens;

Console.WriteLine("Press any key to start...");
Console.ReadKey(true);

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "hh:mm:ss ";
}));

ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

var token = JwtTokenFactory.Create();

Result<ExternalToken> result = ExternalToken.Create(token);

if(result.Failure)
{
    foreach (ErrorResult errorResult in result.Errors!)
    {
        logger.LogError(errorResult.Message);
    }
}

Console.WriteLine("No errors...");