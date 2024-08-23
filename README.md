# Document database
This piece of software is simple database for storing plain text documents and they can be searched based on key words specified by user. Whole system uses vector model and inverted index implementation for more effective information retrieval.

## Installation
- When you fork and download this project program expects folder `data` to be present and have unlimited access to (if not, it will create one)
- after downloading run command `dotnet run --project Server/Server.csproj server` which will trigger the database and then it should be running as a server
- if you would like to run database as a console application, then run `dotnet run --project Server/Server.csproj console`

## API
- server will run on localhost:4696 (if not, the correct url will be displayed in console when the database load)
- `POST /`
    - endpoint which will accept json dcoument in body with command
    - json must have field "command" and value of this must be one of the commands from the list below
    - Result of this is json with field "ok" or "error", value of it is some message which comments result or error of the command

## User tips
- keywords for commands are written here in uppercase, but in fact the command processing is case independent
- text is case sensitive when it is saved as a document, but is again case independent when the text processing for text indexing is happening

### Commands
- `LIST`
    - Command which will print out names of all collections in database
- `LIST <collectionName>`
    - Command which will print out all names of documents in specified collection
- `CREATE <collectionName>`
    - Command which will create a collection of specified name
- `REMOVE <collectionName>`
    - Command which will delete existent collection of specified name
- `TRESHHOLD <value> FOR <collectionName>`
    - sets new query treshhold for specified collection
- `LOAD ${<directory>}$ TO <collectionName>`
    - Command which will work only in console application
    - Takes all txt documents in inputted directory and bulk load them as new documents to specified collection
        - if specified collection do not exist, then it will create new one
        - it will end with error if the directory does not conatin any txt files
    - `${...}$` are delimiters and anything between them is taken as it is and used as a directory path
- `ADD ${<documentContent>}$ AS <documentName> TO <colllectionName>`
    - Command which will create new document of given name and content in specified collection
    - `${...}$` are content delimiters and anything between them is taken as it is and saved as a document content
- `GET <documentName> FROM <colectionName>`
    - Command which will print out the exact content for the specified document in collection
- `REMOVE <documentName> FROM <collectionName>`
    - Command for deleting specified document from the collection
- `FIND <keyWords> IN <collectionName>`
    - Command for finding keywords in collection
    - it will print out list of pairs
        - where the pair consists of a score, which indicates how relevant the document is for the query and docuement name for that
    - if you search for key words where none of them are present in collection, then the result will be empty and nothing will be printed
    - if you search for key word which will be present in all of the documents in collection, the result will be empty aswell
        - score for each document is Nan, which is uncomaparable with query treshhold and thus none of the documents will do it to the result
    - `<keyWords>` should be a list of key words, where all have to be delimited by at least one space
        - be careful, that key word `in` will be taken as a command key word and therefore the next token will be considered as a collection name. This word is also present in stopWords so it isn't great idea to search for it and it is also reason why this fact is ignored

