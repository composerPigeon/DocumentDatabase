namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.CommandParserNS.Commands;

internal abstract class State {
    protected CommandBuilder builder;
    protected State() {
        builder = new CommandBuilder();
    }

    protected State(State state) {
        builder = state.builder;
    }

    public abstract State NextState(Token token);
}