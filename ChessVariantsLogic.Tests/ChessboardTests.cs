using System;
using System.Collections.Generic;
using Xunit;

namespace ChessVariantsLogic.Tests;

/// <summary>
/// This class contains unit tests on Chessboard.cs and ChessDriver.cs.
/// </summary>
public class ChessboardTests
{

    /// <summary>
    /// Tests that the FEN representation of the board is of the correct format.
    /// </summary>
    [Fact]
    public void Test_FEN()
    {
        var gameDriver = new GameDriver(new Chessboard(6), Piece.AllStandardPieces());

        Assert.Equal("6/6/6/6/6/6", gameDriver.Board.ReadBoardAsFEN());

        gameDriver.Board = new Chessboard(12, 3);
        Assert.Equal("3/3/3/3/3/3/3/3/3/3/3/3", gameDriver.Board.ReadBoardAsFEN());

        gameDriver.Board.Insert(Constants.BlackBishopIdentifier, "b2");
        Assert.Equal("3/3/3/3/3/3/3/3/3/3/1b1/3", gameDriver.Board.ReadBoardAsFEN());

        gameDriver.Board = Chessboard.StandardChessboard();
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", gameDriver.Board.ReadBoardAsFEN());

        gameDriver.Move("a2a4");
        Assert.Equal("rnbqkbnr/pppppppp/8/8/P7/8/1PPPPPPP/RNBQKBNR", gameDriver.Board.ReadBoardAsFEN());

    }

