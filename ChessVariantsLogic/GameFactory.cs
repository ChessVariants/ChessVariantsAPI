namespace ChessVariantsLogic;

using ChessVariantsLogic.Actions;
using Predicates;

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

        List<IAction> whiteCastleKingSideActions = new List<IAction>
        {
            new ActionMovePiece("e1g1"),
            new ActionMovePiece("h1f1")
        };

        IPredicate castleKingSidePredicate = !(whiteKingCheckedThisTurn);

        Move whiteCastleKingSide = new Move(whiteCastleKingSideActions, castleKingSidePredicate, "e1h1");

        
        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, new HashSet<Move>() { whiteCastleKingSide }, new HashSet<Move>() {  });
        RuleSet rulesBlack = new RuleSet(blackMoveRule, blackWinRule, new HashSet<Move>() { whiteCastleKingSide }, new HashSet<Move>() { });



        return new Game(new MoveWorker(Chessboard.StandardChessboard()), Player.White, 1, rulesWhite, rulesBlack);
    }

    public static Game CaptureTheKing()
    {
        IPredicate whiteMoveRule = new Const(true);
        IPredicate whiteWinRule = new PiecesLeft(Constants.BlackKingIdentifier, Comparator.EQUALS, 0, BoardState.THIS);

        
        IPredicate blackMoveRule = new Const(true);
        IPredicate blackWinRule = new PiecesLeft(Constants.WhiteKingIdentifier, Comparator.EQUALS, 0, BoardState.THIS);

        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, new List<Move>());
        RuleSet rulesBlack = new RuleSet(whiteMoveRule, whiteWinRule, new List<Move>());
        
        return new Game(new MoveWorker(Chessboard.StandardChessboard()), Player.White, 1, rulesWhite, rulesBlack);
    }

    public static Game AntiChess()
    {
        
        IPredicate whiteMoveRule = new Operator(new Attacked(BoardState.THIS, "ANY_BLACK"), IMPLIES, new PieceCaptured("ANY_BLACK"));
        IPredicate whiteWinRule = new PiecesLeft("ANY_WHITE", Comparator.EQUALS, 0, BoardState.THIS);
        
        IPredicate blackMoveRule = new Operator(new Attacked(BoardState.THIS, "ANY_WHITE"), IMPLIES, new PieceCaptured("ANY_WHITE"));
        IPredicate blackWinRule = new PiecesLeft("ANY_BLACK", Comparator.EQUALS, 0, BoardState.THIS);

        RuleSet rulesWhite = new RuleSet(whiteMoveRule, whiteWinRule, new List<Move>());
        RuleSet rulesBlack = new RuleSet(blackMoveRule, blackWinRule, new List<Move>());

        return new Game(new MoveWorker(Chessboard.StandardChessboard()), Player.White, 1, rulesWhite, rulesBlack);
    }
}