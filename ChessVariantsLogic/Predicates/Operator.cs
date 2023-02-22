﻿using System;

public class Operator : IPredicate
{
    private readonly PredType type;
    private readonly IPredicate p, q;

    public bool evaluatesThisBoardState { get; }


    public Operator(PredType type, IPredicate p, IPredicate q)
	{
        this.type = type;
        this.p = p;
        this.q = q;
        this.evaluatesThisBoardState = true;
	}


    public bool evaluate(string[,] thisBoardState, string[,] nextBoardState)
    {
        switch(type)
        {
            case PredType.AND: return p.evaluate(thisBoardState, nextBoardState) && q.evaluate(thisBoardState, nextBoardState);
            case PredType.OR: return p.evaluate(thisBoardState, nextBoardState) || q.evaluate(thisBoardState, nextBoardState);
            case PredType.IMPLIES: return !(p.evaluate(thisBoardState, nextBoardState)) || q.evaluate(thisBoardState, nextBoardState);
            case PredType.XOR: return p.evaluate(thisBoardState, nextBoardState) ^ q.evaluate(thisBoardState, nextBoardState);
            case PredType.EQUALS: return p.evaluate(thisBoardState, nextBoardState) == q.evaluate(thisBoardState, nextBoardState);
            default: return false;
        }
    }

}

public enum PredType {
    AND, OR, IMPLIES, XOR, EQUALS
}