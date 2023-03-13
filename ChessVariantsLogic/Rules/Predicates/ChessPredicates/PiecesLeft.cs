namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;

/// <summary>
/// This predicate determines if how many pieces of a certain type are left in the game
/// and compares it with the supplied <see cref="Comparator"/> to evaluate to either
/// true or false.
/// </summary>
public class PiecesLeft : IPredicate
{
    private readonly Comparator _comparator;
    private readonly int _compareValue;
    private readonly BoardState _state;
    private readonly string _pieceIdentifier;

    public PiecesLeft(string pieceIdentifier, Comparator comparator, int compareValue, BoardState state)
    {
        _comparator = comparator;
        _compareValue = compareValue;
        _state = state;
        _pieceIdentifier = pieceIdentifier;
    }

    private bool CompareValue(int piecesLeft)
    {
        return _comparator switch
        {
            Comparator.GREATER_THAN => piecesLeft > _compareValue,
            Comparator.LESS_THAN => piecesLeft < _compareValue,
            Comparator.LESS_THAN_OR_EQUALS => piecesLeft <= _compareValue,
            Comparator.GREATER_THAN_OR_EQUALS => piecesLeft >= _compareValue,
            Comparator.EQUALS => piecesLeft == _compareValue,
            Comparator.NOT_EQUALS => piecesLeft != _compareValue,
            _ => false,
        };
    }

    /// <summary>
    /// Evaluates the boolean value of how many pieces left there are compared to the internal <see cref="Comparator"/>.
    /// </summary>
    /// <inheritdoc/>
    /// <returns>The boolean value of how many pieces left there are compared to the internal <see cref="Comparator"/>.</returns>
    public bool Evaluate(BoardTransition transition)
    {
        var board = _state == BoardState.THIS ? transition.ThisState : transition.NextState;
        int piecesLeft = Utils.FindPiecesOfType(board, _pieceIdentifier).Count();
        return CompareValue(piecesLeft);
    }

}

/// <summary>
/// Enum for the supported comparator types.
/// </summary>
public enum Comparator
{
    GREATER_THAN, LESS_THAN, GREATER_THAN_OR_EQUALS, LESS_THAN_OR_EQUALS, EQUALS, NOT_EQUALS
}

