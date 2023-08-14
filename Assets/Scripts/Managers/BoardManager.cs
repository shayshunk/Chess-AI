using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    public Canvas gameCanvas;
    public Button rewindButton, forwardButton, skipLastButton, skipFirstButton;

    public static BoardManager Instance;
    public Sprite[] whiteSprites;
    public Sprite[] blackSprites;

    private GameObject[] _pieceObjects, _tileObjects;

    public AudioSource soundPlayer;
    public AudioClip notify, capture, regularMove;

    // Game variables
    public bool playerWhite, AIenabled, whiteToMove, currentPlayerInCheck, checkmate;
    public int[] square;
    public int[] kingSquares;
    public List<int> pieceList, attackedSquares, pinnedPieces, pinnedDirection;
    public List<List<int>> allowedMoves;
    public bool[] squaresAroundBlackKing;
    public bool[] squaresAroundWhiteKing;

    public int whiteIndex = 0;
    public int blackIndex = 1;

    public int plyCount, fiftyMoveCounter, currentBoardHistoryIndex;
    public bool whiteToMoveStart, whiteToMoveEnd;
    Stack<int> epFileHistory;
    Stack<int[]> boardHistory;
    Stack<int[]> highlightHistory;
    Stack<List<int>> pieceListHistory;

    public bool whiteCastleKingside, whiteCastleQueenside, blackCastleKingside, blackCastleQueenside;
    public int epFile;

    void Awake()
    {
        Instance = this;

        notify = (AudioClip) Resources.Load("notify");
        capture = (AudioClip) Resources.Load("capture");
        regularMove = (AudioClip) Resources.Load("move-self");

    }

    public void GenerateGrid(int oldRank = 10, int oldFile = 10, int newRank = 10, int newFile = 10)
    {
        _tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        
        foreach (GameObject _tileObj in _tileObjects)
            Destroy(_tileObj);

        int index = whiteToMove? whiteIndex : blackIndex;
        int rank = kingSquares[index] / 8;
        int file = kingSquares[index] % 8;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.GetComponent<SpriteRenderer>().sortingLayerName = "Regular Tile";
                spawnedTile.tag = "Tile";
                spawnedTile.transform.SetParent(gameCanvas.transform, false);

                var isDarkColor = (x + y) % 2 == 0;
                spawnedTile.Init(isDarkColor);

                if (x == oldFile && y == oldRank)
                {
                    var highlightTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    highlightTile.tag = "Tile";
                    highlightTile.GetComponent<SpriteRenderer>().sortingLayerName = "Highlight Tile";
                    highlightTile.transform.SetParent(gameCanvas.transform, false);
                    highlightTile.Highlight();
                }

                if (x == newFile && y == newRank)
                {
                    var highlightTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    highlightTile.tag = "Tile";
                    highlightTile.GetComponent<SpriteRenderer>().sortingLayerName = "Highlight Tile";
                    highlightTile.transform.SetParent(gameCanvas.transform, false);
                    highlightTile.Highlight();
                }
                
                if (currentPlayerInCheck)
                {
                    if (x == file && y == rank)
                        spawnedTile.Check();
                }
                /*if (attackedSquares.Contains(y * 8 + x))
                {
                    var highlightTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    highlightTile.tag = "Tile";
                    highlightTile.GetComponent<SpriteRenderer>().sortingLayerName = "Highlight Tile";
                    highlightTile.Attacked();
                }*/
            }
        }

        //_cam.transform.SetParent(gameCanvas.transform, false);
        //_cam.transform.position = new Vector3(0, 0, -10);
    }

    public void LoadStartPosition()
    {
        rewindButton.interactable = false;
        forwardButton.interactable = false;
        skipFirstButton.interactable = false;
        skipLastButton.interactable = false;

        LoadPosition(FenUtility.startFen);

        kingSquares[whiteIndex] = 4;
        kingSquares[blackIndex] = 60;

        whiteToMove = true;
        whiteToMoveStart = whiteToMove;
    }

    public void LoadPosition(string fen)
    {   
        Initialize();

        var loadedPosition = FenUtility.PositionFromFen(fen);

        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = loadedPosition.squares[squareIndex];
            square[squareIndex] = piece;
            if (piece != 0)
                pieceList.Add(squareIndex);
        }

        whiteCastleKingside = loadedPosition.whiteCastleKingside;
        whiteCastleQueenside = loadedPosition.whiteCastleQueenside;
        blackCastleKingside = loadedPosition.blackCastleKingside;
        blackCastleQueenside = loadedPosition.blackCastleQueenside;

        epFile = loadedPosition.epFile;

        if (!playerWhite)
            System.Array.Reverse(square);

        GenerateAllowedMoves();

        boardHistory.Push(square.Clone() as int[]);
        List<int> tempPieceList = new List<int>(pieceList);
        pieceListHistory.Push(tempPieceList);
        currentBoardHistoryIndex = 0;

        epFileHistory.Push(epFile);

        int[] highlightArr = new int[2];
        highlightArr[0] = 100;
        highlightArr[1] = 100;
        highlightHistory.Push(highlightArr.Clone() as int[]);
    }

    public void DrawPieces()
    {
        _pieceObjects = GameObject.FindGameObjectsWithTag("Piece");
        
        foreach (GameObject _pieceObj in _pieceObjects)
            Destroy(_pieceObj);
        
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file ++)
            {
                if (square[rank * 8 + file] == 0)
                    continue;
                
                GameObject pieceObj = new();
                pieceObj.name = $"Piece{file}{rank}";
                pieceObj.AddComponent<BoxCollider2D>();
                pieceObj.GetComponent<BoxCollider2D>().size = new Vector2(3f, 3f);
                pieceObj.gameObject.tag = "Piece";
                pieceObj.transform.SetParent(gameCanvas.transform, true);

                SpriteRenderer pieceRenderer = pieceObj.AddComponent<SpriteRenderer>();
                pieceRenderer.sortingLayerName = "Piece";
                
                pieceObj.transform.localPosition = new Vector3(file, rank, -2);
                pieceObj.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                pieceObj.AddComponent<DragDrop>();

                int piece = square[rank * 8 + file];
                pieceRenderer.sprite = GetSprite(piece);
                pieceObj.GetComponent<DragDrop>().piece = piece;
                pieceObj.GetComponent<DragDrop>().file = file;
                pieceObj.GetComponent<DragDrop>().rank = rank;
            }
        }
    }

    private Sprite GetSprite(int piece)
    {
        int pieceColor = Piece.IsColor(piece, Piece.White) ? whiteIndex : blackIndex;
        int pieceType = Piece.PieceType(piece);

        Sprite pieceSprite;

        if (pieceColor == 0)
        {
            pieceSprite = whiteSprites[pieceType];
        } else
        {
            pieceSprite = blackSprites[pieceType];
        }

        return pieceSprite;
    }

    void Initialize()
    {
        square = new int[64];
        kingSquares = new int[2];
        squaresAroundBlackKing = new bool[8];
        squaresAroundWhiteKing = new bool[8];

        playerWhite = true;
        AIenabled = false;
        currentPlayerInCheck = false;
        whiteToMove = true;
        checkmate = false;

        epFileHistory = new Stack<int>();
        boardHistory = new Stack<int[]>();
        highlightHistory = new Stack<int[]>();
        pieceListHistory = new Stack<List<int>>();
        plyCount = 0;
        fiftyMoveCounter = 0;

        for (int i = 0; i < 8; i++)
        {
            squaresAroundWhiteKing[i] = false;
            squaresAroundBlackKing[i] = false;
        }

        pieceList = new List<int>();
        attackedSquares = new List<int>();
        pinnedPieces = new List<int>();
        pinnedDirection = new List<int>();
        allowedMoves = new List<List<int>>();
    }

    public void GenerateAllowedMoves()
    {
        int length = pieceList.Count;

        if (allowedMoves.Count == 0)
        {
            GenerateAllAttackedSquares();

            for (int i = 0; i < length; i++)
            {
                if (pieceList[i] == -1)
                    continue;
                   
                List<int> allowedSquares = MoveGenerator.GenerateLegal(pieceList[i], currentPlayerInCheck);
                allowedMoves.Add(allowedSquares);
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
            
            List<int> allowedSquares = MoveGenerator.GenerateLegal(pieceList[i], currentPlayerInCheck);
            allowedMoves[i] = allowedSquares;
        }

        if (currentPlayerInCheck)
        {
            Debug.Log("Current player is in check so let's remove some moves.");

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
                MoveGenerator.GenerateLegal(pieceList[i], currentPlayerInCheck);
                tempAttackedSquares = MoveGenerator.GetAttackedSquares();

                attackedSquares = Enumerable.Union(attackedSquares, tempAttackedSquares).ToList();
            }
            else
            {
                continue;
            }
        }
    }

    public void GenerateSlidingAttackedSquares(int oldPieceIndex, int newPieceIndex)
    {
        int length = pieceList.Count;

        List<int> tempAttackedSquares = new List<int>();
        List<int> oldAttackedSquares = new List<int>();

        MoveGenerator.GenerateLegal(oldPieceIndex, currentPlayerInCheck);
        oldAttackedSquares = MoveGenerator.GetAttackedSquares();

        attackedSquares = attackedSquares.Except(oldAttackedSquares).ToList();
        
        MoveGenerator.GenerateLegal(newPieceIndex, currentPlayerInCheck);
        tempAttackedSquares = MoveGenerator.GetAttackedSquares();

        attackedSquares = Enumerable.Union(attackedSquares, tempAttackedSquares).ToList();

        for (int i = 0; i < length; i++)
        {
            if (pieceList[i] == -1)
                continue;
                
            if (whiteToMove == Piece.IsColor(square[pieceList[i]], Piece.White))
            {
                
                int pieceType = square[pieceList[i]];

                if (!(pieceType == Piece.Queen || pieceType == Piece.Bishop || pieceType == Piece.Rook))
                    continue;
                
                Debug.Log("Regenerating attacked squares for: " + square[pieceList[i]]);
                MoveGenerator.GenerateLegal(pieceList[i], currentPlayerInCheck);
                tempAttackedSquares = MoveGenerator.GetAttackedSquares();

                attackedSquares = Enumerable.Union(attackedSquares, tempAttackedSquares).ToList();
            }
            else
            {
                continue;
            }
        }
    }

    public void RemoveIllegalMoves()
    {
        int[] tempSquare = new int[64];
        bool tempCheck = currentPlayerInCheck;

        List<List<int>> tempAllowed = new List<List<int>>();
        List<int> tempMoves = new List<int>();

        foreach (List<int> moveList in allowedMoves)
        {
            square.CopyTo(tempSquare, 0);
            tempMoves = new List<int>();

            int pieceListIndex = allowedMoves.IndexOf(moveList);

            if (pieceList[pieceListIndex] == -1)
                continue;
            
            int piece = square[pieceList[pieceListIndex]];

            if (whiteToMove != Piece.IsColor(piece, Piece.White))
            {
                continue;
            }

            int pieceType = Piece.PieceType(piece);
            int kingColor = Piece.IsColor(piece, Piece.White)? whiteIndex : blackIndex;

            foreach (int move in moveList)
            {
                square[move] = piece;
                square[pieceList[pieceListIndex]] = 0;
                int oldKingSquare = kingSquares[kingColor];

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
            }

            tempAllowed.Add(tempMoves);
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

    public void MakeMove(int piece, int startFile, int startRank, Vector3 newPosition)
    {
        Debug.Log("Make move called.");

        AudioClip clipToPlay = regularMove;
        
        int file = Mathf.RoundToInt(newPosition.x);
        int rank = Mathf.RoundToInt(newPosition.y);

        int pieceIndex = startRank * 8 + startFile;
        int pieceListIndex = pieceList.IndexOf(pieceIndex);

        if (allowedMoves[pieceListIndex].Count == 0)
        {
            DrawPieces();
            return;
        }

        if (!allowedMoves[pieceListIndex].Contains(rank * 8 + file))
        {
            DrawPieces();
            return;
        }

        int takenPieceIndex = -1;

        if (square[rank * 8 + file] != 0)
        {
            takenPieceIndex = rank * 8 + file;
            clipToPlay = capture;
        }
        
        whiteToMove = !whiteToMove;
        whiteToMoveEnd = whiteToMove;

        pinnedPieces.Clear();
        pinnedDirection.Clear();
        
        int pieceColor = Piece.Color(piece);
        int pieceType = Piece.PieceType(piece);

        if (pieceType == Piece.Pawn) 
        {
            if (rank == 7 || rank == 0)
                pieceType = Piece.Queen;
            
            if ((startFile == epFile + 1 || startFile == epFile - 1) && file == epFile)
            {
                if (startRank == 4 && pieceColor == Piece.White)
                {
                    square[startRank * 8 + epFile] = 0;
                    takenPieceIndex = startRank * 8 + epFile;
                }
                else if (startRank == 3 && pieceColor == Piece.Black)
                {
                    square[startRank * 8 + epFile] = 0;
                    takenPieceIndex = startRank * 8 + epFile;
                }
            }

            if ((startRank == 1 && rank == 3) || (startRank == 6 && rank == 4))
                epFile = file;
            else
                epFile = 88;

        }

        else if (pieceType == Piece.King)
        {
            if (pieceColor == Piece.White)
            {
                kingSquares[whiteIndex] = rank * 8 + file;

                if (rank * 8 + file == 6 && whiteCastleKingside && square[7] == 14)
                {
                    square[7] = 0;
                    square[5] = 14;
                }
                else if (rank * 8 + file == 2 && whiteCastleQueenside && square[0] == 14)
                {
                    square[0] = 0;
                    square[3] = 14;
                }

                whiteCastleKingside = false;
                whiteCastleQueenside = false;
            }
            else
            {
                kingSquares[blackIndex] = rank * 8 + file;

                if (rank * 8 + file == 62 && blackCastleKingside && square[63] == 22)
                {
                    square[63] = 0;
                    square[61] = 22;
                }
                else if (rank * 8 + file == 58 && blackCastleQueenside && square[56] == 22)
                {
                    square[56] = 0;
                    square[59] = 22;
                }

                blackCastleKingside = false;
                blackCastleQueenside = false;
            }
        }

        else if (pieceType == Piece.Rook)
        {
            if (startRank * 8 + startFile == 0)
                whiteCastleQueenside = false;
            else if (startRank * 8 + startFile == 7)
                whiteCastleKingside = false;
            else if (startRank * 8 + startFile == 56)
                blackCastleQueenside = false;
            else if (startRank * 8 + startFile == 63)
                blackCastleKingside = false;
        }

        else
        {
            epFile = 88;
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

            Debug.Log("Piece at: " + takenPieceIndex + " was taken!");
        }

        square[startRank * 8 + startFile] = 0;
        square[rank * 8 + file] = piece;

        if (pieceListIndex != -1)
            pieceList[pieceListIndex] = rank * 8 + file;
        
        if (whiteToMove)
            squaresAroundWhiteKing = PinHandler.GeneratePins();
        
        else
            squaresAroundBlackKing = PinHandler.GeneratePins();

        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
            pinnedDirection = PinHandler.GetPinDirections();
        
        GenerateAllAttackedSquares();
        //GenerateSlidingAttackedSquares(startRank * 8 + startFile, rank * 8 + file);

        IsInCheck();

        if (currentPlayerInCheck)
            clipToPlay = notify;

        GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();

        rewindButton.interactable = true;
        skipFirstButton.interactable = true;

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
        
        Debug.Log("Current player in check?: " + currentPlayerInCheck);
        Debug.Log("Checkmate? " + checkmate);

        soundPlayer.PlayOneShot(clipToPlay);

        while (currentBoardHistoryIndex != 0)
        {
            boardHistory.Pop();
            pieceListHistory.Pop();
            highlightHistory.Pop();
            currentBoardHistoryIndex--;
        }

        boardHistory.Push(square.Clone() as int[]);
        List<int> tempPieceList = new List<int>(pieceList);
        pieceListHistory.Push(tempPieceList);

        epFileHistory.Push(epFile);

        int[] highlightArr = new int[2];
        highlightArr[0] = startRank * 8 + startFile;
        highlightArr[1] = rank * 8 + file;
        highlightHistory.Push(highlightArr.Clone() as int[]);

        TurnHandler();
    }

    public void TurnHandler()
    {
        if (!AIenabled)
            return;

        if (checkmate)
        {
            Debug.Log("You won, checkmate!");
            return;
        }

        Debug.Log("Trying to make move through AI");
        
        if (playerWhite && !whiteToMove)
        {
            AIManager.Instance.MakeMove();
        }
        else if (!playerWhite && whiteToMove)
        {
            AIManager.Instance.MakeMove();
        }
        else
        {
            return;
        }

        int piece = AIManager.Instance.piece;
        int startFile = AIManager.Instance.startFile;
        int startRank = AIManager.Instance.startRank;
        Vector3 newPos = AIManager.Instance.newPosition;

        MakeMove(piece, startFile, startRank, newPos);
    }

    public void UnmakeMove()
    {
        Debug.Log("Unmaking move.");
        int length = boardHistory.Count;

        boardHistory.ElementAt(currentBoardHistoryIndex + 1).CopyTo(square, 0);
        pieceList = pieceListHistory.ElementAt(currentBoardHistoryIndex + 1);
        epFile = epFileHistory.ElementAt(currentBoardHistoryIndex + 1);

        int startFile = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[0] / 8;
        int file = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[1] % 8;
        int rank = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[1] / 8;

        kingSquares[whiteIndex] = Array.IndexOf(square, 9);
        kingSquares[blackIndex] = Array.IndexOf(square, 17);

        whiteToMove = !whiteToMove;

        if (whiteToMove)
            squaresAroundWhiteKing = PinHandler.GeneratePins();
        
        else
            squaresAroundBlackKing = PinHandler.GeneratePins();

        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
            pinnedDirection = PinHandler.GetPinDirections();

        GenerateAllAttackedSquares();
        IsInCheck();

        GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();
        currentBoardHistoryIndex++;

        forwardButton.interactable = true;
        skipLastButton.interactable = true;

        if (currentBoardHistoryIndex + 1 == length)
        {
            rewindButton.interactable = false;
            skipFirstButton.interactable = false;
        }
    }

    public void RemakeMove()
    {
        Debug.Log("Remaking move.");
        
        boardHistory.ElementAt(currentBoardHistoryIndex - 1).CopyTo(square, 0);
        pieceList = pieceListHistory.ElementAt(currentBoardHistoryIndex - 1);
        epFile = epFileHistory.ElementAt(currentBoardHistoryIndex - 1);

        int startFile = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[0] / 8;
        int file = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[1] % 8;
        int rank = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[1] / 8;

        kingSquares[whiteIndex] = Array.IndexOf(square, 9);
        kingSquares[blackIndex] = Array.IndexOf(square, 17);

        whiteToMove = !whiteToMove;

        if (whiteToMove)
            squaresAroundWhiteKing = PinHandler.GeneratePins();
        
        else
            squaresAroundBlackKing = PinHandler.GeneratePins();

        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
            pinnedDirection = PinHandler.GetPinDirections();

        GenerateAllAttackedSquares();
        IsInCheck();

        GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();
        currentBoardHistoryIndex--;

        rewindButton.interactable = true;
        skipFirstButton.interactable = true;

        if (currentBoardHistoryIndex == 0)
        {
            forwardButton.interactable = false;
            skipLastButton.interactable = false;
        }
    }

    public void JumpToFirstPosition()
    {
        Debug.Log("Jumping to earliest position.");
        int length = boardHistory.Count;

        boardHistory.ElementAt(length - 1).CopyTo(square, 0);
        pieceList = pieceListHistory.ElementAt(length - 1);
        epFile = epFileHistory.ElementAt(length - 1);

        int startFile = highlightHistory.ElementAt(length - 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(length - 1)[0] / 8;
        int file = highlightHistory.ElementAt(length - 1)[1] % 8;
        int rank = highlightHistory.ElementAt(length - 1)[1] / 8;

        kingSquares[whiteIndex] = Array.IndexOf(square, 9);
        kingSquares[blackIndex] = Array.IndexOf(square, 17);

        whiteToMove = whiteToMoveStart;

        if (whiteToMove)
            squaresAroundWhiteKing = PinHandler.GeneratePins();
        
        else
            squaresAroundBlackKing = PinHandler.GeneratePins();

        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
            pinnedDirection = PinHandler.GetPinDirections();

        GenerateAllAttackedSquares();
        IsInCheck();

        GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();
        currentBoardHistoryIndex = length - 1;

        rewindButton.interactable = false;
        skipFirstButton.interactable = false;

        forwardButton.interactable = true;
        skipLastButton.interactable = true;
    }

    public void JumpToLatestPosition()
    {
        Debug.Log("Jumping to most recent position.");

        boardHistory.ElementAt(0).CopyTo(square, 0);
        pieceList = pieceListHistory.ElementAt(0);
        epFile = epFileHistory.ElementAt(0);

        int startFile = highlightHistory.ElementAt(0)[0] % 8;
        int startRank = highlightHistory.ElementAt(0)[0] / 8;
        int file = highlightHistory.ElementAt(0)[1] % 8;
        int rank = highlightHistory.ElementAt(0)[1] / 8;

        kingSquares[whiteIndex] = Array.IndexOf(square, 9);
        kingSquares[blackIndex] = Array.IndexOf(square, 17);

        whiteToMove = whiteToMoveEnd;

        if (whiteToMove)
            squaresAroundWhiteKing = PinHandler.GeneratePins();
        
        else
            squaresAroundBlackKing = PinHandler.GeneratePins();

        pinnedPieces = PinHandler.GetPins();
        if (pinnedPieces.Count != 0)
            pinnedDirection = PinHandler.GetPinDirections();

        GenerateAllAttackedSquares();
        IsInCheck();
        
        GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();
        currentBoardHistoryIndex = 0;

        forwardButton.interactable = false;
        skipLastButton.interactable = false;

        rewindButton.interactable = true;
        skipFirstButton.interactable = true;
    }
}
