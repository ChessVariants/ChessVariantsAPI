namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;

/// <summary>
/// This predicate determines if a piece has been captured when transitioning to a new board state.
/// </summary>
public class PieceCaptured : IPredicate
{
    private readonly string _pieceIdentifier;

    public PieceCaptured(string pieceIdentifier)
    {
        _pieceIdentifier = pieceIdentifier;
    }

    /// <summary>
    /// Returns true if a piece was captured when transitioning from <paramref name="thisBoardState"/> to <paramref name="nextBoardState"/>.
    /// </summary>
    /// <inheritdoc/>
    /// <returns>True if a piece was captured during transition from <paramref name="thisBoardState"/> to <paramref name="nextBoardState"/>.</returns>
    public bool Evaluate(BoardTransition transition)
    {
        int amountOfPieces = Utils.FindPiecesOfType(transition.ThisState, _pieceIdentifier).Count();
        int amountOfPiecesNextState = Utils.FindPiecesOfType(transition.NextState, _pieceIdentifier).Count();
        int diff = amountOfPieces - amountOfPiecesNextState;
        return diff > 0;
    }

}