using Newtonsoft.Json;
using static ChessVariantsLogic.Game;
namespace ChessVariantsLogic.Export;
public static class GameExporter
{
    public static string ExportGameStateAsJson(Chessboard chessboard, Player sideToMove, Dictionary<string, List<string>> moveDict)
    {
        var gameState = ExportGameState(chessboard, sideToMove, moveDict);
        return gameState.AsJson();
    }

    /// <summary>
    /// Gives a <see cref="GameState"/> object representing the current state of the game
    /// </summary>
    /// <param name="chessboard">The board to export</param>
    /// <param name="sideToMove">The side whose turn it is to move</param>
    /// <param name="moveDict">Moves to export</param>
    /// <returns></returns>
    public static GameState ExportGameState(Chessboard chessboard, Player sideToMove, Dictionary<string, List<string>> moveDict)
    {
        return new GameState
        {
            SideToMove = sideToMove.AsString(),
            Board = ExportBoard(chessboard),
            BoardSize = new BoardSize { Rows = chessboard.Rows, Cols = chessboard.Cols },
            Moves = ExportMoves(moveDict)
        };
    }

    private static List<MoveRecord> ExportMoves(Dictionary<string, List<string>> moveDict)
    {
        var moves = new List<MoveRecord>();
        foreach (var move in moveDict)
        {
            moves.Add(new MoveRecord { From = move.Key, To = move.Value });
        }
        return moves;
    }

    private static List<string> ExportBoard(Chessboard board)
    {
        var boardPieces = new List<string>();
        int sameConsecutivePieceCount = 1;
        string previousPieceIdentifier = "";

        foreach (var pos in board.GetAllCoordinates())
        {
            string pieceIdentifier = TryGetPieceIdentifier(board, pos);

            if (pieceIdentifier == previousPieceIdentifier)
            {
                sameConsecutivePieceCount++;
            }
            else
            {
                AddPieces(boardPieces, sameConsecutivePieceCount, previousPieceIdentifier);
                sameConsecutivePieceCount = 1;
            }
            previousPieceIdentifier = pieceIdentifier;
        }

        AddPieces(boardPieces, sameConsecutivePieceCount, previousPieceIdentifier);
        return boardPieces;
    }

    private static void AddPieces(List<string> boardPieces, int sameConsecutivePieceCount, string previousPieceIdentifier)
    {
        if (previousPieceIdentifier == "")
        {
            return;
        }
        var count = sameConsecutivePieceCount > 1 ? $"{sameConsecutivePieceCount}" : "";
        boardPieces.Add($"{previousPieceIdentifier}{count}");
    }

    private static string TryGetPieceIdentifier(Chessboard board, (int, int) pos)
    {
        var pieceIdentifier = board.GetPieceIdentifier(pos.Item1, pos.Item2);
        if (pieceIdentifier == null)
        {
            throw new InvalidOperationException($"Position {pos} was eligible for board of size {board.Rows}x{board.Cols}");
        }
        return pieceIdentifier;
    }
}

/// <summary>
/// Represents an exportation of the state of the game and
/// can be exported to a JSON formatted string. for communication between entities.
/// </summary>
public record GameState
{
    [JsonProperty("sideToMove")]
    public string SideToMove { get; set; } = null!;

    [JsonProperty("board")]
    public List<string> Board { get; set; } = null!;

    [JsonProperty("boardSize")]
    public BoardSize BoardSize { get; set; } = null!;

    [JsonProperty("moves")]
    public List<MoveRecord> Moves { get; set; } = null!;

    public string AsJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}

/// <summary>
/// Represents a json-object of the board size
/// </summary>
public record BoardSize
{
    [JsonProperty("row")]
    public int Rows { get; set; }

    [JsonProperty("col")]
    public int Cols { get; set; }
}

/// <summary>
/// Represents a json-object of which moves a piece can make.
/// </summary>
public record MoveRecord
{
    [JsonProperty("from")]
    public string From { get; set; } = null!;

    [JsonProperty("to")]
    public IEnumerable<string> To { get; set; } = null!;
}