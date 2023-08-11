using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PinHandler
{
    static bool[] squaresAroundKing;
    static List<int> pinnedPieces, pinnedDirection;
    
    public static bool[] GeneratePins()
    {
        squaresAroundKing = new bool[8];
        pinnedPieces = new List<int>();
        pinnedDirection = new List<int>();

        int blackOrWhite = BoardManager.Instance.whiteToMove? 0 : 1;
        int pieceIndex = BoardManager.Instance.kingSquares[blackOrWhite];
        int pieceColor = BoardManager.Instance.whiteToMove? Piece.White : Piece.Black;

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

        int possiblePinned = -1;
        bool foundFriendly = false;

        while (squareUp <= 63)
        {
            if (squareUp / 8 == 8)
            {
                foundFriendly = false;
                break;
            }
            
            moveUp = MoveGenerator.IsSquareFree(squareUp);
            enemyUp = MoveGenerator.IsEnemySquare(squareUp, pieceColor);
            
            if (moveUp || enemyUp)
            {
                if (foundFriendly && enemyUp)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareUp]);

                    if (enemyPieceType == Piece.Rook || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[1] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(8);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }

                if (enemyUp)
                {
                    foundFriendly = false;
                    break;
                }

                squareUp += 8;
                continue;

            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareUp;
                foundFriendly = true;
                squareUp += 8;
            }
        }

        foundFriendly = false;
        
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
            {
                foundFriendly = false;
                break;
            }
            
            moveLeft = MoveGenerator.IsSquareFree(squareLeft);
            enemyLeft = MoveGenerator.IsEnemySquare(squareLeft, pieceColor);
            
            if (moveLeft || enemyLeft)
            {
                if (foundFriendly && enemyLeft)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareLeft]);

                    if (enemyPieceType == Piece.Rook || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[3] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(1);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }

                if (enemyLeft)
                {
                    foundFriendly = false;
                    break;
                }

                squareLeft -= 1;
                continue;

            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }

                possiblePinned = squareLeft;
                foundFriendly = true;
                squareLeft -= 1;
            }
        }

        foundFriendly = false;

        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
            {
                foundFriendly = false;
                break;
            }
            
            moveRight = MoveGenerator.IsSquareFree(squareRight);
            enemyRight = MoveGenerator.IsEnemySquare(squareRight, pieceColor);

            if (moveRight || enemyRight)
            {
                if (foundFriendly && enemyRight)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareRight]);

                    if (enemyPieceType == Piece.Rook || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[4] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(1);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }

                if (enemyRight)
                {
                    foundFriendly = false;
                    break;
                }
                
                squareRight += 1;
                continue;
            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareRight;
                foundFriendly = true;
                squareRight += 1;
            }
        }

        foundFriendly = false;

        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(squareDown);
            enemyDown = MoveGenerator.IsEnemySquare(squareDown, pieceColor);

            if (moveDown || enemyDown)
            {
                if (foundFriendly && enemyDown)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareDown]);

                    if (enemyPieceType == Piece.Rook || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[6] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(8);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }
                if (enemyDown)
                {
                    foundFriendly = false;
                    break;
                }

                squareDown -= 8;
                continue;
            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareDown;
                foundFriendly = true;
                squareDown -= 8;
            }
        }

        foundFriendly = false;

        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
            {
                foundFriendly = false;
                break;
            }
            
            moveUpRight = MoveGenerator.IsSquareFree(squareUpRight);
            enemyUpRight = MoveGenerator.IsEnemySquare(squareUpRight, pieceColor);
            
            if (moveUpRight || enemyUpRight)
            {
                if (foundFriendly && enemyUpRight)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareUpRight]);

                    if (enemyPieceType == Piece.Bishop || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[2] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(9);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }
                if (enemyUpRight)
                {
                    foundFriendly = false;
                    break;
                }

                squareUpRight += 9;
                continue;
            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareUpRight;
                foundFriendly = true;
                squareUpRight += 9;
            }
        }

        foundFriendly = false;

        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
            {
                foundFriendly = false;
                break;
            }
            
            moveUpLeft = MoveGenerator.IsSquareFree(squareUpLeft);
            enemyUpLeft = MoveGenerator.IsEnemySquare(squareUpLeft, pieceColor);
            
            if (moveUpLeft || enemyUpLeft)
            {
                if (foundFriendly && enemyUpLeft)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareUpLeft]);

                    if (enemyPieceType == Piece.Bishop || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[0] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(7);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }
                if (enemyUpLeft)
                {
                    foundFriendly = false;
                    break;
                }

                squareUpLeft += 7;
                continue;
            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareUpLeft;
                foundFriendly = true;
                squareUpLeft += 7;
            }
        }

        foundFriendly = false;

        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
            {
                foundFriendly = false;
                break;
            }
            
            moveDownRight = MoveGenerator.IsSquareFree(squareDownRight);
            enemyDownRight = MoveGenerator.IsEnemySquare(squareDownRight, pieceColor);

            if (moveDownRight || enemyDownRight)
            {
                if (foundFriendly && enemyDownRight)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareDownRight]);

                    if (enemyPieceType == Piece.Bishop || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[7] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(7);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }
                if (enemyDownRight)
                {
                    foundFriendly = false;
                    break;
                }

                squareDownRight -= 7;
                continue;
                
            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareDownRight;
                foundFriendly = true;
                squareDownRight -= 7;
            }
        }

        foundFriendly = false;
        
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
            {
                foundFriendly = false;
                break;
            }
            
            moveDownLeft = MoveGenerator.IsSquareFree(squareDownLeft);
            enemyDownLeft = MoveGenerator.IsEnemySquare(squareDownLeft, pieceColor);

            if (moveDownLeft || enemyDownLeft)
            {
                if (foundFriendly && enemyDownLeft)
                {
                    int enemyPieceType = Piece.PieceType(BoardManager.Instance.square[squareDownLeft]);

                    if (enemyPieceType == Piece.Bishop || enemyPieceType == Piece.Queen)
                    {
                        foundFriendly = false;
                        squaresAroundKing[5] = true;
                        pinnedPieces.Add(possiblePinned);
                        pinnedDirection.Add(9);
                        break;
                    } else
                    {
                        foundFriendly = false;
                        break;
                    }
                }
                if (enemyDownLeft)
                {
                    foundFriendly = false;
                    break;
                }
                squareDownLeft -= 9;
                continue;

            } else
            {
                if (foundFriendly)
                {
                    foundFriendly = false;
                    break;
                }
                
                possiblePinned = squareDownLeft;
                foundFriendly = true;

                squareDownLeft -= 9;
            }
        }

        return squaresAroundKing;
    }

    public static List<int> GetPins()
    {
        return pinnedPieces;
    }

    public static List<int> GetPinDirections()
    {
        return pinnedDirection;
    }
}
