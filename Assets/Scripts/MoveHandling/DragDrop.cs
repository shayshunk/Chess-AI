using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    private bool _dragging;
    private Vector2 _offset;
    private Vector3 _origPos, placedPos;
    public GameObject _circlePrefab;
    private GameObject[] _circleObjects;
    public int piece, file, rank;

    void Awake()
    {
        _origPos = new Vector3(file, rank, -2);
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
        if (BoardManager.Instance.whiteToMove == Piece.IsColor(piece, Piece.White))   
        {
            _dragging = true;
            _offset = GetMousePos() - (Vector2) transform.position;

            int pieceIndex = rank * 8 + file;

            int pieceListIndex = BoardManager.Instance.pieceList.IndexOf(pieceIndex);

            List<int> allowedSquares = BoardManager.Instance.allowedMoves[pieceListIndex];

            foreach (int index in allowedSquares)
            {
                int newRank = index / 8;
                int newFile = index % 8;
                GameObject highlightCircle = Instantiate(Resources.Load("Circle") as GameObject, new Vector3(newFile, newRank, -2), Quaternion.identity);
                highlightCircle.tag = "Circle";
            }
        }
    }

    void OnMouseUp()
    {
        if (_dragging == true)
        {
            var mousePos = GetMousePos();

            placedPos = mousePos - _offset;
            placedPos.z = -2;

            _dragging = false;

            _circleObjects = GameObject.FindGameObjectsWithTag("Circle");
            foreach (GameObject circleObject in _circleObjects)
            {
                Destroy(circleObject);
            }

            BoardManager.Instance.MakeMove(piece, file, rank, placedPos);
        }
    }
}
