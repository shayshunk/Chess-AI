using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Evaluator
{
    private static float _evaluation;
    private static int[] values;

    static void Awake()
    {
        values = new int[7];
        values[0] = 0;
        values[1] = 0;
        values[2] = 1;
        values[3] = 3;
        values[5] = 3;
        values[6] = 5;
        values[7] = 9;
    }

    public static void Evaluate(int[] board)
    {
        int whiteValues = 0, blackValues = 0;

        for (int i = 0; i < 64; i++)
        {
            int piece = board[i];
            int pieceType = Piece.PieceType(piece);
            int pieceColor = Piece.Color(piece);

            if (pieceColor == Piece.White)
            {
                whiteValues += values[pieceType];

            } else
            {
                blackValues += values[pieceType];
            }
        }

        _evaluation = whiteValues - blackValues;
    }

    public static float GetEvaluation()
    {
        return _evaluation;
    }
}
