using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Board
{
    public int plyCount, fiftyMoveCounter, whiteIndex, blackIndex, epFile;
    public int[] square, kingSquares;
    public bool whiteToMove, whiteCastleKingside, whiteCastleQueenside, blackCastleKingside, blackCastleQueenside;
    public bool currentPlayerInCheck, checkmate;

    public List<List<int>> allowedMoves;
    public List<int> pieceList, attackedSquares, pinnedPieces, pinnedDirection;

    public Board(int ep, int[] squares, bool whiteMove, bool whiteKingCastle, bool whiteQueenCastle, bool blackKingCastle, bool blackQueenCastle)
    {
        whiteIndex = 0;
        blackIndex = 1;

        square = new int[64];
        pieceList = new List<int>();
        kingSquares = new int[2];

        pinnedPieces = new List<int>();
        pinnedDirection = new List<int>();

        allowedMoves = new List<List<int>>();
        attackedSquares = new List<int>();

        squares.CopyTo(square, 0);

        int index = 0;
        foreach (int piece in squares)
        {
            int pieceType = Piece.PieceType(piece);
            int pieceColor = Piece.Color(piece);

            if (piece != 0)
            {
                pieceList.Add(index);
            }

            if (pieceType == Piece.King)
            {
                int kingColor = pieceColor == Piece.White? whiteIndex: blackIndex;

                kingSquares[kingColor] = index;
            }

            index++;
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
            if (attackedSquares.Count == 0)
            {
                GenerateAllAttackedSquares();
            }

            for (int i = 0; i < length; i++)
            {
                if (pieceList[i] == -1)
                    continue;
                   
                List<int> allowedSquares = MoveGenerator.GenerateLegal(this, pieceList[i], currentPlayerInCheck);
                allowedMoves.Add(allowedSquares.GetRange(0, allowedSquares.Count));
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
            
            List<int> allowedSquares = MoveGenerator.GenerateLegal(this, pieceList[i], currentPlayerInCheck);
            allowedMoves[i] = allowedSquares.GetRange(0, allowedSquares.Count);
        }

        if (currentPlayerInCheck)
        {
            RemoveIllegalMoves();
        }
    }

    public void GenerateAllAttackedSquares()
    {
        int length = pieceList.Count;

        List<int> tempAttackedSquares = new List<int>();
        attackedSquares = new List<int>();

        for (int i = 0; i < length; i++)
        {
            if (pieceList[i] == -1)
                continue;

            if (whiteToMove != Piece.IsColor(square[pieceList[i]], Piece.White))
            {   
                MoveGenerator.GenerateAttackedSquares(this, pieceList[i]);
                tempAttackedSquares = MoveGenerator.GetAttackedSquares();

                attackedSquares = Enumerable.Union(attackedSquares, tempAttackedSquares).ToList();
            }
        }
    }

    public void RemoveIllegalMoves()
    {
        int[] tempSquare = new int[64];
        square.CopyTo(tempSquare, 0);
        bool tempCheck = currentPlayerInCheck;

        List<List<int>> tempAllowed = new List<List<int>>();
        List<int> tempMoves = new List<int>();

        foreach (List<int> moveList in allowedMoves)
        {
            int pieceListIndex = allowedMoves.IndexOf(moveList);

            if (pieceList[pieceListIndex] == -1)
                continue;
            
            int piece = square[pieceList[pieceListIndex]];
            int pieceIndex = pieceList[pieceListIndex];

            if (whiteToMove != Piece.IsColor(piece, Piece.White))
            {
                continue;
            }

            int pieceType = Piece.PieceType(piece);
            int kingColor = Piece.IsColor(piece, Piece.White)? whiteIndex : blackIndex;

            foreach (int move in moveList)
            {
                int newIndex;

                if (move >= 100)
                {
                    newIndex = move % 100;
                }
                else
                {
                    newIndex = move;
                }

                square[newIndex] = piece;
                square[pieceList[pieceListIndex]] = 0;
                int oldKingSquare = kingSquares[kingColor];

                int pieceListOppIndex = pieceList.IndexOf(newIndex);
                pieceList[pieceListIndex] = newIndex;
                if (pieceListOppIndex != -1)
                {
                    pieceList[pieceListOppIndex] = -1;
                }

                if (pieceType == Piece.King)
                    kingSquares[kingColor] = move;

                GenerateAllAttackedSquares();
                IsInCheck();

                if (currentPlayerInCheck)
                {
                    tempMoves.Add(move);
                }

                tempSquare.CopyTo(square, 0);
                kingSquares[kingColor] = oldKingSquare;
                pieceList[pieceListIndex] = pieceIndex;
                if (pieceListOppIndex != -1)
                {
                    pieceList[pieceListOppIndex] = newIndex;
                }
            }

            tempAllowed.Add(tempMoves);
            tempMoves = new List<int>();
        }

        int tempCounter = 0;

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

            allowedMoves[pieceListIndex] = allowedMoves[pieceListIndex].Except(tempAllowed[tempCounter]).ToList();

            tempCounter++;
        }

        currentPlayerInCheck = tempCheck;
    }

    public void IsInCheck()
    {
        if (whiteToMove)
        {
            if (attackedSquares.Contains(kingSquares[whiteIndex]))
                currentPlayerInCheck = true;
            else
                currentPlayerInCheck = false;
        }
        else
        {
            if (attackedSquares.Contains(kingSquares[blackIndex]))
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
                else if (pieceIndex == 4 &&newIndex == 2 && square[0] == 14)
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
            pieceList[pieceListIndex] = newIndex;

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

            foreach (List<int> moveList in allowedMoves)
            {
                if (pieceList[allowedMoves.IndexOf(moveList)] == -1)
                    continue;
                if (whiteToMove != Piece.IsColor(square[pieceList[allowedMoves.IndexOf(moveList)]], Piece.White))
                    continue;
                
                if (moveList.Count != 0)
                {
                    checkmate = false;
                    break;
                }
            }
        }
    }
}
