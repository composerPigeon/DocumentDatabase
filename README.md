# Document database
This piece of software is simple database for storing plain text documents and they can be searched based on key words specified by user. Whole system uses vector model and inverted index implementation for more effective information retrieval.

# Installation
- When you fork and download this project program expects folder `data` to be present and have unlimited access to (if not, it will create one)
- after downloading run command `dotnet run --project Server/Server.csproj` which will trigger the database and then it should be running as a server
- if you would like to run database as a console application, then run `dotnet run --project Database/Database.csproj`

# API
- server will run on localhost:4696 (if not, the correct url will be displayed in console when the database load)
- `POST /`
    - endpoint which will accept json dcoument in body with command
    - json must have field "command" and value of this must be one of the commands from the list below
    - Result of this is json with field "ok" or "error", value of it is some message which comments result or error of the command

# User tips
- keywords for commands are written here in uppercase, but in fact the command processing is case independent
- text is case sensitive when it is saved as a document, but is again case independent when the text processing for text indexing is happening

## Commands
- `LIST`
    - Command which will print out names of all collections in database
- `LIST <collectionName>`
    - Command which will print out all names of documents in specified collection
- `CREATE <collectionName>`
    - Command which will create a collection of specified name
- `REMOVE <collectionName>`
    - Command which will delete existent collection of specified name
- `TRESHHOLD <value> FOR <collectionName>`
    - sets new query treshhold for specified collection
- `LOAD <directory> TO <collectionName>`
    - Command which will work only in console application
    - Takes all txt documents in inputted directory and bulk load them as new documents to specified collection
        - if specified collection do not exist, then it will create new one
        - it will end with error if the directory does not conatin any txt files
- `ADD ${<documentContent>}$ AS <documentName> TO <colllectionName>`
    - Command which will create new document of given name and content in specified collection
    - `${...}$` are content delimiters and anything between them is taken as it is and saved as a document content
- `GET <documentName> FROM <colectionName>`
    - Command which will print out the exact content for the specified document in collection
- `REMOVE <documentName> FROM <collectionName>`
    - Command for deleting specified document from the collection
- `FIND <keyWords> IN <collectionName>`
    - Command for finding keywords in collection
    - it will print out list of pairs
        - where the pair consists of a score, which indicates how relevant the document is for the query and docuement name for that
    - if you will search for key words where none of them are present in collection, then the result will be empty and nothing will be printed
    - on the other hand if you will search for word which is present in all of the documents the score will be NaN
    - `<keyWords>` should be a list of key words, where all have to be delimited by at least one space
        - be careful, that key word `in` will be taken as a command key word and therefore the next token will be considered as a collection name. This word is also present in stopWords so it isn't great idea to search for it and it is also reason why this fact is ignored