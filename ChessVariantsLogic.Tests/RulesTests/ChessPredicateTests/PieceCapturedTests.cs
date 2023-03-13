using Xunit;
using System;
using System.Diagnostics;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;

namespace ChessVariantsLogic.Tests;

public class PieceCapturedTests : IDisposable {
    MoveWorker board0;
    MoveWorker board1;

    BoardTransition board01;
    BoardTransition board00;

    public PieceCapturedTests()
    {
        board0 = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());



        board0.Move("e2e3");
        board0.Move("e3e4");
        board0.Move("e7e6");
        board0.Move("e6e5");

        board0.Move("d1h5");
        board0.Move("b8c6");

        board0.Move("f1c4");
        board0.Move("g8f6");


        board1 = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());


        board1.Move("e2e3");
        board1.Move("e3e4");
        board1.Move("e7e6");
        board1.Move("e6e5");
        board1.Move("d1h5");
        board1.Move("b8c6");
        board1.Move("f1c4");
        board1.Move("g8f6");

        board1.Move("h5f7");

        board00 = new BoardTransition(board0, board0, "a1a1");
        board01 = new BoardTransition(board0, board1, "h5f7");

    }

    public void Dispose()
    {
        board0 = new MoveWorker(Chessboard.StandardChessboard());
        board1 = new MoveWorker(Chessboard.StandardChessboard());
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void PieceCaptured_ShouldReturnTrue()
    {
        IPredicate pawnCaptured = new PieceCaptured(Constants.BlackPawnIdentifier);
        IPredicate pieceCaptured = new PieceCaptured("ANY_BLACK");
        Assert.True(pawnCaptured.Evaluate(board01));
        Assert.True(pieceCaptured.Evaluate(board01));
    }
    [Fact]
    public void CheckMate_ShouldReturnFalse()
    {
        IPredicate pawnCaptured = new PieceCaptured(Constants.BlackPawnIdentifier);
        Assert.False(pawnCaptured.Evaluate(board00));
    }

}