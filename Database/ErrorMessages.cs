namespace DatabaseNS;

internal static class ErrorMessages {
    public static readonly string DOCUMENT_MISSING = "Document does not exist in current collection.";
    public static readonly string DOCUMENT_EXIST = "Document already exist in current database.";
    public static readonly string COLLECTION_MISSING = "Collection does not exist in current database.";
    public static readonly string COLLECTION_EXIST = "Collection already exist in current database.";


    public static readonly string COMMANDPARSE_EOF = "Unexpected EOF occured during command parsing process.";
    public static readonly string COMMANDPARSE_INVALID_TOKEN = "Unexpected token occured during command parsing process.";
    public static readonly string COMMANDPARSE_LONG = "Command is too long.";
    public static readonly string COMMAND_INVALID = "Invalid command inputed.";

    public static readonly string TRESHHOLD_INVALID_VALUE = "Inputed value of treshhold has invalid format. It must be double.";

    public static readonly string QUERY_INVALID = "Invalid query.";

    public static readonly string INDEX_LOAD = "Index for some collection couldn't be loaded.";
    public static readonly string STATS_LOAD = "Document statistics from some of the collections couldn't be loaded.";

    public static readonly string DATABASE_CREATE = "Database couldn't be created.";
    public static readonly string DOCUMENT_CREATE = "Document from some of the collections couldn't be created.";
    public static readonly string STATS_CREATE = "Document statistics couldn't be created.";
    public static readonly string INDEX_CREATE = "Index for some collection couldn't be created.";
    public static readonly string COLLECTION_CREATE = "Some of the collections couldn't be created.";


}