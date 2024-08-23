using System.Diagnostics.CodeAnalysis;

namespace DatabaseNS.Tokenization;

// Represents one unit of text stream, usually word or EOF
internal readonly struct Token : IEquatable<Token> {
    public string? Word {get; }
    public bool IsLast {get;}

    public Token(string word) {
        Word = word;
        IsLast = false;
    }

    private Token(string? word, bool isLast) {
        Word = word;
        IsLast = isLast;
    }

    public bool Equals(Token other) {
        if (IsLast == other.IsLast) {
            if (IsLast)
                return true;
            else 
                return Word == other.Word;
        }
        return false;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        if (obj != null && obj is Token other) {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode() {
        if (!IsLast && Word != null)
            return Word.GetHashCode();
        return "\n".GetHashCode();
    }

    public override string ToString() {
        if (Word == null && IsLast)
            return "EOF";
        else if (Word == null)
            return "";
        else
            return Word;
    }

    public static bool operator ==(Token left, Token right) {
        return left.Equals(right);
    }

    public static bool operator !=(Token left, Token right) {
        return !left.Equals(right);
    }

    public static Token Last {
        get { return new Token(null, true); }
    }
    public static Token From {
        get { return new Token("from", false); }
    }
    public static Token To {
        get { return new Token("to", false); }
    }
    public static Token In {
        get { return new Token("in", false); }
    }
    public static Token As {
        get { return new Token("as", false); }
    }
}