using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Evaluator))]

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;
    public int piece, startFile, startRank;
    public Vector3 newPosition;

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
        BoardManager.Instance.square.CopyTo(tempBoard, 0);

        chesster.BeginThinking(tempBoard);
    }

    public void MakeMove()
    {   
        //MakeRandomMove();
    }

    private void MakeRandomMove()
    {
        int pieceOffset = 0;
        if (_playerWhite)
        {
            pieceOffset = 16;
        }

        int randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);

        while (BoardManager.Instance.pieceList[randomPiece] == -1)
        {
            randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);
        }

        int noOfMoves = BoardManager.Instance.allowedMoves[randomPiece].Count;

        while (noOfMoves == 0)
        {
            randomPiece = Random.Range(0 + pieceOffset, 16 + pieceOffset);
            noOfMoves = BoardManager.Instance.allowedMoves[randomPiece].Count;
        }

        Debug.Log("Checking piece at: " + BoardManager.Instance.pieceList[randomPiece]);

        int randomMove = Random.Range(0, noOfMoves);

        Debug.Log("Move will be: " + randomMove);

        piece = BoardManager.Instance.square[BoardManager.Instance.pieceList[randomPiece]];
        startFile = BoardManager.Instance.pieceList[randomPiece] % 8;
        startRank = BoardManager.Instance.pieceList[randomPiece] / 8;
        int file = BoardManager.Instance.allowedMoves[randomPiece][randomMove] % 8;
        int rank = BoardManager.Instance.allowedMoves[randomPiece][randomMove] / 8;

        newPosition = new Vector3(file, rank, -2);  
    }
    private void Evaluation()
    {
        _evaluation = Evaluator.GetEvaluation();
    }
}
