namespace Database_CommandParser;

using Database_DbComponents;

internal enum CommandType {
    CreateColletion, // CREATE collectionName;
    DropCollection, // DROP collectionName;
    GetDocument, // GET documentName FROM collectionName;
    AddDocument, // ADD fileName AS documentName TO collectionName;
    //BulkLoadDocuments, // LOAD fileName1, fileName2, ... AS documentName1, documentName2, ... TO collectionName;
    RemoveDocument, // REMOVE documentName FROM collectionName
    //BulkRemoveDocuments,
    Find,
    Start,
    Exit,
    ShutDown
}

internal struct Command {
    public CommandType Type {get; init;}

    public ComponentName? CollectionName {get; init;}

    public ComponentName? DocumentName {get; init;}
    public string[] Content {get; init;}
}