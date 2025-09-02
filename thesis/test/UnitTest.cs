using ChessEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ChessEngineTest;

[TestClass]
public class UnitTest
{
    Board _board;
    Engine _engine;
    UciControl _uciControl;

    public void CompareTwoList(List<int[,]> a, List<int[,]> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Assert.AreEqual(a[i][0, j], b[i][0, j]);
            }
        }
    }
    public void CompareTwoList(List<int> a, List<int> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            Assert.AreEqual(a[i], b[i]);
        }
    }
    //beállítjuk az engine-t, a táblán nincs egy bábú sem
    [TestInitialize]
    public void Initialize()
    {
        _board = new Board(new Pieces[8, 8]);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _board.PutPieceOnTheBoard(i, j, new Empty(Color.Empty, i, j, false));
            }
        }
        _engine = new Engine(_board);
        _uciControl = new UciControl(_engine);

    }

    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnMovementFromStartPos()
    {
        _board.PutPieceOnTheBoard(6, 1, new Pawn(Color.White, 6, 1, false));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(5, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(4, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(5, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(5, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(7, 1, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 6, 1, 4, 1 } });
        moves.Add(new int[,] { { 6, 1, 5, 1 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnMovementFromNotStartPos()
    {
        _board.PutPieceOnTheBoard(5, 1, new Pawn(Color.White, 5, 1, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(6, 1, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 5, 1, 4, 1 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnMovementCanTakeToRight()
    {
        _board.PutPieceOnTheBoard(5, 1, new Pawn(Color.White, 5, 1, true));
        _board.PutPieceOnTheBoard(4, 2, new Pawn(Color.Black, 4, 2, true));
        _board.PutPieceOnTheBoard(4, 0, new Pawn(Color.White, 4, 0, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 2, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 4, 0, 3, 0 } });
        moves.Add(new int[,] { { 5, 1, 4, 1 } });
        moves.Add(new int[,] { { 5, 1, 4, 2 } });
        List<int[,]> pawn51Moves = new List<int[,]>();
        pawn51Moves.Add(new int[,] { { 5, 1, 4, 1 } });
        pawn51Moves.Add(new int[,] { { 5, 1, 4, 2 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
        CompareTwoList(pawn51Moves, _board.GetPieceInfoFromTheBoard(5, 1).PotencialMoves(_board, Color.White));
    }

    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnMovementCanTakeToLeft()
    {
        _board.PutPieceOnTheBoard(5, 1, new Pawn(Color.White, 5, 1, true));
        _board.PutPieceOnTheBoard(4, 2, new Pawn(Color.White, 4, 2, true));
        _board.PutPieceOnTheBoard(4, 0, new Pawn(Color.Black, 4, 0, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 1).IsMoveLegal(4, 2, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 4, 2, 3, 2 } });
        moves.Add(new int[,] { { 5, 1, 4, 0 } });
        moves.Add(new int[,] { { 5, 1, 4, 1 } });
        List<int[,]> pawn51Moves = new List<int[,]>();
        pawn51Moves.Add(new int[,] { { 5, 1, 4, 0 } });
        pawn51Moves.Add(new int[,] { { 5, 1, 4, 1 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
        CompareTwoList(pawn51Moves, _board.GetPieceInfoFromTheBoard(5, 1).PotencialMoves(_board, Color.White));
    }
    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnCanEnPassant()
    {
        _board.PutPieceOnTheBoard(3, 3, new Pawn(Color.White, 3, 3, true));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(3, 4, new Pawn(Color.Black, 3, 4, false));
        _board.PutPieceOnTheBoard(5, 5, new Pawn(Color.White, 5, 5, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(2, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(2, 2, _board));
        _board.MakeAMoveWithAPiece(3, 2, _board.GetPieceInfoFromTheBoard(1, 2));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(2, 2, _board));
        _board.MakeAMoveWithAPiece(4, 5, _board.GetPieceInfoFromTheBoard(5, 5));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(2, 2, _board));
    }
    [TestMethod]
    [TestCategory("WhitePawn")]
    public void WhitePawnCanPromote()
    {
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.White, 1, 1, true));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.White, 1, 2, true));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.White, 1, 3, true));
        _board.PutPieceOnTheBoard(1, 4, new Pawn(Color.White, 1, 4, true));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 2).IsMoveLegal(0, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 3).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 4).IsMoveLegal(0, 4, _board));
        _board.MakeAMoveWithAPiece(0, 1, _board.GetPieceInfoFromTheBoard(1, 1), 2);
        _board.MakeAMoveWithAPiece(0, 2, _board.GetPieceInfoFromTheBoard(1, 2), 3);
        _board.MakeAMoveWithAPiece(0, 3, _board.GetPieceInfoFromTheBoard(1, 3), 4);
        _board.MakeAMoveWithAPiece(0, 4, _board.GetPieceInfoFromTheBoard(1, 4), 5);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 1).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 2).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 4).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 1).GetPieceValue(), 2);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 2).GetPieceValue(), 3);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 3).GetPieceValue(), 4);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 4).GetPieceValue(), 5);
    }
    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnMovementFromStartPos()
    {
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.Black, 1, 1, false));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(2, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(3, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(2, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(2, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(1, 1).IsMoveLegal(0, 1, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 1, 1, 2, 1 } });
        moves.Add(new int[,] { { 1, 1, 3, 1 } });

        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnMovementFromNotStartPos()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(4, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(1, 1, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 2, 1, 3, 1 } });

        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnMovementCanTakeToRight()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        _board.PutPieceOnTheBoard(3, 2, new Pawn(Color.White, 3, 2, true));
        _board.PutPieceOnTheBoard(3, 0, new Pawn(Color.Black, 3, 0, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(4, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 2, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 2, 1, 3, 1 } });
        moves.Add(new int[,] { { 2, 1, 3, 2 } });
        moves.Add(new int[,] { { 3, 0, 4, 0 } });
        List<int[,]> pawn21Moves = new List<int[,]>();
        pawn21Moves.Add(new int[,] { { 2, 1, 3, 1 } });
        pawn21Moves.Add(new int[,] { { 2, 1, 3, 2 } });

        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
        CompareTwoList(pawn21Moves, _board.GetPieceInfoFromTheBoard(2, 1).PotencialMoves(_board, Color.Black));
    }

    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnMovementCanTakeToLeft()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        _board.PutPieceOnTheBoard(3, 2, new Pawn(Color.Black, 3, 2, true));
        _board.PutPieceOnTheBoard(3, 0, new Pawn(Color.White, 3, 0, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(4, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 1).IsMoveLegal(3, 2, _board));

        List<int[,]> moves = new List<int[,]>();
        moves.Add(new int[,] { { 2, 1, 3, 0 } });
        moves.Add(new int[,] { { 2, 1, 3, 1 } });
        moves.Add(new int[,] { { 3, 2, 4, 2 } });
        List<int[,]> pawn21Moves = new List<int[,]>();
        pawn21Moves.Add(new int[,] { { 2, 1, 3, 0 } });
        pawn21Moves.Add(new int[,] { { 2, 1, 3, 1 } });

        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(moves, _engine.GetPossibelMoves());
        CompareTwoList(pawn21Moves, _board.GetPieceInfoFromTheBoard(2, 1).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnCanEnPassant()
    {
        _board.PutPieceOnTheBoard(4, 3, new Pawn(Color.Black, 4, 3, true));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.White, 6, 2, false));
        _board.PutPieceOnTheBoard(4, 4, new Pawn(Color.White, 4, 4, false));
        _board.PutPieceOnTheBoard(5, 5, new Pawn(Color.Black, 5, 5, true));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 3).IsMoveLegal(5, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(5, 4, _board));
        _board.MakeAMoveWithAPiece(4, 2, _board.GetPieceInfoFromTheBoard(6, 2));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 3).IsMoveLegal(5, 2, _board));
        _board.MakeAMoveWithAPiece(6, 5, _board.GetPieceInfoFromTheBoard(5, 5));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 3).IsMoveLegal(5, 2, _board));
    }
    [TestMethod]
    [TestCategory("BlackPawn")]
    public void BlackPawnCanPromote()
    {
        _board.PutPieceOnTheBoard(6, 1, new Pawn(Color.Black, 6, 1, true));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.Black, 6, 2, true));
        _board.PutPieceOnTheBoard(6, 3, new Pawn(Color.Black, 6, 3, true));
        _board.PutPieceOnTheBoard(6, 4, new Pawn(Color.Black, 6, 4, true));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 1).IsMoveLegal(7, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 2).IsMoveLegal(7, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 3).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(6, 4).IsMoveLegal(7, 4, _board));
        _board.MakeAMoveWithAPiece(7, 1, _board.GetPieceInfoFromTheBoard(6, 1), 2);
        _board.MakeAMoveWithAPiece(7, 2, _board.GetPieceInfoFromTheBoard(6, 2), 3);
        _board.MakeAMoveWithAPiece(7, 3, _board.GetPieceInfoFromTheBoard(6, 3), 4);
        _board.MakeAMoveWithAPiece(7, 4, _board.GetPieceInfoFromTheBoard(6, 4), 5);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 1).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 2).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 1).GetPieceValue(), 2);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 2).GetPieceValue(), 3);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 3).GetPieceValue(), 4);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 4).GetPieceValue(), 5);
    }

    [TestMethod]
    [TestCategory("Knight")]
    public void KnightMovementForBothColor()
    {
        _board.PutPieceOnTheBoard(4, 4, new Knight(Color.White, 4, 4, true));
        _board.PutPieceOnTheBoard(4, 5, new Knight(Color.Black, 4, 5, true));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(6, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(6, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(2, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(2, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(3, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(3, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(5, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(5, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(6, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(6, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(2, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(2, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(3, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(5, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(5, 7, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesWhite.Add(new int[,] { { 4, 4, 2, 3 } });
        movesWhite.Add(new int[,] { { 4, 4, 2, 5 } });
        movesWhite.Add(new int[,] { { 4, 4, 3, 2 } });
        movesWhite.Add(new int[,] { { 4, 4, 3, 6 } });
        movesWhite.Add(new int[,] { { 4, 4, 5, 2 } });
        movesWhite.Add(new int[,] { { 4, 4, 5, 6 } });
        movesWhite.Add(new int[,] { { 4, 4, 6, 3 } });
        movesWhite.Add(new int[,] { { 4, 4, 6, 5 } });

        movesBlack.Add(new int[,] { { 4, 5, 2, 4 } });
        movesBlack.Add(new int[,] { { 4, 5, 2, 6 } });
        movesBlack.Add(new int[,] { { 4, 5, 3, 3 } });
        movesBlack.Add(new int[,] { { 4, 5, 3, 7 } });
        movesBlack.Add(new int[,] { { 4, 5, 5, 3 } });
        movesBlack.Add(new int[,] { { 4, 5, 5, 7 } });
        movesBlack.Add(new int[,] { { 4, 5, 6, 4 } });
        movesBlack.Add(new int[,] { { 4, 5, 6, 6 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("Knight")]
    public void KnightCantJumpOutBoard()
    {
        _board.PutPieceOnTheBoard(0, 0, new Knight(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(7, 7, new Knight(Color.Black, 7, 7, true));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-1, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-2, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-2, -1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-1, -2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, -2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, -1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(8, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(9, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(9, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(8, 9, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 9, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 8, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesWhite.Add(new int[,] { { 0, 0, 1, 2 } });
        movesWhite.Add(new int[,] { { 0, 0, 2, 1 } });
        movesBlack.Add(new int[,] { { 7, 7, 5, 6 } });
        movesBlack.Add(new int[,] { { 7, 7, 6, 5 } });
        List<int[,]> movesWhiteKnight00 = new List<int[,]>();
        List<int[,]> movesBlackKnight77 = new List<int[,]>();
        movesWhiteKnight00.Add(new int[,] { { 0, 0, 1, 2 } });
        movesWhiteKnight00.Add(new int[,] { { 0, 0, 2, 1 } });
        movesBlackKnight77.Add(new int[,] { { 7, 7, 5, 6 } });
        movesBlackKnight77.Add(new int[,] { { 7, 7, 6, 5 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
        CompareTwoList(movesWhiteKnight00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackKnight77, _board.GetPieceInfoFromTheBoard(7, 7).PotencialMoves(_board, Color.Black));

    }
    [TestMethod]
    [TestCategory("Knight")]
    public void KnightCanTakeEnemyButNotTeammate()
    {
        _board.PutPieceOnTheBoard(0, 0, new Knight(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.White, 2, 1, true));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(7, 7, new Knight(Color.Black, 7, 7, true));
        _board.PutPieceOnTheBoard(6, 5, new Pawn(Color.White, 6, 5, false));
        _board.PutPieceOnTheBoard(5, 6, new Pawn(Color.Black, 5, 6, true));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 5, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesWhite.Add(new int[,] { { 0, 0, 1, 2 } });
        movesWhite.Add(new int[,] { { 2, 1, 1, 1 } });
        movesWhite.Add(new int[,] { { 2, 1, 1, 2 } });
        movesWhite.Add(new int[,] { { 6, 5, 4, 5 } });
        movesWhite.Add(new int[,] { { 6, 5, 5, 5 } });
        movesWhite.Add(new int[,] { { 6, 5, 5, 6 } });
        movesBlack.Add(new int[,] { { 1, 2, 2, 1 } });
        movesBlack.Add(new int[,] { { 1, 2, 2, 2 } });
        movesBlack.Add(new int[,] { { 1, 2, 3, 2 } });
        movesBlack.Add(new int[,] { { 5, 6, 6, 5 } });
        movesBlack.Add(new int[,] { { 5, 6, 6, 6 } });
        movesBlack.Add(new int[,] { { 7, 7, 6, 5 } });
        List<int[,]> movesWhiteKnight00 = new List<int[,]>();
        List<int[,]> movesBlackKnight77 = new List<int[,]>();
        movesWhiteKnight00.Add(new int[,] { { 0, 0, 1, 2 } });
        movesBlackKnight77.Add(new int[,] { { 7, 7, 6, 5 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
        CompareTwoList(movesWhiteKnight00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackKnight77, _board.GetPieceInfoFromTheBoard(7, 7).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Bishop")]
    public void BishopMovementForBothColor()
    {
        _board.PutPieceOnTheBoard(4, 4, new Bishop(Color.White, 4, 4, true));
        _board.PutPieceOnTheBoard(4, 5, new Bishop(Color.Black, 4, 5, true));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(2, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(1, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(0, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(5, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(6, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(7, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(5, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(6, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(7, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(3, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(2, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(1, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(5, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(6, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(3, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(2, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(1, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(3, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(2, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(5, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(6, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(7, 2, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesWhite.Add(new int[,] { { 4, 4, 0, 0 } });
        movesWhite.Add(new int[,] { { 4, 4, 1, 1 } });
        movesWhite.Add(new int[,] { { 4, 4, 1, 7 } });
        movesWhite.Add(new int[,] { { 4, 4, 2, 2 } });
        movesWhite.Add(new int[,] { { 4, 4, 2, 6 } });
        movesWhite.Add(new int[,] { { 4, 4, 3, 3 } });
        movesWhite.Add(new int[,] { { 4, 4, 3, 5 } });
        movesWhite.Add(new int[,] { { 4, 4, 5, 3 } });
        movesWhite.Add(new int[,] { { 4, 4, 5, 5 } });
        movesWhite.Add(new int[,] { { 4, 4, 6, 2 } });
        movesWhite.Add(new int[,] { { 4, 4, 6, 6 } });
        movesWhite.Add(new int[,] { { 4, 4, 7, 1 } });
        movesWhite.Add(new int[,] { { 4, 4, 7, 7 } });

        movesBlack.Add(new int[,] { { 4, 5, 0, 1 } });
        movesBlack.Add(new int[,] { { 4, 5, 1, 2 } });
        movesBlack.Add(new int[,] { { 4, 5, 2, 3 } });
        movesBlack.Add(new int[,] { { 4, 5, 2, 7 } });
        movesBlack.Add(new int[,] { { 4, 5, 3, 4 } });
        movesBlack.Add(new int[,] { { 4, 5, 3, 6 } });
        movesBlack.Add(new int[,] { { 4, 5, 5, 4 } });
        movesBlack.Add(new int[,] { { 4, 5, 5, 6 } });
        movesBlack.Add(new int[,] { { 4, 5, 6, 3 } });
        movesBlack.Add(new int[,] { { 4, 5, 6, 7 } });
        movesBlack.Add(new int[,] { { 4, 5, 7, 2 } });


        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());

    }
    [TestMethod]
    [TestCategory("Bishop")]
    public void BishopCantOutJumpFromBoard()
    {
        _board.PutPieceOnTheBoard(4, 4, new Bishop(Color.White, 4, 4, true));
        _board.PutPieceOnTheBoard(4, 5, new Bishop(Color.Black, 4, 5, true));

        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(-1, -1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 4).IsMoveLegal(8, 8, _board));

        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(1, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(4, 5).IsMoveLegal(8, 1, _board));
        List<int[,]> movesWhiteBishop44 = new List<int[,]>();
        List<int[,]> movesBlackBlack45 = new List<int[,]>();
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 0, 0 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 1, 1 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 1, 7 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 2, 2 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 2, 6 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 3, 3 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 3, 5 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 5, 3 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 5, 5 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 6, 2 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 6, 6 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 7, 1 } });
        movesWhiteBishop44.Add(new int[,] { { 4, 4, 7, 7 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 0, 1 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 1, 2 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 2, 3 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 2, 7 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 3, 4 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 3, 6 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 5, 4 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 5, 6 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 6, 3 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 6, 7 } });
        movesBlackBlack45.Add(new int[,] { { 4, 5, 7, 2 } });
        CompareTwoList(movesWhiteBishop44, _board.GetPieceInfoFromTheBoard(4, 4).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackBlack45, _board.GetPieceInfoFromTheBoard(4, 5).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Bishop")]
    public void BishopCantOverjump()
    {
        _board.PutPieceOnTheBoard(0, 0, new Bishop(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(0, 7, new Bishop(Color.Black, 0, 7, true));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.White, 1, 1, true));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.Black, 1, 6, false));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 7, _board));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(1, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(2, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(3, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(4, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(5, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(6, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(7, 0, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();

        movesWhite.Add(new int[,] { { 1, 1, 0, 1 } });
        movesBlack.Add(new int[,] { { 1, 6, 2, 6 } });
        movesBlack.Add(new int[,] { { 1, 6, 3, 6 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("Bishop")]
    public void BishopCanTake()
    {
        _board.PutPieceOnTheBoard(0, 0, new Bishop(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(0, 7, new Bishop(Color.Black, 0, 7, true));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.Black, 1, 1, false));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.White, 1, 6, true));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 7, _board));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(1, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(2, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(3, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(4, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(5, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(6, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 7).IsMoveLegal(7, 0, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();

        movesWhite.Add(new int[,] { { 0, 0, 1, 1 } });
        movesWhite.Add(new int[,] { { 1, 6, 0, 6 } });
        movesWhite.Add(new int[,] { { 1, 6, 0, 7 } });

        movesBlack.Add(new int[,] { { 0, 7, 1, 6 } });
        movesBlack.Add(new int[,] { { 1, 1, 2, 1 } });
        movesBlack.Add(new int[,] { { 1, 1, 3, 1 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());

        List<int[,]> movesWhiteBishop00 = new List<int[,]>();
        List<int[,]> movesBlackBlack07 = new List<int[,]>();

        movesWhiteBishop00.Add(new int[,] { { 0, 0, 1, 1 } });
        movesBlackBlack07.Add(new int[,] { { 0, 7, 1, 6 } });
        CompareTwoList(movesWhiteBishop00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackBlack07, _board.GetPieceInfoFromTheBoard(0, 7).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Rook")]
    public void RookMovementForBothColor()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(4, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(3, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(2, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(1, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 7, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesBlack.Add(new int[,] { { 0, 0, 0, 1 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 2 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 3 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 4 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 5 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 6 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 7 } });
        movesBlack.Add(new int[,] { { 0, 0, 1, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 2, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 3, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 4, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 5, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 6, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 7, 0 } });

        movesWhite.Add(new int[,] { { 7, 7, 0, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 1, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 2, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 3, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 4, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 5, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 6, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 0 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 1 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 2 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 3 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 4 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 5 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 6 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("Rook")]
    public void RookCantOutJumpFromBoard()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(8, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, -1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-1, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(8, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(-1, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, -1, _board));

        List<int[,]> movesWhiteRook77 = new List<int[,]>();
        List<int[,]> movesBlackRook00 = new List<int[,]>();
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 1 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 2 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 3 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 4 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 5 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 6 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 7 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 1, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 2, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 3, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 4, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 5, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 6, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 7, 0 } });

        movesWhiteRook77.Add(new int[,] { { 7, 7, 0, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 1, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 2, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 3, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 4, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 5, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 6, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 0 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 1 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 2 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 3 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 4 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 5 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 6 } });

        CompareTwoList(movesWhiteRook77, _board.GetPieceInfoFromTheBoard(7, 7).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackRook00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Rook")]
    public void RookCantOverJump()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        _board.PutPieceOnTheBoard(7, 3, new Pawn(Color.White, 7, 3, false));
        _board.PutPieceOnTheBoard(0, 3, new Pawn(Color.White, 0, 3, false));
        _board.PutPieceOnTheBoard(4, 0, new Pawn(Color.Black, 4, 0, false));
        _board.PutPieceOnTheBoard(4, 7, new Pawn(Color.Black, 4, 7, false));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(4, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 0, _board));

        List<int[,]> movesWhiteRook77 = new List<int[,]>();
        List<int[,]> movesBlackRook00 = new List<int[,]>();
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 1 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 2 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 3 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 1, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 2, 0 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 3, 0 } });

        movesWhiteRook77.Add(new int[,] { { 7, 7, 4, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 5, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 6, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 4 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 5 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 6 } });

        CompareTwoList(movesWhiteRook77, _board.GetPieceInfoFromTheBoard(7, 7).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackRook00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Rook")]
    public void RookCanTake()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        _board.PutPieceOnTheBoard(0, 1, new Pawn(Color.White, 0, 1, false));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.White, 1, 0, false));
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.Black, 6, 7, false));
        _board.PutPieceOnTheBoard(7, 6, new Pawn(Color.Black, 7, 6, false));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 6, _board));

        List<int[,]> movesWhiteRook77 = new List<int[,]>();
        List<int[,]> movesBlackRook00 = new List<int[,]>();
        movesBlackRook00.Add(new int[,] { { 0, 0, 0, 1 } });
        movesBlackRook00.Add(new int[,] { { 0, 0, 1, 0 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 6, 7 } });
        movesWhiteRook77.Add(new int[,] { { 7, 7, 7, 6 } });

        CompareTwoList(movesWhiteRook77, _board.GetPieceInfoFromTheBoard(7, 7).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackRook00, _board.GetPieceInfoFromTheBoard(0, 0).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Queen")]
    public void QueenMovementForBothColor()
    {
        _board.PutPieceOnTheBoard(0, 0, new Queen(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Queen(Color.White, 7, 7, false));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(1, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(2, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(4, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(5, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(6, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(7, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(7, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(6, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(5, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(4, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(4, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(3, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(2, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(2, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(1, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(1, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 7, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 7).IsMoveLegal(0, 0, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesBlack.Add(new int[,] { { 0, 0, 0, 1 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 2 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 3 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 4 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 5 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 6 } });
        movesBlack.Add(new int[,] { { 0, 0, 0, 7 } });
        movesBlack.Add(new int[,] { { 0, 0, 1, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 1, 1 } });
        movesBlack.Add(new int[,] { { 0, 0, 2, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 2, 2 } });
        movesBlack.Add(new int[,] { { 0, 0, 3, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 3, 3 } });
        movesBlack.Add(new int[,] { { 0, 0, 4, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 4, 4 } });
        movesBlack.Add(new int[,] { { 0, 0, 5, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 5, 5 } });
        movesBlack.Add(new int[,] { { 0, 0, 6, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 6, 6 } });
        movesBlack.Add(new int[,] { { 0, 0, 7, 0 } });
        movesBlack.Add(new int[,] { { 0, 0, 7, 7 } });

        movesWhite.Add(new int[,] { { 7, 7, 0, 0 } });
        movesWhite.Add(new int[,] { { 7, 7, 0, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 1, 1 } });
        movesWhite.Add(new int[,] { { 7, 7, 1, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 2, 2 } });
        movesWhite.Add(new int[,] { { 7, 7, 2, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 3, 3 } });
        movesWhite.Add(new int[,] { { 7, 7, 3, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 4, 4 } });
        movesWhite.Add(new int[,] { { 7, 7, 4, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 5, 5 } });
        movesWhite.Add(new int[,] { { 7, 7, 5, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 6, 6 } });
        movesWhite.Add(new int[,] { { 7, 7, 6, 7 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 0 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 1 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 2 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 3 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 4 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 5 } });
        movesWhite.Add(new int[,] { { 7, 7, 7, 6 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("Queen")]
    public void QueenCantOutJumpFromBoard()
    {
        _board.PutPieceOnTheBoard(0, 0, new Queen(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 0, new Queen(Color.White, 7, 0, false));

        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(-1, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(0, -1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 0).IsMoveLegal(8, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 0).IsMoveLegal(7, -1, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 0).IsMoveLegal(8, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 0).IsMoveLegal(7, 8, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 0).IsMoveLegal(-1, -1, _board));
    }
    [TestMethod]
    [TestCategory("Queen")]
    public void QuuenCantOverJump()
    {
        _board.PutPieceOnTheBoard(2, 2, new Queen(Color.Black, 2, 2, false));
        _board.PutPieceOnTheBoard(5, 5, new Queen(Color.White, 5, 5, false));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.Black, 1, 1, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.Black, 1, 3, false));
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, false));
        _board.PutPieceOnTheBoard(2, 3, new Pawn(Color.Black, 2, 3, false));
        _board.PutPieceOnTheBoard(3, 1, new Pawn(Color.Black, 3, 1, false));
        _board.PutPieceOnTheBoard(3, 2, new Pawn(Color.Black, 3, 2, false));
        _board.PutPieceOnTheBoard(3, 3, new Pawn(Color.Black, 3, 3, false));
        _board.PutPieceOnTheBoard(4, 4, new Pawn(Color.White, 4, 4, false));
        _board.PutPieceOnTheBoard(4, 5, new Pawn(Color.White, 4, 5, false));
        _board.PutPieceOnTheBoard(4, 6, new Pawn(Color.White, 4, 6, false));
        _board.PutPieceOnTheBoard(5, 4, new Pawn(Color.White, 5, 4, false));
        _board.PutPieceOnTheBoard(5, 6, new Pawn(Color.White, 5, 6, false));
        _board.PutPieceOnTheBoard(6, 4, new Pawn(Color.White, 6, 4, false));
        _board.PutPieceOnTheBoard(6, 5, new Pawn(Color.White, 6, 5, false));
        _board.PutPieceOnTheBoard(6, 6, new Pawn(Color.White, 6, 6, false));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(4, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(4, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(4, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(5, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(5, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(6, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(6, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(6, 6, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(0, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(0, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(0, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(4, 0, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(4, 2, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(4, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(7, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(5, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(3, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(5, 7, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(5, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(3, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(3, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(3, 7, _board));

        List<int[,]> movesWhiteQueen55 = new List<int[,]>();
        List<int[,]> movesBlackQueen22 = new List<int[,]>();
        CompareTwoList(movesWhiteQueen55, _board.GetPieceInfoFromTheBoard(5, 5).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackQueen22, _board.GetPieceInfoFromTheBoard(2, 2).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("Queen")]
    public void QuuenCanTake()
    {
        _board.PutPieceOnTheBoard(2, 2, new Queen(Color.Black, 2, 2, false));
        _board.PutPieceOnTheBoard(2, 5, new Queen(Color.White, 2, 5, false));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.White, 1, 1, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.White, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.White, 1, 3, false));
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.White, 2, 1, false));
        _board.PutPieceOnTheBoard(2, 3, new Pawn(Color.White, 2, 3, false));
        _board.PutPieceOnTheBoard(3, 1, new Pawn(Color.White, 3, 1, false));
        _board.PutPieceOnTheBoard(3, 2, new Pawn(Color.White, 3, 2, false));
        _board.PutPieceOnTheBoard(3, 3, new Pawn(Color.White, 3, 3, false));
        _board.PutPieceOnTheBoard(1, 4, new Pawn(Color.Black, 4, 4, false));
        _board.PutPieceOnTheBoard(1, 5, new Pawn(Color.Black, 4, 5, false));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.Black, 4, 6, false));
        _board.PutPieceOnTheBoard(2, 4, new Pawn(Color.Black, 5, 4, false));
        _board.PutPieceOnTheBoard(2, 6, new Pawn(Color.Black, 5, 6, false));
        _board.PutPieceOnTheBoard(3, 4, new Pawn(Color.Black, 6, 4, false));
        _board.PutPieceOnTheBoard(3, 5, new Pawn(Color.Black, 6, 5, false));
        _board.PutPieceOnTheBoard(3, 6, new Pawn(Color.Black, 6, 6, false));

        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(1, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(2, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 1, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 2, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 2).IsMoveLegal(3, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(1, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(1, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(1, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(2, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(2, 6, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(3, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(3, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(2, 5).IsMoveLegal(3, 6, _board));

        List<int[,]> movesWhiteQueen55 = new List<int[,]>();
        List<int[,]> movesBlackQueen22 = new List<int[,]>();
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 1, 4 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 1, 5 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 1, 6 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 2, 4 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 2, 6 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 3, 4 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 3, 5 } });
        movesWhiteQueen55.Add(new int[,] { { 2, 5, 3, 6 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 1, 1 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 1, 2 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 1, 3 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 2, 1 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 2, 3 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 3, 1 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 3, 2 } });
        movesBlackQueen22.Add(new int[,] { { 2, 2, 3, 3 } });

        CompareTwoList(movesWhiteQueen55, _board.GetPieceInfoFromTheBoard(5, 5).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackQueen22, _board.GetPieceInfoFromTheBoard(2, 2).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("King")]
    public void KingMovementNoChecks()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));


        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(0, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(7, 5, _board));
        
        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        movesBlack.Add(new int[,] { { 0, 4, 0, 3 } });
        movesBlack.Add(new int[,] { { 0, 4, 0, 5 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 3 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 4 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 5 } });

        movesWhite.Add(new int[,] { { 7, 4, 6, 3 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 4 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 5 } });
        movesWhite.Add(new int[,] { { 7, 4, 7, 3 } });
        movesWhite.Add(new int[,] { { 7, 4, 7, 5 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("King")]
    public void KingCantLeaveBoard()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));


        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(0, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(0, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(1, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 4, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(6, 5, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(7, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(-1, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(-1, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(0, 4).IsMoveLegal(-1, 5, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(8, 3, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(8, 4, _board));
        Assert.IsFalse(_board.GetPieceInfoFromTheBoard(7, 4).IsMoveLegal(8, 5, _board));


        List<int[,]> movesWhiteKing74 = new List<int[,]>();
        List<int[,]> movesBlackKing04 = new List<int[,]>();
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 3 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 5 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 3 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 4 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 5 } });

        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 3 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 4 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 5 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 3 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 5 } });

        CompareTwoList(movesWhiteKing74, _board.GetPieceInfoFromTheBoard(7, 4).PotencialMoves(_board, Color.White));
        CompareTwoList(movesBlackKing04, _board.GetPieceInfoFromTheBoard(0, 4).PotencialMoves(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("King")]
    public void WhiteKingCanCastle()
    {
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        List<int[,]> movesWhiteKing74 = new List<int[,]>();
        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 3 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 4 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 6, 5 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 3 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 5 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 6 } });
        movesWhiteKing74.Add(new int[,] { { 7, 4, 7, 2 } });
        CompareTwoList(movesWhiteKing74, _board.GetPieceInfoFromTheBoard(7, 4).PotencialMoves(_board, Color.White));
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleLong(_board,Color.White));
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleShort(_board, Color.White));
    }
    [TestMethod]
    [TestCategory("King")]
    public void BlackKingCanCastle()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
        
        List<int[,]> movesBlackKing04 = new List<int[,]>();
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 3 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 5 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 3 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 4 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 1, 5 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 6 } });
        movesBlackKing04.Add(new int[,] { { 0, 4, 0, 2 } });
        CompareTwoList(movesBlackKing04, _board.GetPieceInfoFromTheBoard(0, 4).PotencialMoves(_board, Color.Black));

        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("King")]
    public void WhiteKingCantCastle()
    {
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        //megnézzük h működik
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleLong(_board, Color.White));
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleShort(_board, Color.White));
        //megnézzük ha a király már mozgott
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleLong(_board, Color.White));
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleShort(_board, Color.White));
        //megnézzük ha valamelyik rook már mozgott
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, true));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        Assert.IsFalse(new King(Color.White, 7, 4, false).isPossibleCastleLong(_board, Color.White));
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleShort(_board, Color.White));
        //másik rook mozgott
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, true));
        Assert.IsTrue(new King(Color.White, 7, 4, false).isPossibleCastleLong(_board, Color.White));
        Assert.IsFalse(new King(Color.White, 7, 4, false).isPossibleCastleShort(_board, Color.White));
        //sakkban áll
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        _board.PutPieceOnTheBoard(2, 4, new Rook(Color.Black, 2, 4, true));
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleLong(_board, Color.White));
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleShort(_board, Color.White));
        //blokkolva van az út
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        _board.PutPieceOnTheBoard(2, 3, new Rook(Color.Black, 2, 3, true));
        _board.PutPieceOnTheBoard(2, 5, new Rook(Color.Black, 2, 5, true));
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleLong(_board, Color.White));
        Assert.IsFalse(new King(Color.White, 7, 4, true).isPossibleCastleShort(_board, Color.White));
    }
    [TestMethod]
    [TestCategory("King")]
    public void BlackKingCantCastle()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
        //megnézzük h működik
        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
        //megnézzük ha a király már mozgott
        Assert.IsFalse(new King(Color.Black, 0, 4, true).isPossibleCastleLong(_board, Color.Black));
        Assert.IsFalse(new King(Color.Black, 0, 4, true).isPossibleCastleShort(_board, Color.Black));
        //egyik rook már mozgott
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, true));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
        //másik rook
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, true));
        Assert.IsTrue(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
        //sakkban áll
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
        _board.PutPieceOnTheBoard(5, 4, new Rook(Color.White, 5, 4, true));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
        //blokkolják a sáncot
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 7, new Rook(Color.Black, 0, 7, false));
        _board.PutPieceOnTheBoard(5, 3, new Rook(Color.White, 5, 3, true));
        _board.PutPieceOnTheBoard(5, 5, new Rook(Color.White, 5, 5, true));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleLong(_board, Color.Black));
        Assert.IsFalse(new King(Color.Black, 0, 4, false).isPossibleCastleShort(_board, Color.Black));
    }
    [TestMethod]
    [TestCategory("King")]
    public void CantPlayMoveIfWeAreInCheck()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.Black, 7, 7, true));
        _board.PutPieceOnTheBoard(3, 3, new Rook(Color.Black, 3, 3, true));
        _board.PutPieceOnTheBoard(5, 5, new Rook(Color.White, 5, 5, true));

        //a minta alapján léphetnénk ezt de a potenciális lépések között már nem fog szerepelni
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(3, 3).IsMoveLegal(7, 3, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 5).IsMoveLegal(0, 5, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();
        //kitérünk a királlyal
        movesBlack.Add(new int[,] { { 0, 4, 1, 3 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 4 } });
        //belépünk blokkolni
        movesBlack.Add(new int[,] { { 3, 3, 0, 3 } });

        //belépünk blokkolni
        movesWhite.Add(new int[,] { { 5, 5, 7, 5 } });
        //kitérünk a királlyal
        movesWhite.Add(new int[,] { { 7, 4, 6, 4 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 5 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("King")]
    public void CantPlayMoveIfWeWouldBeInCheck()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
        _board.PutPieceOnTheBoard(3, 4, new Rook(Color.Black, 3, 4, true));
        _board.PutPieceOnTheBoard(5, 4, new Rook(Color.White, 5, 4, true));
        //a minta alapján léphetnénk ezt de a potenciális lépések között már nem fog szerepelni
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(3, 4).IsMoveLegal(3, 0, _board));
        Assert.IsTrue(_board.GetPieceInfoFromTheBoard(5, 4).IsMoveLegal(5, 0, _board));

        List<int[,]> movesWhite = new List<int[,]>();
        List<int[,]> movesBlack = new List<int[,]>();

        movesBlack.Add(new int[,] { { 0, 4, 0, 3 } });
        movesBlack.Add(new int[,] { { 0, 4, 0, 5 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 3 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 4 } });
        movesBlack.Add(new int[,] { { 0, 4, 1, 5 } });
        movesBlack.Add(new int[,] { { 3, 4, 1, 4 } });
        movesBlack.Add(new int[,] { { 3, 4, 2, 4 } });
        movesBlack.Add(new int[,] { { 3, 4, 4, 4 } });
        movesBlack.Add(new int[,] { { 3, 4, 5, 4 } });

        movesWhite.Add(new int[,] { { 5, 4, 3, 4 } });
        movesWhite.Add(new int[,] { { 5, 4, 4, 4 } });
        movesWhite.Add(new int[,] { { 5, 4, 6, 4 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 3 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 4 } });
        movesWhite.Add(new int[,] { { 7, 4, 6, 5 } });
        movesWhite.Add(new int[,] { { 7, 4, 7, 3 } });
        movesWhite.Add(new int[,] { { 7, 4, 7, 5 } });

        _engine.FindPossibleMoves(Color.White, _board);
        CompareTwoList(movesWhite, _engine.GetPossibelMoves());
        _engine.FindPossibleMoves(Color.Black, _board);
        CompareTwoList(movesBlack, _engine.GetPossibelMoves());
    }
    [TestMethod]
    [TestCategory("Board")]
    public void MakeAMoveTest()
    {
        //fekete bábú léptetése
        _engine.StartingBoardPostion();
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 1).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 1).GetPieceValue(), 2);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 2).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 2).GetPieceValue(), 0);
        _board.MakeAMoveWithAPiece(2, 2, _board.GetPieceInfoFromTheBoard(0, 1));
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 1).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 1).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 2).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 2).GetPieceValue(), 2);
        //fehér bábú
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 4).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 4).GetPieceValue(), 0);
        _board.MakeAMoveWithAPiece(5, 4, _board.GetPieceInfoFromTheBoard(6, 4));
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 4).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 4).GetPieceValue(), 1);
    }
    [TestMethod]
    [TestCategory("Board")]
    public void DeleteAllGhostSquareTest()
    {
        _board.PutPieceOnTheBoard(2, 3, new Ghost(Color.Ghost, 2, 3, false));
        _board.PutPieceOnTheBoard(2, 5, new Ghost(Color.Ghost, 2, 5, false));
        _board.PutPieceOnTheBoard(5, 3, new Ghost(Color.Ghost, 5, 3, false));
        _board.PutPieceOnTheBoard(5, 5, new Ghost(Color.Ghost, 5, 5, false));
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 3).GetColor(), Color.Ghost);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 5).GetColor(), Color.Ghost);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 5).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 3).GetColor(), Color.Ghost);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 5).GetColor(), Color.Ghost);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 5).GetPieceValue(), 0);
        _board.DeleteAllGhostSquare();
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 3).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 5).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(2, 5).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 3).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 3).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 5).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(5, 5).GetPieceValue(), 0);
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsSquareUnderAttackTest()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(5, 7, new Pawn(Color.Black, 5, 7, true));
        Assert.IsTrue(_board.IsSquareUnderAttack(6, 0, Color.White));
        Assert.IsTrue(_board.IsSquareUnderAttack(5, 0, Color.White));
        Assert.IsFalse(_board.IsSquareUnderAttack(5, 1, Color.White));
        Assert.IsFalse(_board.IsSquareUnderAttack(5, 1, Color.Black));
        Assert.IsFalse(_board.IsSquareUnderAttack(0, 0, Color.Black));
        Assert.IsFalse(_board.IsSquareUnderAttack(5, 7, Color.Black));
        Assert.IsFalse(_board.IsSquareUnderAttack(6, 7, Color.White));
        _board.PutPieceOnTheBoard(4, 6, new Knight(Color.Black, 4, 6, true));
        Assert.IsTrue(_board.IsSquareUnderAttack(6, 7, Color.White));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsOurKingUnderAttackTest()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
        Assert.IsFalse(_board.IsOurKingUnderAttack(Color.White));
        Assert.IsFalse(_board.IsOurKingUnderAttack(Color.Black));
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.White, 0, 0, true));
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.Black, 7, 7, true));
        Assert.IsTrue(_board.IsOurKingUnderAttack(Color.White));
        Assert.IsTrue(_board.IsOurKingUnderAttack(Color.Black));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsGameInLateGameTest()
    {
        _board.PutPieceOnTheBoard(0, 4, new King(Color.Black, 0, 4, false));
        _board.PutPieceOnTheBoard(7, 4, new King(Color.White, 7, 4, false));
        _board.PutPieceOnTheBoard(0, 3, new Queen(Color.Black, 0, 3, false));
        _board.PutPieceOnTheBoard(7, 3, new Queen(Color.White, 7, 3, false));
        _board.IsGameInLateGame();
        Assert.IsFalse(_board.GetGameStatus());
        _board.MakeAMoveWithAPiece(0, 3, _board.GetPieceInfoFromTheBoard(7, 3));
        _board.IsGameInLateGame();
        Assert.IsTrue(_board.GetGameStatus());
    }
    [TestMethod]
    [TestCategory("Board")]
    public void EvalBoardTest()
    {
        _engine.StartingBoardPostion();
        Assert.AreEqual(_board.EvaluateBoard(), -322);
        _board.MakeAMoveWithAPiece(4, 4, _board.GetPieceInfoFromTheBoard(6, 4));
        Assert.AreEqual(_board.EvaluateBoard(), -160);
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsPawnIsolatedTest()
    {
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(6, 5, new Pawn(Color.White, 6, 5, false));
        _board.PutPieceOnTheBoard(6, 4, new Pawn(Color.White, 6, 4, false));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.White, 6, 2, false));
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        Assert.IsTrue(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(6, 7)));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(6, 5)));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(6, 4)));
        Assert.IsTrue(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(6, 2)));
        Assert.IsTrue(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(6, 0)));
        _board.PutPieceOnTheBoard(1, 7, new Pawn(Color.Black, 1, 7, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.Black, 1, 3, false));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.Black, 1, 0, false));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.Black, 1, 6, false));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(1, 7)));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(1, 6)));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(1, 3)));
        Assert.IsFalse(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(1, 2)));
        Assert.IsTrue(_board.IsPawnIsolated(_board.GetPieceInfoFromTheBoard(1, 0)));

    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsPawnAPasserTest()
    {
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(6, 5, new Pawn(Color.White, 6, 5, false));
        _board.PutPieceOnTheBoard(6, 4, new Pawn(Color.White, 6, 4, false));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.White, 6, 2, false));
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 7)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 5)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 4)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 2)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 0)));
        _board.PutPieceOnTheBoard(1, 7, new Pawn(Color.Black, 1, 7, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.Black, 1, 3, false));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.Black, 1, 0, false));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.Black, 1, 6, false));
        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 7)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 5)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 4)));
        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 2)));
        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(6, 0)));

        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(1, 7)));
        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(1, 2)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(1, 3)));
        Assert.IsFalse(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(1, 0)));
        Assert.IsTrue(_board.IsPawnAPasser(_board.GetPieceInfoFromTheBoard(1, 6)));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsPawnDoubledTest()
    {
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(6, 5, new Pawn(Color.White, 6, 5, false));
        _board.PutPieceOnTheBoard(6, 4, new Pawn(Color.White, 6, 4, false));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.White, 6, 2, false));
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 7)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 5)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 4)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 2)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 0)));
        _board.PutPieceOnTheBoard(5, 4, new Pawn(Color.White, 5, 4, true));
        _board.PutPieceOnTheBoard(5, 2, new Pawn(Color.White, 5, 2, true));
        _board.PutPieceOnTheBoard(5, 0, new Pawn(Color.White, 5, 0, true));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 7)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 5)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 4)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 2)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(6, 0)));
        _board.PutPieceOnTheBoard(1, 7, new Pawn(Color.Black, 1, 7, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.Black, 1, 3, false));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.Black, 1, 0, false));
        _board.PutPieceOnTheBoard(1, 6, new Pawn(Color.Black, 1, 6, false));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 7)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 2)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 3)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 0)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 6)));
        _board.PutPieceOnTheBoard(2, 2, new Pawn(Color.Black, 2, 2, true));
        _board.PutPieceOnTheBoard(2, 3, new Pawn(Color.Black, 2, 3, true));
        _board.PutPieceOnTheBoard(2, 0, new Pawn(Color.Black, 2, 0, true));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 7)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 2)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 3)));
        Assert.IsTrue(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 0)));
        Assert.IsFalse(_board.IsDoubledPawn(_board.GetPieceInfoFromTheBoard(1, 6)));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsRookOnOpenFileTest()
    {
        _board.PutPieceOnTheBoard(7, 7, new Rook(Color.White, 7, 7, false));
        _board.PutPieceOnTheBoard(7, 6, new Rook(Color.White, 7, 6, true));
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(0, 1, new Rook(Color.Black, 0, 1, true));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.Black, 1, 0, false));
        
        Assert.IsTrue(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(7, 6)));
        Assert.IsFalse(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(7, 7)));
        Assert.IsTrue(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(0, 1)));
        Assert.IsFalse(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(0, 0)));
        _board.PutPieceOnTheBoard(6, 6, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.Black, 1, 0, false));
        Assert.IsFalse(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(7, 6)));
        Assert.IsFalse(_board.IsRookOnOpenFile(_board.GetPieceInfoFromTheBoard(0, 1)));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void CanBeTakenWithPawnTest()
    {
        _board.PutPieceOnTheBoard(6, 7, new Pawn(Color.White, 6, 7, false));
        _board.PutPieceOnTheBoard(6, 2, new Pawn(Color.White, 6, 2, false));
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        _board.PutPieceOnTheBoard(1, 7, new Pawn(Color.Black, 1, 7, false));
        _board.PutPieceOnTheBoard(1, 2, new Pawn(Color.Black, 1, 2, false));
        _board.PutPieceOnTheBoard(1, 3, new Pawn(Color.Black, 1, 3, false));
        _board.PutPieceOnTheBoard(2, 4, new Knight(Color.White, 2, 4, true));
        _board.PutPieceOnTheBoard(2, 0, new Bishop(Color.White, 2, 0, true));
        _board.PutPieceOnTheBoard(5, 6, new Rook(Color.Black, 5, 6, true));
        _board.PutPieceOnTheBoard(4, 4, new Queen(Color.Black, 4, 4, true));

        Assert.IsTrue(_board.CanBeTakenWithPawn(_board.GetPieceInfoFromTheBoard(2, 4)));
        Assert.IsFalse(_board.CanBeTakenWithPawn(_board.GetPieceInfoFromTheBoard(2, 0)));
        Assert.IsTrue(_board.CanBeTakenWithPawn(_board.GetPieceInfoFromTheBoard(5, 6)));
        Assert.IsFalse(_board.CanBeTakenWithPawn(_board.GetPieceInfoFromTheBoard(4, 4)));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void CanBeTakenWithKnightTest()
    {
        _board.PutPieceOnTheBoard(2, 4, new Knight(Color.White, 2, 4, true));
        _board.PutPieceOnTheBoard(3, 4, new Knight(Color.Black, 3, 4, true));
        _board.PutPieceOnTheBoard(4, 5, new Bishop(Color.Black, 4, 5, true));
        _board.PutPieceOnTheBoard(3, 7, new Queen(Color.Black, 3, 7, true));
        _board.PutPieceOnTheBoard(4, 6, new Rook(Color.White, 4, 6, true));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.White, 1, 0, true));

        Assert.IsTrue(_board.CanBeTakenWithKnight(_board.GetPieceInfoFromTheBoard(4, 5)));
        Assert.IsFalse(_board.CanBeTakenWithKnight(_board.GetPieceInfoFromTheBoard(3, 7)));
        Assert.IsTrue(_board.CanBeTakenWithKnight(_board.GetPieceInfoFromTheBoard(4, 6)));
        Assert.IsFalse(_board.CanBeTakenWithKnight(_board.GetPieceInfoFromTheBoard(1, 0)));
    }
    [TestMethod]
    [TestCategory("Board")]
    public void CanBeTakenWithBishopTest()
    {
        _board.PutPieceOnTheBoard(4, 5, new Bishop(Color.White, 4, 5, true));
        _board.PutPieceOnTheBoard(4, 4, new Bishop(Color.Black, 4, 4, true));
        _board.PutPieceOnTheBoard(3, 4, new Knight(Color.Black, 3, 4, true));
        _board.PutPieceOnTheBoard(3, 7, new Queen(Color.Black, 3, 7, true));
        _board.PutPieceOnTheBoard(1, 1, new Pawn(Color.White, 1, 1, true));
        _board.PutPieceOnTheBoard(4, 6, new Rook(Color.White, 4, 6, true));
        _board.PutPieceOnTheBoard(2, 3, new Knight(Color.Black, 2, 3, true));

        Assert.IsTrue(_board.CanBeTakenWithBishop(_board.GetPieceInfoFromTheBoard(3, 4)));
        Assert.IsFalse(_board.CanBeTakenWithBishop(_board.GetPieceInfoFromTheBoard(3, 7)));
        Assert.IsTrue(_board.CanBeTakenWithBishop(_board.GetPieceInfoFromTheBoard(1, 1)));
        Assert.IsFalse(_board.CanBeTakenWithBishop(_board.GetPieceInfoFromTheBoard(4, 6)));
        Assert.IsFalse(_board.CanBeTakenWithBishop(_board.GetPieceInfoFromTheBoard(2, 3)));

    }
    [TestMethod]
    [TestCategory("Board")]
    public void CanBeTakenWithRookTest()
    {
        _board.PutPieceOnTheBoard(0, 0, new Rook(Color.Black, 0, 0, false));
        _board.PutPieceOnTheBoard(7, 0, new Rook(Color.White, 7, 0, false));
        _board.PutPieceOnTheBoard(7, 1, new Knight(Color.Black, 7, 1, true));
        _board.PutPieceOnTheBoard(7, 2, new Queen(Color.Black, 7, 2, true));
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.White, 1, 0, false));
        _board.PutPieceOnTheBoard(2, 0, new Bishop(Color.White, 2, 0, true));

        Assert.IsTrue(_board.CanBeTakenWithRook(_board.GetPieceInfoFromTheBoard(7, 1)));
        Assert.IsFalse(_board.CanBeTakenWithRook(_board.GetPieceInfoFromTheBoard(7, 2)));
        Assert.IsTrue(_board.CanBeTakenWithRook(_board.GetPieceInfoFromTheBoard(1, 0)));
        Assert.IsFalse(_board.CanBeTakenWithRook(_board.GetPieceInfoFromTheBoard(2, 0)));

    }
    [TestMethod]
    [TestCategory("Board")]
    public void isPeaceHangingTest()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        _board.PutPieceOnTheBoard(3, 0, new Pawn(Color.White, 3, 0, true));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(2, 1)), 1);
        _board.PutPieceOnTheBoard(1, 0, new Pawn(Color.Black, 1, 0, false));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(2, 1)), 0);

        _board.PutPieceOnTheBoard(5, 1, new Pawn(Color.White, 5, 1, true));
        _board.PutPieceOnTheBoard(4, 0, new Pawn(Color.Black, 4, 0, true));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(5, 1)), 1);
        _board.PutPieceOnTheBoard(6, 0, new Pawn(Color.White, 6, 0, false));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(5, 1)), 0);

        _board.PutPieceOnTheBoard(2, 6, new Pawn(Color.Black, 2, 6, true));
        _board.PutPieceOnTheBoard(3, 5, new Pawn(Color.White, 3, 5, true));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(3, 5)), 1);
        _board.PutPieceOnTheBoard(5, 4, new Knight(Color.White, 5, 4, true));
        Assert.AreEqual(_board.IsPieceHanging(_board.GetPieceInfoFromTheBoard(3, 5)), 0);
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsPieceInSmallMiddleTest()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        _board.PutPieceOnTheBoard(3, 3, new Pawn(Color.White, 3, 3, true));
        _board.PutPieceOnTheBoard(4, 4, new Knight(Color.White, 4, 4, true));
        _board.PutPieceOnTheBoard(5, 4, new Knight(Color.White, 5, 4, true));

        Assert.AreEqual(_board.IsPieceInTheSmallMiddle(_board.GetPieceInfoFromTheBoard(2, 1)), 0);
        Assert.AreEqual(_board.IsPieceInTheSmallMiddle(_board.GetPieceInfoFromTheBoard(3, 3)), 1);
        Assert.AreEqual(_board.IsPieceInTheSmallMiddle(_board.GetPieceInfoFromTheBoard(4, 4)), 1);
        Assert.AreEqual(_board.IsPieceInTheSmallMiddle(_board.GetPieceInfoFromTheBoard(5, 4)), 0);
    }
    [TestMethod]
    [TestCategory("Board")]
    public void IsPieceInBiggerMiddleTest()
    {
        _board.PutPieceOnTheBoard(2, 1, new Pawn(Color.Black, 2, 1, true));
        _board.PutPieceOnTheBoard(3, 3, new Pawn(Color.White, 3, 3, true));
        _board.PutPieceOnTheBoard(4, 4, new Knight(Color.White, 4, 4, true));
        _board.PutPieceOnTheBoard(5, 4, new Knight(Color.White, 5, 4, true));

        Assert.AreEqual(_board.IsPieceInTheBiggerMiddle(_board.GetPieceInfoFromTheBoard(2, 1)), 0);
        Assert.AreEqual(_board.IsPieceInTheBiggerMiddle(_board.GetPieceInfoFromTheBoard(3, 3)), 1);
        Assert.AreEqual(_board.IsPieceInTheBiggerMiddle(_board.GetPieceInfoFromTheBoard(4, 4)), 1);
        Assert.AreEqual(_board.IsPieceInTheBiggerMiddle(_board.GetPieceInfoFromTheBoard(5, 4)), 1);
    }

    [TestMethod]
    [TestCategory("Engine")]
    public void StartPostionTest()
    {
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 4).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 7).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 2).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 5).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 7).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 2).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 5).GetColor(), Color.Empty);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 4).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 7).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 2).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 5).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 7).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 2).GetPieceValue(), 0);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 5).GetPieceValue(), 0);
        _engine.StartingBoardPostion();
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 4).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 7).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 2).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 5).GetColor(), Color.Black);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 7).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 2).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 5).GetColor(), Color.White);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 4).GetPieceValue(), 6);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(0, 7).GetPieceValue(), 4);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 2).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(1, 5).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 7).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 2).GetPieceValue(), 3);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(7, 5).GetPieceValue(), 3);
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void CreateByteArrayFromBoardTest()
    {
        byte[,] testResult = new byte[64, 2] { 
            {10, 0 },{ 8, 0 }, { 9, 0 }, {11, 0 }, {12, 0 }, { 9, 0 }, { 8, 0 }, {10, 0 },
            { 7, 0 },{ 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 1, 0 },{ 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, 
            { 4, 0 },{ 2, 0 }, { 3, 0 }, { 5, 0 }, { 6, 0 }, { 3, 0 }, { 2, 0 }, { 4, 0 }
        };
        _engine.StartingBoardPostion();
        for(int i = 0; i<64;  i++)
        {
            for (int j=0; j<2; j++)
            {
                Assert.AreEqual(_engine.CreateByteArrayFromBoard(_board)[i,j], testResult[i,j]);
            }
        }
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void CreateBoardFromByteArrayTest()
    {
        _engine.StartingBoardPostion();
        byte[,] testResult = new byte[64, 2] {
            {10, 0 },{ 8, 0 }, { 9, 0 }, {11, 0 }, {12, 0 }, { 9, 0 }, { 8, 0 }, {10, 0 },
            { 7, 0 },{ 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 }, { 7, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 0, 0 },{ 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 },
            { 1, 0 },{ 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 },
            { 4, 0 },{ 2, 0 }, { 3, 0 }, { 5, 0 }, { 6, 0 }, { 3, 0 }, { 2, 0 }, { 4, 0 }
        };
        Board testBoard = _engine.CreateBoardFromByteArray(testResult);
        for(int i  = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                Assert.AreEqual(testBoard.GetPieceInfoFromTheBoard(i, j).GetPieceValue(), _board.GetPieceInfoFromTheBoard(i, j).GetPieceValue());
                Assert.AreEqual(testBoard.GetPieceInfoFromTheBoard(i, j).GetColor(), _board.GetPieceInfoFromTheBoard(i, j).GetColor());
                Assert.AreEqual(testBoard.GetPieceInfoFromTheBoard(i, j).GetPieceHasAlreadyMoved(), _board.GetPieceInfoFromTheBoard(i, j).GetPieceHasAlreadyMoved());
            }
        }

    }
    [TestMethod]
    [TestCategory("Engine")]
    public void SetUpRootForTreeTest()
    {
        _engine.StartingBoardPostion();
        _engine.SetUpRootForTree();
        Assert.IsFalse(_engine.GetFindRouteBool());
        Assert.AreEqual(_engine.GetNode().GetScore(), -322);
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void BuildUpTreeTest()
    {
        _engine.StartingBoardPostion();
        _engine.SetUpRootForTree();
        Assert.AreEqual(_engine.GetNode().GetChildren().Count, 0);
        _engine.BuildUpTree(_engine.GetNode(), _engine.GetDepth(), Color.White);
        Assert.AreNotEqual(_engine.GetNode().GetChildren().Count, 0);
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void SearchForBestMoveTest()
    {
        Node rootTmp = new Node(1,new List<int> {},0,new byte[,] {},1,1,1,1);
        Node bigparent1 = new Node(1, new List<int> {1 }, 0, new byte[,] { }, 1, 2, 1, 1);
        Node bigparent2 = new Node(2, new List<int> {2 }, 0, new byte[,] { }, 1, 1, 2, 1);
        Node smallparent1 = new Node(1, new List<int> {1,1 }, 0, new byte[,] { }, 1, 1, 1, 2);
        Node smallparent2 = new Node(2, new List<int> {1,2 }, 0, new byte[,] { }, 1, 3, 1, 1);
        Node smallparent3 = new Node(1, new List<int> {2,1 }, 0, new byte[,] { }, 1, 1, 3, 1);
        Node smallparent4 = new Node(2, new List<int> {2,2 }, 0, new byte[,] { }, 1, 1, 1, 3);
        Node leaf1 = new Node(1, new List<int> {1,1,1 }, 3, new byte[,] { }, 1, 4, 1, 1);
        Node leaf2 = new Node(2, new List<int> {1,1,2 }, 5, new byte[,] { }, 1, 1, 4, 1);
        Node leaf3 = new Node(1, new List<int> {1,2,1 }, 6, new byte[,] { }, 1, 1, 1, 4);
        Node leaf4 = new Node(2, new List<int> {1,2,2 }, 9, new byte[,] { }, 1, 5, 1, 1);
        Node leaf5 = new Node(1, new List<int> {2,1,1 }, 1, new byte[,] { }, 1, 1, 5, 1);
        Node leaf6 = new Node(2, new List<int> {2,1,2 }, 2, new byte[,] { }, 1, 1, 1, 5);
        Node leaf7 = new Node(1, new List<int> {2,2,2 }, 0, new byte[,] { }, 1, 6, 1, 1);
        Node leaf8 = new Node(2, new List<int> {2,2,2 }, -1, new byte[,] { }, 1, 1, 6, 1);
        smallparent1.AddChild(leaf1);
        smallparent1.AddChild(leaf2);
        smallparent2.AddChild(leaf3);
        smallparent2.AddChild(leaf4);
        smallparent3.AddChild(leaf5);
        smallparent3.AddChild(leaf6);
        smallparent4.AddChild(leaf7);
        smallparent4.AddChild(leaf8);
        bigparent1.AddChild(smallparent1);
        bigparent1.AddChild(smallparent2);
        bigparent2.AddChild(smallparent3);
        bigparent2.AddChild(smallparent4);
        rootTmp.AddChild(bigparent1);
        rootTmp.AddChild(bigparent2);

        Assert.AreEqual(_engine.SearchForBestMove(rootTmp, 3, -100, 100, true),5);
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void FindNodeThatHasBeenChosenToMovePreTest()
    {
        Node rootTmp = new Node(1, new List<int> { }, 0, new byte[,] { }, 1, 1, 1, 1);
        Node bigparent1 = new Node(1, new List<int> { 0 }, 0, new byte[,] { }, 1, 2, 1, 1);
        Node bigparent2 = new Node(2, new List<int> { 1 }, 0, new byte[,] { }, 1, 1, 2, 1);
        Node smallparent1 = new Node(1, new List<int> { 0, 0 }, 0, new byte[,] { }, 1, 1, 1, 2);
        Node smallparent2 = new Node(2, new List<int> { 0, 1 }, 0, new byte[,] { }, 1, 3, 1, 1);
        Node smallparent3 = new Node(1, new List<int> { 1, 0 }, 0, new byte[,] { }, 1, 1, 3, 1);
        Node smallparent4 = new Node(2, new List<int> { 1, 1 }, 0, new byte[,] { }, 1, 1, 1, 3);
        Node leaf1 = new Node(1, new List<int> { 0, 0, 0 }, 3, new byte[,] { }, 1, 4, 1, 1);
        Node leaf2 = new Node(2, new List<int> { 0, 0, 1 }, 5, new byte[,] { }, 1, 1, 4, 1);
        Node leaf3 = new Node(1, new List<int> { 0, 1, 0 }, 6, new byte[,] { }, 1, 1, 1, 4);
        Node leaf4 = new Node(2, new List<int> { 0, 1, 1 }, 9, new byte[,] { }, 1, 5, 1, 1);
        Node leaf5 = new Node(1, new List<int> { 1, 0, 0 }, 1, new byte[,] { }, 1, 1, 5, 1);
        Node leaf6 = new Node(2, new List<int> { 1, 0, 1 }, 2, new byte[,] { }, 1, 1, 1, 5);
        Node leaf7 = new Node(1, new List<int> { 1, 1, 0 }, 0, new byte[,] { }, 1, 6, 1, 1);
        Node leaf8 = new Node(2, new List<int> { 1, 1, 1 }, -1, new byte[,] { }, 1, 1, 6, 1);
        smallparent1.AddChild(leaf1);
        smallparent1.AddChild(leaf2);
        smallparent2.AddChild(leaf3);
        smallparent2.AddChild(leaf4);
        smallparent3.AddChild(leaf5);
        smallparent3.AddChild(leaf6);
        smallparent4.AddChild(leaf7);
        smallparent4.AddChild(leaf8);
        bigparent1.AddChild(smallparent1);
        bigparent1.AddChild(smallparent2);
        bigparent2.AddChild(smallparent3);
        bigparent2.AddChild(smallparent4);
        rootTmp.AddChild(bigparent1);
        rootTmp.AddChild(bigparent2);
        _engine.FindNodeThatHasBeenChosenToMovePre(rootTmp, 5, 3);
        CompareTwoList(_engine.GetFindRoute(), new List<int> { 0 });

    }
    [TestMethod]
    [TestCategory("Engine")]
    public void FindNodeThatHasBeenChosenToMoveTest()
    {
        Node rootTmp = new Node(1, new List<int> { }, 0, new byte[,] { }, 1, 1, 1, 1);
        Node bigparent1 = new Node(1, new List<int> { 0 }, 0, new byte[,] { }, 1, 2, 1, 1);
        Node bigparent2 = new Node(2, new List<int> { 1 }, 0, new byte[,] { }, 1, 1, 2, 1);
        Node smallparent1 = new Node(1, new List<int> { 0, 0 }, 0, new byte[,] { }, 1, 1, 1, 2);
        Node smallparent2 = new Node(2, new List<int> { 0, 1 }, 0, new byte[,] { }, 1, 3, 1, 1);
        Node smallparent3 = new Node(1, new List<int> { 1, 0 }, 0, new byte[,] { }, 1, 1, 3, 1);
        Node smallparent4 = new Node(2, new List<int> { 1, 1 }, 0, new byte[,] { }, 1, 1, 1, 3);
        Node leaf1 = new Node(1, new List<int> { 0, 0, 0 }, 3, new byte[,] { }, 1, 4, 1, 1);
        Node leaf2 = new Node(2, new List<int> { 0, 0, 1 }, 5, new byte[,] { }, 1, 1, 4, 1);
        Node leaf3 = new Node(1, new List<int> { 0, 1, 0 }, 6, new byte[,] { }, 1, 1, 1, 4);
        Node leaf4 = new Node(2, new List<int> { 0, 1, 1 }, 9, new byte[,] { }, 1, 5, 1, 1);
        Node leaf5 = new Node(1, new List<int> { 1, 0, 0 }, 1, new byte[,] { }, 1, 1, 5, 1);
        Node leaf6 = new Node(2, new List<int> { 1, 0, 1 }, 2, new byte[,] { }, 1, 1, 1, 5);
        Node leaf7 = new Node(1, new List<int> { 1, 1, 0 }, 0, new byte[,] { }, 1, 6, 1, 1);
        Node leaf8 = new Node(2, new List<int> { 1, 1, 1 }, -1, new byte[,] { }, 1, 1, 6, 1);
        smallparent1.AddChild(leaf1);
        smallparent1.AddChild(leaf2);
        smallparent2.AddChild(leaf3);
        smallparent2.AddChild(leaf4);
        smallparent3.AddChild(leaf5);
        smallparent3.AddChild(leaf6);
        smallparent4.AddChild(leaf7);
        smallparent4.AddChild(leaf8);
        bigparent1.AddChild(smallparent1);
        bigparent1.AddChild(smallparent2);
        bigparent2.AddChild(smallparent3);
        bigparent2.AddChild(smallparent4);
        rootTmp.AddChild(bigparent1);
        rootTmp.AddChild(bigparent2);
        _engine.SetFindRoute(new List<int> { });
        _engine.FindNodeThatHasBeenChosenToMove(rootTmp, 5, 3);
        CompareTwoList(_engine.GetFindRoute(), new List<int> { 0 });
    }
    [TestMethod]
    [TestCategory("Engine")]
    public void GetBestMoveCoordinatesTest()
    {
        Node rootTmp = new Node(1, new List<int> { }, 0, new byte[,] { }, 1, 1, 1, 1);
        Node bigparent1 = new Node(1, new List<int> { 0 }, 0, new byte[,] { }, 1, 2, 1, 1);
        Node bigparent2 = new Node(2, new List<int> { 1 }, 0, new byte[,] { }, 1, 1, 2, 1);
        Node smallparent1 = new Node(1, new List<int> { 0, 0 }, 0, new byte[,] { }, 1, 1, 1, 2);
        Node smallparent2 = new Node(2, new List<int> { 0, 1 }, 0, new byte[,] { }, 1, 3, 1, 1);
        Node smallparent3 = new Node(1, new List<int> { 1, 0 }, 0, new byte[,] { }, 1, 1, 3, 1);
        Node smallparent4 = new Node(2, new List<int> { 1, 1 }, 0, new byte[,] { }, 1, 1, 1, 3);
        Node leaf1 = new Node(1, new List<int> { 0, 0, 0 }, 3, new byte[,] { }, 1, 4, 1, 1);
        Node leaf2 = new Node(2, new List<int> { 0, 0, 1 }, 5, new byte[,] { }, 1, 1, 4, 1);
        Node leaf3 = new Node(1, new List<int> { 0, 1, 0 }, 6, new byte[,] { }, 1, 1, 1, 4);
        Node leaf4 = new Node(2, new List<int> { 0, 1, 1 }, 9, new byte[,] { }, 1, 5, 1, 1);
        Node leaf5 = new Node(1, new List<int> { 1, 0, 0 }, 1, new byte[,] { }, 1, 1, 5, 1);
        Node leaf6 = new Node(2, new List<int> { 1, 0, 1 }, 2, new byte[,] { }, 1, 1, 1, 5);
        Node leaf7 = new Node(1, new List<int> { 1, 1, 0 }, 0, new byte[,] { }, 1, 6, 1, 1);
        Node leaf8 = new Node(2, new List<int> { 1, 1, 1 }, -1, new byte[,] { }, 1, 1, 6, 1);
        smallparent1.AddChild(leaf1);
        smallparent1.AddChild(leaf2);
        smallparent2.AddChild(leaf3);
        smallparent2.AddChild(leaf4);
        smallparent3.AddChild(leaf5);
        smallparent3.AddChild(leaf6);
        smallparent4.AddChild(leaf7);
        smallparent4.AddChild(leaf8);
        bigparent1.AddChild(smallparent1);
        bigparent1.AddChild(smallparent2);
        bigparent2.AddChild(smallparent3);
        bigparent2.AddChild(smallparent4);
        rootTmp.AddChild(bigparent1);
        rootTmp.AddChild(bigparent2);
        _engine.FindNodeThatHasBeenChosenToMovePre(rootTmp, 5, 3);
        CompareTwoList(_engine.GetFindRoute(), new List<int> { 0 });

        int[] result = _engine.GetBestMoveCoordinates(rootTmp);
        Assert.AreEqual(result[0], 1);
        Assert.AreEqual(result[1], 2);
        Assert.AreEqual(result[2], 1);
        Assert.AreEqual(result[3], 1);
    }
    [TestMethod]
    [TestCategory("UciControl")]
    public void GetIntFromCharTest()
    {
        Assert.AreEqual(_uciControl.GetIntFromChar('r'), 4);
        Assert.AreEqual(_uciControl.GetIntFromChar('2'), 6);
    }
    [TestMethod]
    [TestCategory("UciControl")]
    public void GetIntFromCharForPromotionTest()
    {
        Assert.AreEqual(_uciControl.GetIntFromCharForPromotion('r'), 4);
        Assert.AreEqual(_uciControl.GetIntFromCharForPromotion('q'), 5);
    }
    [TestMethod]
    [TestCategory("UciControl")]
    public void MakeAMoveFromIntArrayTest()
    {
        _engine.StartingBoardPostion();
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(6, 4).GetPieceValue(), 1);
        Assert.AreEqual(_board.GetPieceInfoFromTheBoard(4, 4).GetPieceValue(), 0);
        _uciControl.MakeAMoveFromIntArray(new int[] { 6, 4, 4, 4 });

    }
}
