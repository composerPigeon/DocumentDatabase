namespace DatabaseNS.CommandParserNS;

using DatabaseNS.FileSystem;

internal enum CommandType {
    CreateColletion, // CREATE collectionName;
    DropCollection, // DROP collectionName;
    GetDocument, // GET documentName FROM collectionName;
    AddDocument, // ADD fileName AS documentName TO collectionName;
    RemoveDocument, // REMOVE documentName FROM collectionName
    Find,
    Load,
    Start,
    Exit,
    SetTreshhold,
    ShutDown
}

internal struct Command {
    public CommandType Type {get; init;}

    public ComponentName? CollectionName {get; init;}

    public ComponentName? DocumentName {get; init;}
    public string[] Content {get; init;}
}