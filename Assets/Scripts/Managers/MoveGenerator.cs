using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class MoveGenerator
{
    private static bool _inCheck;
    static List<int> allowedSquares, attackedSquares;

    public static List<int> GenerateLegal(Board board, int pieceIndex, bool check)
    {
        _inCheck = check;

        bool isPinned = false;
        int pinnedDir = 0;

        allowedSquares = new List<int>();
        attackedSquares = new List<int>();
        List<int> pinMoveCalc = new List<int>();

        int piece = board.square[pieceIndex];
        int pieceType = Piece.PieceType(piece);
        int pieceColor = Piece.Color(piece);

        if (board.pinnedPieces.Contains(pieceIndex))
        {
            isPinned = true;
            int pinIndex = board.pinnedPieces.IndexOf(pieceIndex);
            pinnedDir = board.pinnedDirection[pinIndex];
        }

        if (pieceType == Piece.Pawn)
        {
            allowedSquares = GeneratePseudoLegal.GeneratePawnPseudoLegal(board, pieceIndex, pieceColor);
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
            tempSquares = GeneratePseudoLegal.GenerateKingPseudoLegal(board, pieceIndex, pieceColor);
            attackedSquares = GeneratePseudoLegal.GetAttackedSquares();
            
            allowedSquares = tempSquares.Except(board.attackedSquares).ToList();
        }
        else if (pieceType == Piece.Bishop)
        {
            allowedSquares = GeneratePseudoLegal.GenerateBishopPseudoLegal(board, pieceIndex, pieceColor);
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
            allowedSquares = GeneratePseudoLegal.GenerateRookPseudoLegal(board, pieceIndex, pieceColor);
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
            allowedSquares = GeneratePseudoLegal.GenerateQueenPseudoLegal(board, pieceIndex, pieceColor);
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
            allowedSquares = GeneratePseudoLegal.GenerateKnightPseudoLegal(board, pieceIndex, pieceColor);
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

    public static bool IsSquareFree(Board board, int squareIndex)
    {
        if (squareIndex > 63 || squareIndex < 0)
            return false;

        if (board.square[squareIndex] == 0)
            return true;
        else
            return false;        
    }

    public static bool IsEnemySquare(Board board, int squareIndex, int pieceColor)
    {
        if (squareIndex > 63 || squareIndex < 0)
            return false;
        
        if (pieceColor == Piece.White)
        {
            if (Piece.IsColor(board.square[squareIndex], Piece.Black))
                return true;
            else
                return false;
        } else
        {
            if (Piece.IsColor(board.square[squareIndex], Piece.White))
                return true;
            else
                return false;
        }            
    }
}
