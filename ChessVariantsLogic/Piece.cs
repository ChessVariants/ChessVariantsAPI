namespace ChessVariantsLogic;

/// <summary>
/// Represents a piece with a fixed movement pattern.
/// </summary>
public class Piece
{
#region Fields, properties and constructors
    private readonly MovementPattern movementPattern;
    private readonly MovementPattern capturePattern;
    private readonly bool royal;
    private readonly PieceClassifier pieceClassifier;
    private bool hasMoved;

    private string pieceIdentifier;

    private readonly int repeat;

    public bool Royal
    {
        get { return this.royal; }
    }

    public PieceClassifier PieceClassifier
    {
        get { return this.pieceClassifier;}
    }

    public int Repeat
    {
        get { return this.repeat; }
    }

    public string PieceIdentifier
    {
        get { return this.pieceIdentifier; }
    }

    /// <summary>
    /// Constructor for a new Piece.
    /// </summary>
    /// <param name="movementPattern">is the custom movement pattern of the type MovementPattern</param>
    /// <param name="capturePattern">is the custom capture pattern of the type MovementPattern</param>
    /// <param name="royal">set true if the piece is royal</param>
    /// <param name="pc">is the player the piece belongs to</param>
    /// <param name="hasMoved">set true if the piece has previously moved</param>
    /// <param name="repeat">is the amount of times the movement pattern can be repeated on the same turn</param>
    /// <param name="pieceIdentifier">is the unique string representation of the piece</param>
    public Piece(MovementPattern movementPattern, MovementPattern capturePattern, bool royal, PieceClassifier pc, bool hasMoved, int repeat, string pieceIdentifier)
    {
        this.movementPattern = movementPattern;
        this.capturePattern = capturePattern;
        this.royal = royal;
        this.pieceClassifier = pc;
        this.hasMoved = hasMoved;
        this.repeat = repeat;
        this.pieceIdentifier = pieceIdentifier;
    }

    public Piece(MovementPattern movementPattern, MovementPattern capturePattern, bool royal, PieceClassifier pc, int repeat, string pieceIdentifier)
    : this(movementPattern, capturePattern, royal, pc, false, repeat, pieceIdentifier) {}
    
    public Piece(MovementPattern movementPattern, MovementPattern capturePattern, bool royal, PieceClassifier pc, string pieceIdentifier)
    : this(movementPattern, capturePattern, royal, pc, false, 0, pieceIdentifier) {}

#endregion

    /// <summary>
    /// Gets a specific movement pattern by index.
    /// </summary>
    /// <param name="index" is the index of the movement pattern></param>
    /// <returns>the movement pattern at <paramref name="index"/> if the index is valid, otherwise null.</returns>
    public IPattern? GetMovementPattern(int index)
    {
        return this.movementPattern.GetPattern(index);
    }

    /// <summary>
    /// Gets a specific capture pattern by index.
    /// </summary>
    /// <param name="index" is the index of the capture pattern></param>
    /// <returns>the capture pattern at <paramref name="index"/> if the index is valid, otherwise null.</returns>
    public IPattern? GetCapturePattern(int index)
    {
        return this.capturePattern.GetPattern(index);
    }

    /// <summary>
    /// Yield returns all IPatterns existing in this movement pattern.
    /// </summary>
    /// <returns>each IPattern in this movement pattern individually.</returns>
    public IEnumerable<IPattern> GetAllMovementPatterns()
    {
        return this.movementPattern.GetAllPatterns();
    }

    /// <summary>
    /// Yield returns all IPatterns existing in this capture pattern.
    /// </summary>
    /// <returns>each IPattern in this capture pattern individually.</returns>
    public IEnumerable<IPattern> GetAllCapturePatterns()
    {
        return this.capturePattern.GetAllPatterns();
    }

    /// <summary>
    /// Checks that this and <paramref name="other"/> are of opposite colors.
    /// </summary>
    /// <param name="other"> is the other piece</param>
    /// <returns> true if this is of opposite color than other, otherwise false.</returns>
    public bool CanTake(Piece other)
    {
        return !this.pieceClassifier.Equals(other.pieceClassifier);
    }

#region Static methods

