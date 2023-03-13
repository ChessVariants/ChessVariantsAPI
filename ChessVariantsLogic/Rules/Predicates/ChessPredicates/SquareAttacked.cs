
namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;
/// <summary>
/// This predicate determines if a square is attacked or not by the given player, either in the current board state or the next.
/// The target position can be calculated relatively or absolutely.
/// </summary>
public class SquareAttacked : IPredicate
{
    private readonly IPosition _position;
    private readonly BoardState _boardState;
    private readonly Player _attacker;


    public SquareAttacked(IPosition position, BoardState boardState, Player attacker)
    {
        _position = position;
        _boardState = boardState;
        _attacker = attacker;
    }
    /// <summary>
    /// Evaluates to true/false if a square is attacked or not in either the current board state or the next, depending on what was specified at object-creation.
    /// </summary>
    /// <inheritdoc/>
    /// <returns>true/false if a square is attacked or not in either the current board state or the next, depending on what was specified at object-creation.</returns>

    public bool Evaluate(BoardTransition boardTransition)
    {
        bool isThisBoardState = _boardState == BoardState.THIS;
        var board = isThisBoardState ? boardTransition.ThisState : boardTransition.NextState;
        var pivotPosition = boardTransition.MoveFrom;

        string? finalPosition = _position.GetPosition(board, pivotPosition);
        if (finalPosition == null) return false;

        return Utils.SquareAttacked(board, finalPosition, _attacker);
    }
}
