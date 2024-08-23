namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.CommandParserNS.Commands;

// Parse inputted string to a valid Command instance
internal static class CommandParser {

    public static Command Parse(string command) {
        using (TokenReader reader = new CommandTokenReader(command)) {
            State state = new StartState();

            Token token = reader.Read();
            while (true) {
                state = state.NextState(token);
                if (token.IsLast)
                    break;
                token = reader.Read();
            }

            if (state is FinalState finalState)
                return finalState.GetCommand(command);
            else
                throw Handlers.Exception.ThrowCommandInvalid(command);
        }
    }
}
