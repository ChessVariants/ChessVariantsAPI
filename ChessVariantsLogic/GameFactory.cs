namespace ChessVariantsLogic;

using ChessVariantsLogic.Rules;
using ChessVariantsLogic.Rules.Moves;
using ChessVariantsLogic.Rules.Moves.Actions;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules.Predicates.ChessPredicates;

/// <summary>
/// Factory with predetermined rules for the known variants. WIP
/// </summary>
public static class GameFactory
{

    private static OperatorType AND = OperatorType.AND;
    private static OperatorType OR = OperatorType.OR;
    private static OperatorType IMPLIES = OperatorType.IMPLIES;
    private static OperatorType XOR = OperatorType.XOR;
    private static OperatorType EQUALS = OperatorType.EQUALS;
    private static OperatorType NOT = OperatorType.NOT;

    public const string StandardIdentifier = "standard";
    public const string CaptureTheKingIdentifier = "captureTheKing";
    public const string AntiChessIdentifier = "antiChess";

    public static Game StandardChess()
    {
        IPredicate blackKingCheckedThisTurn = new Attacked(BoardState.THIS, Constants.BlackKingIdentifier);
        IPredicate blackKingCheckedNextTurn = new Attacked(BoardState.NEXT, Constants.BlackKingIdentifier);
        IPredicate whiteKingCheckedThisTurn = new Attacked(BoardState.THIS, Constants.WhiteKingIdentifier);
        IPredicate whiteKingCheckedNextTurn = new Attacked(BoardState.NEXT, Constants.WhiteKingIdentifier);

        IPredicate blackKingCheckedThisAndNextTurn = new Operator(blackKingCheckedThisTurn, AND, blackKingCheckedNextTurn);
        IPredicate whiteKingCheckedThisAndNextTurn = new Operator(whiteKingCheckedThisTurn, AND, whiteKingCheckedNextTurn);
        
        IPredicate whiteWinRule = new ForEvery(blackKingCheckedThisAndNextTurn, Player.Black);  
        IPredicate blackWinRule = new ForEvery(whiteKingCheckedThisAndNextTurn, Player.White);

        IPredicate whiteMoveRule = new Operator(NOT, whiteKingCheckedNextTurn);        
        IPredicate blackMoveRule = new Operator(NOT, blackKingCheckedNextTurn);

        

        ISet<MoveTemplate> movesWhite = new HashSet<MoveTemplate>
        {
            MoveTemplate.CastleMove(Player.White, true, false),
            MoveTemplate.CastleMove(Player.White, false, false),
            MoveTemplate.PawnDoubleMove(Player.White),
            MoveTemplate.EnPassantMove(Player.White, true),
            MoveTemplate.EnPassantMove(Player.White, false),
        };


        ISet<MoveTemplate> movesBlack = new HashSet<MoveTemplate>
        {
            MoveTemplate.CastleMove(Player.Black, true, false),
            MoveTemplate.CastleMove(Player.Black, false, false),
            MoveTemplate.PawnDoubleMove(Player.Black),
            MoveTemplate.EnPassantMove(Player.Black, true),
            MoveTemplate.EnPassantMove(Player.Black, false),
        };


        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, movesWhite);
        RuleSet rulesBlack = new RuleSet(blackMoveRule, blackWinRule, movesBlack);


        return new Game(new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces()), Player.White, 1, rulesWhite, rulesBlack);
    }

    public static Game CaptureTheKing()
    {
        IPredicate whiteMoveRule = new Const(true);
        IPredicate whiteWinRule = new PiecesLeft(Constants.BlackKingIdentifier, Comparator.EQUALS, 0, BoardState.THIS);

        
        IPredicate blackMoveRule = new Const(true);
        IPredicate blackWinRule = new PiecesLeft(Constants.WhiteKingIdentifier, Comparator.EQUALS, 0, BoardState.THIS);

        ISet<MoveTemplate> movesWhite = new HashSet<MoveTemplate>
        {
            MoveTemplate.CastleMove(Player.White, true, true),
            MoveTemplate.CastleMove(Player.White, false, true),
            MoveTemplate.PawnDoubleMove(Player.White),
            MoveTemplate.EnPassantMove(Player.White, true),
            MoveTemplate.EnPassantMove(Player.White, false),
        };


        ISet<MoveTemplate> movesBlack = new HashSet<MoveTemplate>
        {
            MoveTemplate.CastleMove(Player.Black, true, true),
            MoveTemplate.CastleMove(Player.Black, false, true),
            MoveTemplate.PawnDoubleMove(Player.Black),
            MoveTemplate.EnPassantMove(Player.Black, true),
            MoveTemplate.EnPassantMove(Player.Black, false),
        };

        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, movesWhite);
        RuleSet rulesBlack = new RuleSet(whiteMoveRule, whiteWinRule, movesBlack);


        return new Game(new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces()), Player.White, 1, rulesWhite, rulesBlack);

    }

    public static Game AntiChess()
    {
        
        IPredicate whiteMoveRule = new Operator(new Attacked(BoardState.THIS, "ANY_BLACK"), IMPLIES, new PieceCaptured("ANY_BLACK"));
        IPredicate whiteWinRule = new PiecesLeft("ANY_WHITE", Comparator.EQUALS, 0, BoardState.THIS);
        
        IPredicate blackMoveRule = new Operator(new Attacked(BoardState.THIS, "ANY_WHITE"), IMPLIES, new PieceCaptured("ANY_WHITE"));
        IPredicate blackWinRule = new PiecesLeft("ANY_BLACK", Comparator.EQUALS, 0, BoardState.THIS);

        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, new HashSet<MoveTemplate>());
        RuleSet rulesBlack = new RuleSet(blackMoveRule, blackWinRule, new HashSet<MoveTemplate>());

        return new Game(new MoveWorker(Chessboard.StandardChessboard(), Piece.AllStandardPieces()), Player.White, 1, rulesWhite, rulesBlack);
    }

    /// <summary>
    /// Returns a game corresponding to the <paramref name="identifier"/>>.
    /// </summary>
    /// <param name="identifier">Variant identifier for the variant to create</param>
    /// <returns>A game corresponding to the <paramref name="identifier"/>>.</returns>
    /// <exception cref="ArgumentException">If there's no correspondence to the identifier</exception>
    public static Game FromIdentifier(string identifier)
    {
        return identifier switch
        {
            StandardIdentifier => StandardChess(),
            AntiChessIdentifier => AntiChess(),
            CaptureTheKingIdentifier => CaptureTheKing(),
            _ => throw new ArgumentException($"No variant corresponds to identifier: {identifier}"),
        };

    }
}