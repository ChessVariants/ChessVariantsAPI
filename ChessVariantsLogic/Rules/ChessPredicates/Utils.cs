using static ChessVariantsLogic.Game;
namespace ChessVariantsLogic.Predicates;

/// <summary>
/// Utility methods used by to get information about the game. Should be moved eventually.
/// </summary>
public class Utils
{

    /// <summary>
    /// Returns whether a piece with identifier <paramref name="pieceIdentifier"/> is attacked or not.
    /// </summary>
    /// <param name="board">The board state which you want to find information about</param>
    /// <param name="pieceIdentifier">The identifier for the piece you're trying to see if attacked</param>
    /// <returns>True if a piece with the supplied <paramref name="pieceIdentifier"/> is attacked, otherwise false.</returns>
    public static bool PieceAttacked(IBoardState board, string pieceIdentifier)
    {
        Player player = GetPlayer(pieceIdentifier);
        Player attacker = player == Player.White ? Player.Black : Player.White;
        var piecePositions = FindPiecesOfType(board, pieceIdentifier);
        foreach (var attackerMove in board.GetAllValidMoves(attacker))
        {
            var (_, to) = board.parseMove(attackerMove);
            if (piecePositions.Contains(to))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the owner of the piece with the supplied <paramref name="pieceIdentifier"/>.
    /// </summary>
    /// <param name="pieceIdentifier">The piece identifier to check ownership of</param>
    /// <returns>The <see cref="Player"/> who owns the piece</returns>
    public static Player GetPlayer(string pieceIdentifier)
    {
        if ((pieceIdentifier.Any(char.IsUpper) && pieceIdentifier != "ANY_BLACK" && pieceIdentifier != "ROYAL_BLACK") || pieceIdentifier == "ANY_WHITE")
        {
            return Player.White;
        }
        else if (pieceIdentifier.Any(char.IsLower) || pieceIdentifier == "ANY_BLACK" || pieceIdentifier == "ROYAL_BLACK")
        {
            return Player.Black;
        }
        return Player.None;
    }

    /// <summary>
    /// Returns all positions where a piece with <paramref name="pieceIdentifier"/> are located.
    /// </summary>
    /// <param name="board">The board to find pieces on</param>
    /// <param name="pieceIdentifier">The identifier for the piece type whose locations to find</param>
    /// <returns>All positions where a piece with <paramref name="pieceIdentifier"/> are located.</returns>
    public static IEnumerable<string> FindPiecesOfType(IBoardState board, string pieceIdentifier)
    {
        var pieceLocations = new List<string>();
        foreach (var position in board.Board.CoorToIndex.Keys)
        {
            if (IsOfType(position, board, pieceIdentifier))
            {
                pieceLocations.Add(position);
            }
        }
        return pieceLocations;
    }

    /// <summary>
    /// Returns whether a piece at <paramref name="position"/> is of the supplied <paramref name="pieceIdentifier"/> type.
    /// </summary>
    /// <param name="position">The location to check</param>
    /// <param name="board">The board to check on</param>
    /// <param name="pieceIdentifier">The identifier for the type</param>
    /// <returns>True if the piece at <paramref name="position"/> conforms to the type of the <paramref name="pieceIdentifier"/></returns>
    public static bool IsOfType(string position, IBoardState board, string pieceIdentifier)
    {
        var piece = board.Board.GetPieceIdentifier(position);
        switch (pieceIdentifier)
        {
            case "ANY":
                return piece != Constants.UnoccupiedSquareIdentifier;
            case "ANY_BLACK":
                if (piece != null)
                {
                    return GetPlayer(piece) == Player.Black;
                }
                return false;
            case "ANY_WHITE":
                if (piece != null)
                {
                    return GetPlayer(piece) == Player.White;
                }
                return false;
            case "ROYAL_BLACK":
                return piece == Constants.BlackKingIdentifier;
            case "ROYAL_WHITE":
                return piece == Constants.WhiteKingIdentifier;
            default:
                return piece == pieceIdentifier;
        }
    }
}