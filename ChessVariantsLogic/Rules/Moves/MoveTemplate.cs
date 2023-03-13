using ChessVariantsLogic.Rules.Moves.Actions;

namespace ChessVariantsLogic.Rules.Moves;

using ChessVariantsLogic.Rules.Predicates.ChessPredicates;
using Predicates;
using System;

/// <summary>
/// A MoveTemplate object represents a special move that can be performed by all pieces with the given _pieceIdentifier.
/// The core idea is that you have a _predicate and a list of _actions. The predicate must hold for the actions to be performed.
/// </summary>
public class MoveTemplate
{
    private readonly IEnumerable<IAction> _actions;
    private readonly IPredicate _predicate;
    private readonly string _pieceIdentifier;
    private readonly IPosition _to;

    public IEnumerable<IAction> Actions => _actions;
    public string PieceIdentifier => _pieceIdentifier;

    public MoveTemplate(IEnumerable<IAction> actions, IPredicate predicate, string pieceIdentifier, IPosition to)
    {
        _actions = actions;
        _predicate = predicate;
        _pieceIdentifier = pieceIdentifier;
        _to = to;
    }

    /// <summary>
    /// Constructs a Move for each piece with the given _pieceIdentifier.
    /// The Move is constructed from the internal _actions, _to and that piece's position.
    /// </summary>
    /// <param name="thisBoard">The current board state used to construct the Move list from</param>
    /// <param name="moveRule">The moveRule of the RuleSet retrieving the valid moves</param>
    /// 
    /// <returns>A list of special moves that can be performed on the given board state.</returns>
    /// 
    public IEnumerable<Move> GetValidMoves(MoveWorker thisBoard, IPredicate moveRule)
    {
        List<string> positions = (List<string>) Utils.FindPiecesOfType(thisBoard, _pieceIdentifier);
        HashSet<Move> moves = new HashSet<Move>();

        foreach(string from in positions)
        {
            string? to = _to.GetPosition(thisBoard, from);
            if (to == null) continue;

            Move move = new Move(_actions, from + to);
            
            BoardTransition transition = new BoardTransition(thisBoard, move);
            if(_predicate.Evaluate(transition) && moveRule.Evaluate(transition) && transition.IsValid())
            {
                moves.Add(move);
            }

        }

        return moves;
    }

    public static MoveTemplate CastleMove(Player player, bool kingSide, bool kingCanMoveThroughChecks)
    {
        string KingIdentifier = player == Player.White ? Constants.WhiteKingIdentifier : Constants.BlackKingIdentifier;
        string RookIdentifier = player == Player.White ? Constants.WhiteRookIdentifier : Constants.BlackRookIdentifier;
        Player attacker = player == Player.White ? Player.Black : Player.White;
        
        IPredicate castlePredicate;

        string rank = player == Player.White ? "1" : "8";
        string kingFromFile = "e";
        string kingToFile = kingSide ? "g" : "c";
        string rookFromFile = kingSide ? "h" : "a";
        string rookToFile = kingSide ? "f" : "d";
        int kingSideMultiplier = kingSide ? 1 : -1;

        int relativeRookX = kingSide ? 3 : -4;

        IPosition kingPosition = new PositionRelative(row: 0, col: 0);
        IPosition squareBetween = new PositionRelative(row: 0, col: kingSideMultiplier);
        IPosition rookPosition = new PositionRelative(row: 0, col: relativeRookX);

        IPredicate kingCheckedThisState = new Attacked(BoardState.THIS, KingIdentifier);
        IPredicate kingCheckedNextState = new Attacked(BoardState.NEXT, KingIdentifier);


        IPredicate squareBetweenAttacked = new SquareAttacked(squareBetween, BoardState.THIS, attacker);

        IPredicate kingMovesThroughCheck = kingCheckedThisState | squareBetweenAttacked | kingCheckedNextState;

        IPredicate squareEmpty1 = new PieceAt(Constants.UnoccupiedSquareIdentifier, new PositionRelative(row: 0, col: 1 * kingSideMultiplier), BoardState.THIS);
        IPredicate squareEmpty2 = new PieceAt(Constants.UnoccupiedSquareIdentifier, new PositionRelative(row: 0, col: 2 * kingSideMultiplier), BoardState.THIS);
        IPredicate squareEmpty3 = new PieceAt(Constants.UnoccupiedSquareIdentifier, new PositionRelative(row: 0, col: 3 * kingSideMultiplier), BoardState.THIS);

        IPredicate kingHasMoved = new HasMoved(kingPosition);
        IPredicate rookHasMoved = new HasMoved(rookPosition);

        IPredicate kingAtSquare = new PieceAt(KingIdentifier, kingPosition, BoardState.THIS);
        IPredicate rookAtSquare = new PieceAt(RookIdentifier, rookPosition, BoardState.THIS);

        castlePredicate = squareEmpty1 & squareEmpty2 & kingAtSquare & rookAtSquare & !kingHasMoved & !rookHasMoved; ;

        if (!kingSide)
            castlePredicate &= squareEmpty3;

        if (!kingCanMoveThroughChecks)
        {
            castlePredicate &= !kingMovesThroughCheck;
        }

        IPosition kingFromPos = new PositionAbsolute(kingFromFile + rank);
        IPosition kingToPos = new PositionAbsolute(kingToFile + rank);

        IPosition rookFromPos = new PositionAbsolute(rookFromFile + rank);
        IPosition rookToPos = new PositionAbsolute(rookToFile + rank);

        IEnumerable<IAction> castleActions = new List<IAction>
        {
            new ActionMovePiece(kingFromPos, kingToPos),
            new ActionMovePiece(rookFromPos, rookToPos)
        };

        return new MoveTemplate(castleActions, castlePredicate, KingIdentifier, kingToPos);
    }

    
    
