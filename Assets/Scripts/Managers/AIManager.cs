using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Evaluator))]

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;
    public int piece, startFile, startRank;
    public int newPosition;

    private bool _playerWhite;
    private float _evaluation;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _playerWhite = BoardManager.Instance.playerWhite;
    }

    public void ChooseBestMove()
    {
        Chesster chesster = new Chesster();

        int[] tempBoard = new int[64];
        BoardManager.Instance.MainBoard.square.CopyTo(tempBoard, 0);

        chesster.BeginThinking(tempBoard);
    }

    public void MakeMove()
    {   
        //MakeRandomMove();
    }

    private void MakeRandomMove()
    {
        /*int pieceOffset = 0;
        if (_playerWhite)
        {
            pieceOffset = 16;
        }

        int randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);

        while (BoardManager.Instance.MainBoard.pieceList[randomPiece] == -1)
        {
            randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);
        }

        int noOfMoves = BoardManager.Instance.MainBoard.allowedMoves[randomPiece].Count;

        while (noOfMoves == 0)
        {
            randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);
            noOfMoves = BoardManager.Instance.MainBoard.allowedMoves[randomPiece].Count;
        }

        Debug.Log("Checking piece at: " + BoardManager.Instance.MainBoard.pieceList[randomPiece]);

        int randomMove = Random.Range(0, noOfMoves);

        Debug.Log("Move will be: " + randomMove);

        piece = BoardManager.Instance.MainBoard.square[BoardManager.Instance.MainBoard.pieceList[randomPiece]];
        startFile = BoardManager.Instance.MainBoard.pieceList[randomPiece] % 8;
        startRank = BoardManager.Instance.MainBoard.pieceList[randomPiece] / 8;
        int file = BoardManager.Instance.MainBoard.allowedMoves[randomPiece][randomMove] % 8;
        int rank = BoardManager.Instance.MainBoard.allowedMoves[randomPiece][randomMove] / 8;

        newPosition = rank * 8 + file; */
    }
    private void Evaluation()
    {
        _evaluation = Evaluator.GetEvaluation();
    }
}
