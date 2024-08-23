namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.CommandParserNS.Commands;

// States represent state machine of parsing query language of this document database
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