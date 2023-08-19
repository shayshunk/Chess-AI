using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FenUtility
{
    static Dictionary<char, int> pieceTypeFromSymbol = new Dictionary<char, int> () {
			['k'] = Piece.King, ['p'] = Piece.Pawn, ['n'] = Piece.Knight, ['b'] = Piece.Bishop, ['r'] = Piece.Rook, ['q'] = Piece.Queen
		};
    
    public const string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    public const string position2 = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - ";
    public const string position3 = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - ";
    public const string position4 = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1";
    public const string position5 = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8  ";
    public const string position6 = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10 ";

    public static LoadedPositionInfo PositionFromFen(string fen)
    {
        LoadedPositionInfo loadedPosition = new LoadedPositionInfo();

        string[] sections = fen.Split(' ');

        int file = 0;
        int rank = 7;

        foreach (char symbol in sections[0])
        {
            if (symbol == '/')
            {
                file = 0;
                rank--;
            }
            else
            {
                if (char.IsDigit(symbol))
                {
                    file += (int) char.GetNumericValue(symbol);
                }
                else
                {
                    int pieceColor = (char.IsUpper(symbol)) ? Piece.White : Piece.Black;
                    int pieceType = pieceTypeFromSymbol[char.ToLower(symbol)];

                    loadedPosition.squares[rank * 8 + file] = pieceType | pieceColor;
                    file++;
                }
            }
        }

        loadedPosition.whiteToMove = (sections[1] == "w");

        string castlingRights = (sections.Length > 2) ? sections[2] : "KQkq";

        loadedPosition.whiteCastleKingside = castlingRights.Contains("K");
        loadedPosition.whiteCastleQueenside = castlingRights.Contains("Q");
        loadedPosition.blackCastleKingside = castlingRights.Contains("k");
        loadedPosition.blackCastleQueenside = castlingRights.Contains("q");

        if (sections.Length > 3)
        {
            char enPassantFile = sections[3][0];
            loadedPosition.epFile = 88;
            
            if (enPassantFile != '-')
                loadedPosition.epFile = char.ToUpper(enPassantFile) - 64;
        }

        if (sections.Length > 4)
        {
            int.TryParse(sections[4], out loadedPosition.plyCount);
        }

        return loadedPosition;
    }

    public class LoadedPositionInfo
    {
        public int[] squares;
        public bool whiteCastleKingside;
        public bool whiteCastleQueenside;
        public bool blackCastleKingside;
        public bool blackCastleQueenside;
        public int epFile;
        public bool whiteToMove;
        public int plyCount;

        public LoadedPositionInfo()
        {
            squares = new int[64];
        }
    }
}
