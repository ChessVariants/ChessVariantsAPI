namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;

/// <summary>
/// This predicate evaluates if the last move is equal to the compare values.
/// </summary>
public class LastMove : IPredicate
{
    private readonly IPosition _compareFrom;
    private readonly IPosition _compareTo;

    public LastMove(IPosition compareFrom, IPosition compareTo)
    {
        _compareFrom = compareFrom;
        _compareTo = compareTo;
    }

    public bool Evaluate(BoardTransition transition)
    {
        string? lastMove = transition.ThisState.getLastMove();
        string? from = _compareFrom.GetPosition(transition.ThisState, transition.MoveFrom);
        string? to   = _compareTo.GetPosition(transition.ThisState, transition.MoveFrom);

        if (lastMove == null || from == null || to == null) return false;
        string compareMove = from + to;
        return compareMove == lastMove;
    }
}
