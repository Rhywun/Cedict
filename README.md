# Cedict
Cedict is a small, cross-platform client/server application which features a Chinese-English dictionary lookup.

The dictionary is sourced from the [CC-CEDICT](https://en.wikipedia.org/wiki/CEDICT) project, "with the aim to provide a complete Chinese to English dictionary with pronunciation in pinyin for the Chinese characters." ([Project home](https://cc-cedict.org/))

The server is a REST-style .NET Core Web API implementation. The client is an Angular implementation (`ClientApp` directory).

## Requirements
- [.NET Core](https://dotnet.microsoft.com/) >= 3.1
- [Node](https://nodejs.org/) >= 13.7.0
- Other dependencies will be downloaded during installation and build (see below).
- The CEDICT Sqlite database needs to be provided and the connection string specified in the server's `appsettings` file. I chose to generate the database using the [cedict_to_sqlite](https://github.com/matthin/cedict_to_sqlite) project.

## Installation
- Copy the CEDICT Sqlite database (described above) to the `Cedict` project directory. (It will be copied to the `bin` directory upon building.)
- Open a terminal and run `npm install` from the `ClientApp` directory to install the Angular client dependencies.
- Build the solution - the server dependencies will be installed, including:
    - `EntityFrameworkCore`, `EntityFrameworkCore.Sqlite`, and `System.Linq.Dynamic.Core`
    - [LinqKit](http://www.albahari.com/nutshell/linqkit.aspx)
- Launch the solution. The server and client launch together.

## Usage
- Enter a search term in the text box. The search term can be any one of:
    - Mandarin character(s) ("traditional" or "simplified")
    - Pinyin (with or without tone markers such as "1", "2", "3", etc. - see below regarding the "Ignore Tones" option)
    - English
- You can use the wildcard character ("*") before and/or after the search term in order to perform "starts with", "ends with", or "contains" searches.
- Two kinds of searches are supported:
    - Literal search - the default. The dictionary is searched for the term as entered.
    - Multi-character search - tick the "Multi-character search" option. With this option selected, a search for a term that includes multiple Mandarin characters will return the combination of the search results for each individual character. This is useful to better understand the individual components of a Chinese word or phrase that consists of multiple characters.
 - Check the "Ignore Tones" option if you are searching for a Pinyin term and wish to ignore tones. For example, a search for "zi" will return results for "zi1", "zi2", "zi3", and so forth.

## Roadmap
- ~~Complete the asynchronous support~~
- ~~Add pagination and sorting to the search results table~~
- Add better formatting to the search results
