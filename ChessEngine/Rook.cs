using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Rook : Pieces
    {
        public Rook(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved) : base(color, curRow, curCol, actpeaceHasAlreadyMoved)
        {
        }

        public override int GetPieceValue()
        {
            return 4;
        }

        public override bool IsMoveLegal(int targetRow, int targetCol, Board actboard)
        {
            //biztonsági okokból, bár nem tudjuk így meghívni a függvényt
            if (targetCol < 0 || targetRow < 0 || targetCol > 7 || targetRow > 7)
            {
                return false;
            }
            //azt a mezőt nézzük ahol áll
            if (this.GetPieceCurRow() == targetRow && this.GetPieceCurCol() == targetCol)
            {
                return false;
            }
            //megnézzük h a target mezőnk és az alap mező között van-e bábú
            //oszlop szerinti lépések
            else if (this.GetPieceCurCol()== targetCol)
            {
                //oszlop lenti része
                if(targetRow<this.GetPieceCurRow())
                {
                    //beállítunk egy tmp countert a kijelölt mezőnk felé
                    int tmpRowCount = targetRow + 1;
                    //amíg nem érem el a mezőt amin a bábunk van
                    while (tmpRowCount != this.GetPieceCurRow())
                    {
                        //ha az nem üres vagy nem ghost
                        if (actboard.GetPieceInfoFromTheBoard(tmpRowCount, targetCol).GetColor() == Color.White || actboard.GetPieceInfoFromTheBoard(tmpRowCount, targetCol).GetColor() == Color.Black)
                        {
                            return false;
                        }
                        // lépünk a kövi mezőre
                        tmpRowCount += 1;
                    }
                    //ha nem léptünk bele a belső if ágba akkor a köztes mezők szabadok kell h legyenek.
                    return true;
                }
                //oszlop fenti része
                else
                {
                    //beállítunk egy tmp countert a kijelölt mezőnk alá
                    int tmpRowCount = targetRow - 1;
                    //amíg nem érem el a mezőt amin a bábunk van
                    while (tmpRowCount != this.GetPieceCurRow())
                    {
                        //ha az nem üres vagy nem ghost
                        if (actboard.GetPieceInfoFromTheBoard(tmpRowCount, targetCol).GetColor() == Color.White || actboard.GetPieceInfoFromTheBoard(tmpRowCount, targetCol).GetColor() == Color.Black)
                        {
                            return false;
                        }
                        //lépünk a kövi mezőre
                        tmpRowCount -= 1;
                    }
                    //ha nem léptünk bele a belső if ágba akkor a köztes mezők szabadok kell h legyenek.
                    return true;
                }
            }
            //sor szerinti lépések
            else if (this.GetPieceCurRow() == targetRow)
            {
                //sor bal része
                if (targetCol < this.GetPieceCurCol())
                {
                    //beállítunk egy tmp countert jobbra a kijelölt mezőtől
                    int tmpColCount = targetCol + 1;
                    //amíg nem érem el a mezőt amin a bábunk van
                    while (tmpColCount != this.GetPieceCurCol())
                    {
                        //ha az nem üres vagy nem ghost
                        if (actboard.GetPieceInfoFromTheBoard(targetRow, tmpColCount).GetColor() == Color.White || actboard.GetPieceInfoFromTheBoard(targetRow, tmpColCount).GetColor() == Color.Black)
                        {
                            return false;
                        }
                        //lépünk a kövi mezőre
                        tmpColCount += 1;
                    }
                    //ha nem léptünk bele a belső if ágba akkor a köztes mezők szabadok kell h legyenek.
                    return true;
                }
                //sor jobb része
                else
                {
                    //beállítunk egy tmp countert balra a kijelölt mezőtől
                    int tmpColCount = targetCol - 1;
                    //amíg nem érem el a mezőt amin a bábunk van
                    while (tmpColCount != this.GetPieceCurCol())
                    {
                        //ha az nem üres vagy nem ghost
                        if (actboard.GetPieceInfoFromTheBoard(targetRow, tmpColCount).GetColor() == Color.White || actboard.GetPieceInfoFromTheBoard(targetRow, tmpColCount).GetColor() == Color.Black)
                        {
                            return false;
                        }
                        //lépünk a kövi mezőre
                        tmpColCount -= 1;
                    }
                    //ha nem léptünk bele a belső if ágba akkor a köztes mezők szabadok kell h legyenek.
                    return true;
                }
            }
            //ha nem jó a logika(nem a sorba vagy oszlopban keresünk)
            else
            {
                return false;
            }
        }

        public override List<int[,]> PotencialMoves(Board actBoard, Color actColor)
        {
            //ideiglenes tároló
            List<int[,]> possibleMovesTmp = new List<int[,]> { };
            //megnézzük az összes mezőt
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //lépésformának megfelel-e az adott mező
                    if (IsMoveLegal(i, j, actBoard))
                    {
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
