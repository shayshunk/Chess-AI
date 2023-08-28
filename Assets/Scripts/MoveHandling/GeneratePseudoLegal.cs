using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratePseudoLegal
{
    public static void GeneratePawnPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                    bool[] tempAttacked = new bool[64];
                    board.attackedSquares.CopyTo(tempAttacked, 0);
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

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.Black)
                        {
                            allowedMoves[pieceIndex + 9] = true;
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
                    bool[] tempAttacked = new bool[64];
                    board.attackedSquares.CopyTo(tempAttacked, 0);
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

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.Black)
                        {
                            allowedMoves[pieceIndex + 9] = true;
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
                allowedMoves[squareUp] = true;
            }
            if (rank == 1)
            {
                int squareUpTwo = pieceIndex + 16;
                bool moveTwo = MoveGenerator.IsSquareFree(board, squareUpTwo);
                if (moveTwo && moveOne)
                {
                    allowedMoves[squareUpTwo] = true;
                }
            }
            if (file != 0)
            {
                int squareUpLeft = pieceIndex + 7;
                if (MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor))
                {
                    allowedMoves[squareUpLeft] = true;
                }
            }
            if (file != 7)
            {
                int squareUpRight = pieceIndex + 9;
                if (MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor))
                {
                    allowedMoves[squareUpRight] = true;
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
                    bool[] tempAttacked = new bool[64];
                    board.attackedSquares.CopyTo(tempAttacked, 0);
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

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceRightType == Piece.Pawn && pieceRightColor == Piece.White)
                        {
                            allowedMoves[pieceIndex - 7] = true;
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
                    bool[] tempAttacked = new bool[64];
                    board.attackedSquares.CopyTo(tempAttacked, 0);
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

                    if (!board.currentPlayerInCheck)
                    {
                        if (pieceLeftType == Piece.Pawn && pieceLeftColor == Piece.White)
                        {
                            allowedMoves[pieceIndex - 9] = true;
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
                allowedMoves[squareDown] = true;
            }
            
            if (rank == 6)
            {
                bool moveTwo = MoveGenerator.IsSquareFree(board, pieceIndex - 16);
                if (moveTwo && moveOne)
                {
                    allowedMoves[pieceIndex - 16] = true;
                }
            }

            if (file != 7)
            {
                int squareDownRight = pieceIndex - 7;

                if (MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor))
                {
                    allowedMoves[squareDownRight] = true;
                }
            }
            
            if (file != 0)
            {
                int squareDownLeft = pieceIndex - 9;

                if (MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor))
                {
                    allowedMoves[squareDownLeft] = true;
                }
            }
        }
    }

    public static void GenerateKingPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                allowedMoves[pieceIndex + 8] = true;
            if (moveUpLeft)
                allowedMoves[pieceIndex + 7] = true;
            if (moveUpRight)
                allowedMoves[pieceIndex + 9] = true;
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
                allowedMoves[pieceIndex - 8] = true;
            if (moveDownLeft)
                allowedMoves[pieceIndex - 9] = true;
            if (moveDownRight)
                allowedMoves[pieceIndex - 7] = true;
        }
        if (file != 0)
        {
            moveLeft = MoveGenerator.IsSquareFree(board, pieceIndex - 1) || MoveGenerator.IsEnemySquare(board, pieceIndex - 1, pieceColor);

            if (moveLeft)
                allowedMoves[pieceIndex - 1] = true;
        }
        if (file != 7)
        {
            moveRight = MoveGenerator.IsSquareFree(board, pieceIndex + 1) || MoveGenerator.IsEnemySquare(board, pieceIndex + 1, pieceColor);

            if (moveRight)
                allowedMoves[pieceIndex + 1] = true;
        }

        if (pieceColor == Piece.White)
        {
            if (board.whiteCastleKingside && !board.currentPlayerInCheck)
            {
                bool castleKingSide = MoveGenerator.IsSquareFree(board, pieceIndex + 1) && MoveGenerator.IsSquareFree(board, pieceIndex + 2);

                if (board.attackedSquares[pieceIndex + 1] || board.attackedSquares[pieceIndex + 2])
                    castleKingSide = false;

                if (castleKingSide)
                {
                    allowedMoves[pieceIndex + 2] = true;
                }
            }
            if (board.whiteCastleQueenside && !board.currentPlayerInCheck)
            {
                bool castleQueenSide = MoveGenerator.IsSquareFree(board, pieceIndex - 1) && MoveGenerator.IsSquareFree(board, pieceIndex - 2) && MoveGenerator.IsSquareFree(board, pieceIndex - 3);

                if (board.attackedSquares[pieceIndex - 1] || board.attackedSquares[pieceIndex - 2])
                    castleQueenSide = false;

                if (castleQueenSide)
                {
                    allowedMoves[pieceIndex - 2] = true;
                }
            }
        }
        else
        {
            if (board.blackCastleKingside && !board.currentPlayerInCheck)
            {
                bool castleKingSide = MoveGenerator.IsSquareFree(board, pieceIndex + 1) && MoveGenerator.IsSquareFree(board, pieceIndex + 2);

                if (board.attackedSquares[pieceIndex + 1] || board.attackedSquares[pieceIndex + 2])
                    castleKingSide = false;

                if (castleKingSide)
                {
                    allowedMoves[pieceIndex + 2] = true;
                }
            }
            if (board.blackCastleQueenside && !board.currentPlayerInCheck)
            {
                bool castleQueenSide = MoveGenerator.IsSquareFree(board, pieceIndex - 1) && MoveGenerator.IsSquareFree(board, pieceIndex - 2) && MoveGenerator.IsSquareFree(board, pieceIndex - 3);

                if (board.attackedSquares[pieceIndex - 1] || board.attackedSquares[pieceIndex - 2])
                    castleQueenSide = false;
                
                if (castleQueenSide)
                {
                    allowedMoves[pieceIndex - 2] = true;
                }
            }
        }
    }

    public static void GenerateBishopPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                allowedMoves[squareUpRight] = true;
                break;
            }

            bool moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            if (moveUpRight)
            {
                allowedMoves[squareUpRight] = true;
                squareUpRight += 9;
                enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareUpLeft <= 62)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            if (enemyUpLeft)
            {
                allowedMoves[squareUpLeft] = true;
                break;
            }

            bool moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            if (moveUpLeft)
            {
                allowedMoves[squareUpLeft] = true;
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
                allowedMoves[squareDownRight] = true;
                break;
            }

            bool moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            if (moveDownRight)
            {
                allowedMoves[squareDownRight] = true;
                squareDownRight -= 7;
                enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDownLeft >= 0)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            else if (enemyDownLeft)
            {
                allowedMoves[squareDownLeft] = true;
                break;
            }

            bool moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            if (moveDownLeft)
            {
                allowedMoves[squareDownLeft] = true;
                squareDownLeft -= 9;
                enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);    
            } 
            else
            {
                break;
            }
        }
    }

    public static void GenerateRookPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                allowedMoves[squareUp] = true;
                break;
            }

            bool moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            if (moveUp)
            {
                allowedMoves[squareUp] = true;
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
                allowedMoves[squareLeft] = true;
                break;
            }

            bool moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            if (moveLeft)
            {
                allowedMoves[squareLeft] = true;
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
                allowedMoves[squareRight] = true;
                break;
            }

            bool moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            if (moveRight)
            {
                allowedMoves[squareRight] = true;
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
                allowedMoves[squareDown] = true;
                break;
            }
            
            bool moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            if (moveDown)
            {
                allowedMoves[squareDown] = true;
                squareDown -= 8;
                enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);
            } 
            else
            {
                break;
            }
        }
    }

    public static void GenerateQueenPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                allowedMoves[squareUp] = true;
                break;
            }

            bool moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            if (moveUp)
            {
                allowedMoves[squareUp] = true;
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
                allowedMoves[squareLeft] = true;
                break;
            }

            bool moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            if (moveLeft)
            {
                allowedMoves[squareLeft] = true;
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
                allowedMoves[squareRight] = true;
                break;
            }

            bool moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            if (moveRight)
            {
                allowedMoves[squareRight] = true;
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
                allowedMoves[squareDown] = true;
                break;
            }
            
            bool moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            if (moveDown)
            {
                allowedMoves[squareDown] = true;
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
                allowedMoves[squareUpRight] = true;
                break;
            }

            bool moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            if (moveUpRight)
            {
                allowedMoves[squareUpRight] = true;
                squareUpRight += 9;
                enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareUpLeft <= 62)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            if (enemyUpLeft)
            {
                allowedMoves[squareUpLeft] = true;
                break;
            }

            bool moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            if (moveUpLeft)
            {
                allowedMoves[squareUpLeft] = true;
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
                allowedMoves[squareDownRight] = true;
                break;
            }

            bool moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            if (moveDownRight)
            {
                allowedMoves[squareDownRight] = true;
                squareDownRight -= 7;
                enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);
            } 
            else
            {
                break;
            }
        }
        while (squareDownLeft >= 0)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            else if (enemyDownLeft)
            {
                allowedMoves[squareDownLeft] = true;
                break;
            }

            bool moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            if (moveDownLeft)
            {
                allowedMoves[squareDownLeft] = true;
                squareDownLeft -= 9;
                enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);    
            } 
            else
            {
                break;
            }
        }
    }

    public static void GenerateKnightPseudoLegal(Board board, int pieceIndex, int pieceColor, bool[] allowedMoves)
    {
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
                allowedMoves[squareUpUpRight] = true;

        }
        if (rank < 7 && file < 6)
        {
            moveUpRightRight = MoveGenerator.IsSquareFree(board, squareUpRightRight);
            enemyUpRightRight = MoveGenerator.IsEnemySquare(board, squareUpRightRight, pieceColor);

            if (moveUpRightRight || enemyUpRightRight)
                allowedMoves[squareUpRightRight] = true;
        }
        if (rank < 6 && file > 0)
        {
            moveUpUpLeft = MoveGenerator.IsSquareFree(board, squareUpUpLeft);
            enemyUpUpLeft = MoveGenerator.IsEnemySquare(board, squareUpUpLeft, pieceColor);
            
            if (moveUpUpLeft || enemyUpUpLeft)
                allowedMoves[squareUpUpLeft] = true;
        }
        if (rank < 7 && file > 1)
        {
            moveUpLeftLeft = MoveGenerator.IsSquareFree(board, squareUpLeftLeft);
            enemyUpLeftLeft = MoveGenerator.IsEnemySquare(board, squareUpLeftLeft, pieceColor);

            if (moveUpLeftLeft || enemyUpLeftLeft)
                allowedMoves[squareUpLeftLeft] = true;
        }
        if (rank > 1 && file > 0)
        {
            moveDownDownLeft = MoveGenerator.IsSquareFree(board, squareDownDownLeft);
            enemyDownDownLeft = MoveGenerator.IsEnemySquare(board, squareDownDownLeft, pieceColor);

            if (moveDownDownLeft || enemyDownDownLeft)
                allowedMoves[squareDownDownLeft] = true;
        }
        if (rank > 0 && file > 1)
        {
            moveDownLeftLeft = MoveGenerator.IsSquareFree(board, squareDownLeftLeft);
            enemyDownLeftLeft = MoveGenerator.IsEnemySquare(board, squareDownLeftLeft, pieceColor);

            if (moveDownLeftLeft || enemyDownLeftLeft)
                allowedMoves[squareDownLeftLeft] = true;
        }
        if (rank > 1 && file < 7)
        {
            moveDownDownRight = MoveGenerator.IsSquareFree(board, squareDownDownRight);
            enemyDownDownRight = MoveGenerator.IsEnemySquare(board, squareDownDownRight, pieceColor);

            if (moveDownDownRight || enemyDownDownRight)
                allowedMoves[squareDownDownRight] = true;
        }
        if (rank > 0 && file < 6)
        {
            moveDownRightRight = MoveGenerator.IsSquareFree(board, squareDownRightRight);
            enemyDownRightRight = MoveGenerator.IsEnemySquare(board, squareDownRightRight, pieceColor);

            if (moveDownRightRight || enemyDownRightRight)
                allowedMoves[squareDownRightRight] = true;
        }
    }

    public static void GeneratePawnAttacked(Board board, int pieceIndex, bool[] attacked)
    {
        int pieceColor = Piece.Color(board.square[pieceIndex]);
        
        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (pieceColor == Piece.White)
        {
            attacked[pieceIndex + 7] = true;
            attacked[pieceIndex + 9] = true;

        } else
        {
            attacked[pieceIndex - 7] = true;
            attacked[pieceIndex - 9] = true;
        }
    }

    public static void GenerateKingAttacked(Board board, int pieceIndex, bool[] attacked)
    {
        int rank = pieceIndex / 8;
        int file = pieceIndex % 8;

        if (rank != 7)
        {
            attacked[pieceIndex + 8] = true;
            
            if (file != 7)
            {
                attacked[pieceIndex + 9] = true;
                attacked[pieceIndex + 1] = true;
            }   
            if (file != 0)
            {
                attacked[pieceIndex + 7] = true;
                attacked[pieceIndex - 1] = true;
            }
        }
        if (rank != 0)
        {
            attacked[pieceIndex - 8] = true;

            if (file != 7)
            {
                attacked[pieceIndex - 7] = true;
                attacked[pieceIndex + 1] = true;
            }
            if (file != 0)
            {
                attacked[pieceIndex - 9] = true;
                attacked[pieceIndex - 1] = true;
            }
        }
    }

    public static void GenerateBishopAttacked(Board board, int pieceIndex, bool[] attacked)
    {
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
                attacked[squareUpRight] = true;
                squareUpRight += 9;
            } 
            else
            {
                attacked[squareUpRight] = true;
                break;
            }
        }
        while (squareUpLeft <= 62)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            
            if (moveUpLeft)
            {
                attacked[squareUpLeft] = true;
                squareUpLeft += 7;
            } 
            else
            {
                attacked[squareUpLeft] = true;
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
                attacked[squareDownRight] = true;
                squareDownRight -= 7;
            } 
            else
            {
                attacked[squareDownRight] = true;
                break;
            }
        }
        while (squareDownLeft >= 0)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);

            if (moveDownLeft)
            {
                attacked[squareDownLeft] = true;
                squareDownLeft -= 9;
            } 
            else
            {
                attacked[squareDownLeft] = true;
                break;
            }
        }
    }

    public static void GenerateRookAttacked(Board board, int pieceIndex, bool[] attacked)
    {
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
                attacked[squareUp] = true;
                squareUp += 8;
            } 
            else
            {
                attacked[squareUp] = true;
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
                attacked[squareLeft] = true;
                squareLeft -= 1;
            } 
            else
            {
                attacked[squareLeft] = true;
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
                attacked[squareRight] = true;
                squareRight += 1;
            } 
            else
            {
                attacked[squareRight] = true;
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);

            if (moveDown)
            {
                attacked[squareDown] = true;
                squareDown -= 8;
            } 
            else
            {
                attacked[squareDown] = true;
                break;
            }
        }
    }

    public static void GenerateQueenAttacked(Board board, int pieceIndex, bool[] attacked)
    {
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
                attacked[squareUp] = true;
                squareUp += 8;
            } 
            else
            {
                attacked[squareUp] = true;
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
                attacked[squareLeft] = true;
                squareLeft -= 1;
            } 
            else
            {
                attacked[squareLeft] = true;
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
                attacked[squareRight] = true;
                squareRight += 1;
            } 
            else
            {
                attacked[squareRight] = true;
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);

            if (moveDown)
            {
                attacked[squareDown] = true;
                squareDown -= 8;
            } 
            else
            {
                attacked[squareDown] = true;
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
                attacked[squareUpRight] = true;
                squareUpRight += 9;
            } 
            else
            {
                attacked[squareUpRight] = true;
                break;
            }
        }
        while (squareUpLeft <= 62)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            
            if (moveUpLeft)
            {
                attacked[squareUpLeft] = true;
                squareUpLeft += 7;
            } 
            else
            {
                attacked[squareUpLeft] = true;
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
                attacked[squareDownRight] = true;
                squareDownRight -= 7;
            } 
            else
            {
                attacked[squareDownRight] = true;
                break;
            }
        }
        while (squareDownLeft >= 0)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);

            if (moveDownLeft)
            {
                attacked[squareDownLeft] = true;
                squareDownLeft -= 9;
            } 
            else
            {
                attacked[squareDownLeft] = true;
                break;
            }
        }
    }

    public static void GenerateKnightAttacked(Board board, int pieceIndex, bool[] attacked)
    {
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
            attacked[squareUpUpRight] = true;
        }
        if (rank < 7 && file < 6)
        {
            attacked[squareUpRightRight] = true;
        }
        if (rank < 6 && file > 0)
        {
            attacked[squareUpUpLeft] = true;
        }
        if (rank < 7 && file > 1)
        {
            attacked[squareUpLeftLeft] = true;
        }
        if (rank > 1 && file > 0)
        {
            attacked[squareDownDownLeft] = true;
        }
        if (rank > 0 && file > 1)
        {
            attacked[squareDownLeftLeft] = true;
        }
        if (rank > 1 && file < 7)
        {
            attacked[squareDownDownRight] = true;
        }
        if (rank > 0 && file < 6)
        {
            attacked[squareDownRightRight] = true;
        }
    }
}
