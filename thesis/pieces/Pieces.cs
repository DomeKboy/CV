using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public abstract class Pieces
    {
        private Color pieceColor;
        private int pieceCurRow;
        private int pieceCurCol;
        private List<int[,]> possibleMoves;
        private bool peaceHasAlreadyMoved;

        protected Pieces(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved)
        {
            this.pieceColor = color;
            this.pieceCurRow = curRow;
            this.pieceCurCol = curCol;
            this.possibleMoves = new List<int[,]> { };
            this.peaceHasAlreadyMoved = actpeaceHasAlreadyMoved;
        }

        public Color GetColor()
        {
            return this.pieceColor;
        }
        public int GetPieceCurRow()
        {
            return this.pieceCurRow;
        }
        public int GetPieceCurCol()
        {
            return this.pieceCurCol;
        }
        public bool GetPieceHasAlreadyMoved()
        {
            return this.peaceHasAlreadyMoved;
        }
        private void SetColor(Color newcolor)
        {
            this.pieceColor = newcolor;
        }
        private void SetPieceCurRow(int newrow)
        {
            this.pieceCurRow= newrow;
        }
        private void SetPieceCurCol(int newcol)
        {
            this.pieceCurCol = newcol;
        }
        private void SetPieceHasAlreadyMoved(bool actpeaceHasAlreadyMoved)
        {
            this.peaceHasAlreadyMoved = actpeaceHasAlreadyMoved;
        }
        public abstract List<int[,]> PotencialMoves(Board actBoard, Color actColor);

        public abstract Boolean IsMoveLegal(int targetRow, int targetCol, Board actboard);

        public abstract int GetPieceValue();
       
    }
}
