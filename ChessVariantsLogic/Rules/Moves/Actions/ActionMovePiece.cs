namespace ChessVariantsLogic.Rules.Moves.Actions;
/// <summary>
/// When performed this action will forcefully move a piece on the board according to the given positions.
/// </summary>
public class ActionMovePiece : IAction
{
    private readonly IPosition _from;
    private readonly IPosition _to;

    public ActionMovePiece(IPosition from, IPosition to)
    {
        _from = from;
        _to = to;
    }

    //If from position is not specified, the piece performing the action will be moved.

    public ActionMovePiece(IPosition to)
    {
        _from = new PositionRelative(Tuple.Create(0, 0));
        _to = to;
    }

    /// <summary>
    /// Forcefully move a piece on the board according to the given positions.
    /// If Positions are defined as relative, it will be calculated relative to the piece performing the action.
    /// </summary>
    /// <param name="moveWorker">MoveWorker that should perform the action.</param>
    /// <param name="performingPiecePosition">This variable is used to calculate the from and to positions if they are relative.</param>
    /// 
    /// <returns>A GameEvent that represents whether or not the action was successfully performed.</returns>
    /// 
    public GameEvent Perform(MoveWorker moveWorker, string performingPiecePosition)
    {
        string? from = _from.GetPosition(moveWorker, performingPiecePosition);
        if (from == null) return GameEvent.InvalidMove;
        string? to = _to.GetPosition(moveWorker, performingPiecePosition);
        if (to == null) return GameEvent.InvalidMove;
        string move = from + to;
        return moveWorker.ForceMove(move);
    }
}
