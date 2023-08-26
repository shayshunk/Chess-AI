using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratePseudoLegal
{
    private static List<int> _allowedSquares, _attackedSquares;

    public static void ResetLists()
    {
        _allowedSquares = new List<int>();
        _attackedSquares = new List<int>();
    }

    public static List<int> GeneratePawnPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();
        
        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (pieceColor == Piece.White)
        {
            int squareUp = pieceIndex + 8;
            bool moveOne = MoveGenerator.IsSquareFree(board, squareUp);

            if (file == board.epFile - 1)
            {
                if (rank == 4)
                {                    
                    int squareRight = pieceIndex + 1;
                    int pieceRight = board.square[squareRight];
                    int pieceRightType = Piece.PieceType(pieceRight);
                    int pieceRightColor = Piece.Color(pieceRight);
                    int piece = board.square[pieceIndex];

                    List<int> tempPieceList = new List<int>(board.pieceList);
                    List<int> tempAttacked = new List<int>(board.attackedSquares);
                    bool tempCheck = board.currentPlayerInCheck;
                    int[] tempSquares = new int[64];
                    board.square.CopyTo(tempSquares, 0);

                    int pieceListIndex = board.pieceList.IndexOf(pieceIndex);
                    int pieceListOppIndex = board.pieceList.IndexOf(squareRight);
                    board.pieceList[pieceListIndex] = squareRight + 8;
                    board.pieceList[pieceListOppIndex] = -1;
                    board.square[pieceIndex + 9] = piece;
                    board.square[pieceIndex] = 0;
                    board.square[squareRight] = 0;


                    board.GenerateAllAttackedSquares();
                    board.IsInCheck();

                    _attackedSquares = new List<int>();

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.Black)
                        {
                            _allowedSquares.Add(pieceIndex + 9);
                        }
                    }

                    tempSquares.CopyTo(board.square, 0);
                    board.pieceList = tempPieceList;
                    board.attackedSquares = tempAttacked;
                    board.currentPlayerInCheck = tempCheck;
                }
            }
            else if (file == board.epFile + 1)
            {
                if (rank == 4)
                {
                    int squareLeft = pieceIndex - 1;
                    int pieceLeft = board.square[squareLeft];
                    int pieceLeftType = Piece.PieceType(pieceLeft);
                    int pieceLeftColor = Piece.Color(pieceLeft);
                    int piece = board.square[pieceIndex];

                    List<int> tempPieceList = new List<int>(board.pieceList);
                    List<int> tempAttacked = new List<int>(board.attackedSquares);
                    bool tempCheck = board.currentPlayerInCheck;
                    int[] tempSquares = new int[64];
                    board.square.CopyTo(tempSquares, 0);

                    int pieceListIndex = board.pieceList.IndexOf(pieceIndex);
                    int pieceListOppIndex = board.pieceList.IndexOf(squareLeft);
                    board.pieceList[pieceListIndex] = squareLeft + 8;
                    board.pieceList[pieceListOppIndex] = -1;
                    board.square[pieceIndex + 7] = piece;
                    board.square[pieceIndex] = 0;
                    board.square[squareLeft] = 0;


                    board.GenerateAllAttackedSquares();
                    board.IsInCheck();

                    _attackedSquares = new List<int>();

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.Black)
                        {
                            _allowedSquares.Add(pieceIndex + 9);
                        }
                    }

                    tempSquares.CopyTo(board.square, 0);
                    board.pieceList = tempPieceList;
                    board.attackedSquares = tempAttacked;
                    board.currentPlayerInCheck = tempCheck;
                }
            }
            if (moveOne)
            {

                if (squareUp / 8 == 7)
                {
                    _allowedSquares.Add(100 + squareUp);
                    _allowedSquares.Add(200 + squareUp);
                    _allowedSquares.Add(300 + squareUp);
                    _allowedSquares.Add(400 + squareUp);
                }
                else
                {
                    _allowedSquares.Add(squareUp);
                }
            }
            if (rank == 1)
            {
                int squareUpTwo = pieceIndex + 16;
                bool moveTwo = MoveGenerator.IsSquareFree(board, squareUpTwo);
                if (moveTwo && moveOne)
                {
                    _allowedSquares.Add(squareUpTwo);
                }
            }
            if (file != 0)
            {
                int squareUpLeft = pieceIndex + 7;
                if (MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor))
                {

                    if (squareUpLeft / 8 == 7)
                    {
                        _allowedSquares.Add(100 + squareUpLeft);
                        _allowedSquares.Add(200 + squareUpLeft);
                        _allowedSquares.Add(300 + squareUpLeft);
                        _allowedSquares.Add(400 + squareUpLeft);
                    }
                    else
                    {
                        _allowedSquares.Add(squareUpLeft);
                    }
                }
            }
            if (file != 7)
            {
                int squareUpRight = pieceIndex + 9;
                if (MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor))
                {

                    if (squareUpRight / 8 == 7)
                    {
                        _allowedSquares.Add(100 + squareUpRight);
                        _allowedSquares.Add(200 + squareUpRight);
                        _allowedSquares.Add(300 + squareUpRight);
                        _allowedSquares.Add(400 + squareUpRight);
                    }
                    else
                    {
                        _allowedSquares.Add(squareUpRight);
                    }
                }
            }
        } else
        {
            if (file == board.epFile - 1)
            {
                if (rank == 3)
                {
                    int squareRight = pieceIndex + 1;
                    int pieceRight = board.square[squareRight];
                    int pieceRightType = Piece.PieceType(pieceRight);
                    int pieceRightColor = Piece.Color(pieceRight);
                    int piece = board.square[pieceIndex];

                    List<int> tempPieceList = new List<int>(board.pieceList);
                    List<int> tempAttacked = new List<int>(board.attackedSquares);
                    bool tempCheck = board.currentPlayerInCheck;
                    int[] tempSquares = new int[64];
                    board.square.CopyTo(tempSquares, 0);

                    int pieceListIndex = board.pieceList.IndexOf(pieceIndex);
                    int pieceListOppIndex = board.pieceList.IndexOf(squareRight);
                    board.pieceList[pieceListIndex] = squareRight - 8;
                    board.pieceList[pieceListOppIndex] = -1;
                    board.square[pieceIndex - 7] = piece;
                    board.square[pieceIndex] = 0;
                    board.square[squareRight] = 0;

                    board.GenerateAllAttackedSquares();
                    board.IsInCheck();

                    ResetLists();

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.White)
                        {
                            _allowedSquares.Add(pieceIndex - 7);
                        }
                    }

                    tempSquares.CopyTo(board.square, 0);
                    board.pieceList = tempPieceList;
                    board.attackedSquares = tempAttacked;
                    board.currentPlayerInCheck = tempCheck;
                }
            }
            else if (file == board.epFile + 1)
            {
                if (rank == 3)
                {
                    int squareLeft = pieceIndex - 1;
                    int pieceLeft = board.square[squareLeft];
                    int pieceLeftType = Piece.PieceType(pieceLeft);
                    int pieceLeftColor = Piece.Color(pieceLeft);
                    int piece = board.square[pieceIndex];

                    List<int> tempPieceList = new List<int>(board.pieceList);
                    List<int> tempAttacked = new List<int>(board.attackedSquares);
                    bool tempCheck = board.currentPlayerInCheck;
                    int[] tempSquares = new int[64];
                    board.square.CopyTo(tempSquares, 0);

                    int pieceListIndex = board.pieceList.IndexOf(pieceIndex);
                    int pieceListOppIndex = board.pieceList.IndexOf(squareLeft);
                    board.pieceList[pieceListIndex] = squareLeft - 8;
                    board.pieceList[pieceListOppIndex] = -1;
                    board.square[pieceIndex - 9] = piece;
                    board.square[pieceIndex] = 0;
                    board.square[squareLeft] = 0;

                    board.GenerateAllAttackedSquares();
                    board.IsInCheck();

                    ResetLists();

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.White)
                        {
                            _allowedSquares.Add(pieceIndex - 9);
                        }
                    }

                    tempSquares.CopyTo(board.square, 0);
                    board.pieceList = tempPieceList;
                    board.attackedSquares = tempAttacked;
                    board.currentPlayerInCheck = tempCheck;
                }
            }

            int squareDown = pieceIndex - 8;

            bool moveOne = MoveGenerator.IsSquareFree(board, squareDown);
            if (moveOne)
            {
                if (squareDown / 8 == 0)
                {
                    _allowedSquares.Add(100 + squareDown);
                    _allowedSquares.Add(200 + squareDown);
                    _allowedSquares.Add(300 + squareDown);
                    _allowedSquares.Add(400 + squareDown);
                }
                else
                {
                    _allowedSquares.Add(squareDown);
                }
            }
            
            if (rank == 6)
            {
                bool moveTwo = MoveGenerator.IsSquareFree(board, pieceIndex - 16);
                if (moveTwo && moveOne)
                {
                    _allowedSquares.Add(pieceIndex - 16);
                }
            }

            if (file != 7)
            {
                int squareDownRight = pieceIndex - 7;

                if (MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor))
                {

                    if (squareDownRight / 8 == 0)
                    {
                        _allowedSquares.Add(100 + squareDownRight);
                        _allowedSquares.Add(200 + squareDownRight);
                        _allowedSquares.Add(300 + squareDownRight);
                        _allowedSquares.Add(400 + squareDownRight);
                    }
                    else
                    {
                        _allowedSquares.Add(squareDownRight);
                    }
                }
            }
            
            if (file != 0)
            {
                int squareDownLeft = pieceIndex - 9;

                if (MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor))
                {
                    if (squareDownLeft / 8 == 0)
                    {
                        _allowedSquares.Add(100 + squareDownLeft);
                        _allowedSquares.Add(200 + squareDownLeft);
                        _allowedSquares.Add(300 + squareDownLeft);
                        _allowedSquares.Add(400 + squareDownLeft);
                    }
                    else
                    {
                        _allowedSquares.Add(squareDownLeft);
                    }
                }
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateKingPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();

        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        bool moveUp = false, moveDown = false, moveUpRight = false, moveUpLeft = false, moveDownRight = false, moveDownLeft = false, moveLeft = false, moveRight = false;

        if (rank != 7)
        {
            moveUp = MoveGenerator.IsSquareFree(board, pieceIndex + 8) || MoveGenerator.IsEnemySquare(board, pieceIndex + 8, pieceColor);
            
            if (file != 7)
            {
                moveUpRight = MoveGenerator.IsSquareFree(board, pieceIndex + 9) || MoveGenerator.IsEnemySquare(board, pieceIndex + 9, pieceColor);
            }
                
            if (file != 0)
            {
                moveUpLeft = MoveGenerator.IsSquareFree(board, pieceIndex + 7) || MoveGenerator.IsEnemySquare(board, pieceIndex + 7, pieceColor);
            }
                
            
            if (moveUp)
                _allowedSquares.Add(pieceIndex + 8);
            if (moveUpLeft)
                _allowedSquares.Add(pieceIndex + 7);
            if (moveUpRight)
                _allowedSquares.Add(pieceIndex + 9);
        }
        if (rank != 0)
        {
            moveDown = MoveGenerator.IsSquareFree(board, pieceIndex - 8) || MoveGenerator.IsEnemySquare(board, pieceIndex - 8, pieceColor);

            if (file != 7)
            {
                moveDownRight = MoveGenerator.IsSquareFree(board, pieceIndex - 7) || MoveGenerator.IsEnemySquare(board, pieceIndex - 7, pieceColor);
            }
            if (file != 0)
            {
                moveDownLeft = MoveGenerator.IsSquareFree(board, pieceIndex - 9) || MoveGenerator.IsEnemySquare(board, pieceIndex - 9, pieceColor);
            }
            
            if (moveDown)
                _allowedSquares.Add(pieceIndex - 8);
            if (moveDownLeft)
                _allowedSquares.Add(pieceIndex - 9);
            if (moveDownRight)
                _allowedSquares.Add(pieceIndex - 7);
        }
        if (file != 0)
        {
            moveLeft = MoveGenerator.IsSquareFree(board, pieceIndex - 1) || MoveGenerator.IsEnemySquare(board, pieceIndex - 1, pieceColor);

            if (moveLeft)
                _allowedSquares.Add(pieceIndex - 1);
        }
        if (file != 7)
        {
            moveRight = MoveGenerator.IsSquareFree(board, pieceIndex + 1) || MoveGenerator.IsEnemySquare(board, pieceIndex + 1, pieceColor);

            if (moveRight)
                _allowedSquares.Add(pieceIndex + 1);
        }

        if (pieceColor == Piece.White)
        {
            if (board.whiteCastleKingside && !board.currentPlayerInCheck)
            {
                bool castleKingSide = MoveGenerator.IsSquareFree(board, pieceIndex + 1) && MoveGenerator.IsSquareFree(board, pieceIndex + 2);

                if (board.attackedSquares.Contains(pieceIndex + 1) || board.attackedSquares.Contains(pieceIndex + 2))
                    castleKingSide = false;

                if (castleKingSide)
                {
                    _allowedSquares.Add(pieceIndex + 2);
                }
            }
            if (board.whiteCastleQueenside && !board.currentPlayerInCheck)
            {
                bool castleQueenSide = MoveGenerator.IsSquareFree(board, pieceIndex - 1) && MoveGenerator.IsSquareFree(board, pieceIndex - 2) && MoveGenerator.IsSquareFree(board, pieceIndex - 3);

                if (board.attackedSquares.Contains(pieceIndex - 1) || board.attackedSquares.Contains(pieceIndex - 2))
                    castleQueenSide = false;

                if (castleQueenSide)
                {
                    _allowedSquares.Add(pieceIndex - 2);
                }
            }
        }
        else
        {
            if (board.blackCastleKingside && !board.currentPlayerInCheck)
            {
                bool castleKingSide = MoveGenerator.IsSquareFree(board, pieceIndex + 1) && MoveGenerator.IsSquareFree(board, pieceIndex + 2);

                if (board.attackedSquares.Contains(pieceIndex + 1) || board.attackedSquares.Contains(pieceIndex + 2))
                    castleKingSide = false;

                if (castleKingSide)
                {
                    _allowedSquares.Add(pieceIndex + 2);
                }
            }
            if (board.blackCastleQueenside && !board.currentPlayerInCheck)
            {
                bool castleQueenSide = MoveGenerator.IsSquareFree(board, pieceIndex - 1) && MoveGenerator.IsSquareFree(board, pieceIndex - 2) && MoveGenerator.IsSquareFree(board, pieceIndex - 3);

                if (board.attackedSquares.Contains(pieceIndex - 1) || board.attackedSquares.Contains(pieceIndex - 2))
                    castleQueenSide = false;
                
                if (castleQueenSide)
                {
                    _allowedSquares.Add(pieceIndex - 2);
                }
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateBishopPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();

        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
        bool enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
        bool enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
        bool enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);

        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            if (enemyUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                break;
            }

            bool moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            if (moveUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                squareUpRight += 9;
                enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            if (enemyUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                break;
            }

            bool moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            if (moveUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                squareUpLeft += 7;
                enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
            }
            else
            {
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            if (enemyDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                break;
            }

            bool moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            if (moveDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                squareDownRight -= 7;
                enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            else if (enemyDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                break;
            }

            bool moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            if (moveDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                squareDownLeft -= 9;
                enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);    
            } 
            else
            {
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateRookPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        ResetLists();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;

        bool enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
        bool enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
        bool enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);
        bool enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);

        while (squareUp <= 63)
        {   
            if (enemyUp)
            {
                _allowedSquares.Add(squareUp);
                break;
            }

            bool moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            if (moveUp)
            {
                _allowedSquares.Add(squareUp);
                squareUp += 8;
                enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            if (enemyLeft)
            {
                _allowedSquares.Add(squareLeft);
                break;
            }

            bool moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            if (moveLeft)
            {
                _allowedSquares.Add(squareLeft);
                squareLeft -= 1;
                enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            if (enemyRight)
            {
                _allowedSquares.Add(squareRight);
                break;
            }

            bool moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            if (moveRight)
            {
                _allowedSquares.Add(squareRight);
                squareRight += 1;
                enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDown >= 0)
        {            
            if (enemyDown)
            {
                _allowedSquares.Add(squareDown);
                break;
            }
            
            bool moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            if (moveDown)
            {
                _allowedSquares.Add(squareDown);
                squareDown -= 8;
                enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);
            } 
            else
            {
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateQueenPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;
        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
        bool enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
        bool enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);
        bool enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);
        bool enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
        bool enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
        bool enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
        bool enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);

        while (squareUp <= 63)
        {   
            if (enemyUp)
            {
                _allowedSquares.Add(squareUp);
                break;
            }

            bool moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            if (moveUp)
            {
                _allowedSquares.Add(squareUp);
                squareUp += 8;
                enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            if (enemyLeft)
            {
                _allowedSquares.Add(squareLeft);
                break;
            }

            bool moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            if (moveLeft)
            {
                _allowedSquares.Add(squareLeft);
                squareLeft -= 1;
                enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            if (enemyRight)
            {
                _allowedSquares.Add(squareRight);
                break;
            }

            bool moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            if (moveRight)
            {
                _allowedSquares.Add(squareRight);
                squareRight += 1;
                enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDown >= 0)
        {            
            if (enemyDown)
            {
                _allowedSquares.Add(squareDown);
                break;
            }
            
            bool moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            if (moveDown)
            {
                _allowedSquares.Add(squareDown);
                squareDown -= 8;
                enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            if (enemyUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                break;
            }

            bool moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            if (moveUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                squareUpRight += 9;
                enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            if (enemyUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                break;
            }

            bool moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            if (moveUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                squareUpLeft += 7;
                enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
            }
            else
            {
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            if (enemyDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                break;
            }

            bool moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            if (moveDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                squareDownRight -= 7;
                enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            else if (enemyDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                break;
            }

            bool moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            if (moveDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                squareDownLeft -= 9;
                enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);    
            } 
            else
            {
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateKnightPseudoLegal(Board board, int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();

        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        int squareUpUpRight = pieceIndex + 17;
        int squareUpRightRight = pieceIndex + 10;
        int squareUpUpLeft = pieceIndex + 15;
        int squareUpLeftLeft = pieceIndex + 6;
        int squareDownDownRight = pieceIndex - 15;
        int squareDownRightRight = pieceIndex - 6;
        int squareDownDownLeft = pieceIndex - 17;
        int squareDownLeftLeft = pieceIndex - 10;

        bool moveUpUpRight = false, enemyUpUpRight = false;
        bool moveUpRightRight = false, enemyUpRightRight = false;
        bool moveUpUpLeft = false, enemyUpUpLeft = false;
        bool moveUpLeftLeft = false, enemyUpLeftLeft = false;
        bool moveDownDownRight = false, enemyDownDownRight = false;
        bool moveDownRightRight = false, enemyDownRightRight = false;
        bool moveDownDownLeft = false, enemyDownDownLeft = false;
        bool moveDownLeftLeft = false, enemyDownLeftLeft = false;
   
        if (rank < 6 && file < 7)
        {
            moveUpUpRight = MoveGenerator.IsSquareFree(board, squareUpUpRight);
            enemyUpUpRight = MoveGenerator.IsEnemySquare(board, squareUpUpRight, pieceColor);

            if (moveUpUpRight || enemyUpUpRight)
                _allowedSquares.Add(squareUpUpRight);

        }
        if (rank < 7 && file < 6)
        {
            moveUpRightRight = MoveGenerator.IsSquareFree(board, squareUpRightRight);
            enemyUpRightRight = MoveGenerator.IsEnemySquare(board, squareUpRightRight, pieceColor);

            if (moveUpRightRight || enemyUpRightRight)
                _allowedSquares.Add(squareUpRightRight);
        }
        if (rank < 6 && file > 0)
        {
            moveUpUpLeft = MoveGenerator.IsSquareFree(board, squareUpUpLeft);
            enemyUpUpLeft = MoveGenerator.IsEnemySquare(board, squareUpUpLeft, pieceColor);
            
            if (moveUpUpLeft || enemyUpUpLeft)
                _allowedSquares.Add(squareUpUpLeft);
        }
        if (rank < 7 && file > 1)
        {
            moveUpLeftLeft = MoveGenerator.IsSquareFree(board, squareUpLeftLeft);
            enemyUpLeftLeft = MoveGenerator.IsEnemySquare(board, squareUpLeftLeft, pieceColor);

            if (moveUpLeftLeft || enemyUpLeftLeft)
                _allowedSquares.Add(squareUpLeftLeft);
        }
        if (rank > 1 && file > 0)
        {
            moveDownDownLeft = MoveGenerator.IsSquareFree(board, squareDownDownLeft);
            enemyDownDownLeft = MoveGenerator.IsEnemySquare(board, squareDownDownLeft, pieceColor);

            if (moveDownDownLeft || enemyDownDownLeft)
                _allowedSquares.Add(squareDownDownLeft);
        }
        if (rank > 0 && file > 1)
        {
            moveDownLeftLeft = MoveGenerator.IsSquareFree(board, squareDownLeftLeft);
            enemyDownLeftLeft = MoveGenerator.IsEnemySquare(board, squareDownLeftLeft, pieceColor);

            if (moveDownLeftLeft || enemyDownLeftLeft)
                _allowedSquares.Add(squareDownLeftLeft);
        }
        if (rank > 1 && file < 7)
        {
            moveDownDownRight = MoveGenerator.IsSquareFree(board, squareDownDownRight);
            enemyDownDownRight = MoveGenerator.IsEnemySquare(board, squareDownDownRight, pieceColor);

            if (moveDownDownRight || enemyDownDownRight)
                _allowedSquares.Add(squareDownDownRight);
        }
        if (rank > 0 && file < 6)
        {
            moveDownRightRight = MoveGenerator.IsSquareFree(board, squareDownRightRight);
            enemyDownRightRight = MoveGenerator.IsEnemySquare(board, squareDownRightRight, pieceColor);

            if (moveDownRightRight || enemyDownRightRight)
                _allowedSquares.Add(squareDownRightRight);
        }

        return _allowedSquares;
    }

    public static void GeneratePawnAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int pieceColor = Piece.Color(board.square[pieceIndex]);
        
        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (pieceColor == Piece.White)
        {
            _attackedSquares.Add(pieceIndex + 7);
            _attackedSquares.Add(pieceIndex + 9);

        } else
        {
            _attackedSquares.Add(pieceIndex - 7);
            _attackedSquares.Add(pieceIndex - 9);
        }
    }

    public static void GenerateKingAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (rank != 7)
        {
            _attackedSquares.Add(pieceIndex + 8);
            
            if (file != 7)
            {
                _attackedSquares.Add(pieceIndex + 9);
                _attackedSquares.Add(pieceIndex + 1);
            }   
            if (file != 0)
            {
                _attackedSquares.Add(pieceIndex + 7);
                _attackedSquares.Add(pieceIndex - 1);
            }
        }
        if (rank != 0)
        {
            _attackedSquares.Add(pieceIndex - 8);

            if (file != 7)
            {
                _attackedSquares.Add(pieceIndex - 7);
                _attackedSquares.Add(pieceIndex + 1);
            }
            if (file != 0)
            {
                _attackedSquares.Add(pieceIndex - 9);
                _attackedSquares.Add(pieceIndex - 1);
            }
        }
    }

    public static void GenerateBishopAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool moveUpRight = false;
        bool moveUpLeft = false;
        bool moveDownRight = false;
        bool moveDownLeft = false;

        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            
            if (moveUpRight)
            {
                _attackedSquares.Add(squareUpRight);
                squareUpRight += 9;
            } 
            else
            {
                _attackedSquares.Add(squareUpRight);
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            
            if (moveUpLeft)
            {
                _attackedSquares.Add(squareUpLeft);
                squareUpLeft += 7;
            } 
            else
            {
                _attackedSquares.Add(squareUpLeft);
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);

            if (moveDownRight)
            {
                _attackedSquares.Add(squareDownRight);
                squareDownRight -= 7;
            } 
            else
            {
                _attackedSquares.Add(squareDownRight);
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);

            if (moveDownLeft)
            {
                _attackedSquares.Add(squareDownLeft);
                squareDownLeft -= 9;
            } 
            else
            {
                _attackedSquares.Add(squareDownLeft);
                break;
            }
        }
    }

    public static void GenerateRookAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;

        bool moveUp = false;
        bool moveLeft = false;
        bool moveRight = false;
        bool moveDown = false;

        while (squareUp <= 63)
        {   
            moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            
            if (moveUp)
            {
                _attackedSquares.Add(squareUp);
                squareUp += 8;
            } 
            else
            {
                _attackedSquares.Add(squareUp);
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            
            if (moveLeft)
            {
                _attackedSquares.Add(squareLeft);
                squareLeft -= 1;
            } 
            else
            {
                _attackedSquares.Add(squareLeft);
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(board, squareRight);

            if (moveRight)
            {
                _attackedSquares.Add(squareRight);
                squareRight += 1;
            } 
            else
            {
                _attackedSquares.Add(squareRight);
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);

            if (moveDown)
            {
                _attackedSquares.Add(squareDown);
                squareDown -= 8;
            } 
            else
            {
                _attackedSquares.Add(squareDown);
                break;
            }
        }
    }

    public static void GenerateQueenAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;
        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool moveUp = false;
        bool moveLeft = false;
        bool moveRight = false;
        bool moveDown = false;
        bool moveUpRight = false;
        bool moveUpLeft = false;
        bool moveDownRight = false;
        bool moveDownLeft = false;

        while (squareUp <= 63)
        {   
            moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            
            if (moveUp)
            {
                _attackedSquares.Add(squareUp);
                squareUp += 8;
            } 
            else
            {
                _attackedSquares.Add(squareUp);
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            
            if (moveLeft)
            {
                _attackedSquares.Add(squareLeft);
                squareLeft -= 1;
            } 
            else
            {
                _attackedSquares.Add(squareLeft);
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(board, squareRight);

            if (moveRight)
            {
                _attackedSquares.Add(squareRight);
                squareRight += 1;
            } 
            else
            {
                _attackedSquares.Add(squareRight);
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);

            if (moveDown)
            {
                _attackedSquares.Add(squareDown);
                squareDown -= 8;
            } 
            else
            {
                _attackedSquares.Add(squareDown);
                break;
            }
        }
        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
    
            if (moveUpRight)
            {
                _attackedSquares.Add(squareUpRight);
                squareUpRight += 9;
            } 
            else
            {
                _attackedSquares.Add(squareUpRight);
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            
            if (moveUpLeft)
            {
                _attackedSquares.Add(squareUpLeft);
                squareUpLeft += 7;
            } 
            else
            {
                _attackedSquares.Add(squareUpLeft);
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);

            if (moveDownRight)
            {
                _attackedSquares.Add(squareDownRight);
                squareDownRight -= 7;
            } 
            else
            {
                _attackedSquares.Add(squareDownRight);
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);

            if (moveDownLeft)
            {
                _attackedSquares.Add(squareDownLeft);
                squareDownLeft -= 9;
            } 
            else
            {
                _attackedSquares.Add(squareDownLeft);
                break;
            }
        }
    }

    public static void GenerateKnightAttacked(Board board, int pieceIndex)
    {
        _attackedSquares = new List<int>();

        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        int squareUpUpRight = pieceIndex + 17;
        int squareUpRightRight = pieceIndex + 10;
        int squareUpUpLeft = pieceIndex + 15;
        int squareUpLeftLeft = pieceIndex + 6;
        int squareDownDownRight = pieceIndex - 15;
        int squareDownRightRight = pieceIndex - 6;
        int squareDownDownLeft = pieceIndex - 17;
        int squareDownLeftLeft = pieceIndex - 10;
   
        if (rank < 6 && file < 7)
        {
            _attackedSquares.Add(squareUpUpRight);
        }
        if (rank < 7 && file < 6)
        {
            _attackedSquares.Add(squareUpRightRight);
        }
        if (rank < 6 && file > 0)
        {
            _attackedSquares.Add(squareUpUpLeft);
        }
        if (rank < 7 && file > 1)
        {
            _attackedSquares.Add(squareUpLeftLeft);
        }
        if (rank > 1 && file > 0)
        {
            _attackedSquares.Add(squareDownDownLeft);
        }
        if (rank > 0 && file > 1)
        {
            _attackedSquares.Add(squareDownLeftLeft);
        }
        if (rank > 1 && file < 7)
        {
            _attackedSquares.Add(squareDownDownRight);
        }
        if (rank > 0 && file < 6)
        {
            _attackedSquares.Add(squareDownRightRight);
        }
    }

    public static List<int> GetAttackedSquares()
    {
        return _attackedSquares;
    }
}
