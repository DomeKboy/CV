using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
   public class UciControl
   {
        private string line;
        private Engine engine;
        private bool findColor;

        public UciControl(Engine engine)
        {
            this.engine = engine;
            this.line = "\0";
            this.findColor = false;
        }
        public Engine GetEngine()
        {
            return engine;
        }
        public string GetLine()
        {
            return line;
        }

        public bool GetFindColor()
        {
            return findColor;
        }
        public void SetFindColor(bool findColor)
        {
            this.findColor = findColor;
        }
        public void SetLine(string actText)
        {
            this.line = actText;
        }
        public int GetIntFromChar(char c)
        {
            switch (c)
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1; 
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
                case '1':
                    return 7;
                case '2':
                    return 6;
                case '3':
                    return 5;
                case '4':
                    return 4;
                case '5':
                    return 3;
                case '6':
                    return 2;
                case '7':
                    return 1;
                case '8':
                    return 0;
                case 'q':
                    return 5;
                case 'r':
                    return 4;
                case 'n':
                    return 2;

            }
            return 0;
        }
        public int GetIntFromCharForPromotion(char c)
        {
            switch (c)
            {
                case 'q':
                    return 5;
                case 'r':
                    return 4;
                case 'b':
                    return 3;
                case 'n':
                    return 2;   
            }
            return 0;
        }
        public void MakeAMoveFromIntArray(int[] movesInInt)
        {
            string result = "";
            
            switch (movesInInt[1])
            {
                case 0:
                    result += "a";
                    break;
                case 1:
                    result += "b";
                    break;
                case 2:
                    result += "c";
                    break;
                case 3:
                    result += "d";
                    break;
                case 4:
                    result += "e";
                    break;
                case 5:
                    result += "f";
                    break;
                case 6:
                    result += "g";
                    break;
                case 7:
                    result += "h";
                    break;
            }
            switch (movesInInt[0])
            {
                case 0:
                    result += "8";
                    break;
                case 1:
                    result += "7";
                    break;
                case 2:
                    result += "6";
                    break;
                case 3:
                    result += "5";
                    break;
                case 4:
                    result += "4";
                    break;
                case 5:
                    result += "3";
                    break;
                case 6:
                    result += "2";
                    break;
                case 7:
                    result += "1";
                    break;
            } 
            switch (movesInInt[3])
            {
                case 0:
                    result += "a";
                    break;
                case 1:
                    result += "b";
                    break;
                case 2:
                    result += "c";
                    break;
                case 3:
                    result += "d";
                    break;
                case 4:
                    result += "e";
                    break;
                case 5:
                    result += "f";
                    break;
                case 6:
                    result += "g";
                    break;
                case 7:
                    result += "h";
                    break;
            }
            switch (movesInInt[2])
            {
                case 0:
                    result += "8";
                    break;
                case 1:
                    result += "7";
                    break;
                case 2:
                    result += "6";
                    break;
                case 3:
                    result += "5";
                    break;
                case 4:
                    result += "4";
                    break;
                case 5:
                    result += "3";
                    break;
                case 6:
                    result += "2";
                    break;
                case 7:
                    result += "1";
                    break;
            }

            Console.WriteLine("bestmove " + result);
        }
        
        public static void Main()
        {
            UciControl control = new UciControl(new Engine(new Board(new Pieces[8, 8])));
            Console.WriteLine("id name DomeKChessBot" );
            Console.WriteLine("id author Dominik Gergely");
            Console.WriteLine("uciok");

            while(true)
            {

                control.SetLine(Console.ReadLine());

                if(control.GetLine().Split(' ')[0] == "quit")
                {
                    System.Environment.Exit(0);
                }
                if(control.GetLine().Split(' ')[0] == "isready")
                {
                    Console.WriteLine("readyok");
                }
                if(control.GetLine().Split(' ')[0] == "ucinewgame")
                {
                    control.SetFindColor(false);
                }
                if(control.GetLine().Split(' ')[0] == "position")
                {
                    //megnézzük a színünket ha még nem tettük
                    if(!control.GetFindColor())
                    {
                        if(control.GetLine() == "position startpos")
                        {
                            control.GetEngine().SetCurrentPlayer(Color.White);
                            control.SetFindColor(true);
                        }
                        if(!control.GetFindColor())
                        {
                            if(control.GetLine().Split(' ').Length % 2 == 0)
                            {
                                control.GetEngine().SetCurrentPlayer(Color.Black);
                                control.SetFindColor(true);
                            }
                            else
                            {
                                control.GetEngine().SetCurrentPlayer(Color.White);
                                control.SetFindColor(true);
                            }
                        }
                    }
                    string[] startPosMoves;
                    //felállítjuk a kezdőállást
                    control.GetEngine().StartingBoardPostion();
                    //ha egyáltalán történt-e már lépés
                    if(control.GetLine().Length > 22)
                    {
                        startPosMoves = control.GetLine().Remove(0, 24).Split(' ');
                        //megnézzük egyesével a moveokat
                        foreach (string s in startPosMoves)
                        {
                            //ha 4 hosszú a string akkor sima lépés
                            if (s.Length == 4)
                            {
                                //szétszedem a stringet karakterekre
                                var chars = s.ToCharArray();
                                //tmp változók
                                int startRow = control.GetIntFromChar(chars[1]);
                                int startCol = control.GetIntFromChar(chars[0]);
                                int endRow = control.GetIntFromChar(chars[3]);
                                int endCol = control.GetIntFromChar(chars[2]);
                                //végrehajtuk a lépést
                                control.GetEngine().GetBoard().MakeAMoveWithAPiece(endRow, endCol, control.GetEngine().GetBoard().GetPieceInfoFromTheBoard(startRow, startCol));

                            }
                            //promóció
                            else if (s.Length == 5)
                            {
                                //szétszedem a stringet karakterekre
                                var chars = s.ToCharArray();
                                //tmp változók
                                int startRow = control.GetIntFromChar(chars[1]);
                                int startCol = control.GetIntFromChar(chars[0]);
                                int endRow = control.GetIntFromChar(chars[3]);
                                int endCol = control.GetIntFromChar(chars[2]);
                                int promotionPieceValue = control.GetIntFromCharForPromotion(chars[4]);
                                //végrehajtuk a lépést
                                control.GetEngine().GetBoard().MakeAMoveWithAPiece(endRow, endCol, control.GetEngine().GetBoard().GetPieceInfoFromTheBoard(startRow, startCol), promotionPieceValue);
                            }
                        }
                    }
                }
                if (control.GetLine().Split(' ')[0] == "go")
                {
                    control.GetEngine().SetUpRootForTree();
                    control.GetEngine().BuildUpTree(control.GetEngine().GetNode(), control.GetEngine().GetDepth(), control.GetEngine().GetCurrentPlayer());
                    int value = control.GetEngine().SearchForBestMove(control.GetEngine().GetNode(), control.GetEngine().GetDepth() + 1, int.MinValue, int.MaxValue, true);
                    control.GetEngine().FindNodeThatHasBeenChosenToMovePre(control.GetEngine().GetNode(), value, control.GetEngine().GetDepth() + 1);
                    int[] result = control.GetEngine().GetBestMoveCoordinates(control.GetEngine().GetNode());
                    control.MakeAMoveFromIntArray(result);
                }
                if (control.GetLine().Split(' ')[0] == "test")
                {
                    control.GetEngine().SetUpRootForTree();
                    control.GetEngine().BuildUpTree(control.GetEngine().GetNode(), control.GetEngine().GetDepth(), control.GetEngine().GetCurrentPlayer());
                    int value = control.GetEngine().SearchForBestMove(control.GetEngine().GetNode(), control.GetEngine().GetDepth() + 1, int.MinValue, int.MaxValue, true);
                    Console.WriteLine(value);
                    Console.WriteLine();
                    control.GetEngine().HELP(control.GetEngine().GetNode(), value, control.GetEngine().GetDepth() + 1);
                }
                if (control.GetLine().Split(' ')[0] == "test2")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            control.GetEngine().GetBoard().PutPieceOnTheBoard(i, j, new Empty(Color.Empty, i, j, false));
                        }
                    }
                    control.GetEngine().StartingBoardPostion();
                    Console.WriteLine(control.GetEngine().GetBoard().EvaluateBoard());
                    control.GetEngine().GetBoard().MakeAMoveWithAPiece(4, 4, control.GetEngine().GetBoard().GetPieceInfoFromTheBoard(6, 4));
                    Console.WriteLine(control.GetEngine().GetBoard().EvaluateBoard());
                }
            }
        }
        
    }
}
//BISHOP PROMÓCIÓVAL BAJ VAN, mert az is "b" betűt szeretne