    /// <summary>
    /// Creates a Piece object that behaves like a standard rook.
    /// </summary>
    /// <param name="pieceClassifier"> is the side that the rook belongs to</param>
    /// <returns> an instance of Piece with the movement pattern of a standard rook.</returns>
    public static Piece Rook(PieceClassifier pieceClassifier)
    {
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North, 1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.East,  1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.South, 1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.West,  1, Constants.MaxBoardHeigth)
        };
        var mp = new MovementPattern(patterns);

        if(pieceClassifier.Equals(PieceClassifier.WHITE))
            return new Piece(mp, mp,  false, pieceClassifier, Constants.WhiteRookIdentifier);
        return new Piece(mp, mp, false, pieceClassifier, Constants.BlackRookIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard bishop.
    /// </summary>
    /// <param name="pieceClassifier"> is the side that the bishop belongs to</param>
    /// <returns> an instance of Piece with the movement pattern of a standard bishop.</returns>
    public static Piece Bishop(PieceClassifier pieceClassifier)
    {
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.NorthEast,  1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.SouthEast,  1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.SouthWest,  1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.NorthWest,  1, Constants.MaxBoardHeigth)
        };
        var mp = new MovementPattern(patterns);

        if(pieceClassifier.Equals(PieceClassifier.WHITE))
            return new Piece(mp, mp, false, pieceClassifier, Constants.WhiteBishopIdentifier);
        return new Piece(mp, mp, false, pieceClassifier, Constants.BlackBishopIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard queen.
    /// </summary>
    /// <param name="pieceClassifier"> is the side that the queen belongs to</param>
    /// <returns> an instance of Piece with the movement pattern of a standard queen.</returns>
    public static Piece Queen(PieceClassifier pieceClassifier)
    {
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North,     1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.NorthEast, 1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.East,      1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.SouthEast, 1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.South,     1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.SouthWest, 1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.West,      1, Constants.MaxBoardHeigth),
            new RegularPattern(Constants.NorthWest, 1, Constants.MaxBoardHeigth)
        };
        var mp = new MovementPattern(patterns);

        if(pieceClassifier.Equals(PieceClassifier.WHITE))
            return new Piece(mp, mp, false, pieceClassifier, Constants.WhiteQueenIdentifier);
        return new Piece(mp, mp, false, pieceClassifier, Constants.BlackQueenIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard king.
    /// </summary>
    /// <param name="pieceClassifier"> is the side that the king belongs to</param>
    /// <returns> an instance of Piece with the movement pattern of a standard king.</returns>
    public static Piece King(PieceClassifier pieceClassifier)
    {
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North,     1, 1),
            new RegularPattern(Constants.NorthEast, 1, 1),
            new RegularPattern(Constants.East,      1, 1),
            new RegularPattern(Constants.SouthEast, 1, 1),
            new RegularPattern(Constants.South,     1, 1),
            new RegularPattern(Constants.SouthWest, 1, 1),
            new RegularPattern(Constants.West,      1, 1),
            new RegularPattern(Constants.NorthWest, 1, 1)
        };
        var mp = new MovementPattern(patterns);
        if(pieceClassifier.Equals(PieceClassifier.WHITE))
            return new Piece(mp, mp, true, pieceClassifier, Constants.WhiteKingIdentifier);
        return new Piece(mp, mp, true, pieceClassifier, Constants.BlackKingIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard knight.
    /// </summary>
    /// <param name="pieceClassifier"> is the side that the knight belongs to</param>
    /// <returns> an instance of Piece with the movement pattern of a standard knight.</returns>
    public static Piece Knight(PieceClassifier pieceClassifier)
    {
        var pattern = new List<IPattern> {
            new JumpPattern( 1, 2),
            new JumpPattern( 2, 1),
            new JumpPattern( 1,-2),
            new JumpPattern( 2,-1),
            new JumpPattern(-1, 2),
            new JumpPattern(-2, 1),
            new JumpPattern(-1,-2),
            new JumpPattern(-2,-1),
        };
        var mp = new MovementPattern(pattern);
        if(pieceClassifier.Equals(PieceClassifier.WHITE))
            return new Piece(mp, mp, false, pieceClassifier,Constants.WhiteKnightIdentifier);
        return new Piece(mp, mp, false, pieceClassifier,Constants.BlackKnightIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard black pawn.
    /// </summary>
    /// <returns>an instance of Piece with the movement pattern of a standard black pawn.</returns>
    public static Piece BlackPawn()
    {
        var capturePatterns = new List<IPattern> {
            new RegularPattern(Constants.SouthEast, 1, 1),
            new RegularPattern(Constants.SouthWest, 1, 1)
        };
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.South, 1,1),
        };
        var mp = new MovementPattern(patterns);
        var cp = new MovementPattern(capturePatterns);
        return new Piece(mp, cp, false, PieceClassifier.BLACK, Constants.BlackPawnIdentifier);
    }

    /// <summary>
    /// Creates a Piece object that behaves like a standard white pawn.
    /// </summary>
    /// <returns>an instance of Piece with the movement pattern of a standard white pawn.</returns>
    public static Piece WhitePawn()
    {
        var capturePatterns = new List<IPattern> {
            new RegularPattern(Constants.NorthEast, 1, 1),
            new RegularPattern(Constants.NorthWest, 1, 1)
        };
        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North, 1,1),
        };
        var mp = new MovementPattern(patterns);
        var cp = new MovementPattern(capturePatterns);
        return new Piece(mp, cp, false, PieceClassifier.WHITE, Constants.WhitePawnIdentifier);
    }

    /// <summary>
    /// Instantiates a HashSet of all the standard pieces.
    /// </summary>
    /// <returns>A HashSet containing all the standard pieces.</returns>
    public static HashSet<Piece> AllStandardPieces()
    {
        return new HashSet<Piece> 
        {
            Rook(PieceClassifier.WHITE), Rook(PieceClassifier.BLACK),
            Knight(PieceClassifier.WHITE), Knight(PieceClassifier.BLACK),
            Bishop(PieceClassifier.WHITE), Bishop(PieceClassifier.BLACK),
            Queen(PieceClassifier.WHITE), Queen(PieceClassifier.BLACK),
            King(PieceClassifier.WHITE), King(PieceClassifier.BLACK),
            WhitePawn(), BlackPawn()
        };
    }
    
#endregion

}