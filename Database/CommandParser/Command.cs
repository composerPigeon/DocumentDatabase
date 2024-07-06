namespace DatabaseNS.CommandParserNS;

using DatabaseNS.Components;

internal enum CommandType {
    CreateColletion, // CREATE collectionName;
    DropCollection, // DROP collectionName;
    UpdateCollection,
    GetDocument, // GET documentName FROM collectionName;
    AddDocument, // ADD fileName AS documentName TO collectionName;
    RemoveDocument, // REMOVE documentName FROM collectionName
    Find,
    Start,
    Exit,
    SetDirectory,
    ShutDown
}

internal struct Command {
    public CommandType Type {get; init;}

    public ComponentName? CollectionName {get; init;}

    public ComponentName? DocumentName {get; init;}
    public string[] Content {get; init;}
}