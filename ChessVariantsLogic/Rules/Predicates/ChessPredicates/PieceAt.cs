namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;
/// <summary>
/// This predicate evaluates if a given board coordinate contains a piece that has the type of the internal _pieceIdentifier.
/// </summary>
public class PieceAt : IPredicate
{
    private readonly string _pieceIdentifier;
    private readonly IPosition _position;
    private readonly BoardState _boardState;


    public PieceAt(string pieceIdentifier, IPosition position, BoardState boardState)
    {
        _pieceIdentifier = pieceIdentifier;
        _position = position;
        _boardState = boardState;
    }

    /// <summary>
    /// Evaluates to true/false if the identifier of the piece at the internal _position is equal to the internal _pieceIdentifier.
    /// </summary>
    /// <inheritdoc/>
    /// <returns>true/false if the identifier of the piece at the internal _position is equal to the internal _pieceIdentifier.</returns>

    public bool Evaluate(BoardTransition transition)
    {
        bool isThisBoardState = _boardState == BoardState.THIS;
        var board = isThisBoardState ? transition.ThisState : transition.NextState;
        var relativePosition = transition.MoveFrom;

        string? finalPosition = _position.GetPosition(board, relativePosition);
        if (finalPosition == null) return false;

        string? pieceAt = board.Board.GetPieceIdentifier(finalPosition);

        return _pieceIdentifier.Equals(pieceAt);
    }
}
