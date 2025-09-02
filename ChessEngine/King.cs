using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class King : Pieces
    {
        public King(Color color, int curRow, int curCol, bool actpeaceHasAlreadyMoved) : base(color, curRow, curCol, actpeaceHasAlreadyMoved)
        {
        }
        

        public override int GetPieceValue()
        {
            return 6;
        }

        public override bool IsMoveLegal(int targetRow, int targetCol, Board actboard)
        {
            //biztonsági okokból, bár nem tudjuk így meghívni a függvényt
            if (targetCol < 0 || targetRow < 0 || targetCol > 7 || targetRow > 7)
            {
                return false;
            }
            //jobb felülre lép
            if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() + 1 == targetCol)
            {
                return true;
            }
            //jobbra lép
            else if (this.GetPieceCurRow() == targetRow && this.GetPieceCurCol() +1 == targetCol)
            {
                return true;
            }
            //jobb alulra lép
            else if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() + 1 == targetCol)
            {
                return true;
            }
            //alulra lép
            else if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() == targetCol)
            {
                return true;
            }
            //bal alulra lép
            else if (this.GetPieceCurRow() - 1 == targetRow && this.GetPieceCurCol() - 1 == targetCol)
            {
                return true;
            }
            //balra lép
            else if (this.GetPieceCurRow() == targetRow && this.GetPieceCurCol() - 1 == targetCol)
            {
                return true;
            }
            //bal felülre lép
            else if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() - 1 == targetCol)
            {
                return true;
            }
            //felülre lép
            else if (this.GetPieceCurRow() + 1 == targetRow && this.GetPieceCurCol() == targetCol)
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
            List<int[,]> possibleMovesTmp = new List<int[,]> { };
            //megnézzük az összes mezőt
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //lépésformának megfelel-e az adott mező
                    //sima lépések
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
            //megnézzük a rövid sáncolást
            if(this.isPossibleCastleShort(actBoard, this.GetColor()))
            {
                //ha fehér a bábú
                if(this.GetColor() == Color.White)
                {
                    int[,] value = new int[,] { { this.GetPieceCurRow(), this.GetPieceCurCol(), 7, 6 } };
                    possibleMovesTmp.Add(value);
                }
                //ha fekete a bábú
                else
                {
                    int[,] value = new int[,] { { this.GetPieceCurRow(), this.GetPieceCurCol(), 0, 6 } };
                    possibleMovesTmp.Add(value);
                }
            }
            //megnézzük a hosszú sáncolást
            if(this.isPossibleCastleLong(actBoard, this.GetColor()))
            {
                //ha fehér a bábú
                if (this.GetColor() == Color.White)
                {
                    int[,] value = new int[,] { { this.GetPieceCurRow(), this.GetPieceCurCol(), 7, 2 } };
                    possibleMovesTmp.Add(value);
                }
                //ha fekete a bábú
                else
                {
                    int[,] value = new int[,] { { this.GetPieceCurRow(), this.GetPieceCurCol(), 0, 2 } };
                    possibleMovesTmp.Add(value);
                }
            }
            return possibleMovesTmp;
        }
        
        //rövid sáncolás lehetősége
        public bool isPossibleCastleShort(Board actboard, Color actcolor)
        {
            //megnézzük, hogy a király mozgott-e már
            if(this.GetPieceHasAlreadyMoved())
            {
                return false;
            }
            //megnézzük h fehér vagy fekete színnel játszunk
            //fehér
            if (actcolor == Color.White)
            {
                //king és rook között nincs semmi
                if(actboard.GetPieceInfoFromTheBoard(7,5).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(7, 6).GetPieceValue() != 0)
                {
                    return false;
                }
                else
                {
                    //köztes mezőket és a királyt nem támadják
                    if (actboard.IsSquareUnderAttack(7, 5, Color.White) || actboard.IsSquareUnderAttack(7, 6, Color.White) || actboard.IsSquareUnderAttack(7, 4, Color.White))
                    {
                        return false;
                    }
                    else
                    {
                        // van-e rook a megadott poziban és nem mozgott még a játék folyamán (színt nem kell nézni mert, akkor már mozgott)
                        if(actboard.GetPieceInfoFromTheBoard(7,7).GetPieceValue() == 4 && actboard.GetPieceInfoFromTheBoard(7, 7).GetPieceHasAlreadyMoved() == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            //fekete
            else
            {
                //king és rook között nincs semmi
                if (actboard.GetPieceInfoFromTheBoard(0, 5).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(0, 6).GetPieceValue() != 0)
                {
                    return false;
                }
                else
                {
                    //köztes mezőket és a királyt nem támadják
                    if (actboard.IsSquareUnderAttack(0, 5, Color.Black) || actboard.IsSquareUnderAttack(0, 6, Color.Black) || actboard.IsSquareUnderAttack(0, 4, Color.Black))
                    {
                        return false;
                    }
                    else
                    {
                        // van-e rook a megadott poziban és nem mozgott még a játék folyamán (színt nem kell nézni mert, akkor már mozgott)
                        if (actboard.GetPieceInfoFromTheBoard(0, 7).GetPieceValue() == 4 && actboard.GetPieceInfoFromTheBoard(0, 7).GetPieceHasAlreadyMoved() == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        //hosszú sáncolás lehetősége
        public bool isPossibleCastleLong(Board actboard, Color actcolor)
        {
            //megnézzük, hogy a király mozgott-e már
            if (this.GetPieceHasAlreadyMoved())
            {
                return false;
            }
            //megnézzük h fehér vagy fekete színnel játszunk
            //fehér
            if (actcolor == Color.White)
            {
                //king és rook között nincs semmi
                if (actboard.GetPieceInfoFromTheBoard(7, 1).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(7, 2).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(7, 3).GetPieceValue() != 0)
                {
                    return false;
                }
                else
                {
                    //köztes mezőket és a királyt nem támadják
                    if (actboard.IsSquareUnderAttack(7, 1, Color.White) || actboard.IsSquareUnderAttack(7, 2, Color.White) || actboard.IsSquareUnderAttack(7, 3, Color.White) || actboard.IsSquareUnderAttack(7, 4, Color.White))
                    {
                        return false;
                    }
                    else
                    {
                        // van-e rook a megadott poziban és nem mozgott még a játék folyamán (színt nem kell nézni mert, akkor már mozgott)
                        if (actboard.GetPieceInfoFromTheBoard(7, 0).GetPieceValue() == 4 && actboard.GetPieceInfoFromTheBoard(7, 0).GetPieceHasAlreadyMoved() == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            //fekete
            else
            {
                //king és rook között nincs semmi
                if (actboard.GetPieceInfoFromTheBoard(0, 1).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(0, 2).GetPieceValue() != 0 || actboard.GetPieceInfoFromTheBoard(0, 3).GetPieceValue() != 0)
                {
                    return false;
                }
                else
                {
                    //köztes mezőket és a királyt nem támadják
                    if (actboard.IsSquareUnderAttack(0, 1, Color.Black) || actboard.IsSquareUnderAttack(0, 2, Color.Black) || actboard.IsSquareUnderAttack(0, 3, Color.Black) || actboard.IsSquareUnderAttack(0, 4, Color.Black))
                    {
                        return false;
                    }
                    else
                    {
                        // van-e rook a megadott poziban és nem mozgott még a játék folyamán (színt nem kell nézni mert, akkor már mozgott)
                        if (actboard.GetPieceInfoFromTheBoard(0, 0).GetPieceValue() == 4 && actboard.GetPieceInfoFromTheBoard(0, 0).GetPieceHasAlreadyMoved() == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
