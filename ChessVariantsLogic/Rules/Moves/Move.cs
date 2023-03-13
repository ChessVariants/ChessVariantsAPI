using ChessVariantsLogic.Rules.Moves.Actions;

namespace ChessVariantsLogic.Rules.Moves;
/// <summary>
/// Represents a special move that can be performed on the board. (examples: castling, en passant, double pawn move...).
/// _actions is a list of actions that will be performed when the Move is performed.
/// _fromTo is a pair of coordinates (e.g. "e2e4) that represent which piece performs the move and where to click to perform the move respectively.
/// <para>IMPORTANT: If _actions == null, a standard move will be performed.</para>
/// </summary>
public class Move
{
    private readonly IEnumerable<IAction>? _actions;
    private readonly string _fromTo;

    public string FromTo => _fromTo;


    /// <summary>
    /// Constructor that takes a list of actions and a string fromTo.
    /// </summary>
    /// <param name="actions">The list of actions that the move performs.</param>
    /// <param name="fromTo">A pair of coordinates, the position of the performing piece and where it ends up.</param>
    public Move(IEnumerable<IAction>? actions, string fromTo)
    {
        _actions = actions;
        _fromTo = fromTo;
    }

    /// <summary>
    /// Constructs a standard move.
    /// </summary>
    /// <param name="fromTo">A pair of coordinates, the position of the piece to be moved and where it ends up.</param>
    public Move(string fromTo)
    {
        _actions = null;
        _fromTo = fromTo;
    }

    /// <summary>
    /// Performs all actions in the internal list _actions on the given moveWorker.
    /// If any single action fails, the whole move fails.
    /// <para> If _actions == null, a standard move will be performed.</para>
    /// </summary>
    /// <param name="moveWorker">The board state to perform the actions on.</param>
    /// 
    /// <returns>A GameEvent that determines whether the move succeeded or was invalid.</returns>
    /// 
    public GameEvent Perform(MoveWorker moveWorker)
    {
        (string from, string _) = moveWorker.parseMove(_fromTo);
        if (from == null) throw new Exception("Move needs to contain proper fromTo coordinate");
        if(_actions == null)
        {
            return moveWorker.Move(_fromTo);
        }
        foreach (var action in _actions)
        {
            GameEvent gameEvent = action.Perform(moveWorker, from);
            if(gameEvent == GameEvent.InvalidMove)
                return GameEvent.InvalidMove;
        }
        return GameEvent.MoveSucceeded;
    }

}
