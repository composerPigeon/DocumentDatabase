namespace DatabaseNS.ResultNS.Messages;

using DatabaseNS.Components.Values;
using DatabaseNS.Tokenization;

// Contains all messages for errors
public static class ErrorMessages {
    //Missing & exists components
    internal static Message DocumentMissing(ComponentName documentName) {
        return new Message($"Document '{documentName}' does not exist in current collection.");
    }
    internal static Message DocumentExists(ComponentName documentName) {
        return new Message($"Document '{documentName}' already exist in current collection.");
    }
    internal static Message CollectionMissing(ComponentName collectionName) {
        return new Message($"Collection '{collectionName}' does not exist in current database.");
    }
    internal static Message CollectionExists(ComponentName collectionName) {
        return new Message($"Collection '{collectionName}' already exist in current database.");
    }
    internal static Message ComponentNameMissing() {
        return new Message("Component name is missing.");
    }
    internal static Message LoadDocumentsSomeExisted(ComponentName collection) {
        return new Message($"Some of the loaded documents to the collection '{collection}' already existed.");
    }
    internal static Message LoadDocumentsEmpty(ComponentName collection) {
        return new Message($"No text files were found in directory when loading documents to collection '{collection}'.");
    }
    internal static Message LoadDocumentsInvalidDirectory(ComponentPath path) {
        return new Message($"Inputted path '{path}' for load command is not valid directory.");
    }

    // ====== Command parse errors ======
    internal static Message CommandParseCommandEmpty() {
        return new Message("Command is empty.");
    }
    internal static Message CommandParseInvalidToken(Token token) {
        return new Message($"Unexpected token '{token}' occured during command parsing process.");
    }
    internal static Message CommandParseInvalidState() {
        return new Message("Command parsing process appeared in unexpected state.");
    }

    // ====== Invalid values ======
    internal static Message CommandInvalid(string command) {
        return new Message($"Invalid command '{command}' was inputted.");
    }
    internal static Message ComponentNameInvalid(string componentName, ComponentName name) {
        return new Message($"Name '{name}' is invalid for {componentName}");
    }
    internal static Message TreshholdInvalidFormatValue(string value) {
        return new Message($"Inputted value '{value}' of treshhold has invalid format.");
    }
    internal static Message TreshholdInvalidInterval(double value) {
        return new Message($"Value '{value}' must be in range from 0 to 1.");
    }
    internal static Message QueryInvalid(int queryLen, int documentLen) {
        return new Message($"Query with length '{queryLen}' and document with length '{documentLen}' does not match.");
    }

    // ====== Load errors ======
    internal static Message IndexLoad(ComponentName collectionName) {
        return new Message($"Index for collection '{collectionName}' couldn't be loaded.");
    }
    internal static Message StatsLoad(ComponentName collectionName, ComponentName documentName) {
        return new Message($"Document statistics '{documentName}' from the collection '{collectionName}' couldn't be loaded.");
    }

    // ====== File system access errors ======
    internal static Message CollectionDirectoryCreate(ComponentName collectionName) {
        return new Message($"Directory for collection '{collectionName}' couldn't be created.");
    }

    internal static Message CollectionDirectoryRemove(ComponentName collectionName) {
        return new Message($"Directory for collection '{collectionName}' couldn't be removed.");
    }

    internal static Message IndexDirectoryCreate(ComponentName indexName) {
        return new Message($"Index directory for index '{indexName}' coudln't be created.");
    }

    internal static Message DocumentFileCreate(ComponentName documentName) {
        return new Message($"File for document '{documentName}' couldn't be created.");
    }

    internal static Message DocumentFileRemove(ComponentName documentName) {
        return new Message($"File for document '{documentName}' couldn't be removed.");
    }

    internal static Message DocumentFileRead(ComponentName documentName) {
        return new Message($"File for document '{documentName}' couldn't be read.");
    }

    internal static Message FileRead(ComponentPath path) {
        return new Message($"File '{path}' coudln't be opened.");
    }

    internal static Message ComponentAsJsonSave(ComponentPath path) {
        return new Message($"Component '{path}' couldn't be saved into json file.");
    }

    internal static Message ComponentFromJsonLoad(ComponentPath path) {
        return new Message($"Component '{path}' couldn't be loaded from json file.");
    }

    // ====== Component build errors ======
    internal static Message DatabaseCreate() {
        return new Message("Database couldn't be created.");
    }
    internal static Message DocumentCreate(ComponentName documentName) {
        return new Message($"Document '{documentName}' couldn't be created.");
    }
    internal static Message StatsCreate(ComponentName documentName) {
        return new Message($"Document statistics '{documentName}' couldn't be created.");
    }
    internal static Message IndexCreate(ComponentName indexName) {
        return new Message($"Index '{indexName}' couldn't be created.");
    }
    internal static Message CollectionCreate(ComponentName collectionName) {
        return new Message($"Collection '{collectionName}' couldn't be created.");
    }

    // ====== Command not supported for server
    internal static Message CommandNotSupported(string command) {
        return new Message($"Command '{command}' is not supported to use via server.");
    }

    // ===== Unexpected exception
    internal static Message UnexpectedException() {
        return new Message("Unexpected exception occured during the process.");
    }

    // ====== Invalid Command line arguments ======
    public static Message InvalidFirstArgument(string argument) {
        return new Message($"First argument must be 'server' or 'console' depends on which application you'd like to run. Argument '{argument}' is not valid.");
    }
    public static Message NoArgumentProvided() {
        return new Message("First argument must be 'server' or 'console' depends on which application you'd like to run. No argument was given.");
    }

    // ======= Invalid json on server API ======
    public static Message InvalidJsonOnEndpoint(string content) {
        return new Message($"Inputted content '{content}' is not valid json.");
    } 

}