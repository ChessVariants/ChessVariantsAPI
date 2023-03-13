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
    [Fact(Skip = "Constants changed to two characters")]
    public void Test_FEN()
    {
        var moveWorker = new MoveWorker(new Chessboard(6), Piece.AllStandardPieces());

        Assert.Equal("6/6/6/6/6/6", moveWorker.Board.ReadBoardAsFEN());

        moveWorker.Board = new Chessboard(12, 3);
        Assert.Equal("3/3/3/3/3/3/3/3/3/3/3/3", moveWorker.Board.ReadBoardAsFEN());

        moveWorker.Board.Insert(Constants.BlackBishopIdentifier, "b2");
        Assert.Equal("3/3/3/3/3/3/3/3/3/3/1b1/3", moveWorker.Board.ReadBoardAsFEN());

        moveWorker.Board = Chessboard.StandardChessboard();
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", moveWorker.Board.ReadBoardAsFEN());

        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("a2a3"));
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/P7/1PPPPPPP/RNBQKBNR", moveWorker.Board.ReadBoardAsFEN());

    }

    /// <summary>
    /// Tests that the standard chessboard is set up correctly.
    /// </summary>
    [Fact]
    public void Test_Standard_Chessboard()
    {
        var board = Chessboard.StandardChessboard();
        Assert.Equal(Constants.WhiteKingIdentifier,         board.GetPieceIdentifier("e1"));
        Assert.Equal(Constants.BlackQueenIdentifier,        board.GetPieceIdentifier("d8"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  board.GetPieceIdentifier("e4"));
        Assert.Equal(Constants.BlackRookIdentifier,         board.GetPieceIdentifier("a8"));
    }

    /// <summary>
    /// Tests that moving a piece updates the board correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Pawn()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("g2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("g3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("g2g3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("g2"));
        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("g3"));
        
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("h2h9"));
        Assert.Equal(Constants.WhitePawnIdentifier, moveWorker.Board.GetPieceIdentifier("h2"));

    }

    /// <summary>
    /// Test that a rook can move correcly on non-standard chessboard.
    /// </summary>
    [Fact]
    public void Test_Rook_Rectangular_Board()
    {
        var moveWorker = new MoveWorker(new Chessboard(4,10));
        
        Assert.True(moveWorker.InsertOnBoard(Piece.Rook(PieceClassifier.BLACK), "b2"));

        Assert.Equal(Constants.BlackRookIdentifier,         moveWorker.Board.GetPieceIdentifier("b2"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("b2i2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("b2"));
        Assert.Equal(Constants.BlackRookIdentifier,         moveWorker.Board.GetPieceIdentifier("i2"));

        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("i2i4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("i2"));
        Assert.Equal(Constants.BlackRookIdentifier,         moveWorker.Board.GetPieceIdentifier("i4"));

    }

    /// <summary>
    /// Tests that invalid moves are not processed.
    /// </summary>
    [Fact]
    public void Test_Invalid_Move()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteRookIdentifier,         moveWorker.Board.GetPieceIdentifier("h1"));
        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("h2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("h4"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("h1h4"));
        Assert.Equal(Constants.WhiteRookIdentifier,         moveWorker.Board.GetPieceIdentifier("h1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("h4"));
        
        Assert.Equal(Constants.BlackKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("b8"));
        Assert.Equal(Constants.BlackPawnIdentifier,         moveWorker.Board.GetPieceIdentifier("d7"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("b8d7"));
        Assert.Equal(Constants.BlackKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("b8"));
        Assert.Equal(Constants.BlackPawnIdentifier,         moveWorker.Board.GetPieceIdentifier("d7"));

        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e5"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("e4e5"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e5"));

    }

    /// <summary>
    /// Test that a serier of moves can be processed correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Serie()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("a2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("a3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("a2a3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("a3a4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("a2"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("a3"));
        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("a4"));
    }

    /// <summary>
    /// Tests that a knight can jump over pieces and move correctly.
    /// </summary>
    [Fact]
    public void Test_Move_Knight()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("g1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("g1h3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("g1"));
        Assert.Equal(Constants.WhiteKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h3f4"));
    }
      
    /// <summary>
    /// Test that pices can be captured correctly.
    /// </summary>
    [Fact]
     public void Test_Take()
     {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("g1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("h3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("g1h3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h3g5"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("g5h7"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("g5"));
        Assert.Equal(Constants.WhiteKnightIdentifier,       moveWorker.Board.GetPieceIdentifier("h7"));
     }

    /// <summary>
    /// Tests that a bishop, a piece that can not jump, can capture opponents pieces.
    /// </summary>
     [Fact]
     public void PieceNotJumpCanCapture_shouldReturnTrue()
     {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.BlackPawnIdentifier, moveWorker.Board.GetPieceIdentifier("b7"));

        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("f1a6"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("a6b7"));

        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("a6"));
        Assert.Equal(Constants.WhiteBishopIdentifier,       moveWorker.Board.GetPieceIdentifier("b7"));
     }

    [Fact]
     public void PawnCanCaptureDiagonally_shouldReturnTrue()
     {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.BlackPawnIdentifier, moveWorker.Board.GetPieceIdentifier("d7"));

        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e3e4"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e4e5"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e5e6"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e6d7"));

        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e6"));
        Assert.Equal(Constants.WhitePawnIdentifier,         moveWorker.Board.GetPieceIdentifier("d7"));
     }
     
     /// <summary>
     /// Test that the bishop can move correctly diagonally.
     /// </summary>
     [Fact]
     public void Test_Bishop()
     {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("f1c4"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("f1c4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("f1"));
        Assert.Equal(Constants.WhiteBishopIdentifier,       moveWorker.Board.GetPieceIdentifier("c4"));
     }

    
     [Fact]
     public void Test_Move_King()
     {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        Assert.Equal(Constants.WhiteKingIdentifier,  moveWorker.Board.GetPieceIdentifier("e1"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e1e2"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("e2e3"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier,  moveWorker.Board.GetPieceIdentifier("e1"));
        Assert.Equal(Constants.WhiteKingIdentifier,  moveWorker.Board.GetPieceIdentifier("e2"));
        Assert.Equal(Constants.WhitePawnIdentifier,  moveWorker.Board.GetPieceIdentifier("e3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2f3"));

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
        var moveWorker = new MoveWorker(new Chessboard(6,5));
        
        Assert.True(moveWorker.InsertOnBoard(Piece.Queen(PieceClassifier.WHITE), "c4"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, moveWorker.Board.GetPieceIdentifier("a6"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("c4a6"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, moveWorker.Board.GetPieceIdentifier("c4"));
        Assert.Equal(Constants.WhiteQueenIdentifier, moveWorker.Board.GetPieceIdentifier("a6"));

        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("a6a1"));
        Assert.Equal(Constants.UnoccupiedSquareIdentifier, moveWorker.Board.GetPieceIdentifier("a6"));
        Assert.Equal(Constants.WhiteQueenIdentifier, moveWorker.Board.GetPieceIdentifier("a1"));

    }

    /// <summary>
    /// Test that a custom piece with a novel movement pattern moves correctly.
    /// </summary>
    [Fact]
    public void Test_Custom_Piece()
    {
        var moveWorker = new MoveWorker(new Chessboard(8));

        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North,     1, 3),
            new RegularPattern(Constants.NorthEast, 1, 1),
            new RegularPattern(Constants.SouthEast, 2, 2),
            new RegularPattern(Constants.SouthWest, 1, 3),
            new RegularPattern(Constants.NorthWest, 2, 4),

        };
        var mp = new MovementPattern(patterns);
        Piece piece = new Piece(mp, mp, false, PieceClassifier.WHITE, "C");

        Assert.True(moveWorker.InsertOnBoard(piece, "c4"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("c4c5"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("c5d6"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("d6b4"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("b4d2"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("d2a5"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("a5b5"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("a5b4"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("a5a4"));

        Assert.True(moveWorker.InsertOnBoard(piece, "h3"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("h3c8"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h3d7"));
    }

    /// <summary>
    /// Tests that a custom piece can repeat its movement pattern, if allowed.
    /// </summary>
    [Fact]
    public void Test_Custom_Piece_Repeat()
    {
        var moveWorker = new MoveWorker(new Chessboard(8));

        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North, 1, 8),
            new RegularPattern(Constants.West,  1, 8),
        };
        var mp = new MovementPattern(patterns);
        Piece piece = new Piece(mp, mp, false, PieceClassifier.WHITE, 1, "C");

        Assert.True(moveWorker.InsertOnBoard(piece, "h4"));
       

        var piece2 = Piece.BlackPawn();
        Assert.True(moveWorker.InsertOnBoard(piece2, "h6"));
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("h4h7"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h4c5"));
        
    }

    /// <summary>
    /// Test that the correct amount of valid moves are found.
    /// </summary>
    [Fact]
    public void Test_GetAllValidMoves()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        var moves = moveWorker.GetAllValidMoves(Player.White);
        Assert.Equal(12, moves.Count);
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        var moves2 = moveWorker.GetAllValidMoves(Player.White);
        Assert.Equal(23, moves2.Count);

        moveWorker.Board = new Chessboard(8);
        Assert.True(moveWorker.InsertOnBoard(Piece.Queen(PieceClassifier.WHITE), "e4"));
        var movesQueen = moveWorker.GetAllValidMoves(Player.White);
        Assert.Equal(27, movesQueen.Count);
    }

    /// <summary>
    /// Test that the correct amount of valid capture movesare found. 
    /// </summary>
    [Fact]
    public void Test_GetAllCaptureMoves()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        var moves = moveWorker.GetAllCaptureMoves(Player.White);
        Assert.Equal(18, moves.Count);
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e2e3"));
        var moves2 = moveWorker.GetAllCaptureMoves(Player.White);
        Assert.Equal(27, moves2.Count);
        
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("e7e6"));
        Assert.Equal(2, moveWorker.Movelog.Count);
        
        moveWorker.Board = new Chessboard(8);

        Assert.True(moveWorker.InsertOnBoard(Piece.Queen(PieceClassifier.WHITE), "e4"));
        var movesQueen = moveWorker.GetAllCaptureMoves(Player.White);
        Assert.Equal(27, movesQueen.Count);
    }

    [Fact]
    public void TestDifferentCapturePattern()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        var patterns = new List<IPattern> {
            new RegularPattern(Constants.North, 1, 8),
            new RegularPattern(Constants.East, 1, 8),
            new RegularPattern(Constants.South, 1, 8),
            new RegularPattern(Constants.West, 1, 8),
        };

        var capturePatterns = new List<IPattern> {
            new RegularPattern(Constants.NorthEast, 1, 8),
            new RegularPattern(Constants.SouthEast, 1, 8),
            new RegularPattern(Constants.SouthWest, 1, 8),
            new RegularPattern(Constants.NorthWest, 1, 8),
        };

        var mp = new MovementPattern(patterns);
        var cp = new MovementPattern(capturePatterns);
        Piece piece = new Piece(mp, cp, false, PieceClassifier.WHITE, "C");

        Assert.True(moveWorker.InsertOnBoard(piece, "h1"));
        
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h2h3"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h3h4"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h1h3"));
        
        Assert.Equal(GameEvent.InvalidMove, moveWorker.Move("h3e6"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("h3d7"));

    }

    [Fact]
    public void MoveLikeBishop_captureLikeKnight()
    {
        var moveWorker = new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces());

        var patterns = new List<IPattern> {
            new RegularPattern(Constants.NorthEast, 1, 8),
            new RegularPattern(Constants.SouthEast, 1, 8),
            new RegularPattern(Constants.SouthWest, 1, 8),
            new RegularPattern(Constants.NorthWest, 1, 8),
        };

        var capturePattern = new List<IPattern> {
            new JumpPattern( 1, 2),
            new JumpPattern( 2, 1),
            new JumpPattern( 1,-2),
            new JumpPattern( 2,-1),
            new JumpPattern(-1, 2),
            new JumpPattern(-2, 1),
            new JumpPattern(-1,-2),
            new JumpPattern(-2,-1),
        };

        var mp = new MovementPattern(patterns);
        var cp = new MovementPattern(capturePattern);
        Piece piece = new Piece(mp, cp, false, PieceClassifier.WHITE, "C");

        Assert.True(moveWorker.InsertOnBoard(piece, "h1"));
        
        Assert.Equal(GameEvent.InvalidMove,     moveWorker.Move("h1g3"));
        Assert.Equal(GameEvent.MoveSucceeded,   moveWorker.Move("g2g3"));
        Assert.Equal(GameEvent.InvalidMove,     moveWorker.Move("h1b7"));
        Assert.Equal(GameEvent.MoveSucceeded,   moveWorker.Move("h1c6"));
        
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("c6d8"));

    }

    [Fact]
    public void TestJumpCapturePattern()
    {
        var moveWorker = new MoveWorker(new Chessboard(8));

        var patterns = new List<IPattern> {
            new JumpPattern( 1, 2),
            new JumpPattern( 2, 1),
            new JumpPattern( 1,-2),
            new JumpPattern( 2,-1),
            new JumpPattern(-1, 2),
            new JumpPattern(-2, 1),
            new JumpPattern(-1,-2),
            new JumpPattern(-2,-1),
        };
        
        var capturePatterns = new List<IPattern> {
            new JumpPattern( 3, 1),
            new JumpPattern( 1, 3),
            new JumpPattern(-1, 3),
            new JumpPattern(-3, 1),
        };
        
        var mp = new MovementPattern(patterns);
        var cp = new  MovementPattern(capturePatterns);
        Piece piece1 = new Piece(mp, cp, false, PieceClassifier.WHITE , "C");
        Piece piece2 = Piece.BlackPawn();
        
        Assert.True(moveWorker.InsertOnBoard(piece1, "d4"));
        Assert.True(moveWorker.InsertOnBoard(piece2, "e7"));
        Assert.Equal(GameEvent.MoveSucceeded, moveWorker.Move("d4e7"));
    }
}
