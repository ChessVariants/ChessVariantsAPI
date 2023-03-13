using Xunit;
using System;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;

namespace ChessVariantsLogic.Tests;

public class PieceAtTests : IDisposable {
    MoveWorker board;
    BoardTransition boardTransition;

    public PieceAtTests()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        boardTransition = new BoardTransition(board, board, "a1a1");
    }

    public void Dispose()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void WhiteKingAtE1_ShouldReturnTrue()
    {
        IPredicate kingAtE1 = new PieceAt(Constants.WhiteKingIdentifier, new PositionAbsolute("e1"), BoardState.THIS);
        Assert.True(kingAtE1.Evaluate(boardTransition));
    }
    [Fact]
    public void WhiteKingAtE2_ShouldReturnFalse()
    {
        IPredicate kingAtE2 = new PieceAt(Constants.WhiteKingIdentifier, new PositionAbsolute("e2"), BoardState.THIS);
        Assert.False(kingAtE2.Evaluate(boardTransition));
    }
    [Fact]
    public void UnoccupiedSquareOnE4_ShouldReturnTrue()
    {
        IPredicate unoccupiedSquareOnE4 = new PieceAt(Constants.UnoccupiedSquareIdentifier, new PositionAbsolute("e4"), BoardState.THIS);
        Assert.True(unoccupiedSquareOnE4.Evaluate(boardTransition));
    }
    [Fact]
    public void UnoccupiedSquareOnE1_ShouldReturnFalse()
    {
        IPredicate unoccupiedSquareOnE1 = new PieceAt(Constants.UnoccupiedSquareIdentifier, new PositionAbsolute("e1"), BoardState.THIS);
        Assert.False(unoccupiedSquareOnE1.Evaluate(boardTransition));
    }
}