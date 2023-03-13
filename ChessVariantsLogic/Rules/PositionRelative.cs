
namespace ChessVariantsLogic.Rules;
/// <summary>
/// Represents a relative position on the board that can be calculated at runtime.
/// </summary>
public class PositionRelative : IPosition
{
    private readonly Tuple<int, int> _relativePosition;

    public PositionRelative(int row, int col)
    {
        _relativePosition = Tuple.Create(row, col);
    }
    public PositionRelative(Tuple<int, int> relativePosition)
    {
        _relativePosition = relativePosition;
    }

    /// <summary>
    /// Calculates the position and returns it as a string coordinate.
    /// </summary>
    /// <param name="moveWorker">The MoveWorker containing the board with the position to calculate.</param>
    /// <param name="pivotPosition">The final position will be calculated relative to the pivotPosition</param>
    /// 
    /// <returns>The calculated position as a string.</returns>
    /// 
    public string? GetPosition(MoveWorker moveWorker, string pivotPosition)
    {
        Tuple<int, int>? pivotPostionTuple = moveWorker.Board.ParseCoordinate(pivotPosition);
        if (pivotPostionTuple == null) return null;
        Tuple<int, int> finalPosition = Tuple.Create(pivotPostionTuple.Item1 + _relativePosition.Item1, pivotPostionTuple.Item2 + _relativePosition.Item2);
        moveWorker.Board.IndexToCoor.TryGetValue(finalPosition, out string? finalPositionString);
        return finalPositionString;
    }
}
