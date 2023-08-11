using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public GameObject piece;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.SelectColor);
        //Instantiate(piece, new Vector3(0, 0, -2), Quaternion.identity);
    }
    
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.SelectColor:
                bool isWhite = true;
                BoardManager.Instance.playerWhite = isWhite;
                BoardManager.Instance.GenerateGrid();
                BoardManager.Instance.LoadStartPosition();
                BoardManager.Instance.DrawPieces();
                Debug.Log("Called Board Manager");
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Draw:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    SelectColor,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Draw,
    Lose
}
