using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Knight : Pieces
    {
        public Knight(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved) : base(color, curRow, curCol, actpeaceHasAlreadyMoved)
        {
        }

        public override int GetPieceValue()
        {
            return 2;
        }

        public override bool IsMoveLegal(int targetRow, int targetCol, Board actboard)
        {
            //biztonsági okokból, bár nem tudjuk így meghívni a függvényt
            if (targetCol < 0 || targetRow < 0 || targetCol > 7 || targetRow > 7 )
            {
                return false;
            }
            //fel 2  és jobbra vagy balra 1 ugrás
            if(this.GetPieceCurRow()+2 == targetRow && (this.GetPieceCurCol()+1 == targetCol || this.GetPieceCurCol() - 1 == targetCol))
            {
                return true;
            }
            //le 2 és jobbra vagy balra 1 ugrás
            else if (this.GetPieceCurRow() - 2 == targetRow && (this.GetPieceCurCol() + 1 == targetCol || this.GetPieceCurCol() - 1 == targetCol))
            {
                return true;
            }
            //jobbra 2 és fel vagy le 1 ugrás
            else if (this.GetPieceCurCol() + 2 == targetCol &&(this.GetPieceCurRow() + 1 == targetRow || this.GetPieceCurRow() - 1 == targetRow))
            {
                return true;
            }
            //ballra 2 és fel vagy le 1 ugrás
            else if (this.GetPieceCurCol() - 2 == targetCol && (this.GetPieceCurRow() + 1 == targetRow || this.GetPieceCurRow() - 1 == targetRow))
            {
                return true;
            }
            //ha nem jó a logika
            else
            {
                return false;
            }
        }

        public override List<int[,]> PotencialMoves(Board actBoard, Color actColor)
        {
            //ideiglenes tároló amit el tudunk küldeni
            List<int[,]> possibleMovesTmp = new List<int[,]> { };
            //véig megyünk a mezőkön
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    //ha lehetséges a lépés
                    if(IsMoveLegal(i,j, actBoard))
                    {
                        //megnézzük h nem-e saját bábunk áll ott
                        if(actBoard.GetPieceInfoFromTheBoard(i,j).GetColor() != actColor)
                        {
                            //készítünk egy tmp boardot
                            Board tmpBoard = new Board(new Pieces[8, 8]);
                            for (int k = 0; k < 8; k++)
                            {
                                for (int l = 0; l < 8; l++)
                                {
                                    tmpBoard.PutPieceOnTheBoard(k, l, actBoard.GetPieceInfoFromTheBoard(k, l));
                                    //hol van a király
                                    if (tmpBoard.GetPieceInfoFromTheBoard(k, l).GetPieceValue() == 6)
                                    {
                                        if (actBoard.GetPieceInfoFromTheBoard(k, l).GetColor() == Color.White)
                                        {
                                            tmpBoard.SetWhiteKingRow(k);
                                            tmpBoard.SetWhiteKingCol(l);
                                        }
                                        else
                                        {
                                            tmpBoard.SetBlackKingRow(k);
                                            tmpBoard.SetBlackKingCol(l);
                                        }
                                    }
                                }
                            }
                            //úgy teszünk rajta mintha már elvégeztük volna a lépést
                            tmpBoard.MakeAMoveWithAPiece(i, j, this);
                            //megnézzük, hogy a lépésünk után nem leszünk-e sakkba
                            if (tmpBoard.IsOurKingUnderAttack(actColor) == false)
                            {
                                //felvesszük az ideiglenes tárolóba
                                int[,] value = new int[,] { { this.GetPieceCurRow(), this.GetPieceCurCol(), i, j } };
                                possibleMovesTmp.Add(value);
                            }
                        }
                    }
                }
            }
            //visszaadjuk a lehetséges lépéseket a báburól
            return possibleMovesTmp;
        }
    }
}
