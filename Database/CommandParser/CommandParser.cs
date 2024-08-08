namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.CommandParserNS.Commands;

internal static class CommandParser {

    public static Command Parse(string command) {
        TokenReader reader = new CommandTokenReader(command);
        State state = new StartState();

        Token token = reader.Read();
        do {
            state = state.NextState(token);
            token = reader.Read();
        } while (!token.IsLast);

        if (state is FinalState finalState)
            return finalState.GetCommand(command);
        else
            throw Handlers.Error.ThrowCommandInvalid(command);
    }
}