### Stopwords
```
"able","about","above","abroad","according","accordingly","across","actually","adj","after","afterwards","again","against","ago","ahead","ain't","all","allow","allows","almost","alone","along","alongside","already","also","although","always","am","amid","amidst","among","amongst","an","and","another","any","anybody","anyhow","anyone","anything","anyway","anyways","anywhere","apart","appear","appreciate","appropriate","are","aren't","around","as","a's","aside","ask","asking","associated","at","available","away","awfully","back","backward","backwards","be","became","because","become","becomes","becoming","been","before","beforehand","begin","behind","being","believe","below","beside","besides","best","better","between","beyond","both","brief","but","by","came","can","cannot","cant","can't","caption","cause","causes","certain","certainly","changes","clearly","c'mon","co","co.","com","come","comes","concerning","consequently","consider","considering","contain","containing","contains","corresponding","could","couldn't","course","c's","currently","dare","daren't","definitely","described","despite","did","didn't","different","directly","do","does","doesn't","doing","done","don't","down","downwards","during","each","edu","eg","eight","eighty","either","else","elsewhere","end","ending","enough","entirely","especially","et","etc","even","ever","evermore","every","everybody","everyone","everything","everywhere","ex","exactly","example","except","fairly","far","farther","few","fewer","fifth","first","five","followed","following","follows","for","forever","former","formerly","forth","forward","found","four","from","further","furthermore","get","gets","getting","given","gives","go","goes","going","gone","got","gotten","greetings","had","hadn't","half","happens","hardly","has","hasn't","have","haven't","having","he","he'd","he'll","hello","help","hence","her","here","hereafter","hereby","herein","here's","hereupon","hers","herself","he's","hi","him","himself","his","hither","hopefully","how","howbeit","however","hundred","i'd","ie","if","ignored","i'll","i'm","immediate","in","inasmuch","inc","inc.","indeed","indicate","indicated","indicates","inner","inside","insofar","instead","into","inward","is","isn't","it","it'd","it'll","its","it's","itself","i've","just","k","keep","keeps","kept","know","known","knows","last","lately","later","latter","latterly","least","less","lest","let","let's","like","liked","likely","likewise","little","look","looking","looks","low","lower","ltd","made","mainly","make","makes","many","may","maybe","mayn't","me","mean","meantime","meanwhile","merely","might","mightn't","mine","minus","miss","more","moreover","most","mostly","mr","mrs","much","must","mustn't","my","myself","name","namely","nd","near","nearly","necessary","need","needn't","needs","neither","never","neverf","neverless","nevertheless","new","next","nine","ninety","no","nobody","non","none","nonetheless","noone","no-one","nor","normally","not","nothing","notwithstanding","novel","now","nowhere","obviously","of","off","often","oh","ok","okay","old","on","once","one","ones","one's","only","onto","opposite","or","other","others","otherwise","ought","oughtn't","our","ours","ourselves","out","outside","over","overall","own","particular","particularly","past","per","perhaps","placed","please","plus","possible","presumably","probably","provided","provides","que","quite","qv","rather","rd","re","really","reasonably","recent","recently","regarding","regardless","regards","relatively","respectively","right","round","said","same","saw","say","saying","says","second","secondly","see","seeing","seem","seemed","seeming","seems","seen","self","selves","sensible","sent","serious","seriously","seven","several","shall","shan't","she","she'd","she'll","she's","should","shouldn't","since","six","so","some","somebody","someday","somehow","someone","something","sometime","sometimes","somewhat","somewhere","soon","sorry","specified","specify","specifying","still","sub","such","sup","sure","take","taken","taking","tell","tends","th","than","thank","thanks","thanx","that","that'll","thats","that's","that've","the","their","theirs","them","themselves","then","thence","there","thereafter","thereby","there'd","therefore","therein","there'll","there're","theres","there's","thereupon","there've","these","they","they'd","they'll","they're","they've","thing","things","think","third","thirty","this","thorough","thoroughly","those","though","three","through","throughout","thru","thus","till","to","together","too","took","toward","towards","tried","tries","truly","try","trying","t's","twice","two","un","under","underneath","undoing","unfortunately","unless","unlike","unlikely","until","unto","up","upon","upwards","us","use","used","useful","uses","using","usually","v","value","various","versus","very","via","viz","vs","want","wants","was","wasn't","way","we","we'd","welcome","well","we'll","went","were","we're","weren't","we've","what","whatever","what'll","what's","what've","when","whence","whenever","where","whereafter","whereas","whereby","wherein","where's","whereupon","wherever","whether","which","whichever","while","whilst","whither","who","who'd","whoever","whole","who'll","whom","whomever","who's","whose","why","will","willing","wish","with","within","without","wonder","won't","would","wouldn't","yes","yet","you","you'd","you'll","your","you're","yours","yourself","yourselves","you've","zero","a","how's","i","when's","why's","b","c","d","e","f","g","h","j","l","m","n","o","p","q","r","s","t","u","uucp","w","x","y","z","I","www","amount","bill","bottom","call","computer","con","couldnt","cry","de","describe","detail","due","eleven","empty","fifteen","fifty","fill","find","fire","forty","front","full","give","hasnt","herse","himse","interest","itse”","mill","move","myse”","part","put","show","side","sincere","sixty","system","ten","thick","thin","top","twelve","twenty","abst","accordance","act","added","adopted","affected","affecting","affects","ah","announce","anymore","apparently","approximately","aren","arent","arise","auth","beginning","beginnings","begins","biol","briefly","ca","date","ed","effect","et-al","ff","fix","gave","giving","heres","hes","hid","home","id","im","immediately","importance","important","index","information","invention","itd","keys","kg","km","largely","lets","line","'ll","means","mg","million","ml","mug","na","nay","necessarily","nos","noted","obtain","obtained","omitted","ord","owing","page","pages","poorly","possibly","potentially","pp","predominantly","present","previously","primarily","promptly","proud","quickly","ran","readily","ref","refs","related","research","resulted","resulting","results","run","sec","section","shed","shes","showed","shown","showns","shows","significant","significantly","similar","similarly","slightly","somethan","specifically","state","states","stop","strongly","substantially","successfully","sufficiently","suggest","thered","thereof","therere","thereto","theyd","theyre","thou","thoughh","thousand","throug","til","tip","ts","ups","usefully","usefulness","'ve","vol","vols","wed","whats","wheres","whim","whod","whos","widely","words","world","youd","youre"
```