namespace Database_Tokenization;

using System.Runtime.InteropServices;
using System.Text;
using Database_CommandParser;
using Database_DbComponents;

internal enum ReaderState {
    InWord,
    InContent,
    Out
}

internal interface TokenReader {
    public Token Read();
}