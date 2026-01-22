# Mantel Coding Challenge

This is my solution for the coding challenge to parse the log file.

## How To

Running the solution should be straightforward from Visual Studio, Rider, or the CLI. The solution will require the .NET 10 SDK. The following commands will get the program running:

```
dotnet restore MantelChallenge.sln
dotnet build MantelChallenge.sln
dotnet run --project ReportUI/ReportUI.csproj
```

Note that I have kept all code platform agnostic but I only had the chance to test this on Linux Mint.

## Solution Structure

There are three projects, the ReportUI project is the executable console program and only serves as a presentation layer.

The ReportEngine project combines data layer concerns (file io) as well as the domain logic, to avoid overcomplicating the structure unnecessarily.

Finally a ReportEngineTests project is available with a set of unit tests as well as some minimal mocking to decouple file IO from some of the unit tests and make the tests cleaner to read.

## Approach

The solution ultimately uses regex to parse the log file. The original approach taken was to leverage a third party library called IISParser. Unfortuantely, it does not work for our use case and it was going to make testing difficult down the road since it does not offer a layer of abstraction from the filesystem (i.e. the ParserEngine constructor will only accept a filepath instead of a streamreader, memorystream, etc)

IISParser returned 0 records from engine.ParseLog(). This is because it expects a header row (see code in IISParser's ParserEngine.cs `T? ProcessLine<T>(string line, Func<T> factory)`). We could work around this by either injecting a header row at the top of our file, but we may not want to mutate the log file if it's being appended to live. We could alternatively make a temporary copy of the log file, inject the header row and clean up afterward, which would be a fast hacky solution but may not be desirable particularly if the log can grow large.

## Assumptions

The biggest assumption is around the size of the log file and how appropriate it will be to load it into memory all at once. I kept an approach in the data layer that allows an enumerable iterator to read through the file without loading it all into memory, but in the domain layer when we start processing reports the implementation becomes more complex and I instead opted to go for a very clean and simple LINQ implementation which requires us to enumerate the ienumerable all at once, which otherwise would need to be re-done if we needed to make this more memory efficient.

## Areas for improvement

Other than the above, I've left a few TODOs scattered around the codebase for appropriate improvements that I didn't have time for. Converting the FileDataProvider class to async using the StreamReader is an obvious improvement for responsiveness, a large file will currently lock up the main thread. 

Making use of an ILogger to record invalid data records, as some corrupt data is present in the log.

Another is in the reports we have hardcoded strings in English, ideally these should be getting pulled from a resource file which enables future localisation work.

Lastly, setting up dependency injection in ReportUI. All the appropriate classes have been structured with interfaces to easily allow for this.

