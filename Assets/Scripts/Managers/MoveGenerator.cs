using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class MoveGenerator
{
    private static bool _inCheck;

    public static void GenerateLegal(Board board, int pieceIndex, bool check, bool[] allowedMoves)
    {
        _inCheck = check;

        bool isPinned = false;
        int pinnedDir = 0;

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
            GeneratePseudoLegal.GeneratePawnPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);

            if (isPinned)
            {
                bool[] pinMoveCalc = new bool[64];

                if (pieceColor == Piece.White)
                {
                    pinMoveCalc[pieceIndex + pinnedDir] = true;
                    if (pieceIndex / 8 == 1)
                        pinMoveCalc[pieceIndex + (2 * pinnedDir)] = true;
                }
                else
                {
                    pinMoveCalc[pieceIndex - pinnedDir] = true;
                    if (pieceIndex / 8 == 6)
                        pinMoveCalc[pieceIndex - (2 * pinnedDir)] = true;
                }

                for (int i = 0; i < 64; i++)
                {
                    if (!(pinMoveCalc[i] == true && allowedMoves[i] == true))
                        allowedMoves[i] = false;
                }
            }
        }
        else if (pieceType == Piece.King)
        {
            GeneratePseudoLegal.GenerateKingPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);

            for (int i = 0; i < 64; i++)
            {
                if (board.attackedSquares[i])
                    allowedMoves[i] = false;
            }   
        }
        else if (pieceType == Piece.Bishop)
        {
            GeneratePseudoLegal.GenerateBishopPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);

            if (isPinned)
            {
                bool[] pinMoveCalc = new bool[64];

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
                    pinMoveCalc[posSquare] = true;

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc[posSquare] = true;
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
                    pinMoveCalc[negSquare] = true;

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc[negSquare] = true;
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }
                    
                    negSquare -= posIterator;
                }

                for (int i = 0; i < 64; i++)
                {
                    if (!(pinMoveCalc[i] == true && allowedMoves[i] == true))
                        allowedMoves[i] = false;
                }
            }
        }
        else if (pieceType == Piece.Rook)
        {
            GeneratePseudoLegal.GenerateRookPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);

            if (isPinned)
            {
                bool[] pinMoveCalc = new bool[64];

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
                    pinMoveCalc[posSquare] = true;

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc[posSquare] = true;
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
                    pinMoveCalc[negSquare] = true;

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc[negSquare] = true;
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }

                    negSquare -= posIterator;
                }

                for (int i = 0; i < 64; i++)
                {
                    if (!(pinMoveCalc[i] == true && allowedMoves[i] == true))
                        allowedMoves[i] = false;
                }
            }
        }
        else if (pieceType == Piece.Queen)
        {
            GeneratePseudoLegal.GenerateQueenPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);

            if (isPinned)
            {
                bool[] pinMoveCalc = new bool[64];

                int posPinnedDir = pinnedDir;
                int negPinnedDir = -1 * posPinnedDir;
                int posIterator = posPinnedDir;

                int posSquare = posPinnedDir + pieceIndex;
                int negSquare = negPinnedDir + pieceIndex;

                while (posSquare <= 63)
                {
                    pinMoveCalc[posSquare] = true;

                    if (IsEnemySquare(board, posSquare, pieceColor))
                    {
                        pinMoveCalc[posSquare] = true;
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
                    pinMoveCalc[negSquare] = true;

                    if (IsEnemySquare(board, negSquare, pieceColor))
                    {
                        pinMoveCalc[negSquare] = true;
                        break;
                    }
                    else if (!IsSquareFree(board, negSquare))
                    {
                        break;
                    }
                    
                    negSquare -= posIterator;
                }
                
                for (int i = 0; i < 64; i++)
                {
                    if (!(pinMoveCalc[i] == true && allowedMoves[i] == true))
                        allowedMoves[i] = false;
                }
            }
        }
        else if (pieceType == Piece.Knight)
        {
            if (!isPinned)
            {
                GeneratePseudoLegal.GenerateKnightPseudoLegal(board, pieceIndex, pieceColor, allowedMoves);
            }
        }
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
