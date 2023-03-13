namespace ChessVariantsLogic;

/// <summary>
/// A board state interface with useful functions that the rule system uses. This is a a temporary solution to make sure the rule system isn't directly dependent on the chessboard system.
/// </summary>

public interface IBoardState
{
    public Chessboard Board
    {
        get { return Chessboard.StandardChessboard(); }
        set { }
    }

    /// <summary>
    /// Updates the chessboard by moving the square from the first coordinate to the last coordinate in move. The first coordinate will be marked as unoccupied.
    /// </summary>
    /// <param name="move"> consists of two coordinates without any space between them. </param>
    /// <returns> A GameEvent representing whether the move was successful or not. </returns>
    public GameEvent Move(string move);


    /// <summary>
    /// Gets all valid move for a given player.
    /// </summary>
    /// <param name="player"> is the player whose moves should be calculated. </param>
    /// <returns>an iterable collection of all valid moves.</returns>
    public HashSet<string> GetAllValidMoves(Player player);

    /// <summary>
    /// Splits <paramref name="move"/> into the two corresponding substrings "from" and "to" squares.   
    /// </summary>
    /// <param name="move"> is a string representing two coordinates on the chessboard.</param>
    /// <returns> the two squares split into separate strings. </returns>
    public Tuple<string, string>? parseMove(string move);

    /// <summary>
    /// Gets all moves for a player as a dict with the from-square as key and a list of all
    /// possible to-squares as value.
    /// </summary>
    /// <param name="player">The player to get moves for</param>
    /// <returns>A <see cref="Dictionary{string, List{string}}"/> of all moves for the given player</returns>
    public Dictionary<string, List<string>> GetMoveDict(Player player);


    /// <summary>
    /// Splits <paramref name="move"/> into the two corresponding substrings "from" and "to" squares.   
    /// </summary>
    public MoveWorker CopyBoardState();
}