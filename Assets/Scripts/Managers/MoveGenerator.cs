using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class MoveGenerator
{
    private static bool _inCheck;
    static List<int> allowedSquares;

    public static List<int> GenerateLegal(Board board, int pieceIndex, bool check)
    {
        _inCheck = check;

        bool isPinned = false;
        int pinnedDir = 0;

        allowedSquares = new List<int>();

        int piece = board.square[pieceIndex];
        int pieceType = Piece.PieceType(piece);
        int pieceColor = Piece.Color(piece);

        if (board.pinnedPieces.Count != 0)
        {
            if (board.pinnedPieces.Contains(pieceIndex))
            {
                isPinned = true;
                int pinIndex = board.pinnedPieces.IndexOf(pieceIndex);
                pinnedDir = board.pinnedDirection[pinIndex];
            }
        }

        if (pieceType == Piece.Pawn)
        {
            allowedSquares = GeneratePseudoLegal.GeneratePawnPseudoLegal(board, pieceIndex, pieceColor);

            if (isPinned)
            {
                List<int> pinMoveCalc = new List<int>();

                pinMoveCalc.Add(pieceIndex + pinnedDir);
                pinMoveCalc.Add(pieceIndex - pinnedDir);

                if (pinnedDir == 8)
                {
                    pinMoveCalc.Add(pieceIndex + 2 * pinnedDir);
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.King)
        {
            List<int> tempSquares = new List<int>();
            tempSquares = GeneratePseudoLegal.GenerateKingPseudoLegal(board, pieceIndex, pieceColor);

            for (int i = 0; i < 64; i++)
            {
                if (board.attackedSquares[i])
                    tempSquares.Remove(i);
            }

            allowedSquares = tempSquares;    
        
        }
        else if (pieceType == Piece.Bishop)
        {
            allowedSquares = GeneratePseudoLegal.GenerateBishopPseudoLegal(board, pieceIndex, pieceColor);

            if (isPinned)
            {
                List<int> pinMoveCalc = new List<int>();

                bool canMove = true;

                if (pinnedDir == 1 || pinnedDir == 8)
                    canMove = false;
                
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63 && canMove)
                {
                    pinMoveCalc.Add(posSquare);

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc.Add(posSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, posSquare))
                    {
                        break;
                    }
                    
                    posSquare += posIterator;
                }

                while (negSquare >= 0 && canMove)
                {
                    pinMoveCalc.Add(negSquare);

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc.Add(negSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }
                    
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Rook)
        {
            allowedSquares = GeneratePseudoLegal.GenerateRookPseudoLegal(board, pieceIndex, pieceColor);

            if (isPinned)
            {
                List<int> pinMoveCalc = new List<int>();

                bool canMove = true;

                if (pinnedDir == 7 || pinnedDir == 9)
                    canMove = false;
                
                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63 && canMove)
                {
                    pinMoveCalc.Add(posSquare);

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc.Add(posSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, posSquare))
                    {
                        break;
                    }

                    posSquare += posIterator;
                }

                while (negSquare >= 0 && canMove)
                {
                    pinMoveCalc.Add(negSquare);

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc.Add(negSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }

                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Queen)
        {
            allowedSquares = GeneratePseudoLegal.GenerateQueenPseudoLegal(board, pieceIndex, pieceColor);

            if (isPinned)
            {
                List<int> pinMoveCalc = new List<int>();

                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc.Add(posSquare);

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc.Add(posSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, posSquare))
                    {
                        break;
                    }
                    
                    posSquare += posIterator;
                }

                while (negSquare >= 0)
                {
                    pinMoveCalc.Add(negSquare);

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc.Add(negSquare);
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }
                    
                    negSquare -= posIterator;
                }

                allowedSquares = Enumerable.Intersect(allowedSquares, pinMoveCalc).ToList();
            }
        }
        else if (pieceType == Piece.Knight)
        {

            if (!isPinned)
            {
                allowedSquares = GeneratePseudoLegal.GenerateKnightPseudoLegal(board, pieceIndex, pieceColor);
            }
        }

        return allowedSquares;
    }

    public static void GenerateAttackedSquares(Board board, int pieceIndex, bool[] attacked)
    {
        int piece = board.square[pieceIndex];
        int pieceType = Piece.PieceType(piece);

        if (pieceType == Piece.Pawn)
        {
            GeneratePseudoLegal.GeneratePawnAttacked(board, pieceIndex, attacked);
        }
        else if (pieceType == Piece.King)
        {
            GeneratePseudoLegal.GenerateKingAttacked(board, pieceIndex, attacked);
        }
        else if (pieceType == Piece.Bishop)
        {
            GeneratePseudoLegal.GenerateBishopAttacked(board, pieceIndex, attacked);
        }
        else if (pieceType == Piece.Rook)
        {
            GeneratePseudoLegal.GenerateRookAttacked(board, pieceIndex, attacked);
        }
        else if (pieceType == Piece.Queen)
        {
            GeneratePseudoLegal.GenerateQueenAttacked(board, pieceIndex, attacked);
        }
        else
        {
            GeneratePseudoLegal.GenerateKnightAttacked(board, pieceIndex, attacked);
        }
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
