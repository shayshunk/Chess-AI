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
    public Board MainBoard;

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
        int rank = MainBoard.kingSquares[index] / 8;
        int file = MainBoard.kingSquares[index] % 8;

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
                
                if (MainBoard.currentPlayerInCheck)
                {
                    if (x == file && y == rank)
                        spawnedTile.Check();
                }
                /*if (MainBoard.attackedSquares.Contains(y * 8 + x))
                {
                    var highlightTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    highlightTile.tag = "Tile";
                    highlightTile.GetComponent<SpriteRenderer>().sortingLayerName = "Highlight Tile";
                    highlightTile.Attacked();
                }*/
            }
        }
    }

    public void LoadStartPosition()
    {
        rewindButton.interactable = false;
        forwardButton.interactable = false;
        skipFirstButton.interactable = false;
        skipLastButton.interactable = false;

        LoadPosition(FenUtility.startFen);

        whiteToMoveStart = MainBoard.whiteToMove;
    }

    public void LoadPosition(string fen)
    {   
        Initialize();

        var loadedPosition = FenUtility.PositionFromFen(fen);

        MainBoard = new Board(loadedPosition.epFile, loadedPosition.squares, loadedPosition.whiteToMove, loadedPosition.whiteCastleKingside, 
                            loadedPosition.whiteCastleQueenside, loadedPosition.blackCastleKingside, loadedPosition.blackCastleQueenside);

        if (!playerWhite)
            System.Array.Reverse(MainBoard.square);

        boardHistory.Push(MainBoard.square.Clone() as int[]);
        List<int> tempPieceList = new List<int>(MainBoard.pieceList);
        pieceListHistory.Push(tempPieceList);
        currentBoardHistoryIndex = 0;

        epFileHistory.Push(MainBoard.epFile);

        int[] highlightArr = new int[2];
        highlightArr[0] = 100;
        highlightArr[1] = 100;
        highlightHistory.Push(highlightArr.Clone() as int[]);

        //int numPositions = MoveGenerationTest(2);
        //Debug.Log("Positions found: " + numPositions);
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
                if (MainBoard.square[rank * 8 + file] == 0)
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

                int piece = MainBoard.square[rank * 8 + file];
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
        playerWhite = true;
        AIenabled = false;

        epFileHistory = new Stack<int>();
        boardHistory = new Stack<int[]>();
        highlightHistory = new Stack<int[]>();
        pieceListHistory = new Stack<List<int>>();
        plyCount = 0;
        fiftyMoveCounter = 0;
    }

    public void MakeMove(int pieceIndex, int newIndex)
    {
        //Debug.Log("Make move called.");

        int file = newIndex % 8;
        int rank = newIndex / 8;

        int startFile = pieceIndex % 8;
        int startRank = pieceIndex / 8;

        int piece = MainBoard.square[pieceIndex];

        int pieceListIndex = MainBoard.pieceList.IndexOf(pieceIndex);

        if (MainBoard.allowedMoves[pieceListIndex].Count == 0)
        {
            GenerateGrid();
            DrawPieces();
            return;
        }

        if (!MainBoard.allowedMoves[pieceListIndex].Contains(rank * 8 + file))
        {
            GenerateGrid();
            DrawPieces();
            return;
        }

        MainBoard.MakeMove(pieceIndex, newIndex);

        whiteToMoveEnd = MainBoard.whiteToMove;

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();

        soundPlayer.PlayOneShot(MainBoard.clipToPlay);

        rewindButton.interactable = true;
        skipFirstButton.interactable = true;

        checkmate = MainBoard.checkmate;

        while (currentBoardHistoryIndex != 0)
        {
            boardHistory.Pop();
            pieceListHistory.Pop();
            highlightHistory.Pop();
            currentBoardHistoryIndex--;
        }

        boardHistory.Push(MainBoard.square.Clone() as int[]);
        List<int> tempPieceList = new List<int>(MainBoard.pieceList);
        pieceListHistory.Push(tempPieceList);

        epFileHistory.Push(MainBoard.epFile);

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

        //Debug.Log("Trying to make move through AI");
        
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
        int oldPos = startRank * 8 + startFile;
        int newPos = AIManager.Instance.newPosition;

        MakeMove(oldPos, newPos);
    }

    public void UnmakeMove()
    {
        //Debug.Log("Unmaking move.");
        int length = boardHistory.Count;

        boardHistory.ElementAt(currentBoardHistoryIndex + 1).CopyTo(MainBoard.square, 0);
        MainBoard.pieceList = pieceListHistory.ElementAt(currentBoardHistoryIndex + 1);
        MainBoard.epFile = epFileHistory.ElementAt(currentBoardHistoryIndex + 1);

        int startFile = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[0] / 8;
        int file = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[1] % 8;
        int rank = highlightHistory.ElementAt(currentBoardHistoryIndex + 1)[1] / 8;

        MainBoard.kingSquares[whiteIndex] = Array.IndexOf(MainBoard.square, 9);
        MainBoard.kingSquares[blackIndex] = Array.IndexOf(MainBoard.square, 17);

        MainBoard.whiteToMove = !MainBoard.whiteToMove;

        PinHandler.GeneratePins(MainBoard);

        MainBoard.pinnedPieces = PinHandler.GetPins();
        if (MainBoard.pinnedPieces.Count != 0)
            MainBoard.pinnedDirection = PinHandler.GetPinDirections();

        MainBoard.GenerateAllAttackedSquares();
        MainBoard.IsInCheck();

        MainBoard.GenerateAllowedMoves();

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
        //Debug.Log("Remaking move.");
        
        boardHistory.ElementAt(currentBoardHistoryIndex - 1).CopyTo(MainBoard.square, 0);
        MainBoard.pieceList = pieceListHistory.ElementAt(currentBoardHistoryIndex - 1);
        MainBoard.epFile = epFileHistory.ElementAt(currentBoardHistoryIndex - 1);

        int startFile = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[0] / 8;
        int file = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[1] % 8;
        int rank = highlightHistory.ElementAt(currentBoardHistoryIndex - 1)[1] / 8;

        MainBoard.kingSquares[whiteIndex] = Array.IndexOf(MainBoard.square, 9);
        MainBoard.kingSquares[blackIndex] = Array.IndexOf(MainBoard.square, 17);

        MainBoard.whiteToMove = !MainBoard.whiteToMove;

        PinHandler.GeneratePins(MainBoard);

        MainBoard.pinnedPieces = PinHandler.GetPins();
        if (MainBoard.pinnedPieces.Count != 0)
            MainBoard.pinnedDirection = PinHandler.GetPinDirections();

        MainBoard.GenerateAllAttackedSquares();
        MainBoard.IsInCheck();

        MainBoard.GenerateAllowedMoves();

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

        boardHistory.ElementAt(length - 1).CopyTo(MainBoard.square, 0);
        List<int> tempPieces = new List<int>(pieceListHistory.ElementAt(length - 1));
        MainBoard.pieceList = tempPieces;
        MainBoard.epFile = epFileHistory.ElementAt(length - 1);

        int startFile = highlightHistory.ElementAt(length - 1)[0] % 8;
        int startRank = highlightHistory.ElementAt(length - 1)[0] / 8;
        int file = highlightHistory.ElementAt(length - 1)[1] % 8;
        int rank = highlightHistory.ElementAt(length - 1)[1] / 8;

        MainBoard.kingSquares[whiteIndex] = Array.IndexOf(MainBoard.square, 9);
        MainBoard.kingSquares[blackIndex] = Array.IndexOf(MainBoard.square, 17);

        MainBoard.whiteToMove = whiteToMoveStart;

        PinHandler.GeneratePins(MainBoard);

        MainBoard.pinnedPieces = PinHandler.GetPins();
        if (MainBoard.pinnedPieces.Count != 0)
            MainBoard.pinnedDirection = PinHandler.GetPinDirections();

        MainBoard.GenerateAllAttackedSquares();
        MainBoard.IsInCheck();

        MainBoard.GenerateAllowedMoves();

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

        boardHistory.ElementAt(0).CopyTo(MainBoard.square, 0);
        MainBoard.pieceList = pieceListHistory.ElementAt(0);
        MainBoard.epFile = epFileHistory.ElementAt(0);

        int startFile = highlightHistory.ElementAt(0)[0] % 8;
        int startRank = highlightHistory.ElementAt(0)[0] / 8;
        int file = highlightHistory.ElementAt(0)[1] % 8;
        int rank = highlightHistory.ElementAt(0)[1] / 8;

        MainBoard.kingSquares[whiteIndex] = Array.IndexOf(MainBoard.square, 9);
        MainBoard.kingSquares[blackIndex] = Array.IndexOf(MainBoard.square, 17);

        MainBoard.whiteToMove = whiteToMoveEnd;

        PinHandler.GeneratePins(MainBoard);

        MainBoard.pinnedPieces = PinHandler.GetPins();
        if (MainBoard.pinnedPieces.Count != 0)
            MainBoard.pinnedDirection = PinHandler.GetPinDirections();

        MainBoard.GenerateAllAttackedSquares();
        MainBoard.IsInCheck();
        
        MainBoard.GenerateAllowedMoves();

        GenerateGrid(startRank, startFile, rank, file);
        DrawPieces();
        currentBoardHistoryIndex = 0;

        forwardButton.interactable = false;
        skipLastButton.interactable = false;

        rewindButton.interactable = true;
        skipFirstButton.interactable = true;
    }

    public int MoveGenerationTest (Board board, int depth)
    {
        int[] tempBoard = new int[64];
        board.CopyTo(tempBoard, 0);

        int numPositions = 0;

        List<List<int>> newAllowedMoves = new List<List<int>>();
        List<int> tempAttackedSquares = new List<int>();

        for (int i = 0; i < 64; i++)
        {
            if (pieceList[i] == -1)
                    continue;

            if (whiteMove != Piece.IsColor(square[pieceList[i]], Piece.White))
            {   
                MoveGenerator.GenerateLegal(MainBoard, pieceList[i], currentPlayerInCheck);
                tempAttackedSquares = MoveGenerator.GetAttackedSquares();

                attackedSquares = Enumerable.Union(attackedSquares, tempAttackedSquares).ToList();
            }
            else
            {
                continue;
            }
        }
        

        while (depth != 0)
        {
            foreach (List<int> moveList in allowedMoves)
            {
                foreach (int move in moveList)
                {
                    int pieceListIndex = allowedMoves.IndexOf(moveList);
                    int pieceIndex = pieceList[pieceListIndex];

                    MakeMove(pieceIndex, move);

                }
            }
        }

        return numPositions;
    }

    public bool PseudoMakeMove(Board tempBoard, int pieceIndex, int newIndex)
    {
        int file = newIndex % 8;
        int rank = newIndex / 8;

        int startFile = pieceIndex % 8;
        int startRank = pieceIndex / 8;

        int piece = tempBoard.square[pieceIndex];

        int pieceListIndex = tempBoard.pieceList.IndexOf(pieceIndex);

        tempBoard.MakeMove(pieceIndex, newIndex);

        bool tempCheckmate = tempBoard.checkmate;

        return tempCheckmate;
    }
}
