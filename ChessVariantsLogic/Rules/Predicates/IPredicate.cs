namespace ChessVariantsLogic.Rules.Predicates;

/// <summary>
/// Represents a logical predicate which can evaluate a <see cref="BoardTransition"/> object to either true or false.
/// </summary>
public interface IPredicate
{
    /// <summary>
    /// Evaluates a <see cref="BoardTransition"/> object to either true or false according to implementation.
    /// </summary>
    /// <param name="transition"></param>
    /// 
    /// <returns>A boolean value according to the implementation</returns>
    /// 
    bool Evaluate(BoardTransition transition);

    public static IPredicate operator |(IPredicate p, IPredicate q)
        => new Operator(p, OperatorType.OR, q);

    public static IPredicate operator &(IPredicate p, IPredicate q)
        => new Operator(p, OperatorType.AND, q);

    public static IPredicate operator !(IPredicate a)
        => new Operator(OperatorType.NOT, a);

    public static IPredicate operator ^(IPredicate p, IPredicate q)
        => new Operator(p, OperatorType.XOR, q);

    public static IPredicate operator -(IPredicate p, IPredicate q)
        => new Operator(p, OperatorType.IMPLIES, q);

}
