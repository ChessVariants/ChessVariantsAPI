
using System.Collections.Generic;

namespace ChessVariantsLogic;
public interface MoveWorker
{

    public GameEvent Move(string move);

    public List<string> GetAllValidMoves(Player player);

    public (string, string) parseMove(string move);

    public Chessboard Board
    {
        get { return Chessboard.StandardChessboard(); }
        set { }
    }

    public MoveWorker CopyBoardState();
}