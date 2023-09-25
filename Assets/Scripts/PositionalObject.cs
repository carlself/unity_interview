using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionalObject : MonoBehaviour
{
    public Vector2Int gridPos;

    public void SetGridPos(Vector2Int gridPos, GameBoard gameBoard)
    {
        this.gridPos = gridPos;
        transform.position = gameBoard.GetCellPosition(this.gridPos);
    }
}
