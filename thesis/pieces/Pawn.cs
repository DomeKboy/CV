using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Pawn : Pieces
    {
        public Pawn(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved) : base(color, curRow, curCol, actpeaceHasAlreadyMoved)
        {
        }

        public override int GetPieceValue()
        {
            return 1;
        }

        public override bool IsMoveLegal(int targetRow, int targetCol, Board actboard)
        {
            //biztonsági okokból, bár nem tudjuk így meghívni a függvényt
            if (targetCol < 0 || targetRow < 0 || targetCol > 7 || targetRow > 7)
            {
                return false;
            }
            //fehér gyalog
            if (this.GetColor() == Color.White)
            {

                //előrelépés 1 mezőt
                if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() == targetCol)
                {
                    if(actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Empty)
                    {
                        return true;
                    }
                    return false;
                }
                //előrelépés 2 mezőt
                else if (this.GetPieceCurRow() - 2 == targetRow && this.GetPieceCurCol() == targetCol && this.GetPieceCurRow() == 6)
                {
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Empty && actboard.GetPieceInfoFromTheBoard(targetRow + 1 , targetCol).GetColor() == Color.Empty)
                    {
                        return true;
                    }
                    return false;
                }
                //leütés jobbra
                else if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() + 1 == targetCol)
                {
                    // van-e elenséges bábú azon a mezőn
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Black)
                    {
                        return true;
                    }
                    //en passant jobbra
                    else if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Ghost)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //leütés balra
                else if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() - 1 == targetCol)
                {
                    // van-e elenséges bábú azon a mezőn
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Black)
                    {
                        return true;
                    }
                    else if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Ghost)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //ha nem jó a logika
                else
                {
                    return false;
                }
            }
            //fekete gyalog
            else
            {
                //előrelépés 1 mezőt
                if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() == targetCol)
                {
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Empty)
                    {
                        return true;
                    }
                    return false;
                }
                //előrelépés 2 mezőt
                else if (this.GetPieceCurRow() + 2 == targetRow && this.GetPieceCurCol() == targetCol && this.GetPieceCurRow() == 1)
                {
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Empty && actboard.GetPieceInfoFromTheBoard(targetRow - 1, targetCol).GetColor() == Color.Empty)
                    {
                        return true;
                    }
                    return false;
                }
                //leütés jobbra
                else if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() + 1 == targetCol)
                {
                    // van-e elenséges bábú azon a mezőn
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.White)
                    {
                        return true;
                    }
                    //en passant jobbra
                    else if(actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Ghost)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //leütés ballra
                else if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() - 1 == targetCol)
                {
                    // van-e elenséges bábú azon a mezőn
                    if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.White)
                    {
                        return true;
                    }
                    // en passant balra
                    else if (actboard.GetPieceInfoFromTheBoard(targetRow, targetCol).GetColor() == Color.Ghost)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //ha nem jó a logika
                else
                {
                    return false;
                }
            }
        }

        public override List<int[,]> PotencialMoves(Board actBoard, Color actColor)
        {
            List<int[,]> possibleMovesTmp = new List<int[,]> {};
            //megnézzük az összes mezőt
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    //ha lehetséges a lépés
                    if (IsMoveLegal(i, j, actBoard))
                    {
                        //megnézzük h nem-e saját bábunk áll ott
                        if (actBoard.GetPieceInfoFromTheBoard(i, j).GetColor() != actColor)
                        {
                            //készítünk egy tmp boardot
                            Board tmpBoard = new Board(new Pieces[8, 8]);
                            for (int k = 0; k < 8; k++)
                            {
                                for (int l = 0; l < 8; l++)
                                {
                                    tmpBoard.PutPieceOnTheBoard(k, l, actBoard.GetPieceInfoFromTheBoard(k, l));
                                    //hol van a király
                                    if(tmpBoard.GetPieceInfoFromTheBoard(k,l).GetPieceValue() == 6)
                                    {
                                        if(actBoard.GetPieceInfoFromTheBoard(k, l).GetColor() == Color.White)
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
            return possibleMovesTmp;
        }
    }
}
