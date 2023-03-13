using Xunit;
using System;
using System.Diagnostics;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using ChessVariantsLogic.Rules;

namespace ChessVariantsLogic.Tests;

public class ForEveryTests : IDisposable {
    MoveWorker board;
    MoveWorker scholarsMateBoard;
    MoveWorker notScholarsMateBoard;
    IPredicate whiteWinRule;
    BoardTransition scholarsMateBoardTransition;
    BoardTransition notScholarsMateBoardTransition;
    BoardTransition boardTransition;

    public ForEveryTests()
    {
        board = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        scholarsMateBoard = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());


        scholarsMateBoard.Move("e2e3");
        scholarsMateBoard.Move("e3e4");
        scholarsMateBoard.Move("e7e6");
        scholarsMateBoard.Move("e6e5");

        scholarsMateBoard.Move("d1h5");
        scholarsMateBoard.Move("b8c6");

        scholarsMateBoard.Move("f1c4");
        scholarsMateBoard.Move("g8f6");

        scholarsMateBoard.Move("h5f7");


        notScholarsMateBoard = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());


        notScholarsMateBoard.Move("e2e3");
        notScholarsMateBoard.Move("e3e4");
        notScholarsMateBoard.Move("e7e6");
        notScholarsMateBoard.Move("e6e5");

        notScholarsMateBoard.Move("d1h5");
        notScholarsMateBoard.Move("b8c6");

        notScholarsMateBoard.Move("f1c4");
        notScholarsMateBoard.Move("d8e7");

        notScholarsMateBoard.Move("h5f7");



        IPredicate blackKingCheckedThisTurn = new Attacked(BoardState.THIS, Constants.BlackKingIdentifier);
        IPredicate blackKingCheckedNextTurn = new Attacked(BoardState.NEXT, Constants.BlackKingIdentifier);

        IPredicate blackKingCheckedThisAndNextTurn = new Operator(blackKingCheckedThisTurn, OperatorType.AND, blackKingCheckedNextTurn);

        whiteWinRule = new ForEvery(blackKingCheckedThisAndNextTurn, Player.Black);

        scholarsMateBoardTransition = new BoardTransition(scholarsMateBoard, scholarsMateBoard, "a1a1");
        notScholarsMateBoardTransition = new BoardTransition(notScholarsMateBoard, notScholarsMateBoard, "a1a1");
        boardTransition = new BoardTransition(board, board, "a1a1");

    }

    public void Dispose()
    {
        board = new MoveWorker(Chessboard.StandardChessboard());
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void CheckMate_ShouldReturnTrue()
    {
        Assert.True(whiteWinRule.Evaluate(scholarsMateBoardTransition));
    }
    [Fact]
    public void CheckMate_ShouldReturnFalse()
    {
        Assert.False(whiteWinRule.Evaluate(notScholarsMateBoardTransition));
        Assert.False(whiteWinRule.Evaluate(boardTransition));
    }

}