using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class MoveGenerator
{
    private static bool _inCheck, _causesCheck;
    static List<int> allowedSquares, attackedSquares;

    public static bool CausesCheck(int piece, int pieceIndex)
    {
        int pieceType = Piece.PieceType(piece);

        GenerateCheck.Reset();

        if (pieceType == Piece.Pawn)
        {
            _causesCheck = GenerateCheck.GeneratePawnCheck(piece, pieceIndex);
        }
        else if (pieceType == Piece.Bishop)
        {
            _causesCheck = GenerateCheck.GenerateBishopCheck(piece, pieceIndex);
        }
        else if (pieceType == Piece.Rook)
        {
            _causesCheck = GenerateCheck.GenerateRookCheck(piece, pieceIndex);
        }
        else if (pieceType == Piece.Queen)
        {
            _causesCheck = GenerateCheck.GenerateQueenCheck(piece, pieceIndex);
        }
        else if (pieceType == Piece.Knight)
        {
            _causesCheck = GenerateCheck.GenerateKnightCheck(piece, pieceIndex);
        }

        return _causesCheck;
    }

    public static List<int> GenerateLegal(int pieceIndex, bool check)
    {
        //_causesCheck = false;
        _inCheck = check;

        bool isPinned = false;
        int pinnedDir = 0;

        allowedSquares = new List<int>();
        attackedSquares = new List<int>();
        List<int> pinMoveCalc = new List<int>();

        int piece = BoardManager.Instance.square[pieceIndex];
        int pieceType = Piece.PieceType(piece);
        int pieceColor = Piece.Color(piece);

        if (BoardManager.Instance.pinnedPieces.Contains(pieceIndex))
        {
            isPinned = true;
            int pinIndex = BoardManager.Instance.pinnedPieces.IndexOf(pieceIndex);
            pinnedDir = BoardManager.Instance.pinnedDirection[pinIndex];
            Debug.Log("We have a pin!");
        }

        if (pieceType == Piece.Pawn)
        {
            allowedSquares = GeneratePseudoLegal.GeneratePawnPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();

            if (isPinned)
            {
                pinMoveCalc.Add(pinnedDir);

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.King)
        {
            List<int> tempSquares = new List<int>();
            tempSquares = GeneratePseudoLegal.GenerateKingPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();
            
            allowedSquares = tempSquares.Except(BoardManager.Instance.attackedSquares).ToList();
        }
        else if (pieceType == Piece.Bishop)
        {
            allowedSquares = GeneratePseudoLegal.GenerateBishopPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();

            if (isPinned)
            {
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc.Add(posSquare);
                    posSquare += posIterator;
                }

                while (negSquare >= 0)
                {
                    pinMoveCalc.Add(negSquare);
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Rook)
        {
            allowedSquares = GeneratePseudoLegal.GenerateRookPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();

            if (isPinned)
            {
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc.Add(posSquare);
                    posSquare += posIterator;
                }

                while (negSquare >= 0)
                {
                    pinMoveCalc.Add(negSquare);
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Queen)
        {
            allowedSquares = GeneratePseudoLegal.GenerateQueenPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();

            if (isPinned)
            {
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc.Add(posSquare);
                    posSquare += posIterator;
                }

                while (negSquare >= 0)
                {
                    pinMoveCalc.Add(negSquare);
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Knight)
        {
            allowedSquares = GeneratePseudoLegal.GenerateKnightPseudoLegal(pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();

            if (isPinned)
            {
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc.Add(posSquare);
                    posSquare += posIterator;
                }

                while (negSquare >= 0)
                {
                    pinMoveCalc.Add(negSquare);
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }

        return allowedSquares;
    }

    public static List<int> GetAttackedSquares()
    {
        return attackedSquares;
    }

    public static bool IsSquareFree(int squareIndex)
    {
        if (squareIndex > 63 || squareIndex < 0)
            return false;

        if (BoardManager.Instance.square[squareIndex] == 0)
            return true;
        else
            return false;        
    }

    public static bool IsEnemySquare(int squareIndex, int pieceColor)
    {
        if (squareIndex > 63 || squareIndex < 0)
            return false;
        
        if (pieceColor == Piece.White)
        {
            if (Piece.IsColor(BoardManager.Instance.square[squareIndex], Piece.Black))
                return true;
            else
                return false;
        } else
        {
            if (Piece.IsColor(BoardManager.Instance.square[squareIndex], Piece.White))
                return true;
            else
                return false;
        }            
    }
}
