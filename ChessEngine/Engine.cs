using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Engine
    {
        private Board board;
        private List<int[,]> possibleMoves;
        private Color currentPlayer;
        private Node root;
        private int depth;
        private List<int> findRoute;
        private bool findRouteBool;

        public Engine(Board board)
        {
            this.board = board;
            this.possibleMoves = new List<int[,]> { };
            this.currentPlayer = Color.White;
            this.depth = 2;
        }
        public Board GetBoard()
        {
            return board;
        }
        public List<int[,]> GetPossibelMoves()
        {
            return possibleMoves;
        }
        public Node GetNode() 
        { 
            return root; 
        }
        public Color GetCurrentPlayer()
        {
            return currentPlayer;
        }
        public int GetDepth()
        {
            return depth;
        }
        public List<int> GetFindRoute()
        {
            return findRoute;
        }
        public bool GetFindRouteBool()
        {
            return findRouteBool;
        }
        public void SetFindRoute(List<int> actRoute)
        {
            findRoute = actRoute;
        }
        public void SetCurrentPlayer(Color actColor)
        {
            currentPlayer = actColor;
        }
        public void SetPossibleMovesToFresh()
        {
            possibleMoves = new List<int[,]>();
        }

        public void StartingBoardPostion()
        {
            board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
            board.PutPieceOnTheBoard(0, 1, new Knight(Color.Black, 0, 1, false));
            board.PutPieceOnTheBoard(0, 2, new Bishop(Color.Black, 0, 2, false));
            board.PutPieceOnTheBoard(0, 3, new Queen(Color.Black, 0, 3, false));
            board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
            board.PutPieceOnTheBoard(0, 5, new Bishop(Color.Black, 0, 5, false));
            board.PutPieceOnTheBoard(0, 6, new Knight(Color.Black, 0, 6, false));
            board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
            for(int i = 0; i< 8; i++)
            {
                board.PutPieceOnTheBoard(1, i, new Pawn(Color.Black, 1, i, false));
            }
            for(int i = 2;  i< 6; i++)
            {
                for(int j = 0; j< 8; j++)
                {
                    board.PutPieceOnTheBoard(i, j, new Empty(Color.Empty, i, j, false)); 
                }
            }
            for (int i = 0; i < 8; i++)
            {
                board.PutPieceOnTheBoard(6, i, new Pawn(Color.White, 6, i, false));
            }
            board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
            board.PutPieceOnTheBoard(7, 1, new Knight(Color.White, 7, 1, false));
            board.PutPieceOnTheBoard(7, 2, new Bishop(Color.White, 7, 2, false));
            board.PutPieceOnTheBoard(7, 3, new Queen(Color.White, 7, 3, false));
            board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
            board.PutPieceOnTheBoard(7, 5, new Bishop(Color.White, 7, 5, false));
            board.PutPieceOnTheBoard(7, 6, new Knight(Color.White, 7, 6, false));
            board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        }
        public void FindPossibleMoves(Color actColor, Board actboard)
        {
            //kitöröljük az előző adatokat
            this.SetPossibleMovesToFresh();
            //végigmegyünk a táblán
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    //megnézzük, hogy van-e bábú a mezőn ÉS EGYENLŐRE MEGNÉZZÜK H FEHÉR
                    if (actboard.GetPieceInfoFromTheBoard(i, j).GetColor() == actColor)
                    {
                        //minden bábú listáján átmegyünk és beletesszük őket egy új listába
                        foreach (int[,] move in actboard.GetPieceInfoFromTheBoard(i, j).PotencialMoves(actboard, actColor))
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }
            }
        }
        public byte[,] CreateByteArrayFromBoard(Board board)
        {
            byte[,] result = new byte[64,2];
            int counter = 0;
            foreach(Pieces item in board.GetActBoard())
            {
                switch (item.GetPieceValue())
                {
                    case 0:
                        result[counter,0] = 0;
                        result[counter,1] = 0;
                        counter++;
                        break;
                    case 1:
                        if(item.GetColor() == Color.White)
                        {
                            if(item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 1;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 1;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 7;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 7;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                    case 2:
                        if (item.GetColor() == Color.White)
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 2;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 2;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 8;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 8;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                    case 3:
                        if (item.GetColor() == Color.White)
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 3;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 3;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 9;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 9;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                    case 4:
                        if (item.GetColor() == Color.White)
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 4;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 4;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 10;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 10;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                    case 5:
                        if (item.GetColor() == Color.White)
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 5;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 5;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 11;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 11;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                    case 6:
                        if (item.GetColor() == Color.White)
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 6;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 6;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        else
                        {
                            if (item.GetPieceHasAlreadyMoved())
                            {
                                result[counter, 0] = 12;
                                result[counter, 1] = 1;
                            }
                            else
                            {
                                result[counter, 0] = 12;
                                result[counter, 1] = 0;
                            }
                            counter++;
                        }
                        break;
                }
            }
            return result;
        }
        public Board CreateBoardFromByteArray(byte[,] array)
        {
            Board result = new Board(new Pieces[8, 8]);
            for(int i = 0; i < 8 ; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    switch (array[i*8+j,0])
                    {
                        case 0:
                            result.PutPieceOnTheBoard(i, j, new Empty(Color.Empty, i, j, false));
                            break;
                        case 1:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i,j,new Pawn(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Pawn(Color.White, i, j, true));
                            }
                            break;
                        case 7:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Pawn(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Pawn(Color.Black, i, j, true));
                            }
                            break;
                        case 2:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Knight(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Knight(Color.White, i, j, true));
                            }
                            break;
                        case 8:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Knight(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Knight(Color.Black, i, j, true));
                            }
                            break;
                        case 3:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Bishop(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Bishop(Color.White, i, j, true));
                            }
                            break;
                        case 9:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Bishop(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Bishop(Color.Black, i, j, true));
                            }
                            break;
                        case 4:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Rook(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Rook(Color.White, i, j, true));
                            }
                            break;
                        case 10:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Rook(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Rook(Color.Black, i, j, true));
                            }
                            break;
                        case 5:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Queen(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Queen(Color.White, i, j, true));
                            }
                            break;
                        case 11:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new Queen(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new Queen(Color.Black, i, j, true));
                            }
                            break;
                        case 6:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new King(Color.White, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new King(Color.White, i, j, true));
                            }
                            break;
                        case 12:
                            if (array[i * 8 + j, 1] == 0)
                            {
                                result.PutPieceOnTheBoard(i, j, new King(Color.Black, i, j, false));
                            }
                            else
                            {
                                result.PutPieceOnTheBoard(i, j, new King(Color.Black, i, j, true));
                            }
                            break;
                    }
                }
            }
            return result;
        }
        public void SetUpRootForTree()
        {
            root = new Node(0, new List<int> { }, this.GetBoard().EvaluateBoard(), this.CreateByteArrayFromBoard(this.GetBoard()), 9, 9, 9, 9);

            findRouteBool = false;
        }
        public void BuildUpTree(Node node, int depthCounter, Color actColorPlayer)
        {
            int idCounter = 1;
            if (depthCounter > 0)
            {
                //megcsináljuk a boardot
                Board tmpBoard = this.CreateBoardFromByteArray(node.GetBoard());
                //megkeressük a board lehetséges lépéseit
                this.FindPossibleMoves(actColorPlayer, tmpBoard);
                //elkezdjük beleparkolni a gyerekeket a Node-ba
                foreach (int[,] item in this.possibleMoves)
                {
                    //készítenünk kell egy új boardot a tmp-ből minden egyes esetnek
                    Board boardFromTmpBoard = new Board(new Pieces[8, 8]);
                    for(int i = 0; i < 8; i++)
                    {
                        for(int j = 0; j< 8; j++)
                        {
                            boardFromTmpBoard.PutPieceOnTheBoard(i, j, tmpBoard.GetPieceInfoFromTheBoard(i, j));
                        }
                    }    
                    //úgy teszünk rajta mintha már elvégeztük volna a lépést
                    boardFromTmpBoard.MakeAMoveWithAPiece(item[0, 2], item[0, 3], boardFromTmpBoard.GetPieceInfoFromTheBoard(item[0, 0], item[0, 1]));
                    //elkészítjük a routetot
                    List<int> tmpRoute = new List<int> { };
                    //hozzáadjuk a node parent routját
                    foreach (int number in node.GetRoute())
                    {
                        tmpRoute.Add(number);
                    }
                    //hozzáadjuk saját magát is
                    tmpRoute.Add(idCounter-1);
                    //elkészítjük az új nodeot
                    Node newNode = new Node(idCounter,tmpRoute, boardFromTmpBoard.EvaluateBoard() ,this.CreateByteArrayFromBoard(boardFromTmpBoard), (byte)item[0, 0], (byte)item[0, 1], (byte)item[0, 2], (byte)item[0, 3]);
                    node.AddChild(newNode);
                    idCounter++;
                    //ami alapján fehér vagy fekete a node meghívjuk az ellentétes színnel a rekurziót
                    if (actColorPlayer == Color.White)
                    {
                        BuildUpTree(newNode, depthCounter - 1, Color.Black);
                    }
                    else
                    {
                        BuildUpTree(newNode, depthCounter - 1, Color.White);
                    }
                }
            }
        }
        
        //ITT LEHET NÖVELNI KELL EGYEL A DEATHCOUNTERT
        public int SearchForBestMove(Node actNode, int depthCounter, int alfa, int beta, bool maximizePlayer)
        {
            //fehérrel vagyunk
            if(this.currentPlayer == Color.White)
            {
                if (depthCounter == 0 || actNode.GetChildren().Count() == 0)
                {
                    return actNode.GetScore();
                }
                if (maximizePlayer)
                {
                    int value = int.MinValue;
                    foreach (Node child in actNode.GetChildren())
                    {
                        value = Math.Max(value, SearchForBestMove(child, depthCounter - 1, alfa, beta, false));
                        if (value > beta)
                        {
                            break;
                        }
                        alfa = Math.Max(alfa, value);
                    }
                    return value;
                }
                else
                {
                    int value = int.MaxValue;
                    foreach (Node child in actNode.GetChildren())
                    {
                        value = Math.Min(value, SearchForBestMove(child, depthCounter - 1, alfa, beta, true));
                        if (value < alfa)
                        {
                            break;
                        }
                        beta = Math.Min(beta, value);
                    }
                    return value;
                }
            }
            //feketével vagyunk
            else
            {
                if (depthCounter == 0 || actNode.GetChildren().Count() == 0)
                {
                    return actNode.GetScore()*(-1);
                }
                if (maximizePlayer)
                {
                    int value = int.MinValue;
                    foreach (Node child in actNode.GetChildren())
                    {
                        value = Math.Max(value, SearchForBestMove(child, depthCounter - 1, alfa, beta, false));
                        if (value > beta)
                        {
                            break;
                        }
                        alfa = Math.Max(alfa, value);
                    }
                    return value;
                }
                else
                {
                    int value = int.MaxValue;
                    foreach (Node child in actNode.GetChildren())
                    {
                        value = Math.Min(value, SearchForBestMove(child, depthCounter - 1, alfa, beta, true));
                        if (value < alfa)
                        {
                            break;
                        }
                        beta = Math.Min(beta, value);
                    }
                    return value;
                }
            }
            
        }

        public void FindNodeThatHasBeenChosenToMovePre(Node node, int value, int depthCounter)
        {
            this.SetFindRoute(new List<int> { });
            FindNodeThatHasBeenChosenToMove(node, value, depthCounter);
        }
        public void FindNodeThatHasBeenChosenToMove(Node node, int value, int depthCounter)
        {
            if( this.GetCurrentPlayer() == Color.White)
            {
                if (depthCounter >= 0)
                {
                    if (node.GetScore() == value && node.GetChildren().Count == 0)
                    {
                        this.GetFindRoute().Add(node.GetRoute()[0]);
                    }
                    foreach (Node child in node.GetChildren())
                    {
                        FindNodeThatHasBeenChosenToMove(child, value, depthCounter - 1);
                    }
                }
            }
            else
            {
                if (depthCounter >= 0)
                {
                    if (node.GetScore()*(-1) == value && node.GetChildren().Count == 0)
                    {
                        this.GetFindRoute().Add(node.GetRoute()[0]);
                    }
                    foreach (Node child in node.GetChildren())
                    {
                        FindNodeThatHasBeenChosenToMove(child, value, depthCounter - 1);
                    }
                }
            }
            
        }
        public int[] GetBestMoveCoordinates(Node node)
        {
            var makeGroupBy = this.GetFindRoute().GroupBy(x => x)
                    .Select(g => new { Value = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count);
            var findTheMostVisitedNote = makeGroupBy.First().Value;
            int[] result = new int[4];
            result[0] = node.GetChildren()[findTheMostVisitedNote].GetStartRowNumber();
            result[1] = node.GetChildren()[findTheMostVisitedNote].GetStartColNumber();
            result[2] = node.GetChildren()[findTheMostVisitedNote].GetMoveRowNumber();
            result[3] = node.GetChildren()[findTheMostVisitedNote].GetMoveColNumber();

            return result;
        }
        //EDDIG
        public void HELP(Node node, int value, int depthCounter)
        {
            if(depthCounter >= 0)
            {
                if (node.GetScore() == value)
                {
                    foreach (int item in node.GetRoute())
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("============================");
                }
                foreach (Node child in node.GetChildren())
                {
                    HELP(child, value, depthCounter - 1);
                }
            }

        }
    }
}
