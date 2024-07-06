namespace DatabaseNS.Tokenization;

internal struct Token {
    public string? Word {get; }

    public bool IsLast {get;}

    public Token(string word) {
        Word = word;
        IsLast = false;
    }

    public Token() {
        IsLast = true;
    }

}