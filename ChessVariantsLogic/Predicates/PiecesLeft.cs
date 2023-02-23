﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessVariantsLogic.Predicates;
internal class PiecesLeft : IPredicate
{
    Comparator comparator;
    int compareValue;
    BoardState state;
    string pieceIdentifier;
    string player;

    public PiecesLeft(Comparator comparator, int compareValue, BoardState state, string pieceIdentifier, string player)
    {
        this.comparator = comparator;
        this.compareValue = compareValue;
        this.state = state;
        this.pieceIdentifier = pieceIdentifier;
        this.player = player;
    }

    public bool evaluate(Chessboard thisBoardState, Chessboard nextBoardState)
    {
        Chessboard board = state == BoardState.THIS ? thisBoardState : nextBoardState;
        int piecesLeft = FindPiecesOfType(board, player, pieceIdentifier).Count();
        return piecesLeft == compareValue;
    }


    private static IEnumerable<string> FindPiecesOfType(Chessboard board, string player, string pieceIdentifier)
    {
        var pieceLocations = new List<string>();
        foreach (var position in board.CoorToIndex.Keys)
        {
            if (IsOfType(position, board, player, pieceIdentifier))
            {
                pieceLocations.Add(position);
            }
        }
        return pieceLocations;
    }

    private static bool IsOfType(string position, Chessboard board, string player, string pieceIdentifier)
    {
        var piece = board.GetPiece(position);
        switch (pieceIdentifier)
        {
            case "ANY":
                return piece != Constants.UnoccupiedSquareIdentifier;
            case "ROYAL":
                {
                    if (player == "black")
                    {
                        return piece == Constants.BlackKingIdentifier;
                    }
                    return piece == Constants.WhiteKingIdentifier;
                }
            default: return piece == pieceIdentifier;
        }
    }

}

public enum Comparator{
    GREATER_THAN, LESS_THAN, GREATER_THAN_OR_EQUALS, LESS_THAN_OR_EQUALS, EQUALS, NOT_EQUALS
}

