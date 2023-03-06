namespace ChessVariantsLogic;

public class JumpPattern : IPattern
{
    private readonly Tuple<int, int> pattern;

    public JumpPattern(int xOffset, int yOffset)
    {
        this.pattern = new Tuple<int, int>(xOffset, yOffset);
    }

#region Interface overrides
    public int GetXDir() { return pattern.Item1; }

    public int GetYDir() { return pattern.Item2; }

    public int GetXLength() { return -1; }

    public int GetYLength() { return -1; }

#endregion

}