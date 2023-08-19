using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateCheck
{
    private static bool _causesCheck;

    public static void Reset()
    {
        _causesCheck = false;
    }
    public static bool GeneratePawnCheck(Board board, int piece, int pieceIndex)
    {
        if (Piece.IsColor(piece, Piece.White))
        {
            if (pieceIndex + 7 == BoardManager.Instance.kingSquares[1] || pieceIndex + 9 == BoardManager.Instance.kingSquares[1])
            {
                _causesCheck = true;
                Debug.Log("Move causes check!");
            }
        } else
        {
            if (pieceIndex - 7 == BoardManager.Instance.kingSquares[0] || pieceIndex - 9 == BoardManager.Instance.kingSquares[0])
            {
                _causesCheck = true;
                Debug.Log("Move causes check!");
            }
        }

        return _causesCheck;
    }

    public static bool GenerateBishopCheck(Board board, int piece, int pieceIndex)
    {
        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        int pieceColor = Piece.Color(piece), enemyColor;

        if (pieceColor == Piece.White)
            enemyColor = 1;
        else
            enemyColor = 0;

        bool moveUpRight = false, enemyUpRight = false;
        bool moveUpLeft = false, enemyUpLeft = false;
        bool moveDownRight = false, enemyDownRight = false;
        bool moveDownLeft = false, enemyDownLeft = false;

        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            
            if (moveUpRight || enemyUpRight)
            {    
                if (enemyUpRight)
                {
                    if (squareUpRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                }
                
                squareUpRight += 9;

            } else
            {
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
            
            if (moveUpLeft || enemyUpLeft)
            {

                
                if (enemyUpLeft)
                {
                    if (squareUpLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                }                     
                
                squareUpLeft += 7;

            } else
            {
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);

            if (moveDownRight || enemyDownRight)
            {   
                if (enemyDownRight)
                {
                    if (squareDownRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                
                squareDownRight -= 7;

            } else
            {
                break;
            }
        }
        while (squareDownLeft >= 10)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);

            if (moveDownLeft || enemyDownLeft)
            {                    
                if (enemyDownLeft)
                {
                    if (squareDownLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 

                squareDownLeft -= 9;

            } else
            {
                break;
            }
        }

        return _causesCheck;
    }

    public static bool GenerateRookCheck(Board board, int piece, int pieceIndex)
    {
        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;

        int pieceColor = Piece.Color(piece), enemyColor;

        if (pieceColor == Piece.White)
            enemyColor = 1;
        else
            enemyColor = 0;

        bool moveUp = false, enemyUp = false;
        bool moveLeft = false, enemyLeft = false;
        bool moveRight = false, enemyRight = false;
        bool moveDown = false, enemyDown = false;

        while (squareUp <= 63)
        {
            if (squareUp / 8 == 8)
                break;
            
            moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
            
            if (moveUp || enemyUp)
            {                    
                if (enemyUp)
                {
                    if (squareUp == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareUp += 8;
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
            
            moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
            
            if (moveLeft || enemyLeft)
            {
                if (enemyLeft)
                {
                    if (squareLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareLeft -= 1;
            } else
            {
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);

            if (moveRight || enemyRight)
            {
                if (enemyRight)
                {
                    if (squareRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareRight += 1;
            } else
            {
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);

            if (moveDown || enemyDown)
            {
                if (enemyDown)
                {
                    if (squareDown == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareDown -= 8;
            } else
            {
                break;
            }
        }

        return _causesCheck;
    }

    public static bool GenerateQueenCheck(Board board, int piece, int pieceIndex)
    {
        int squareUp = pieceIndex + 8;
        int squareLeft = pieceIndex - 1;
        int squareRight = pieceIndex + 1;
        int squareDown = pieceIndex - 8;
        int squareUpRight = pieceIndex + 9;
        int squareUpLeft = pieceIndex + 7;
        int squareDownRight = pieceIndex - 7;
        int squareDownLeft = pieceIndex - 9;

        int pieceColor = Piece.Color(piece), enemyColor;

        if (pieceColor == Piece.White)
            enemyColor = 1;
        else
            enemyColor = 0;

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
            
            moveUp = MoveGenerator.IsSquareFree(board, squareUp);
            enemyUp = MoveGenerator.IsEnemySquare(board, squareUp, pieceColor);
            
            if (moveUp || enemyUp)
            {
                if (enemyUp)
                {
                    if (squareUp == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareUp += 8;
            } else
            {
                break;
            }
        }
        while (squareLeft >= 0)
        {
            if (squareLeft % 8 == 7)
                break;
            
            moveLeft = MoveGenerator.IsSquareFree(board, squareLeft);
            enemyLeft = MoveGenerator.IsEnemySquare(board, squareLeft, pieceColor);
            
            if (moveLeft || enemyLeft)
            {
                if (enemyLeft)
                {
                    if (squareLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareLeft -= 1;
            } else
            {
                break;
            }
        }
        while (squareRight <= 63)
        {
            if (squareRight % 8 == 0)
                break;
            
            moveRight = MoveGenerator.IsSquareFree(board, squareRight);
            enemyRight = MoveGenerator.IsEnemySquare(board, squareRight, pieceColor);

            if (moveRight || enemyRight)
            {
                if (enemyRight)
                {
                    if (squareRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareRight += 1;
            } else
            {
                break;
            }
        }
        while (squareDown >= 0)
        {            
            moveDown = MoveGenerator.IsSquareFree(board, squareDown);
            enemyDown = MoveGenerator.IsEnemySquare(board, squareDown, pieceColor);

            if (moveDown || enemyDown)
            {
                if (enemyDown)
                {
                    if (squareDown == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareDown -= 8;
            } else
            {
                break;
            }
        }
        while (squareUpRight <= 63)
        {
            if (squareUpRight % 8 == 0)
                break;
            
            moveUpRight = MoveGenerator.IsSquareFree(board, squareUpRight);
            enemyUpRight = MoveGenerator.IsEnemySquare(board, squareUpRight, pieceColor);
            
            if (moveUpRight || enemyUpRight)
            {
                if (enemyUpRight)
                {
                    if (squareUpRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareUpRight += 9;
            } else
            {
                break;
            }
        }
        while (squareUpLeft <= 61)
        {
            if (squareUpLeft % 8 == 7)
                break;
            
            moveUpLeft = MoveGenerator.IsSquareFree(board, squareUpLeft);
            enemyUpLeft = MoveGenerator.IsEnemySquare(board, squareUpLeft, pieceColor);
            
            if (moveUpLeft || enemyUpLeft)
            {
                if (enemyUpLeft)
                {
                    if (squareUpLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareUpLeft += 7;
            } else
            {
                break;
            }
        }
        while (squareDownRight >= 1)
        {
            if (squareDownRight % 8 == 0)
                break;
            
            moveDownRight = MoveGenerator.IsSquareFree(board, squareDownRight);
            enemyDownRight = MoveGenerator.IsEnemySquare(board, squareDownRight, pieceColor);

            if (moveDownRight || enemyDownRight)
            {
                if (enemyDownRight)
                {
                    if (squareDownRight == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareDownRight -= 7;
            } else
            {
                break;
            }
        }
        while (squareDownLeft >= 1)
        {
            if (squareDownLeft % 8 == 7)
                break;
            
            moveDownLeft = MoveGenerator.IsSquareFree(board, squareDownLeft);
            enemyDownLeft = MoveGenerator.IsEnemySquare(board, squareDownLeft, pieceColor);

            if (moveDownLeft || enemyDownLeft)
            {
                if (enemyDownLeft)
                {
                    if (squareDownLeft == BoardManager.Instance.kingSquares[enemyColor])
                    {
                        _causesCheck = true;
                        return _causesCheck;
                    }
                    else
                        break;
                } 
                squareDownLeft -= 9;
            } else
            {
                break;
            }
        }

        return _causesCheck;
    }

    public static bool GenerateKnightCheck(Board board, int piece, int pieceIndex)
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

        int pieceColor = Piece.Color(piece), enemyColor;

        if (pieceColor == Piece.White)
            enemyColor = 1;
        else
            enemyColor = 0;

        bool enemyUpUpRight = false;
        bool enemyUpRightRight = false;
        bool enemyUpUpLeft = false;
        bool enemyUpLeftLeft = false;
        bool enemyDownDownRight = false;
        bool enemyDownRightRight = false;
        bool enemyDownDownLeft = false;
        bool enemyDownLeftLeft = false;
   
        if (rank < 6 && file < 7)
        {
            enemyUpUpRight = MoveGenerator.IsEnemySquare(board, squareUpUpRight, pieceColor);
            
            if (enemyUpUpRight && squareUpUpRight == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank < 7 && file < 6)
        {
            enemyUpRightRight = MoveGenerator.IsEnemySquare(board, squareUpRightRight, pieceColor);

            if (enemyUpRightRight && squareUpRightRight == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank < 6 && file > 0)
        {
            enemyUpUpLeft = MoveGenerator.IsEnemySquare(board, squareUpUpLeft, pieceColor);

            if (enemyUpUpLeft && squareUpUpLeft == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank < 7 && file > 1)
        {
            enemyUpLeftLeft = MoveGenerator.IsEnemySquare(board, squareUpLeftLeft, pieceColor);

            if (enemyUpLeftLeft && squareUpLeftLeft == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank > 1 && file > 0)
        {
            enemyDownDownLeft = MoveGenerator.IsEnemySquare(board, squareDownDownLeft, pieceColor);

            if (enemyDownDownLeft && squareDownDownLeft == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank > 0 && file > 1)
        {
            enemyDownLeftLeft = MoveGenerator.IsEnemySquare(board, squareDownLeftLeft, pieceColor);
            
            if (enemyDownLeftLeft && squareDownLeftLeft == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank > 1 && file < 7)
        {
            enemyDownDownRight = MoveGenerator.IsEnemySquare(board, squareDownDownRight, pieceColor);

            if (enemyDownDownRight && squareDownDownRight == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }
        if (rank > 0 && file < 6)
        {
            enemyDownRightRight = MoveGenerator.IsEnemySquare(board, squareDownRightRight, pieceColor);

            if (enemyDownRightRight && squareDownRightRight == BoardManager.Instance.kingSquares[enemyColor])
            {
                _causesCheck = true;
                return _causesCheck;
            }
        }

        return _causesCheck;
    }
}
