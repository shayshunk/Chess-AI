using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    private bool _dragging;
    private Vector2 _offset;
    private Vector3 _origPos, placedPos;
    private GameObject[] _circleObjects;

    public GameObject _circlePrefab;
    public int piece, file, rank;
    public Canvas gameCanvas;

    void Awake()
    {
        _origPos = new Vector3(file, rank, -2);
    }

    void Start()
    {
        gameCanvas = BoardManager.Instance.gameCanvas;
    }

    void Update()
    {
        if (!_dragging)
            return;
        
        var mousePos = GetMousePos();

        Vector3 newPos = mousePos - _offset;
        newPos.z = -3;
        transform.position = newPos;
    }

    public Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDown()
    {
        if (BoardManager.Instance.MainBoard.whiteToMove == Piece.IsColor(piece, Piece.White))   
        {
            _dragging = true;
            _offset = GetMousePos() - (Vector2) transform.position;

            int pieceIndex = rank * 8 + file;

            int pieceListIndex = BoardManager.Instance.MainBoard.pieceList.IndexOf(pieceIndex);

            bool[] allowedSquares = BoardManager.Instance.MainBoard.allowedMoves[pieceListIndex];

            for (int i = 0; i < 64; i++)
            {
                int newIndex = i;

                if (!allowedSquares[newIndex])
                    continue;

                int newRank = newIndex / 8;
                int newFile = newIndex % 8;

                GameObject highlightCircle = Instantiate(Resources.Load("Circle") as GameObject, new Vector3(newFile, newRank, -2), Quaternion.identity);
                highlightCircle.tag = "Circle";
                highlightCircle.transform.SetParent(gameCanvas.transform, false);
            }
        }
    }

    void OnMouseUp()
    {
        if (_dragging == true)
        {

            _dragging = false;

            _circleObjects = GameObject.FindGameObjectsWithTag("Circle");
            foreach (GameObject circleObject in _circleObjects)
            {
                Destroy(circleObject);
            }

            placedPos = this.transform.position;
            placedPos = placedPos - gameCanvas.transform.position;
            placedPos = placedPos/gameCanvas.scaleFactor;

            int newFile = Mathf.RoundToInt(placedPos.x);
            int newRank = Mathf.RoundToInt(placedPos.y);

            int oldPos = rank * 8 + file;
            int newPos = newRank * 8 + newFile;
            
            StartCoroutine(BoardManager.Instance.MakeMove(oldPos, newPos));
        }
    }
}
