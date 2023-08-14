using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesster : MonoBehaviour
{
    private bool _playerWhite;
    List<int[]> boardList;
    List<int> evaluationList;
    List<List<int>> allowedMoves;

    void Start()
    {
        _playerWhite = BoardManager.Instance.playerWhite;
    }
    public void BeginThinking(int[] board)
    {
        int indexOffset = 0;

        if (_playerWhite)
            indexOffset = 16;
        
        boardList = new List<int[]>();
        evaluationList = new List<int>();

        allowedMoves = BoardManager.Instance.allowedMoves;

        for (int i = indexOffset; i < (16 + indexOffset); i++)
        {
            int length = allowedMoves[i].Count;

            for (int j = 0; j < length; j++)
            {
                
            }
        }
    }
}
