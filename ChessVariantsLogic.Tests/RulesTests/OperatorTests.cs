using Xunit;
using System;
using ChessVariantsLogic.Rules.Predicates;
using ChessVariantsLogic.Rules;

namespace ChessVariantsLogic.Tests;

public class OperatorTests : IDisposable {

    IPredicate constTrue;
    IPredicate constFalse;
    MoveWorker board;
    BoardTransition transition;

    public OperatorTests()
    {
        constTrue = new Const(true);
        constFalse = new Const(false);
        board = new MoveWorker(new Chessboard(8));
        transition = new BoardTransition(board, board, "a1a1");
    }

    public void Dispose()
    {
        board = new MoveWorker(new Chessboard(8));
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void OperatorAND_shouldReturnTrue()
    {
        var op = new Operator(constTrue, OperatorType.AND, constTrue);
        Assert.True(op.Evaluate(transition));
    }

    [Fact]
    public void OperatorAND_shouldReturnFalse()
    {
        var op1 = new Operator(constTrue, OperatorType.AND, constFalse);
        var op2 = new Operator(constFalse, OperatorType.AND, constFalse);
        Assert.False(op1.Evaluate(transition));
        Assert.False(op2.Evaluate(transition));
    }

    [Fact]
    public void OperatorOR_shouldReturnTrue()
    {
        var op = new Operator(constTrue, OperatorType.OR, constTrue);
        var op1 = new Operator(constTrue, OperatorType.OR, constFalse);
        var op2 = new Operator(constFalse, OperatorType.OR, constTrue);
        Assert.True(op.Evaluate(transition));
        Assert.True(op1.Evaluate(transition));
        Assert.True(op2.Evaluate(transition));
    }

    [Fact]
    public void OperatorOR_shouldReturnFalse()
    {
        var op = new Operator(constFalse, OperatorType.OR, constFalse);
        Assert.False(op.Evaluate(transition));
    }

    [Fact]
    public void OperatorIMPLIES_shouldReturnTrue()
    {
        var op = new Operator(constTrue, OperatorType.IMPLIES, constTrue);
        var op1 = new Operator(constFalse, OperatorType.IMPLIES, constTrue);
        var op2 = new Operator(constFalse, OperatorType.IMPLIES, constFalse);
        Assert.True(op.Evaluate(transition));
        Assert.True(op1.Evaluate(transition));
        Assert.True(op2.Evaluate(transition));
    }

    [Fact]
    public void OperatorIMPLIES_shouldReturnFalse()
    {
        var op = new Operator(constTrue, OperatorType.IMPLIES, constFalse);
        Assert.False(op.Evaluate(transition));
    }

    [Fact]
    public void OperatorXOR_shouldReturnTrue()
    {
        var op = new Operator(constTrue, OperatorType.XOR, constFalse);
        var op1 = new Operator(constFalse, OperatorType.XOR, constTrue);
        Assert.True(op.Evaluate(transition));
        Assert.True(op1.Evaluate(transition));
    }

    [Fact]
    public void OperatorXOR_shouldReturnFalse()
    {
        var op = new Operator(constTrue, OperatorType.XOR, constTrue);
        var op1 = new Operator(constFalse, OperatorType.XOR, constFalse);
        Assert.False(op.Evaluate(transition));
        Assert.False(op1.Evaluate(transition));
    }

    [Fact]
    public void OperatorEQUALS_shouldReturnTrue()
    {
        var op = new Operator(constTrue, OperatorType.EQUALS, constTrue);
        var op1 = new Operator(constFalse, OperatorType.EQUALS, constFalse);
        Assert.True(op.Evaluate(transition));
        Assert.True(op1.Evaluate(transition));
    }

    [Fact]
    public void OperatorEQUALS_shouldReturnFalse()
    {
        var op = new Operator(constTrue, OperatorType.EQUALS, constFalse);
        var op1 = new Operator(constFalse, OperatorType.EQUALS, constTrue);
        Assert.False(op.Evaluate(transition));
        Assert.False(op1.Evaluate(transition));
    }

    [Fact]
    public void OperatorNOT_shouldReturnTrue()
    {
        var op = new Operator(OperatorType.NOT, constFalse);
        Assert.True(op.Evaluate(transition));
    }

    [Fact]
    public void OperatorNOT_shouldReturnFalse()
    {
        var op = new Operator(OperatorType.NOT, constTrue);
        Assert.False(op.Evaluate(transition));
    }

    [Fact]
    public void OperatorNOT_shouldThrowExceptionWhenTwoPredicates()
    {
        Assert.Throws<ArgumentException>(() => new Operator(constTrue, OperatorType.NOT, constFalse));
    }

    [Fact]
    public void OperatorExceptNOT_shouldThrowExceptionWhenOnePredicate()
    {
        Assert.Throws<ArgumentException>(() => new Operator(OperatorType.AND, constTrue));
        Assert.Throws<ArgumentException>(() => new Operator(OperatorType.OR, constTrue));
        Assert.Throws<ArgumentException>(() => new Operator(OperatorType.IMPLIES, constTrue));
        Assert.Throws<ArgumentException>(() => new Operator(OperatorType.XOR, constTrue));
        Assert.Throws<ArgumentException>(() => new Operator(OperatorType.EQUALS, constTrue));
    }
}