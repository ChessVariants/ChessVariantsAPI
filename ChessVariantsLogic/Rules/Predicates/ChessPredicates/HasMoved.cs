namespace ChessVariantsLogic.Rules.Predicates.ChessPredicates;
/// <summary>
/// This predicate evaluates if a piece at a given location has moved.
/// </summary>
public class HasMoved : IPredicate
{
    private readonly IPosition _position;
    private readonly BoardState _boardState;

    public HasMoved(IPosition position, BoardState boardState = BoardState.THIS)
    {
        _position = position;
        _boardState = boardState;
    }

    public bool Evaluate(BoardTransition transition)
    {
        MoveWorker boardState = _boardState == BoardState.THIS ? transition.ThisState : transition.NextState;
        string? coordinate = _position.GetPosition(boardState, transition.MoveFrom);
        if (coordinate == null) return false;
        Tuple<int, int>? tupleCoordinate = boardState.Board.ParseCoordinate(coordinate);
        if (tupleCoordinate == null) return false;
        return boardState.Board.HasPieceMoved(tupleCoordinate.Item1, tupleCoordinate.Item2);
    }
}