    /// <summary>
    /// Tests that the standard chessboard is set up correctly.
    /// </summary>
    [Fact]
    public void Test_Standard_Chessboard()
    {
        var board = Chessboard.StandardChessboard();
        Assert.Equal(Constants.WhiteKingIdentifier,         board.GetPieceAsString("e1"));
        Assert.Equal(Constants.BlackQueenIdentifier,        board.GetPieceAsString("d8"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  board.GetPieceAsString("e4"));
        Assert.Equal(Constants.BlackRookIdentifier,         board.GetPieceAsString("a8"));
    }

    /// <summary>
    /// Tests that moving a piece updates the board correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Pawn()
    {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhitePawnIdentifier,         gameDriver.Board.GetPieceAsString("g2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("g3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("g2g3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("g2"));
        Assert.Equal(Constants.WhitePawnIdentifier,         gameDriver.Board.GetPieceAsString("g3"));
        
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("h2h9"));
        Assert.Equal(Constants.WhitePawnIdentifier, gameDriver.Board.GetPieceAsString("h2"));

    }

    /// <summary>
    /// Test that a rook can move correcly on non-standard chessboard.
    /// </summary>
    [Fact]
    public void Test_Rook_Rectangular_Board()
    {
        var gameDriver = new GameDriver(new Chessboard(4,10));
        
        Assert.True(gameDriver.InsertOnBoard(Piece.Rook(PieceClassifier.BLACK), "b2"));

        Assert.Equal(Constants.BlackRookIdentifier,         gameDriver.Board.GetPieceAsString("b2"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("b2i2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("b2"));
        Assert.Equal(Constants.BlackRookIdentifier,         gameDriver.Board.GetPieceAsString("i2"));

        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("i2i4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("i2"));
        Assert.Equal(Constants.BlackRookIdentifier,         gameDriver.Board.GetPieceAsString("i4"));

    }

    /// <summary>
    /// Tests that invalid moves are not processed.
    /// </summary>
    [Fact]
    public void Test_Invalid_Move()
    {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteRookIdentifier,         gameDriver.Board.GetPieceAsString("h1"));
        Assert.Equal(Constants.WhitePawnIdentifier,         gameDriver.Board.GetPieceAsString("h2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("h4"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("h1h4"));
        Assert.Equal(Constants.WhiteRookIdentifier,         gameDriver.Board.GetPieceAsString("h1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("h4"));
        
        Assert.Equal(Constants.BlackKnightIdentifier,       gameDriver.Board.GetPieceAsString("b8"));
        Assert.Equal(Constants.BlackPawnIdentifier,         gameDriver.Board.GetPieceAsString("d7"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("b8d7"));
        Assert.Equal(Constants.BlackKnightIdentifier,       gameDriver.Board.GetPieceAsString("b8"));
        Assert.Equal(Constants.BlackPawnIdentifier,         gameDriver.Board.GetPieceAsString("d7"));

        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("e4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("e5"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("e4e5"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("e4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("e5"));

    }

    /// <summary>
    /// Test that a serier of moves can be processed correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Serie()
    {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhitePawnIdentifier,         gameDriver.Board.GetPieceAsString("a2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("a3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("a2a3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("a3a4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("a2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("a3"));
        Assert.Equal(Constants.WhitePawnIdentifier,         gameDriver.Board.GetPieceAsString("a4"));
    }

    /// <summary>
    /// Tests that a knight can jump over pieces and move correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Knight()
    {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKnightIdentifier,       gameDriver.Board.GetPieceAsString("g1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("g1h3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("g1"));
        Assert.Equal(Constants.WhiteKnightIdentifier,       gameDriver.Board.GetPieceAsString("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("h3f4"));
    }
      
    /// <summary>
    /// Test that pices can be captured correctly.
    /// </summary>
    [Fact]
     public void Test_Take()
     {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKnightIdentifier,       gameDriver.Board.GetPieceAsString("g1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("g1h3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("h3g5"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("g5h7"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("g5"));
        Assert.Equal(Constants.WhiteKnightIdentifier,       gameDriver.Board.GetPieceAsString("h7"));
     }
     
     /// <summary>
     /// Test that the bishop can move correctly diagonally.
     /// </summary>
     [Fact]
     public void Test_Bishop()
     {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("f1c4"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("f1c4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("f1"));
        Assert.Equal(Constants.WhiteBishopIdentifier,       gameDriver.Board.GetPieceAsString("c4"));
     }

    
     [Fact]
     public void Test_Move_King()
     {
        var gameDriver = new GameDriver(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKingIdentifier,  gameDriver.Board.GetPieceAsString("e1"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("e1e2"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("e2e3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  gameDriver.Board.GetPieceAsString("e1"));
        Assert.Equal(Constants.WhiteKingIdentifier,  gameDriver.Board.GetPieceAsString("e2"));
        Assert.Equal(Constants.WhitePawnIdentifier,  gameDriver.Board.GetPieceAsString("e3"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("e2f3"));

     }

    /// <summary>
    /// Tests that pieces can not be inserted into squares outside of the chessboard.
    /// </summary>
    [Fact]
    public void Test_Faulty_Indices()
    {
        var board = Chessboard.StandardChessboard();
        Assert.False(board.Insert(Constants.BlackBishopIdentifier, 2, 9));
        Assert.False(board.Insert(Constants.BlackBishopIdentifier, 9, 9));
        Assert.False(board.Insert(Constants.BlackBishopIdentifier, 10, 7));
        Assert.True(board.Insert(Constants.BlackBishopIdentifier, "h8"));

        board = new Chessboard(12, 12);
        Assert.True(board.Insert(Constants.WhiteBishopIdentifier, "j3"));
        Assert.False(board.Insert(Constants.WhiteBishopIdentifier, "n3"));
        Assert.False(board.Insert(Constants.WhiteBishopIdentifier, new Tuple<int, int>(3, 14)));

    }

    /// <summary>
    /// Test that the queen can move both straight and diagnoally.
    /// </summary>
    [Fact]
    public void Test_Queen()
    {
        var gameDriver = new GameDriver(new Chessboard(6,5));
        
        Assert.True(gameDriver.InsertOnBoard(Piece.Queen(PieceClassifier.WHITE), "c4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, gameDriver.Board.GetPieceAsString("a6"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("c4a6"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, gameDriver.Board.GetPieceAsString("c4"));
        Assert.Equal(Constants.WhiteQueenIdentifier, gameDriver.Board.GetPieceAsString("a6"));

        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("a6a1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, gameDriver.Board.GetPieceAsString("a6"));
        Assert.Equal(Constants.WhiteQueenIdentifier, gameDriver.Board.GetPieceAsString("a1"));

    }

    /// <summary>
    /// Test that a custom piece with a novel movement pattern moves correctly.
    /// </summary>
    [Fact]
    public void Test_Custom_Piece()
    {
         var gameDriver = new GameDriver(new Chessboard(8));

         var pattern = new List<Tuple<int,int>> {
            RegularMovementPattern.North,
            RegularMovementPattern.NorthEast,
            RegularMovementPattern.NorthWest,
            RegularMovementPattern.SouthEast,
            RegularMovementPattern.SouthWest
        };
        var moveLength = new List<Tuple<int,int>> {
            new Tuple<int,int> (1,3),
            new Tuple<int,int> (1,1),
            new Tuple<int,int> (2,4),
            new Tuple<int,int> (2,2),
            new Tuple<int,int> (1,3),

        };
        var mp = new RegularMovementPattern(pattern, moveLength);
        Piece piece = new Piece(mp, false, PieceClassifier.WHITE, "C");

        Assert.True(gameDriver.InsertOnBoard(piece, "c4"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("c4c5"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("c5d6"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("d6b4"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("b4d2"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("d2a5"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("a5b5"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("a5b4"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("a5a4"));

        Assert.True(gameDriver.InsertOnBoard(piece, "h3"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("h3c8"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("h3d7"));
    }

    [Fact]
    public void Test_Custom_Piece_Repeat()
    {
        var gameDriver = new GameDriver(new Chessboard(8));

         var pattern = new List<Tuple<int,int>> {
            RegularMovementPattern.North,
            RegularMovementPattern.West,
        };
        var moveLength = new List<Tuple<int,int>> {
            new Tuple<int,int> (1,8),
            new Tuple<int,int> (1,8),
        };
        var mp = new RegularMovementPattern(pattern, moveLength);
        Piece piece = new Piece(mp, false, PieceClassifier.WHITE, 1, "C");

        Assert.True(gameDriver.InsertOnBoard(piece, "h3"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("h3b2"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("h3c5"));

        var piece2 = Piece.BlackPawn();
        Assert.True(gameDriver.InsertOnBoard(piece2, "c7"));
        Assert.Equal(GameEvent.InvalidMove, gameDriver.Move("c5c8"));
        Assert.Equal(GameEvent.MoveSucceeded, gameDriver.Move("c5c7"));
    }
    

}
