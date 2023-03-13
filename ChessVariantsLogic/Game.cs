namespace ChessVariantsLogic;

using ChessVariantsLogic.Rules;
using ChessVariantsLogic.Rules.Moves;
using ChessVariantsLogic.Export;
using System;

public class Game {

    private readonly MoveWorker _moveWorker;
    private Player _playerTurn;
    private int _playerMovesRemaining;
    private readonly int _movesPerTurn;
    private readonly RuleSet _whiteRules;
    private readonly RuleSet _blackRules;

    public Game(MoveWorker moveWorker, Player playerToStart, int movesPerTurn, RuleSet whiteRules, RuleSet blackRules)
    {
        _moveWorker = moveWorker;
        _playerTurn = playerToStart;
        _movesPerTurn = _playerMovesRemaining = movesPerTurn;
        _whiteRules = whiteRules;
        _blackRules = blackRules;
    }

    /// <summary>
    /// Checks whether the given <paramref name="playerRequestingMove"/> is the one to move.
    /// </summary>
    /// <param name="move">The move requested to be made</param>
    /// <param name="playerRequestingMove">The player requesting to make a move</param>
    public GameEvent MakeMove(string move, Player? playerRequestingMove)
    {
        if (playerRequestingMove != _playerTurn)
        {
            return GameEvent.InvalidMove;
        }
        return MakeMoveImpl(move);
    }

    /// <summary>
    /// Checks whether the given <paramref name="move"/> is valid and if so does the move and return correct GameEvent.
    /// </summary>
    /// <param name="move">The move requested to be made</param>
    /// <returns>GameEvent of what happened in the game</returns>
    private GameEvent MakeMoveImpl(string move) {
        IEnumerable<Move> validMoves;
        if (_playerTurn == Player.White) {
            validMoves = _whiteRules.ApplyMoveRule(_moveWorker, _playerTurn);
        } else {
            validMoves = _blackRules.ApplyMoveRule(_moveWorker, _playerTurn);
        }
        Move? movePerformed = GetMove(validMoves, move);
        if (movePerformed == null) return GameEvent.InvalidMove;
        if (validMoves.Contains(movePerformed)) {
        
            GameEvent gameEvent = movePerformed.Perform(_moveWorker);

            if(gameEvent == GameEvent.InvalidMove)
                return gameEvent;

            /// TODO: Check for a tie

            if(_whiteRules.ApplyWinRule(_moveWorker)) {
                return GameEvent.WhiteWon;
            }
            if (_blackRules.ApplyWinRule(_moveWorker))
            {
                return GameEvent.BlackWon;
            }

            if (false)
                return GameEvent.Tie;

            DecrementPlayerMoves();
            return gameEvent;
        }
        return GameEvent.InvalidMove;
    }

    private Move? GetMove(IEnumerable<Move> validMoves, string moveCoords)
    {
        foreach(Move move in validMoves)
        {
            if(move.FromTo.Equals(moveCoords))
            {
                return move;
            }
        }
        return null;
    }

    /// <summary>
    /// Decrements the number of moves remaining for the current player. If the player has no moves left, switches the player turn and resets the number of moves remaining.
    /// </summary>
    private void DecrementPlayerMoves() {
        _playerMovesRemaining--;
        if (_playerMovesRemaining <= 0) {
            _playerTurn = _playerTurn == Player.White ? Player.Black : Player.White;
            _playerMovesRemaining = _movesPerTurn;
        }
    }

    public string ExportStateAsJson()
    {
        RuleSet rules = _playerTurn == Player.White ? _whiteRules : _blackRules;
        return GameExporter.ExportGameStateAsJson(_moveWorker.Board, _playerTurn, rules.GetLegalMoveDict(_playerTurn, _moveWorker));
    }
}



public enum GameEvent {
    InvalidMove,
    MoveSucceeded,
    WhiteWon,
    BlackWon,
    Tie
}

public enum Player {
    White,
    Black,
    None
}

public static class PlayerExtensions
{
    public static string AsString(this Player player)
    {
        return player switch
        {
            Player.White => "white",
            Player.Black => "black",
            _ => throw new ArgumentException("Player must be either white or black"),
        };
    }
}