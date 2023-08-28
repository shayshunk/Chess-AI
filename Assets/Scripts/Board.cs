using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class Board
{
    public int plyCount, fiftyMoveCounter, whiteIndex, blackIndex, epFile;
    public int[] square, kingSquares;
    public bool whiteToMove, whiteCastleKingside, whiteCastleQueenside, blackCastleKingside, blackCastleQueenside;
    public bool currentPlayerInCheck, checkmate;

    public List<bool[]> allowedMoves;
    public List<int> pieceList, pinnedPieces, pinnedDirection;
    public bool[] attackedSquares;

    public Board(int ep, int[] squares, bool whiteMove, bool whiteKingCastle, bool whiteQueenCastle, bool blackKingCastle, bool blackQueenCastle)
    {
        whiteIndex = 0;
        blackIndex = 1;

        square = new int[64];
        pieceList = new List<int>();
        kingSquares = new int[2];

        pinnedPieces = new List<int>();
        pinnedDirection = new List<int>();

        allowedMoves = new List<bool[]>();
        attackedSquares = new bool[64];

        squares.CopyTo(square, 0);

        for (int i = 0; i < 64; i++)
        {
            int piece = square[i];
            int pieceType = Piece.PieceType(piece);
            int pieceColor = Piece.Color(piece);

            if (piece != 0)
            {
                pieceList.Add(i);
            }

            if (pieceType == Piece.King)
            {
                int kingColor = (pieceColor == Piece.White? whiteIndex: blackIndex);
                kingSquares[kingColor] = i;
            }
        }

        whiteToMove = whiteMove;

        whiteCastleKingside = whiteKingCastle;
        whiteCastleQueenside = whiteQueenCastle;
        blackCastleKingside = blackKingCastle;
        blackCastleQueenside = blackQueenCastle;
        epFile = ep;

        PinHandler.GeneratePins(this);
        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
        {
            pinnedDirection = PinHandler.GetPinDirections();
        }

        GenerateAllAttackedSquares();
        IsInCheck();

        GenerateAllowedMoves();
    }

    public void GenerateAllowedMoves()
    {
        int length = pieceList.Count;

        if (allowedMoves.Count == 0)
        {
            GenerateAllAttackedSquares();

            for (int i = 0; i < length; i++)
            {
                if (pieceList[i] == -1)
                    continue;

                bool[] allowedSquares = new bool[64];   
                MoveGenerator.GenerateLegal(this, pieceList[i], currentPlayerInCheck, allowedSquares);
                allowedMoves.Add(allowedSquares);
            }

            if (currentPlayerInCheck)
            {
                RemoveIllegalMoves();
            }

            return;
        }

        for (int i = 0; i < length; i++)
        {
            if (pieceList[i] == -1)
                continue;
            
            if (whiteToMove != Piece.IsColor(square[pieceList[i]], Piece.White))
            {
                continue;
            }

            if (pieceList[i] == -1)
                continue;
            
            bool[] allowedSquares = new bool[64];   
            MoveGenerator.GenerateLegal(this, pieceList[i], currentPlayerInCheck, allowedSquares);
            allowedMoves[i] = allowedSquares;
        }

        if (currentPlayerInCheck)
        {
            RemoveIllegalMoves();
        }
    }

    public void GenerateAllAttackedSquares()
    {
        int length = pieceList.Count;

        attackedSquares = new bool[64];

        for (int i = 0; i < length; i++)
        {
            if (pieceList[i] == -1)
                continue;

            if (whiteToMove != Piece.IsColor(square[pieceList[i]], Piece.White))
            {   
                MoveGenerator.GenerateAttackedSquares(this, pieceList[i], attackedSquares);
            }
        }
    }

    public void RemoveIllegalMoves()
    {
        int[] tempSquare = new int[64];
        square.CopyTo(tempSquare, 0);
        bool tempCheck = currentPlayerInCheck;

        List<bool[]> tempAllowed = new List<bool[]>();
        bool[] tempMoves = new bool[64];

        foreach (bool[] moveList in allowedMoves)
        {
            int pieceListIndex = allowedMoves.IndexOf(moveList);

            if (pieceList[pieceListIndex] == -1)
            {
                tempAllowed.Add(tempMoves);
                tempMoves = new bool[64];
                continue;
            }
            
            int piece = square[pieceList[pieceListIndex]];
            int pieceIndex = pieceList[pieceListIndex];

            if (whiteToMove != Piece.IsColor(piece, Piece.White))
            {
                tempAllowed.Add(tempMoves);
                tempMoves = new bool[64];
                continue;
            }

            int pieceType = Piece.PieceType(piece);
            int kingColor = Piece.IsColor(piece, Piece.White)? whiteIndex : blackIndex;

            for (int i = 0; i < 64; i++)
            {
                if (moveList[i] == false)
                    continue;
                
                int newIndex = i;

                square[newIndex] = piece;
                square[pieceList[pieceListIndex]] = 0;
                int pieceListOppIndex = pieceList.IndexOf(newIndex);
                pieceList[pieceListIndex] = newIndex;

                if (pieceType == Piece.Pawn)
                {
                    if (newIndex % 8 == epFile)
                    {
                        if (kingColor == 0)
                        {
                            if (newIndex / 8 == 5)
                            {
                                square[newIndex - 8] = 0;
                                pieceListOppIndex = pieceList.IndexOf(newIndex - 8);
                            }
                        }
                        else
                        {
                            if (newIndex / 8 == 2)
                            {
                                square[newIndex + 8] = 0;
                                pieceListOppIndex = pieceList.IndexOf(newIndex + 8);
                            }
                        }
                    }
                }

                int oldKingSquare = kingSquares[kingColor];

                if (pieceListOppIndex != -1)
                {
                    pieceList[pieceListOppIndex] = -1;
                }

                if (pieceType == Piece.King)
                    kingSquares[kingColor] = newIndex;

                GenerateAllAttackedSquares();
                IsInCheck();

                if (currentPlayerInCheck)
                {
                    tempMoves[newIndex] = true;
                }

                tempSquare.CopyTo(square, 0);
                kingSquares[kingColor] = oldKingSquare;
                pieceList[pieceListIndex] = pieceIndex;
                if (pieceListOppIndex != -1)
                {
                    if (pieceType == Piece.Pawn && newIndex % 8 == epFile)
                    {
                        if (newIndex / 8 == 5)
                            pieceList[pieceListOppIndex] = newIndex - 8;
                        if (newIndex / 8 == 2)
                            pieceList[pieceListOppIndex] = newIndex + 8;
                    }
                    else
                    {
                        pieceList[pieceListOppIndex] = newIndex;
                    }
                }
            }

            tempAllowed.Add(tempMoves);
            tempMoves = new bool[64];
        }

        for (int i = 0; i < allowedMoves.Count; i++)
        {
            int pieceListIndex = i;

            if (pieceList[i] == -1)
                continue;

            int piece = square[pieceList[pieceListIndex]];

            if (whiteToMove != Piece.IsColor(piece, Piece.White))
            {
                continue;
            }

            for (int j = 0; j < 64; j++)
            {
                if (allowedMoves[pieceListIndex][j] == true && tempAllowed[pieceListIndex][j] == true)
                    allowedMoves[pieceListIndex][j] = false;
            }
        } 

        currentPlayerInCheck = tempCheck;
    }

    public void IsInCheck()
    {
        if (whiteToMove)
        {
            if (attackedSquares[kingSquares[whiteIndex]])
                currentPlayerInCheck = true;
            else
                currentPlayerInCheck = false;
        }
        else
        {
            if (attackedSquares[kingSquares[blackIndex]])
                currentPlayerInCheck = true;
            else
                currentPlayerInCheck = false;
        }
    }

    public void MakeMove(int pieceIndex, int newIndex)
    {
        int file = newIndex % 8;
        int rank = newIndex / 8;

        int startFile = pieceIndex % 8;
        int startRank = pieceIndex / 8;

        int piece = square[pieceIndex];
        int pieceListIndex = pieceList.IndexOf(pieceIndex);
        int takenPieceIndex = -1;
        
        if (newIndex >= 100)
        {
            newIndex = newIndex % 100;
        }
        
        if (square[newIndex] != 0)
        {
            takenPieceIndex = newIndex;
        }
        
        whiteToMove = !whiteToMove;

        pinnedPieces = new List<int>();
        pinnedDirection = new List<int>();
        
        int pieceColor = Piece.Color(piece);
        int pieceType = Piece.PieceType(piece);

        if (pieceType == Piece.Pawn) 
        {
            if (rank == 7 || rank == 0)
            {
                if (BoardManager.Instance.promotionIndex == 100)
                    pieceType = Piece.Queen;
                else if (BoardManager.Instance.promotionIndex == 200)
                    pieceType = Piece.Rook;
                else if (BoardManager.Instance.promotionIndex == 300)
                    pieceType = Piece.Bishop;
                else if (BoardManager.Instance.promotionIndex == 400)
                    pieceType = Piece.Knight;
            }
            
            if ((startFile == epFile + 1 || startFile == epFile - 1) && file == epFile)
            {
                if (startRank == 4 && pieceColor == Piece.White)
                {
                    takenPieceIndex = startRank * 8 + epFile;
                    square[startRank * 8 + epFile] = 0;
                }
                else if (startRank == 3 && pieceColor == Piece.Black)
                {
                    takenPieceIndex = startRank * 8 + epFile;
                    square[startRank * 8 + epFile] = 0;
                }
            }

            if ((startRank == 1 && rank == 3) || (startRank == 6 && rank == 4))
                epFile = file;
            else
                epFile = 88;

        }
        else
        {
            epFile = 88;
        }

        if (pieceType == Piece.King)
        {
            if (pieceColor == Piece.White)
            {
                kingSquares[whiteIndex] = newIndex;

                if (pieceIndex == 4 && newIndex == 6 && square[7] == 14)
                {
                    square[7] = 0;
                    square[5] = 14;
                    int oldRookIndex = pieceList.IndexOf(7);
                    pieceList[oldRookIndex] = 5;
                }
                else if (pieceIndex == 4 && newIndex == 2 && square[0] == 14)
                {
                    square[0] = 0;
                    square[3] = 14;
                    int oldRookIndex = pieceList.IndexOf(0);
                    pieceList[oldRookIndex] = 3;
                }

                whiteCastleKingside = false;
                whiteCastleQueenside = false;
            }
            else
            {
                kingSquares[blackIndex] = newIndex;

                if (pieceIndex == 60 && newIndex == 62 && square[63] == 22)
                {
                    square[63] = 0;
                    square[61] = 22;
                    int oldRookIndex = pieceList.IndexOf(63);
                    pieceList[oldRookIndex] = 61;
                }
                else if (pieceIndex == 60 && newIndex == 58 && square[56] == 22)
                {
                    square[56] = 0;
                    square[59] = 22;
                    int oldRookIndex = pieceList.IndexOf(56);
                    pieceList[oldRookIndex] = 59;
                }

                blackCastleKingside = false;
                blackCastleQueenside = false;
            }
        }

        else if (pieceType == Piece.Rook)
        {
            if (pieceIndex == 0)
                whiteCastleQueenside = false;
            else if (pieceIndex == 7)
                whiteCastleKingside = false;
            else if (pieceIndex == 56)
                blackCastleQueenside = false;
            else if (pieceIndex == 63)
                blackCastleKingside = false;
        }


        piece = pieceType | pieceColor;

        if (takenPieceIndex != -1)
        {
            int takenPieceListIndex = pieceList.IndexOf(takenPieceIndex);

            if (Piece.PieceType(square[pieceList[takenPieceListIndex]]) == Piece.Rook)
            {
                if (pieceList[takenPieceListIndex] == 0)
                    whiteCastleQueenside = false;
                else if (pieceList[takenPieceListIndex] == 7)
                    whiteCastleKingside = false;
                else if (pieceList[takenPieceListIndex] == 56)
                    blackCastleQueenside = false;
                else if (pieceList[takenPieceListIndex] == 63)
                    blackCastleKingside = false;
            }
            pieceList[takenPieceListIndex] = -1;
        }

        square[pieceIndex] = 0;
        square[newIndex] = piece;

        if (pieceListIndex != -1)
        {
            pieceList[pieceListIndex] = newIndex;
        }

        PinHandler.GeneratePins(this);
        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
        {
            pinnedDirection = PinHandler.GetPinDirections();
        }
        
        GenerateAllAttackedSquares();
        IsInCheck();

        GenerateAllowedMoves();

        plyCount++;

        if (currentPlayerInCheck)
        {
            checkmate = true;

            foreach (bool[] moveList in allowedMoves)
            {
                if (pieceList[allowedMoves.IndexOf(moveList)] == -1)
                    continue;
                if (whiteToMove != Piece.IsColor(square[pieceList[allowedMoves.IndexOf(moveList)]], Piece.White))
                    continue;
                
                for (int i = 0; i < 64; i++)
                {
                    if (moveList[i] == true)
                    {
                        checkmate = false;
                        break;
                    }
                }
            }
        }
    }
}
