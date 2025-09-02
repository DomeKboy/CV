using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Node
    {
        private int id;
        private List<int> route;
        private List<Node> children;
        private byte[,] board;
        private int score;
        private byte moveRowNumber;
        private byte moveColNumber;
        private byte startRowNumber;
        private byte startColNumber;


        public Node(int id, List<int> route, int score, byte[,] board, byte startrow, byte startcol, byte endrow, byte endcol)
        {
            this.id = id;
            this.score = score;
            this.children = new List<Node> { };
            this.route = route;
            this.board = board;
            this.startRowNumber = startrow;
            this.startColNumber = startcol;
            this.moveRowNumber = endrow;
            this.moveColNumber = endcol;

        }

        public byte[,] GetBoard()
        {
            return board;
        }
        public int GetScore()
        {
            return score;
        }
        public int GetMoveRowNumber()
        {
            return moveRowNumber;
        }
        public int GetMoveColNumber()
        {
            return moveColNumber;
        }
        public int GetStartRowNumber()
        {
            return startRowNumber;
        }
        public int GetStartColNumber()
        {
            return startColNumber;
        }
        public List<Node> GetChildren()
        {
            return children;
        }
        public List<int> GetRoute()
        {
            return route;
        }
        public void SetRoute(List<int> actroute)
        {
            this.route = actroute;
        }
        public void AddChild(Node child)
        {
            children.Add(child);
        }

    }
}
