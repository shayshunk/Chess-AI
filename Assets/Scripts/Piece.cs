using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Piece
{
    // Defining piece values
    public const int None = 0;
    public const int King = 1;
    public const int Pawn = 2;
    public const int Knight = 3;
    public const int Bishop = 5;
    public const int Rook = 6;
    public const int Queen = 7;

    // Defining colors
    public const int White = 8;
    public const int Black = 16;

    // Defining bit masks
    const int typeMask = 0b00111;
    const int blackMask = 0b10000;
    const int whiteMask = 0b01000;
    const int colorMask = whiteMask | blackMask;

    // Defining methods for returning piece type/color

    public static bool IsColor(int piece, int color)
    {
        return (piece & colorMask) == color;
    }

    public static int Color(int piece)
    {
        return piece & colorMask;
    }

    public static int PieceType(int piece)
    {
        return piece & typeMask;
    }
}
