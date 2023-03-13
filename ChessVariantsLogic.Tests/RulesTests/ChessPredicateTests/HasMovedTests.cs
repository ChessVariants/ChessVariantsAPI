using Xunit;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;
using ChessVariantsLogic.Rules.Moves;

namespace ChessVariantsLogic.Tests;

public class HasMovedTests
{
    MoveWorker board;
    BoardTransition boardTransition;
    IPosition from;
    IPosition to;
    string fromStr;
    string toStr;

    public HasMovedTests()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        fromStr = "e2";
        toStr = "e3";
        from = new PositionAbsolute(fromStr);
        to = new PositionAbsolute(toStr);
        Move move = new Move(fromStr + toStr);

        boardTransition = new BoardTransition(board, move);
    }

    [Fact]
    public void WhitePawnHasMovedNextStateE4_ShouldReturnTrue()
    {
        IPredicate pawnMovedNextState = new HasMoved(to, BoardState.NEXT);
        Assert.True(pawnMovedNextState.Evaluate(boardTransition));
    }
    [Fact]
    public void WhitePawnHasMovedThisStateE2_ShouldReturnFalse()
    {
        IPredicate pawnMovedThisState = new HasMoved(from, BoardState.THIS);
        Assert.False(pawnMovedThisState.Evaluate(boardTransition));
    }
}