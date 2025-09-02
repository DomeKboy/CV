using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Ghost : Pieces
    {
        public Ghost(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved) : base(color, curRow, curCol, actpeaceHasAlreadyMoved)
        {
        }

        public override int GetPieceValue()
        {
            return 0;
        }

        public override bool IsMoveLegal(int targetRow, int targetCol, Board actboard)
        {
            return false;
        }

        public override List<int[,]> PotencialMoves(Board actBoard, Color actColor)
        {
            return new List<int[,]>();
        }
    }
}
