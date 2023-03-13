using Xunit;
using System;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;
using ChessVariantsLogic.Rules.Moves;

namespace ChessVariantsLogic.Tests;

public class LastMoveTests
{
    MoveWorker board;
    BoardTransition boardTransition0;
    BoardTransition boardTransition1;
    IPosition from;
    IPosition to;
    string fromStr;
    string toStr0;
    string toStr1;

    public LastMoveTests()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        fromStr = "e2";
        toStr0 = "e3";
        toStr1 = "e4";
        from = new PositionAbsolute(fromStr);
        to = new PositionAbsolute(toStr0);
        Move move0 = new Move(fromStr + toStr0);
        Move move1 = new Move(toStr0 + toStr1);

        boardTransition0 = new BoardTransition(board, move0);
        boardTransition1 = new BoardTransition(boardTransition0.NextState, move1);
    }

    [Fact]
    public void LastMoveWasE2E3_ShouldReturnTrue()
    {
        IPredicate lastMoveWasE2E3 = new LastMove(from, to);
        Assert.True(lastMoveWasE2E3.Evaluate(boardTransition1));
    }
    [Fact]
    public void LastMoveWasE2E3_ShouldReturnFalse()
    {
        IPredicate lastMoveWasE2E3 = new LastMove(from, to);
        Assert.False(lastMoveWasE2E3.Evaluate(boardTransition0));
    }
}