using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Board
    {

        //tábla

        private Pieces[,] actBoard = new Pieces[8, 8];
        private bool lateGameStatus = false;
        private int whiteKingRow = 7;
        private int whiteKingCol = 4;
        private int blackKingRow = 0;
        private int blackKingCol = 4;

        //evalution-höz fontos értékek.
        public readonly int pawnValue = 100;
        public readonly int knightValue = 320;
        public readonly int bishopValue = 330;
        public readonly int rookValue = 500;
        public readonly int queenValue = 900;
        public readonly int kingValue = 20000;

        public readonly int[] pawnSquareBonus = {
             0,   0,   0,   0,   0,   0,   0,   0,
            78,  83,  86,  73, 102,  82,  85,  90,
             7,  29,  21,  44,  40,  31,  44,   7,
           -17,  16,  -2,  15,  14,   0,  15, -13,
           -26,   3,  10,   9,   6,   1,   0, -23,
           -22,   9,   5, -11, -10,  -2,   3, -19,
           -31,   8,  -7, -37, -36, -14,   3, -31,
             0,   0,   0,   0,   0,   0,   0,   0
        };

        public readonly int[] knightSquareBonus = {
           -66, -53, -75, -75, -10, -55, -58, -70,
            -3,  -6, 100, -36,   4,  62,  -4, -14,
            10,  67,   1,  74,  73,  27,  62,  -2,
            24,  24,  45,  37,  33,  41,  25,  17,
            -1,   5,  31,  21,  22,  35,   2,   0,
           -18,  10,  13,  22,  18,  15,  11, -14,
           -23, -15,   2,   0,   2,   0, -23, -20,
           -74, -23, -26, -24, -19, -35, -22, -69
        };

        public readonly int[] bishopSquareBonus = {
           -66, -53, -75, -75, -10, -55, -58, -70,
            -3,  -6, 100, -36,   4,  62,  -4, -14,
            10,  67,   1,  74,  73,  27,  62,  -2,
            24,  24,  45,  37,  33,  41,  25,  17,
            -1,   5,  31,  21,  22,  35,   2,   0,
           -18,  10,  13,  22,  18,  15,  11, -14,
           -23, -15,   2,   0,   2,   0, -23, -20,
           -74, -23, -26, -24, -19, -35, -22, -69
        };

        public readonly int[] rookSquareBonus = {
            35,  29,  33,   4,  37,  33,  56,  50,
            55,  29,  56,  67,  55,  62,  34,  60,
            19,  35,  28,  33,  45,  27,  25,  15,
             0,   5,  16,  13,  18,  -4,  -9,  -6,
           -28, -35, -16, -21, -13, -29, -46, -30,
           -42, -28, -42, -25, -25, -35, -26, -46,
           -53, -38, -31, -26, -29, -43, -44, -53,
           -30, -24, -18,   5,  -2, -18, -31, -32
        };

        public readonly int[] queenSquareBonus = {
            6,   1,  -8,-104,  69,  24,  88,  26,
            14,  32,  60, -10,  20,  76,  57,  24,
            -2,  43,  32,  60,  72,  63,  43,   2,
             1, -16,  22,  17,  25,  20, -13,  -6,
           -14, -15,  -2,  -5,  -1, -10, -20, -22,
           -30,  -6, -13, -11, -16, -11, -16, -27,
           -36, -18,   0, -19, -15, -15, -21, -38,
           -39, -30, -31, -13, -31, -36, -34, -42
        };

        public readonly int[] earlyKingSquareBonus = {
            4,  54,  47, -99, -99,  60,  83, -62,
           -32,  10,  55,  56,  56,  55,  10,   3,
           -62,  12, -57,  44, -67,  28,  37, -31,
           -55,  50,  11,  -4, -19,  13,   0, -49,
           -55, -43, -52, -28, -51, -47,  -8, -50,
           -47, -42, -43, -79, -64, -32, -29, -32,
            -4,   3, -14, -50, -57, -18,  13,   4,
            17,  30,  -3, -14,   6,  -1,  40,  18
        };
        public readonly int[] lateKingSquareBonus = {
            -50,-40,-30,-20,-20,-30,-40,-50,
            -30,-20,-10,  0,  0,-10,-20,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-30,  0,  0,  0,  0,-30,-30,
            -50,-30,-30,-30,-30,-30,-30,-50
        };
        //bónusz pontok
        public readonly int pawnIsolated = -10;
        public readonly int[] passedPawn = { 0, 5, 10, 15, 30, 60, 100, 100 };
        public readonly int doubledPawn = -5;
        public readonly int rookOnOpenFile = 10;
        
        public Board(Pieces[,] actboard)
        {
            this.actBoard = actboard;
        }
        private void SetActBoard(Pieces[,] newboard)
        {
            actBoard = newboard;
        }
        public Pieces[,] GetActBoard()
        { 
            return actBoard; 
        }
        public void PutPieceOnTheBoard(int actrow, int actcol, Pieces newPiece)
        {
            actBoard [actrow, actcol] = newPiece;
        }
        public Pieces GetPieceInfoFromTheBoard(int actrow, int actcol)
        {
            return actBoard[actrow, actcol];
        }
        public int GetWhiteKingRow()
        {
            return whiteKingRow;
        }
        public int GetBlackKingRow()
        {
            return blackKingRow;
        }
        public int GetWhiteKingCol()
        {
            return whiteKingCol;
        }
        public int GetBlackKingCol()
        {
            return blackKingCol;
        }

        public void SetWhiteKingRow(int actWhiteKingRow)
        {
            whiteKingRow = actWhiteKingRow;
        }
        public void SetBlackKingRow(int actBlackKingRow)
        {
            blackKingRow = actBlackKingRow;
        }
        public void SetWhiteKingCol(int actWhiteKingCol)
        {
            whiteKingCol = actWhiteKingCol;
        }
        public void SetBlackKingCol(int actBlackKingCol)
        {
            blackKingCol = actBlackKingCol;
        }
        public bool GetGameStatus()
        {
            return lateGameStatus;
        }

        public void MakeAMoveWithAPiece(int endRow, int endCol, Pieces actPiece, int newPieceValue)
        {
            //töröljük a lehetséges ghost elemet
            this.DeleteAllGhostSquare();
            //ha egy fehér gyalog ér be
            if (actPiece.GetPieceCurRow() == 1 && actPiece.GetPieceValue() == 1 && actPiece.GetColor() == Color.White)
            {
                //mire szeretnénk promotálni a bábunkat
                switch (newPieceValue)
                {
                    case 2:
                        this.PutPieceOnTheBoard(endRow, endCol, new Knight(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 3:
                        this.PutPieceOnTheBoard(endRow, endCol, new Bishop(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 4:
                        this.PutPieceOnTheBoard(endRow, endCol, new Rook(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 5:
                        this.PutPieceOnTheBoard(endRow, endCol, new Queen(actPiece.GetColor(), endRow, endCol, true));
                        break;
                }
                this.PutPieceOnTheBoard(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), new Empty(Color.Empty, actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), false));
            }
            //ha fekete gyaloggal promotálnánk
            else if(actPiece.GetPieceCurRow() == 6 && actPiece.GetPieceValue() == 1 && actPiece.GetColor() == Color.Black)
            {
                //mire szeretnénk promotálni a bábunkat
                switch (newPieceValue)
                {
                    case 2:
                        this.PutPieceOnTheBoard(endRow, endCol, new Knight(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 3:
                        this.PutPieceOnTheBoard(endRow, endCol, new Bishop(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 4:
                        this.PutPieceOnTheBoard(endRow, endCol, new Rook(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    case 5:
                        this.PutPieceOnTheBoard(endRow, endCol, new Queen(actPiece.GetColor(), endRow, endCol, true));
                        break;
                }
                this.PutPieceOnTheBoard(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), new Empty(Color.Empty, actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), false));
            }
            
        }
        public void MakeAMoveWithAPiece(int endRow, int endCol, Pieces actPiece)
        {
            //töröljük a lehetséges ghost elemet
            this.DeleteAllGhostSquare();
            //ha a királlyal mozognék és sáncolunk röviden
            if (actPiece.GetPieceValue() == 6 && actPiece.GetPieceHasAlreadyMoved() == false && endCol == 6)
            {
                // lehelyezzük a királyt
                this.PutPieceOnTheBoard(endRow, endCol, new King(actPiece.GetColor(), endRow, endCol, true));
                if (actPiece.GetColor() == Color.White)
                {
                    this.SetWhiteKingRow(endRow);
                    this.SetWhiteKingCol(endCol);
                }
                else
                {
                    this.SetBlackKingRow(endRow);
                    this.SetBlackKingCol(endCol);
                }
                //bástyát elhelyezzük tőle balra
                this.PutPieceOnTheBoard(endRow, endCol - 1, new Rook(actPiece.GetColor(), endRow, endCol - 1, true));
                //bástya helyét felszabadítjuk
                this.PutPieceOnTheBoard(endRow, endCol + 1, new Empty(Color.Empty, endRow, endCol + 1, false));
                //a király helyét is felszabadítjuk
                this.PutPieceOnTheBoard(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), new Empty(Color.Empty, actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), false));
            }
            //ha a királlyal mozognék és sáncolunk hosszan
            else if (actPiece.GetPieceValue() == 6 && actPiece.GetPieceHasAlreadyMoved() == false && endCol == 2)
            {
                // lehelyezzük a királyt
                this.PutPieceOnTheBoard(endRow, endCol, new King(actPiece.GetColor(), endRow, endCol, true));
                if (actPiece.GetColor() == Color.White)
                {
                    this.SetWhiteKingRow(endRow);
                    this.SetWhiteKingCol(endCol);
                }
                else
                {
                    this.SetBlackKingRow(endRow);
                    this.SetBlackKingCol(endCol);
                }
                //bástyát elhelyezzük tőle balra
                this.PutPieceOnTheBoard(endRow, endCol + 1, new Rook(actPiece.GetColor(), endRow, endCol + 1, true));
                //bástya helyét felszabadítjuk
                this.PutPieceOnTheBoard(endRow, endCol - 2, new Empty(Color.Empty, endRow, endCol - 2, false));
                //a király helyét is felszabadítjuk
                this.PutPieceOnTheBoard(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), new Empty(Color.Empty, actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), false));
            }
            else
            {
                switch (actPiece.GetPieceValue())
                {
                    //gyalog sima lépés
                    case 1:
                        //ha dupla mezős lépés történik fehérrel
                        if (actPiece.GetPieceCurRow() == 6 && endRow == 4)
                        {
                            this.PutPieceOnTheBoard(endRow, endCol, new Pawn(actPiece.GetColor(), endRow, endCol, true));
                            this.PutPieceOnTheBoard(endRow + 1, endCol, new Ghost(Color.Ghost, endRow + 1, endCol, false));
                        }
                        //ha dupla mezős lépés történik feketével
                        else if (actPiece.GetPieceCurRow() == 1 && endRow == 3)
                        {
                            this.PutPieceOnTheBoard(endRow, endCol, new Pawn(actPiece.GetColor(), endRow, endCol, true));
                            this.PutPieceOnTheBoard(endRow - 1, endCol, new Ghost(Color.Ghost, endRow - 1, endCol, false));
                        }
                        //ha en passant történik
                        else if(this.GetPieceInfoFromTheBoard(endRow, endCol).GetColor() == Color.Empty && endCol != actPiece.GetPieceCurCol())
                        {
                            //fehér gyalog
                            if(actPiece.GetColor() == Color.White)
                            {
                                this.PutPieceOnTheBoard(endRow, endCol, new Pawn(actPiece.GetColor(), endRow, endCol, true));
                                this.PutPieceOnTheBoard(endRow + 1, endCol, new Empty(Color.Empty, endRow + 1, endCol, false));
                            }
                            //fekete gyalog
                            else
                            {
                                this.PutPieceOnTheBoard(endRow, endCol, new Pawn(actPiece.GetColor(), endRow, endCol, true));
                                this.PutPieceOnTheBoard(endRow - 1, endCol, new Empty(Color.Empty, endRow - 1, endCol, false));
                            }
                        }
                        //minden más esetben
                        else
                        {
                            this.PutPieceOnTheBoard(endRow, endCol, new Pawn(actPiece.GetColor(), endRow, endCol, true));
                        }
                        break;
                    //knight lépés
                    case 2:
                        this.PutPieceOnTheBoard(endRow, endCol, new Knight(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    //bishop lépés
                    case 3:
                        this.PutPieceOnTheBoard(endRow, endCol, new Bishop(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    //rook lépés
                    case 4:
                        this.PutPieceOnTheBoard(endRow, endCol, new Rook(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    //queen lépés
                    case 5:
                        this.PutPieceOnTheBoard(endRow, endCol, new Queen(actPiece.GetColor(), endRow, endCol, true));
                        break;
                    //király alap lépés
                    case 6:
                        this.PutPieceOnTheBoard(endRow, endCol, new King(actPiece.GetColor(), endRow, endCol, true));
                        if(actPiece.GetColor() == Color.White)
                        {
                            this.SetWhiteKingRow(endRow);
                            this.SetWhiteKingCol(endCol);
                        }
                        else
                        {
                            this.SetBlackKingRow(endRow);
                            this.SetBlackKingCol(endCol);
                        }
                        break;
                }   
                this.PutPieceOnTheBoard(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), new Empty(Color.Empty, actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), false));
            }
            //megnézzük h áll a board late game van-e
            this.IsGameInLateGame();
        }

        public void DeleteAllGhostSquare()
        {
            foreach (var item in this.GetActBoard())
            {
                if(item.GetColor() == Color.Ghost)
                {
                    this.PutPieceOnTheBoard(item.GetPieceCurRow(), item.GetPieceCurCol(), new Empty(Color.Empty, item.GetPieceCurRow(), item.GetPieceCurCol(), false));
                }
            }
        }

        //egy kiválasztott mezőt az ellenfél bábúja lát-e
        public bool IsSquareUnderAttack(int actRow, int actCol, Color defendColor)
        {
            foreach(Pieces item in this.GetActBoard())
            {
                //ellenséges nem szabad mezőket nézem
                if(item.GetPieceValue() != 0 && item.GetColor()!= defendColor)
                {
                    //létezik-e legális lépés ahol áll
                    if(item.IsMoveLegal(actRow,actCol, this))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //amelyik színt odaadjuk a függvénynek, az a színű király sakkban-van-e
        public bool IsOurKingUnderAttack(Color defendColor)
        {
            if(defendColor == Color.White)
            {
                return IsSquareUnderAttack(whiteKingRow, whiteKingCol, defendColor);
            }
            else
            {
                return IsSquareUnderAttack(blackKingRow, blackKingCol, defendColor);
            }
              
        }
        
        //megnézzük fentvan-e méndkét királynő ha az egyik nincs akkor már azt lategamenek nevezzük
        public void IsGameInLateGame()
        {
            //helyi változók
            bool whiteQueenOn = false;
            bool blackQuuenOn = false;
            //megnézzük az összes mezőt
            foreach (Pieces item in this.GetActBoard())
            {
                //ha királynő van rajta
                if (item.GetPieceValue() == 5)
                {
                    if (item.GetColor() == Color.White)
                    {
                        whiteQueenOn = true;
                    }
                    else
                    {
                        blackQuuenOn = true;
                    }
                }
            }
            //vagy az egyik vagy a másik nincs rajta
            if (!whiteQueenOn || !blackQuuenOn)
            {
                lateGameStatus = true;
            }
        }
        
        // egy numerikus értéket adunk a táblának. (pozitív -> fehér vezet, negatív -> fekete vezet)
        public int EvaluateBoard()
        {
            //felveszünk egy kezdőértéket
            int evaluationScore = 0;
            //ez végjátszma szakasza
            //végimegyünk az összes elemen és hozzáadjuk a súlyozott értékeket
            foreach (Pieces item in this.GetActBoard())
            {
                //ALAP ÉRTÉKEK A MEGLÉVŐ BÁBÚKÉRT
                switch (item.GetPieceValue())
                {
                    //üres
                    case 0:
                        break;
                    //paraszt
                    case 1:
                        if (item.GetColor() == Color.White)
                        {
                            evaluationScore += pawnSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                            evaluationScore += pawnValue;
                        }
                        else
                        {
                            evaluationScore -= pawnSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                            evaluationScore -= pawnValue;
                        }
                        break;
                    //paci
                    case 2:
                        if (item.GetColor() == Color.White)
                        {
                            evaluationScore += knightSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                            evaluationScore += knightValue;
                        }
                        else
                        {
                            evaluationScore -= knightSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                            evaluationScore -= knightValue;
                        }
                        break;
                    //futó
                    case 3:
                        if (item.GetColor() == Color.White)
                        {
                            evaluationScore += bishopSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                            evaluationScore += bishopValue;
                        }
                        else
                        {
                            evaluationScore -= bishopSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                            evaluationScore -= bishopValue;
                        }
                        break;
                    //bástya
                    case 4:
                        if (item.GetColor() == Color.White)
                        {
                            evaluationScore += rookSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                            evaluationScore += rookValue;
                        }
                        else
                        {
                            evaluationScore -= rookSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                            evaluationScore -= rookValue;
                        }
                        break;
                    //királynő
                    case 5:
                        if (item.GetColor() == Color.White)
                        {
                            evaluationScore += queenSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                            evaluationScore += queenValue;
                        }
                        else
                        {
                            evaluationScore -= queenSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                            evaluationScore -= queenValue;
                        }
                        break;
                    //király
                    case 6:
                        if (lateGameStatus)
                        {
                            if (item.GetColor() == Color.White)
                            {
                                evaluationScore += lateKingSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                                evaluationScore += kingValue;
                            }
                            else
                            {
                                evaluationScore -= lateKingSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                                evaluationScore -= kingValue;
                            }
                            break;
                        }
                        else
                        {
                            if (item.GetColor() == Color.White)
                            {
                                evaluationScore += earlyKingSquareBonus[item.GetPieceCurRow() * 8 + item.GetPieceCurCol()];
                                evaluationScore += kingValue;
                            }
                            else
                            {
                                evaluationScore -= earlyKingSquareBonus[(7 - item.GetPieceCurRow()) * 8 + (7 - item.GetPieceCurCol())];
                                evaluationScore -= kingValue;
                            }
                            break;
                        }

                }
                //bónusz pontok
                //gyalog
                if(item.GetPieceValue() == 1)
                {
                    //fehér gyalog
                    if(item.GetColor() == Color.White)
                    {
                        //megnézzük h egyedül van-e
                        if(IsPawnIsolated(item))
                        {
                            evaluationScore += pawnIsolated;
                        }
                        //megnézzük h "passed" gyalog-e
                        if(IsPawnAPasser(item))
                        {
                            evaluationScore += passedPawn[item.GetPieceCurRow()];
                        }
                        //megnézzük h double pawn van-e
                        if(IsDoubledPawn(item))
                        {
                            evaluationScore += doubledPawn;
                        }
                    }
                    //fekete gyalog
                    else
                    {
                        //megnézzük h egyedül van-e
                        if (IsPawnIsolated(item))
                        {
                            evaluationScore -= pawnIsolated;
                        }
                        //megnézzük h "passed" gyalog-e
                        if (IsPawnAPasser(item))
                        {
                            evaluationScore -= passedPawn[7-item.GetPieceCurRow()];
                        }
                        //megnézzük h double pawn van-e
                        if (IsDoubledPawn(item))
                        {
                            evaluationScore -= doubledPawn;
                        }
                    }
                }
                //knight
                if (item.GetPieceValue() == 2)
                {
                    //fehér knight
                    if (item.GetColor() == Color.White)
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore -= knightValue * 2;
                        }
                    }
                    //fekete kngiht
                    else
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore += knightValue * 2;
                        }
                    }
                }
                //bishop
                if (item.GetPieceValue() == 3)
                {
                    //fehér bishop
                    if (item.GetColor() == Color.White)
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore -= bishopValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore -= bishopValue / knightValue * 200;
                        }
                    }
                    //fekete bishop
                    else
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore += bishopValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore -= bishopValue / knightValue * 200;
                        }
                    }
                }
                //rook
                if (item.GetPieceValue() == 4)
                {
                    //fehér rook
                    if (item.GetColor() == Color.White)
                    {
                        if(IsRookOnOpenFile(item))
                        {
                            evaluationScore += rookOnOpenFile;
                        }
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore -= rookValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore -= rookValue / knightValue * 200;
                        }
                        if (CanBeTakenWithBishop(item))
                        {
                            evaluationScore -= rookValue / bishopValue * 200;
                        }
                    }
                    //fekete rook
                    else
                    {
                        if(IsRookOnOpenFile(item))
                        {
                            evaluationScore -= rookOnOpenFile;
                        }
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore += rookValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore += rookValue / knightValue * 200;
                        }
                        if (CanBeTakenWithBishop(item))
                        {
                            evaluationScore += rookValue / bishopValue * 200;
                        }
                    }
                }
                //queen
                if(item.GetPieceValue() == 5)
                {
                    //fehér queen
                    if (item.GetColor() == Color.White)
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore -= queenValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore -= queenValue / knightValue * 400;
                        }
                        if (CanBeTakenWithBishop(item))
                        {
                            evaluationScore -= queenValue / bishopValue * 400;
                        }
                        if (CanBeTakenWithRook(item))
                        {
                            evaluationScore -= queenValue / rookValue * 300;
                        }
                    }
                    //fekete queen
                    else
                    {
                        if (CanBeTakenWithPawn(item))
                        {
                            evaluationScore += queenValue * 2;
                        }
                        if (CanBeTakenWithKnight(item))
                        {
                            evaluationScore += queenValue / knightValue * 200;
                        }
                        if (CanBeTakenWithBishop(item))
                        {
                            evaluationScore += queenValue / bishopValue * 200;
                        }
                        if (CanBeTakenWithRook(item))
                        {
                            evaluationScore += queenValue / rookValue * 200;
                        }
                    }
                }
                //bármelyik bábú
                //fehér
                if(item.GetColor() == Color.White)
                {
                    evaluationScore += IsPieceHanging(item) * (-100) * item.GetPieceValue();
                    evaluationScore += IsPieceInTheSmallMiddle(item) * 50;
                    evaluationScore += IsPieceInTheBiggerMiddle(item) * 10;
                }
                //fekete
                else
                {
                    evaluationScore -= IsPieceHanging(item) * (-100) * item.GetPieceValue();
                    evaluationScore -= IsPieceInTheSmallMiddle(item) * 50;
                    evaluationScore -= IsPieceInTheBiggerMiddle(item) * 10;
                }
            }
            if(this.IsOurKingUnderAttack(Color.White))
            {
                evaluationScore -= 100;
            }
            if(this.IsOurKingUnderAttack(Color.Black))
            {
                evaluationScore += 100;
            }

            return evaluationScore;
        }

        public bool IsPawnIsolated(Pieces actPiece)
        {
            //ha bal szélen van
            if(actPiece.GetPieceCurCol()== 0)
            {
                for(int i = 0; i < 8; i++)
                {
                    if(this.GetPieceInfoFromTheBoard(i, 1).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i,1).GetColor() == actPiece.GetColor())
                    {
                        return false;
                    }
                }
                return true;
            }
            //ha jobb szélen van
            else if (actPiece.GetPieceCurCol() == 7)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this.GetPieceInfoFromTheBoard(i, 6).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, 6).GetColor() == actPiece.GetColor())
                    {
                        return false;
                    }
                }
                return true;
            }
            //ha középen van
            else
            {
                int actPieceCol = actPiece.GetPieceCurCol();
                for (int i = 0; i < 8; i++)
                {
                    //tőle jobbra
                    if (this.GetPieceInfoFromTheBoard(i, actPieceCol + 1).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPieceCol+1).GetColor() == actPiece.GetColor())
                    {
                        return false;
                    }
                    //tőle balra
                    if (this.GetPieceInfoFromTheBoard(i, actPieceCol - 1).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPieceCol - 1).GetColor() == actPiece.GetColor())
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool IsPawnAPasser(Pieces actPiece)
        {
            //fehér gyalog
            if(actPiece.GetColor() == Color.White)
            {
                int actPieceRow = actPiece.GetPieceCurRow();
                int actPieceCol = actPiece.GetPieceCurCol();

                for (int i = actPieceRow - 1; i > 0; i--)   
                {
                    if (this.GetPieceInfoFromTheBoard(i, actPieceCol).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPieceCol).GetColor() == Color.Black)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                int actPieceRow = actPiece.GetPieceCurRow();
                int actPieceCol = actPiece.GetPieceCurCol();

                for (int i = actPieceRow + 1; i < 7; i++)
                {
                    if (this.GetPieceInfoFromTheBoard(i, actPieceCol).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPieceCol).GetColor() == Color.White)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool IsDoubledPawn(Pieces actPiece)
        {
            for(int i = 1; i < 7; i++)
            {
                if(i != actPiece.GetPieceCurRow())
                {
                    if(this.GetPieceInfoFromTheBoard(i, actPiece.GetPieceCurCol()).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPiece.GetPieceCurCol()).GetColor() == actPiece.GetColor())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsRookOnOpenFile(Pieces actPiece)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i != actPiece.GetPieceCurRow())
                {
                    if (this.GetPieceInfoFromTheBoard(i, actPiece.GetPieceCurCol()).GetPieceValue() == 1 && this.GetPieceInfoFromTheBoard(i, actPiece.GetPieceCurCol()).GetColor() == actPiece.GetColor())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CanBeTakenWithPawn(Pieces actPiece)
        {
            if(actPiece.GetColor() == Color.White)
            {
                foreach(Pieces piece in this.GetActBoard())
                {
                    if(piece.GetColor() == Color.Black && piece.GetPieceValue() == 1)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    } 
                }
                return false;
            }
            else
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.White && piece.GetPieceValue() == 1)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool CanBeTakenWithKnight(Pieces actPiece)
        {
            if (actPiece.GetColor() == Color.White)
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.Black && piece.GetPieceValue() == 2)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.White && piece.GetPieceValue() == 2)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool CanBeTakenWithBishop(Pieces actPiece)
        {
            if (actPiece.GetColor() == Color.White)
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.Black && piece.GetPieceValue() == 3)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.White && piece.GetPieceValue() == 3)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public bool CanBeTakenWithRook(Pieces actPiece)
        {
            if (actPiece.GetColor() == Color.White)
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.Black && piece.GetPieceValue() == 4)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                foreach (Pieces piece in this.GetActBoard())
                {
                    if (piece.GetColor() == Color.White && piece.GetPieceValue() == 4)
                    {
                        if (piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public int IsPieceHanging(Pieces actPiece)
        {
            bool canWeProtectOurPiece = false;
            bool canEnemyAttackOurPiece = false;
            //ha fehérrel vagyunk és fehér parasztot keresünk ami védi
            if (!canWeProtectOurPiece && actPiece.GetColor() == Color.White)
            {
                //bal oldali paraszt keresés
                //ne legyen az utolsó sorban és vagy a bal szélen
                if (actPiece.GetPieceCurRow() !=7 && actPiece.GetPieceCurCol() != 0)
                {
                    //megnézzük h fehér bábú van-e azon a helyen
                    if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() + 1, actPiece.GetPieceCurCol() - 1).GetColor() == Color.White)
                    {
                        //megnézzük, hogy gyalog van-e a helyen
                        if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() + 1, actPiece.GetPieceCurCol() - 1).GetPieceValue() == 1)
                        {
                            canWeProtectOurPiece = true;
                        }
                    }
                }
                //jobb oldali paraszt keresés
                //ne legyen az utolsó sorban és vagy a jobb szélen
                if (actPiece.GetPieceCurRow() != 7 && actPiece.GetPieceCurCol() != 7)
                {
                    //megnézzük h fehér bábú van-e azon a helyen
                    if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() + 1, actPiece.GetPieceCurCol() + 1).GetColor() == Color.White)
                    {
                        //megnézzük, hogy gyalog van-e a helyen
                        if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() + 1, actPiece.GetPieceCurCol() + 1).GetPieceValue() == 1)
                        {
                            canWeProtectOurPiece = true;
                        }
                    }
                }
            }
            //ha feketével vagyunk és fekete parasztot keresünk ami védi
            if (!canWeProtectOurPiece && actPiece.GetColor() == Color.Black)
            {
                //bal oldali paraszt keresés
                //ne legyen az első sorban és vagy a bal szélen
                if (actPiece.GetPieceCurRow() != 0 && actPiece.GetPieceCurCol() != 0)
                {
                    //megnézzük h fekete bábú van-e azon a helyen
                    if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() - 1, actPiece.GetPieceCurCol() - 1).GetColor() == Color.Black)
                    {
                        //megnézzük, hogy gyalog van-e a helyen
                        if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() - 1, actPiece.GetPieceCurCol() - 1).GetPieceValue() == 1)
                        {
                            canWeProtectOurPiece = true;
                        }
                    }
                }
                //jobb oldali paraszt keresés
                //ne legyen az első sorban és vagy a jobb szélen
                if (actPiece.GetPieceCurRow() != 0 && actPiece.GetPieceCurCol() != 7)
                {
                    //megnézzük h fekete bábú van-e azon a helyen
                    if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() - 1, actPiece.GetPieceCurCol() + 1).GetColor() == Color.Black)
                    {
                        //megnézzük, hogy gyalog van-e a helyen
                        if (this.GetPieceInfoFromTheBoard(actPiece.GetPieceCurRow() - 1, actPiece.GetPieceCurCol() + 1).GetPieceValue() == 1)
                        {
                            canWeProtectOurPiece = true;
                        }
                    }
                }
            }
            foreach (Pieces piece in this.GetActBoard())
            {
                if(piece.GetColor() == actPiece.GetColor())
                {
                    //majdnem minden bábú
                    if (!canWeProtectOurPiece && piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                    {
                        canWeProtectOurPiece = true;
                    }
                }
                else
                {
                    if (!canEnemyAttackOurPiece && piece.IsMoveLegal(actPiece.GetPieceCurRow(), actPiece.GetPieceCurCol(), this))
                    {
                        canEnemyAttackOurPiece = true;
                    }
                }
            }
            if(canEnemyAttackOurPiece &&  !canWeProtectOurPiece)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int IsPieceInTheSmallMiddle(Pieces actPiece)
        {
            if(actPiece.GetPieceCurCol() == 3 || actPiece.GetPieceCurCol() == 4)
            {
                if(actPiece.GetPieceCurRow() == 3 || actPiece.GetPieceCurRow() == 4)
                {
                    return 1;
                }
            }
            return 0;
        }
        public int IsPieceInTheBiggerMiddle(Pieces actPiece)
        {
            if (actPiece.GetPieceCurCol() == 3 || actPiece.GetPieceCurCol() == 4 || actPiece.GetPieceCurCol() == 2 || actPiece.GetPieceCurCol() == 5)
            {
                if (actPiece.GetPieceCurRow() == 3 || actPiece.GetPieceCurRow() == 4 || actPiece.GetPieceCurRow() == 2 || actPiece.GetPieceCurRow() == 5)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