    public static MoveTemplate PawnDoubleMove(Player player)
    {
        int playerMultiplier = player == Player.White ? -1 : 1;
        string PawnIdentifier = player == Player.White ? Constants.WhitePawnIdentifier : Constants.BlackPawnIdentifier;
        IPosition thisPawnPosition = new PositionRelative(row: 0, col: 0);
        IPosition forwardPosition1 = new PositionRelative(row: 1 * playerMultiplier, col: 0);
        IPosition forwardPosition2 = new PositionRelative(row: 2 * playerMultiplier, col: 0);

        IPredicate hasMoved = new HasMoved(thisPawnPosition);
        IPredicate targetSquareEmpty1 = new PieceAt(Constants.UnoccupiedSquareIdentifier, forwardPosition1, BoardState.THIS);
        IPredicate targetSquareEmpty2 = new PieceAt(Constants.UnoccupiedSquareIdentifier, forwardPosition2, BoardState.THIS);

        IEnumerable<IAction> actions = new List<IAction>
        {
            new ActionMovePiece(forwardPosition2)
        };
        //Times moved not yet implemented but we need to check that pawn hasn't moved before
        return new MoveTemplate(actions, !hasMoved & targetSquareEmpty1 & targetSquareEmpty2, PawnIdentifier, forwardPosition2);
    }



    public static MoveTemplate EnPassantMove(Player player, bool right)
    {
        int playerMultiplier = player == Player.White ? -1 : 1;
        int sideMultiplier = right ? 1 : -1;
        string PawnIdentifier = player == Player.White ? Constants.WhitePawnIdentifier : Constants.BlackPawnIdentifier;
        string OpponentPawnIdentifier = player == Player.White ? Constants.BlackPawnIdentifier : Constants.WhitePawnIdentifier;

        IPosition finalPosition = new PositionRelative(row: playerMultiplier, col: sideMultiplier);
        IPosition enemyPawnPosition = new PositionRelative(row: 0, col: sideMultiplier);
        IPosition enemyPawnPositionFrom = new PositionRelative(row: 2 * playerMultiplier, col: sideMultiplier);

        IPredicate enemyPawnNextTo = new PieceAt(OpponentPawnIdentifier, enemyPawnPosition, BoardState.THIS);
        IPredicate targetSquareEmpty = new PieceAt(Constants.UnoccupiedSquareIdentifier, finalPosition, BoardState.THIS);
        IPredicate pawnJustDidDoubleMove = new LastMove(enemyPawnPositionFrom, enemyPawnPosition);

        IEnumerable<IAction> actions = new List<IAction>
        {
            new ActionMovePiece(finalPosition),
            new ActionDeletePiece(enemyPawnPosition)
        };

        return new MoveTemplate(actions, enemyPawnNextTo & targetSquareEmpty & pawnJustDidDoubleMove, PawnIdentifier, finalPosition);
    }

}