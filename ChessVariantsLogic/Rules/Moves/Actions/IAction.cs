namespace ChessVariantsLogic.Rules.Moves.Actions;

/// <summary>
/// Represents an action that can be performed on the board.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Performs an action on the given moveWorker according to implementation.
    /// </summary>
    /// <param name="moveWorker">MoveWorker that should perform the action.</param>
    /// <param name="performingPiecePosition">The position of the piece that performed the action. Relative positions are calculated relative to this position.</param>
    /// 
    /// <returns>A GameEvent that represents whether or not the action was successfully performed.</returns>
    /// 
    public GameEvent Perform(MoveWorker moveWorker, string performingPiecePosition);
}