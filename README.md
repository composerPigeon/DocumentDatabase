# Document database
This piece of software is simple console database for storing plain text documents and they can be serached based on key words specified by user. Whole system uses vector model and inverted index implementation for more effective information retrieval.

# User manual
- keywords for commands are written here in uppercase, but in fact the command processing is case independent
- text is case sensitive when it is saved as a document, but is again case independent when the text processing for text indexing is happening
- Data folder is root folder of the database
    - it contains sub-folders for each collection
        - where the index and documents are stored
    - and stopWords.json file, which is a file holding all stopwords that the database will skip during the text processing

## Commands
- `START`
    - Command which loads the index and mounts the database.
- `SHUTDOWN`
    - Command which will save the currrent index state and unmount the database.
- `EXIT`
    - Command which will do the `SHUTDOWN` command (if the database was mounted before) and exits the console
- `CREATE <collectionName>`
    - Command which will create a collection of specified name
- `DROP <collectionName>`
    - Command which will delete existent collection of specified name
- `TRESHHOLD <value> FOR <collectionName>`
    - sets new query treshhold for specified collection
- `ADD ${<documentContent>}$ AS <documentName> TO <colllectionName>`
    - Command which will create new document of given name and content in specified collection
    - `${...}$` are content delimiters and anything between them is taken as it is and saved as a document content
- `GET <documentNam> FROM <colectionName>`
    - Command which will priints out the exact content for the specified document in collection
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

- after each command the message about successfull result is printed out
    - for adding document that the document was created, for removing that the document was removed, ...
    - after get document command just the docuemnts content is printed out
    - in other cases some error message should be printed out