using Xunit;
using System;
using System.Diagnostics;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;

namespace ChessVariantsLogic.Tests;

public class AttackedTests : IDisposable {
    MoveWorker board;
    MoveWorker whiteKingAttackedBoard;
    MoveWorker blackKingAttackedBoard;
    MoveWorker blackBishopOnE2;

    BoardTransition boardBoard;
    BoardTransition whiteKingAttackedTransition;
    BoardTransition blackToWhiteKingAttackedTransition;
    BoardTransition blackBishopOnE2Transition;

    public AttackedTests()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        whiteKingAttackedBoard = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        whiteKingAttackedBoard.Board.Insert(Constants.BlackQueenIdentifier, "e2");

        blackKingAttackedBoard = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        blackKingAttackedBoard.Board.Insert(Constants.WhiteRookIdentifier, "e7");

        blackBishopOnE2 = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        blackBishopOnE2.Board.Insert(Constants.BlackBishopIdentifier, "e2");

        boardBoard = new BoardTransition(board, board, "a1a1");
        whiteKingAttackedTransition = new BoardTransition(whiteKingAttackedBoard, whiteKingAttackedBoard, "a1a1");
        blackBishopOnE2Transition = new BoardTransition(blackBishopOnE2, blackBishopOnE2, "a1a1");

        blackToWhiteKingAttackedTransition = new BoardTransition(blackKingAttackedBoard, whiteKingAttackedBoard, "a1a1");

    }

    public void Dispose()
    {
        board = new MoveWorker(Chessboard.StandardChessboard());
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void WhitePawnIsAttacked_ShouldReturnTrue()
    {
        IPredicate whitePawnAttacked = new Attacked(BoardState.THIS, Constants.WhitePawnIdentifier);
        Assert.True(whitePawnAttacked.Evaluate(whiteKingAttackedTransition));
    }
    
    [Fact]
    public void WhitePawnIsAttacked_ShouldReturnFalse()
    {
        IPredicate whitePawnAttacked = new Attacked(BoardState.THIS, Constants.WhitePawnIdentifier);
        Assert.False(whitePawnAttacked.Evaluate(blackBishopOnE2Transition));
    }

    [Fact]
    public void WhiteRoyalIsAttacked_ShouldReturnTrue()
    {
        IPredicate whiteKingAttacked = new Attacked(BoardState.THIS, "ROYAL_WHITE");
        Assert.True(whiteKingAttacked.Evaluate(whiteKingAttackedTransition));
    }
    [Fact]
    public void WhiteRoyalIsAttacked_ShouldReturnFalse()
    {
        IPredicate whiteRoyalAttacked = new Attacked(BoardState.THIS, "ROYAL_WHITE");
        Assert.False(whiteRoyalAttacked.Evaluate(boardBoard));
    }

    [Fact]
    public void BlackRoyalIsAttacked_ShouldReturnTrue()
    {
        IPredicate blackKingAttacked = new Attacked(BoardState.THIS, "ROYAL_BLACK");
        Assert.True(blackKingAttacked.Evaluate(blackToWhiteKingAttackedTransition));
    }
    [Fact]
    public void BlackRoyalIsAttacked_ShouldReturnFalse()
    {
        IPredicate whiteRoyalAttacked = new Attacked(BoardState.THIS, "ROYAL_BLACK");
        Assert.False(whiteRoyalAttacked.Evaluate(boardBoard));
    }

    [Fact]
    public void AnyWhitePieceAttacked_ShouldReturnTrue()
    {
        IPredicate anyWhiteAttacked = new Attacked(BoardState.THIS, "ANY_WHITE");
        Assert.True(anyWhiteAttacked.Evaluate(blackBishopOnE2Transition));
    }

    [Fact]
    public void AnyWhitePieceAttacked_ShouldReturnFalse()
    {
        IPredicate anyWhiteAttacked = new Attacked(BoardState.THIS, "ANY_WHITE");
        Assert.False(anyWhiteAttacked.Evaluate(boardBoard));
    }

    [Fact]
    public void AnyPieceAttacked_ShouldReturnTrue()
    {
        IPredicate anyAttacked = new Attacked(BoardState.THIS, "ANY");
        Assert.True(anyAttacked.Evaluate(blackBishopOnE2Transition));
    }

    [Fact]
    public void AnyPieceAttacked_ShouldReturnFalse()
    {
        IPredicate anyAttacked = new Attacked(BoardState.THIS, "ANY");
        Assert.False(anyAttacked.Evaluate(boardBoard));
    }

    [Fact]
    public void AnyBlackPieceAttacked_ShouldReturnTrue()
    {
        IPredicate anyBlackAttacked = new Attacked(BoardState.THIS, "ANY_BLACK");
        Assert.True(anyBlackAttacked.Evaluate(blackBishopOnE2Transition));
    }
    [Fact]
    public void AnyBlackPieceAttacked_ShouldReturnFalse()
    {
        IPredicate anyBlackAttacked = new Attacked(BoardState.THIS, "ANY_BLACK");
        Assert.False(anyBlackAttacked.Evaluate(boardBoard));
    }
}