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

    public static List<int> GeneratePawnPseudoLegal(int pieceIndex, int pieceColor)
    {
        ResetLists();
        
        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (pieceColor == Piece.White)
        {
            int squareUp = pieceIndex + 8;
            bool moveOne = MoveGenerator.IsSquareFree(squareUp);

            if (file == BoardManager.Instance.epFile - 1)
            {
                Debug.Log("Checking for EP at file: " + file);
                if (rank == 4)
                {
                    Debug.Log("Correct rank for EP.");

                    foreach (int square in _allowedSquares)
                    {
                        Debug.Log("Pawn can move to: " + square);
                    }
                    
                    int squareRight = pieceIndex + 1;
                    int pieceRight = BoardManager.Instance.square[squareRight];
                    int pieceRightType = Piece.PieceType(pieceRight);
                    int pieceRightColor = Piece.Color(pieceRight);

                    int[] tempSquares = new int[64];
                    bool tempCheck = BoardManager.Instance.currentPlayerInCheck;
                    int piece = BoardManager.Instance.square[pieceIndex];

                    BoardManager.Instance.square.CopyTo(tempSquares, 0);

                    BoardManager.Instance.square[pieceIndex + 9] = piece;
                    BoardManager.Instance.square[pieceIndex] = 0;
                    BoardManager.Instance.square[squareRight] = 0;

                    BoardManager.Instance.GenerateAllAttackedSquares();
                    BoardManager.Instance.IsInCheck();

                    ResetLists();

                    if (!BoardManager.Instance.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.Black)
                        {
                            _allowedSquares.Add(pieceIndex + 9);
                        }
                    }

                    BoardManager.Instance.currentPlayerInCheck = tempCheck;
                    tempSquares.CopyTo(BoardManager.Instance.square, 0);
                }
            }
            else if (file == BoardManager.Instance.epFile + 1)
            {
                if (rank == 4)
                {
                    int squareLeft = pieceIndex - 1;
                    int pieceLeft = BoardManager.Instance.square[squareLeft];
                    int pieceLeftType = Piece.PieceType(pieceLeft);
                    int pieceLeftColor = Piece.Color(pieceLeft);

                    int[] tempSquares = new int[64];
                    bool tempCheck = BoardManager.Instance.currentPlayerInCheck;
                    int piece = BoardManager.Instance.square[pieceIndex];

                    BoardManager.Instance.square.CopyTo(tempSquares, 0);

                    BoardManager.Instance.square[pieceIndex + 7] = piece;
                    BoardManager.Instance.square[pieceIndex] = 0;
                    BoardManager.Instance.square[squareLeft] = 0;

                    BoardManager.Instance.GenerateAllAttackedSquares();
                    BoardManager.Instance.IsInCheck();

                    ResetLists();

                    if (!BoardManager.Instance.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.Black)
                        {
                            _allowedSquares.Add(pieceIndex + 7);
                        }
                    }

                    BoardManager.Instance.currentPlayerInCheck = tempCheck;
                    tempSquares.CopyTo(BoardManager.Instance.square, 0);
                }
            }
            if (moveOne)
            {
                _allowedSquares.Add(squareUp);
            }
            if (rank == 1)
            {
                int squareUpTwo = pieceIndex + 16;
                bool moveTwo = MoveGenerator.IsSquareFree(squareUpTwo);
                if (moveTwo)
                {
                    _allowedSquares.Add(squareUpTwo);
                }
            }
            if (file != 0)
            {
                int squareUpLeft = pieceIndex + 7;
                _attackedSquares.Add(squareUpLeft);
                if (MoveGenerator.IsEnemySquare(squareUpLeft, pieceColor))
                {
                    _allowedSquares.Add(squareUpLeft);
                }
            }
            
            if (file != 7)
            {
                int squareUpRight = pieceIndex + 9;
                _attackedSquares.Add(squareUpRight);
                if (MoveGenerator.IsEnemySquare(squareUpRight, pieceColor))
                {
                    _allowedSquares.Add(squareUpRight);
                }
            }
        } else
        {
            if (file == BoardManager.Instance.epFile - 1)
            {
                if (rank == 3)
                {
                    int squareRight = pieceIndex + 1;
                    int pieceRight = BoardManager.Instance.square[squareRight];
                    int pieceRightType = Piece.PieceType(pieceRight);
                    int pieceRightColor = Piece.Color(pieceRight);

                    int[] tempSquares = new int[64];
                    bool tempCheck = BoardManager.Instance.currentPlayerInCheck;
                    int piece = BoardManager.Instance.square[pieceIndex];

                    BoardManager.Instance.square.CopyTo(tempSquares, 0);

                    BoardManager.Instance.square[pieceIndex - 7] = piece;
                    BoardManager.Instance.square[pieceIndex] = 0;
                    BoardManager.Instance.square[squareRight] = 0;

                    BoardManager.Instance.GenerateAllAttackedSquares();
                    BoardManager.Instance.IsInCheck();

                    ResetLists();

                    if (!BoardManager.Instance.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.White)
                        {
                            _allowedSquares.Add(pieceIndex - 7);
                        }
                    }

                    BoardManager.Instance.currentPlayerInCheck = tempCheck;
                    tempSquares.CopyTo(BoardManager.Instance.square, 0);
                }
            }
            else if (file == BoardManager.Instance.epFile + 1)
            {
                if (rank == 3)
                {
                    int squareLeft = pieceIndex - 1;
                    int pieceLeft = BoardManager.Instance.square[squareLeft];
                    int pieceLeftType = Piece.PieceType(pieceLeft);
                    int pieceLeftColor = Piece.Color(pieceLeft);

                    int[] tempSquares = new int[64];
                    bool tempCheck = BoardManager.Instance.currentPlayerInCheck;
                    int piece = BoardManager.Instance.square[pieceIndex];

                    BoardManager.Instance.square.CopyTo(tempSquares, 0);

                    BoardManager.Instance.square[pieceIndex - 9] = piece;
                    BoardManager.Instance.square[pieceIndex] = 0;
                    BoardManager.Instance.square[squareLeft] = 0;

                    BoardManager.Instance.GenerateAllAttackedSquares();
                    BoardManager.Instance.IsInCheck();

                    ResetLists();

                    if (!BoardManager.Instance.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.White)
                        {
                            _allowedSquares.Add(pieceIndex - 9);
                        }
                    }

                    BoardManager.Instance.currentPlayerInCheck = tempCheck;
                    tempSquares.CopyTo(BoardManager.Instance.square, 0);
                }
            }

            bool moveOne = MoveGenerator.IsSquareFree(pieceIndex - 8);
            if (moveOne)
            {
                _allowedSquares.Add(pieceIndex - 8);
            }
            
            if (rank == 6)
            {
                bool moveTwo = MoveGenerator.IsSquareFree(pieceIndex - 16);
                if (moveTwo)
                {
                    _allowedSquares.Add(pieceIndex - 16);
                }
            }

            if (file != 7)
            {
                int squareDownRight = pieceIndex - 7;
                _attackedSquares.Add(squareDownRight);
                if (MoveGenerator.IsEnemySquare(squareDownRight, pieceColor))
                {
                    _allowedSquares.Add(squareDownRight);
                }
            }
            
            if (file != 0)
            {
                int squareDownLeft = pieceIndex - 9;
                _attackedSquares.Add(squareDownLeft);
                if (MoveGenerator.IsEnemySquare(squareDownLeft, pieceColor))
                {
                    _allowedSquares.Add(squareDownLeft);
                }
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateKingPseudoLegal(int pieceIndex, int pieceColor)
    {
        ResetLists();

        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        bool moveUp = false, moveDown = false, moveUpRight = false, moveUpLeft = false, moveDownRight = false, moveDownLeft = false, moveLeft = false, moveRight = false;

        if (rank != 7)
        {
            moveUp = MoveGenerator.IsSquareFree(pieceIndex + 8) || MoveGenerator.IsEnemySquare(pieceIndex + 8, pieceColor);
            _attackedSquares.Add(pieceIndex + 8);
            
            if (file != 7)
            {
                moveUpRight = MoveGenerator.IsSquareFree(pieceIndex + 9) || MoveGenerator.IsEnemySquare(pieceIndex + 9, pieceColor);
                _attackedSquares.Add(pieceIndex + 9);
            }
                
            if (file != 0)
            {
                moveUpLeft = MoveGenerator.IsSquareFree(pieceIndex + 7) || MoveGenerator.IsEnemySquare(pieceIndex + 7, pieceColor);
                _attackedSquares.Add(pieceIndex + 7);
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
            moveDown = MoveGenerator.IsSquareFree(pieceIndex - 8) || MoveGenerator.IsEnemySquare(pieceIndex - 8, pieceColor);
            _attackedSquares.Add(pieceIndex - 8);

            if (file != 7)
            {
                moveDownRight = MoveGenerator.IsSquareFree(pieceIndex - 7) || MoveGenerator.IsEnemySquare(pieceIndex - 7, pieceColor);
                _attackedSquares.Add(pieceIndex - 7);
            }
            if (file != 0)
            {
                moveDownLeft = MoveGenerator.IsSquareFree(pieceIndex - 9) || MoveGenerator.IsEnemySquare(pieceIndex - 9, pieceColor);
                _attackedSquares.Add(pieceIndex - 9);
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
            moveLeft = MoveGenerator.IsSquareFree(pieceIndex - 1) || MoveGenerator.IsEnemySquare(pieceIndex - 1, pieceColor);
            _attackedSquares.Add(pieceIndex - 1);

            if (moveLeft)
                _allowedSquares.Add(pieceIndex - 1);
        }
        if (file != 7)
        {
            moveRight = MoveGenerator.IsSquareFree(pieceIndex + 1) || MoveGenerator.IsEnemySquare(pieceIndex + 1, pieceColor);
            _attackedSquares.Add(pieceIndex + 1);

            if (moveRight)
                _allowedSquares.Add(pieceIndex + 1);
        }

        if (pieceColor == Piece.White)
        {
            if (BoardManager.Instance.whiteCastleKingside)
            {
                bool castleKingSide = moveRight && MoveGenerator.IsSquareFree(pieceIndex + 2);

                if (BoardManager.Instance.attackedSquares.Contains(pieceIndex + 1) || BoardManager.Instance.attackedSquares.Contains(pieceIndex + 2))
                    castleKingSide = false;

                if (castleKingSide)
                {
                    _allowedSquares.Add(pieceIndex + 2);
                }
            }
            if (BoardManager.Instance.whiteCastleQueenside)
            {
                bool castleQueenSide = moveLeft && MoveGenerator.IsSquareFree(pieceIndex - 2);

                if (BoardManager.Instance.attackedSquares.Contains(pieceIndex - 1) || BoardManager.Instance.attackedSquares.Contains(pieceIndex - 2))
                    castleQueenSide = false;

                if (castleQueenSide)
                {
                    _allowedSquares.Add(pieceIndex - 2);
                }
            }
        }
        else
        {
            if (BoardManager.Instance.blackCastleKingside)
            {
                bool castleKingSide = moveRight && MoveGenerator.IsSquareFree(pieceIndex + 2);

                if (BoardManager.Instance.attackedSquares.Contains(pieceIndex + 1) || BoardManager.Instance.attackedSquares.Contains(pieceIndex + 2))
                    castleKingSide = false;

                if (castleKingSide)
                {
                    _allowedSquares.Add(pieceIndex + 2);
                }
            }
            if (BoardManager.Instance.blackCastleQueenside)
            {
                bool castleQueenSide = moveLeft && MoveGenerator.IsSquareFree(pieceIndex - 2);

                if (BoardManager.Instance.attackedSquares.Contains(pieceIndex - 1) || BoardManager.Instance.attackedSquares.Contains(pieceIndex - 2))
                    castleQueenSide = false;
                
                if (castleQueenSide)
                {
                    _allowedSquares.Add(pieceIndex - 2);
                }
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateBishopPseudoLegal(int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();
        _attackedSquares = new List<int>();

        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool moveUpRight = false, enemyUpRight = false;
        bool moveUpLeft = false, enemyUpLeft = false;
        bool moveDownRight = false, enemyDownRight = false;
        bool moveDownLeft = false, enemyDownLeft = false;

        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(squareUpRight);
            enemyUpRight = MoveGenerator.IsEnemySquare(squareUpRight, pieceColor);
            
            if (moveUpRight || enemyUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                _attackedSquares.Add(squareUpRight);

                squareUpRight += 9;
                
                if (enemyUpRight)
                {    
                    break;
                }
            } else
            {
                _attackedSquares.Add(squareUpRight);
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(squareUpLeft);
            enemyUpLeft = MoveGenerator.IsEnemySquare(squareUpLeft, pieceColor);
            
            if (moveUpLeft || enemyUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                _attackedSquares.Add(squareUpLeft);

                squareUpLeft += 7;
                
                if (enemyUpLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareUpLeft);
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(squareDownRight);
            enemyDownRight = MoveGenerator.IsEnemySquare(squareDownRight, pieceColor);

            if (moveDownRight || enemyDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                _attackedSquares.Add(squareDownRight);

                squareDownRight -= 7;
                
                if (enemyDownRight)
                    break;
            } else
            {
                _attackedSquares.Add(squareDownRight);
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(squareDownLeft);
            enemyDownLeft = MoveGenerator.IsEnemySquare(squareDownLeft, pieceColor);

            if (moveDownLeft || enemyDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                _attackedSquares.Add(squareDownLeft);

                squareDownLeft -= 9;
                
                if (enemyDownLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareDownLeft);
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateRookPseudoLegal(int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();
        _attackedSquares = new List<int>();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;

        bool moveUp = false, enemyUp = false;
        bool moveLeft = false, enemyLeft = false;
        bool moveRight = false, enemyRight = false;
        bool moveDown = false, enemyDown = false;

        while (squareUp <= 63)
        {
            if (squareUp / 8 == 8)
                break;
            
            moveUp = MoveGenerator.IsSquareFree(squareUp);
            enemyUp = MoveGenerator.IsEnemySquare(squareUp, pieceColor);
            
            if (moveUp || enemyUp)
            {
                _allowedSquares.Add(squareUp);
                _attackedSquares.Add(squareUp);

                squareUp += 8;
                
                if (enemyUp)
                    break;
            } else
            {
                _attackedSquares.Add(squareUp);
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            moveLeft = MoveGenerator.IsSquareFree(squareLeft);
            enemyLeft = MoveGenerator.IsEnemySquare(squareLeft, pieceColor);
            
            if (moveLeft || enemyLeft)
            {
                _allowedSquares.Add(squareLeft);
                _attackedSquares.Add(squareLeft);

                squareLeft -= 1;
                
                if (enemyLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareLeft);
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(squareRight);
            enemyRight = MoveGenerator.IsEnemySquare(squareRight, pieceColor);

            if (moveRight || enemyRight)
            {
                _allowedSquares.Add(squareRight);
                _attackedSquares.Add(squareRight);

                squareRight += 1;
                
                if (enemyRight)
                    break;
            } else
            {
                _attackedSquares.Add(squareRight);
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(squareDown);
            enemyDown = MoveGenerator.IsEnemySquare(squareDown, pieceColor);

            if (moveDown || enemyDown)
            {
                _allowedSquares.Add(squareDown);
                _attackedSquares.Add(squareDown);

                squareDown -= 8;
                
                if (enemyDown)
                    break;
            } else
            {
                _attackedSquares.Add(squareDown);
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateQueenPseudoLegal(int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();
        _attackedSquares = new List<int>();

        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;
        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        bool moveUp = false, enemyUp = false;
        bool moveLeft = false, enemyLeft = false;
        bool moveRight = false, enemyRight = false;
        bool moveDown = false, enemyDown = false;
        bool moveUpRight = false, enemyUpRight = false;
        bool moveUpLeft = false, enemyUpLeft = false;
        bool moveDownRight = false, enemyDownRight = false;
        bool moveDownLeft = false, enemyDownLeft = false;

        while (squareUp <= 63)
        {
            if (squareUp / 8 == 8)
                break;
            
            moveUp = MoveGenerator.IsSquareFree(squareUp);
            enemyUp = MoveGenerator.IsEnemySquare(squareUp, pieceColor);
            
            if (moveUp || enemyUp)
            {
                _allowedSquares.Add(squareUp);
                _attackedSquares.Add(squareUp);

                squareUp += 8;
                
                if (enemyUp)
                    break;
            } else
            {
                _attackedSquares.Add(squareUp);
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            moveLeft = MoveGenerator.IsSquareFree(squareLeft);
            enemyLeft = MoveGenerator.IsEnemySquare(squareLeft, pieceColor);
            
            if (moveLeft || enemyLeft)
            {
                _allowedSquares.Add(squareLeft);
                _attackedSquares.Add(squareLeft);

                squareLeft -= 1;
                
                if (enemyLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareLeft);
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(squareRight);
            enemyRight = MoveGenerator.IsEnemySquare(squareRight, pieceColor);

            if (moveRight || enemyRight)
            {
                _allowedSquares.Add(squareRight);
                _attackedSquares.Add(squareRight);

                squareRight += 1;
                
                if (enemyRight)
                    break;
            } else
            {
                _attackedSquares.Add(squareRight);
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(squareDown);
            enemyDown = MoveGenerator.IsEnemySquare(squareDown, pieceColor);

            if (moveDown || enemyDown)
            {
                _allowedSquares.Add(squareDown);
                _attackedSquares.Add(squareDown);

                squareDown -= 8;
                
                if (enemyDown)
                    break;
            } else
            {
                _attackedSquares.Add(squareDown);
                break;
            }
        }
        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(squareUpRight);
            enemyUpRight = MoveGenerator.IsEnemySquare(squareUpRight, pieceColor);
            
            if (moveUpRight || enemyUpRight)
            {
                _allowedSquares.Add(squareUpRight);
                _attackedSquares.Add(squareUpRight);

                squareUpRight += 9;
                
                if (enemyUpRight)
                    break;
            } else
            {
                _attackedSquares.Add(squareUpRight);
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(squareUpLeft);
            enemyUpLeft = MoveGenerator.IsEnemySquare(squareUpLeft, pieceColor);
            
            if (moveUpLeft || enemyUpLeft)
            {
                _allowedSquares.Add(squareUpLeft);
                _attackedSquares.Add(squareUpLeft);

                squareUpLeft += 7;
                
                if (enemyUpLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareUpLeft);
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(squareDownRight);
            enemyDownRight = MoveGenerator.IsEnemySquare(squareDownRight, pieceColor);

            if (moveDownRight || enemyDownRight)
            {
                _allowedSquares.Add(squareDownRight);
                _attackedSquares.Add(squareDownRight);

                squareDownRight -= 7;
                
                if (enemyDownRight)
                    break;
            } else
            {
                _attackedSquares.Add(squareDownRight);
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(squareDownLeft);
            enemyDownLeft = MoveGenerator.IsEnemySquare(squareDownLeft, pieceColor);

            if (moveDownLeft || enemyDownLeft)
            {
                _allowedSquares.Add(squareDownLeft);
                _attackedSquares.Add(squareDownLeft);

                squareDownLeft -= 9;
                
                if (enemyDownLeft)
                    break;
            } else
            {
                _attackedSquares.Add(squareDownLeft);
                break;
            }
        }

        return _allowedSquares;
    }

    public static List<int> GenerateKnightPseudoLegal(int pieceIndex, int pieceColor)
    {
        _allowedSquares = new List<int>();
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
            moveUpUpRight = MoveGenerator.IsSquareFree(squareUpUpRight);
            enemyUpUpRight = MoveGenerator.IsEnemySquare(squareUpUpRight, pieceColor);
            _attackedSquares.Add(squareUpUpRight);

            if (moveUpUpRight || enemyUpUpRight)
                _allowedSquares.Add(squareUpUpRight);

        }
        if (rank < 7 && file < 6)
        {
            moveUpRightRight = MoveGenerator.IsSquareFree(squareUpRightRight);
            enemyUpRightRight = MoveGenerator.IsEnemySquare(squareUpRightRight, pieceColor);
            _attackedSquares.Add(squareUpRightRight);

            if (moveUpRightRight || enemyUpRightRight)
                _allowedSquares.Add(squareUpRightRight);
        }
        if (rank < 6 && file > 0)
        {
            moveUpUpLeft = MoveGenerator.IsSquareFree(squareUpUpLeft);
            enemyUpUpLeft = MoveGenerator.IsEnemySquare(squareUpUpLeft, pieceColor);
            _attackedSquares.Add(squareUpUpLeft);
            
            if (moveUpUpLeft || enemyUpUpLeft)
                _allowedSquares.Add(squareUpUpLeft);
        }
        if (rank < 7 && file > 1)
        {
            moveUpLeftLeft = MoveGenerator.IsSquareFree(squareUpLeftLeft);
            enemyUpLeftLeft = MoveGenerator.IsEnemySquare(squareUpLeftLeft, pieceColor);
            _attackedSquares.Add(squareUpLeftLeft);

            if (moveUpLeftLeft || enemyUpLeftLeft)
                _allowedSquares.Add(squareUpLeftLeft);
        }
        if (rank > 1 && file > 0)
        {
            moveDownDownLeft = MoveGenerator.IsSquareFree(squareDownDownLeft);
            enemyDownDownLeft = MoveGenerator.IsEnemySquare(squareDownDownLeft, pieceColor);
            _attackedSquares.Add(squareDownDownLeft);

            if (moveDownDownLeft || enemyDownDownLeft)
                _allowedSquares.Add(squareDownDownLeft);
        }
        if (rank > 0 && file > 1)
        {
            moveDownLeftLeft = MoveGenerator.IsSquareFree(squareDownLeftLeft);
            enemyDownLeftLeft = MoveGenerator.IsEnemySquare(squareDownLeftLeft, pieceColor);
            _attackedSquares.Add(squareDownLeftLeft);

            if (moveDownLeftLeft || enemyDownLeftLeft)
                _allowedSquares.Add(squareDownLeftLeft);
        }
        if (rank > 1 && file < 7)
        {
            moveDownDownRight = MoveGenerator.IsSquareFree(squareDownDownRight);
            enemyDownDownRight = MoveGenerator.IsEnemySquare(squareDownDownRight, pieceColor);
            _attackedSquares.Add(squareDownDownRight);

            if (moveDownDownRight || enemyDownDownRight)
                _allowedSquares.Add(squareDownDownRight);
        }
        if (rank > 0 && file < 6)
        {
            moveDownRightRight = MoveGenerator.IsSquareFree(squareDownRightRight);
            enemyDownRightRight = MoveGenerator.IsEnemySquare(squareDownRightRight, pieceColor);
            _attackedSquares.Add(squareDownRightRight);

            if (moveDownRightRight || enemyDownRightRight)
                _allowedSquares.Add(squareDownRightRight);
        }

        return _allowedSquares;
    }

    public static List<int> GetAttackedSquares()
    {
        return _attackedSquares;
    }
}
