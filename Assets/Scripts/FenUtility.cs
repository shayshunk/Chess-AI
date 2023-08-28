using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FenUtility
{
    static Dictionary<char, int> pieceTypeFromSymbol = new Dictionary<char, int> () {
			['k'] = Piece.King, ['p'] = Piece.Pawn, ['n'] = Piece.Knight, ['b'] = Piece.Bishop, ['r'] = Piece.Rook, ['q'] = Piece.Queen
		};
    
    public const string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    public const string position2 = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/P1N2Q1p/1PPBBPPP/R3K2R b KQkq - 0 1";
    public const string position3 = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -";
    public const string position4 = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1";
    public const string position5 = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8 ";
    public const string position6 = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10 ";
    public const string position7 = "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1";
    public const string position8 = "4k3/8/8/8/8/8/8/4K2R w K - 0 1";
    public const string position9 = "4k3/8/8/8/8/8/8/R3K3 w Q - 0 1 ";
    public const string position10 = "8/1n4N1/2k5/8/8/5K2/1N4n1/8 b - - 0 1";
    public const string position11 = "8/2k1p3/3pP3/3P2K1/8/8/8/8 b - - 0 1 ";
    public const string position12 = "3k4/3pp3/8/8/8/8/3PP3/3K4 w - - 0 1 ";
    public const string position13 = "n1n5/1Pk5/8/8/8/8/5Kp1/5N1N w - - 0 1 ";
    public const string position14 = "r3k2r/8/8/8/8/8/8/2R1K2R b Kkq - 0 1 ";
    public const string position15 = "2r1k2r/8/8/8/8/8/8/R3K2R w KQk - 0 1 ";

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
