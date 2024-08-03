namespace DatabaseNS.ResultNS.Messages;

using DatabaseNS.FileSystem;
using DatabaseNS.Tokenization;

internal static class ErrorMessages {
    //Missing & exists components
    public static Message DocumentMissing(ComponentName documentName) {
        return new Message(
            string.Format("Document '{0}' does not exist in current collection.", documentName)
        );
    }
    public static Message DocumentExists(ComponentName documentName) {
        return new Message(
            string.Format("Document '{0}' already exist in current collection.", documentName)
        );
    }
    public static Message CollectionMissing(ComponentName collectionName) {
        return new Message(
            string.Format("Collection '{0}' does not exist in current database.", collectionName)
        );
    }
    public static Message CollectionExists(ComponentName collectionName) {
        return new Message(
            string.Format("Collection '{0}' already exist in current database.", collectionName)
        );
    }
    public static Message ComponentNameMissing() {
        return new Message("Component name is missing.");
    }

    //Command parse errors
    public static Message CommandParseShort() {
        return new Message("Unexpected end of command.");
    }
    public static Message CommandParseInvalidToken(Token token) {
        return new Message(
            string.Format("Unexpected token '{0}' occured during command parsing process.", token)
        );
    }
    public static Message CommandParseLong() {
        return new Message("Command is too long.");
    }
    public static Message CommandParseInvalidState() {
        return new Message("Command parsing process appeared in unexpected state.");
    }

    //Invalid values
    public static Message CommandInvalid(string command) {
        return new Message(
            string.Format("Invalid command '{0}' was inputted.", command)
        );
    }
    public static Message NameInvalid(ComponentName name) {
        return new Message(
            string.Format("Invalid name {0} was inputted.", name)
        );
    }
    public static Message TreshholdInvalidFormatValue(string value) {
        return new Message(
            string.Format("Inputted value '{0}' of treshhold has invalid format.", value)
        );
    }
    public static Message QueryInvalid(string query) {
        return new Message(
            string.Format("Invalid query '{0}' was inputted.", query)
        );
    }

    //Load errors
    public static Message IndexLoad(ComponentName collectionName) {
        return new Message(
            string.Format("Index for collection '{0}' couldn't be loaded.", collectionName)
        );
    }
    public static Message StatsLoad(ComponentName collectionName, ComponentName documentName) {
        return new Message(
            string.Format("Document statistics '{0}' from the collection '{1}' couldn't be loaded.", documentName, collectionName)
        );
    }

    //Build errors
    public static Message DatabaseCreate() {
        return new Message("Database couldn't be created.");
    }
    public static Message DocumentCreate(ComponentName collectionName, ComponentName documentName) {
        return new Message(
            string.Format("Document '{0}' from the collection '{1}' couldn't be created.", documentName, collectionName)
        );
    }
    public static Message StatsCreate(ComponentName collectionName, ComponentName documentName) {
        return new Message(
            string.Format("Document statistics '{0}' from the collection '{1}' couldn't be created.", documentName, collectionName)
        );
    }
    public static Message IndexCreate(ComponentName indexName) {
        return new Message(
            string.Format("Index '{0}' couldn't be created.", indexName)
        );
    }
    public static Message CollectionCreate(ComponentName collectionName) {
        return new Message(
            string.Format("Collection '{0}' couldn't be created.", collectionName)
        );
    }
}