using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessVariantsLogic.Rules.Moves.Actions;
/// <summary>
/// When performed this action will place an UnoccupiedSquareIdentifier at the target position.
/// </summary>
public class ActionDeletePiece : IAction
{
    private readonly IPosition _at;

    public ActionDeletePiece(IPosition at)
    {
        _at = at;
    }


    /// <summary>
    /// Delete a piece on the board according to the internal _at position.
    /// </summary>
    /// <param name="moveWorker">MoveWorker that should perform the action.</param>
    /// <param name="performingPiecePosition">This position is used to calculate the final position using the GetPosition method.</param>
    /// 
    /// <returns>A GameEvent that represents whether or not the action was successfully performed.</returns>
    /// 
    public GameEvent Perform(MoveWorker moveWorker, string performingPiecePosition)
    {
        string? finalPosition = _at.GetPosition(moveWorker, performingPiecePosition);
        if (finalPosition == null) return GameEvent.InvalidMove;
        bool performedSucessfully = moveWorker.Board.Insert(Constants.UnoccupiedSquareIdentifier, finalPosition);
        if (performedSucessfully)
            return GameEvent.MoveSucceeded;
        else
            return GameEvent.InvalidMove;
    }
}
