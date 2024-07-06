namespace DatabaseNS.CommandParserNS;

using DatabaseNS.Tokenization;
using DatabaseNS;

public class CommandParseException : Exception {
    public CommandParseException(string message) : base(message) { }
    public CommandParseException(string message, Exception innerException) : base(message, innerException) { }
}

internal static class CommandParser {

    public static Command Parse(string command) {
        TokenReader reader = new CommandTokenReader(command);
        State state = new StartState();

        Token token = reader.Read();
        while (!token.IsLast) {
            state = state.NextState(token);
            token = reader.Read();
        }

        if (state is FinalState finalState)
            return finalState.GetCommand();
        else
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_LONG);
    }
}
