using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private int m_Row, m_Column;

    [SerializeField] private Vector2 m_CellSize;

    [SerializeField] private GameObject m_CellPrefab;
    
    private PositionalObject[] m_Objects;
    private List<Vector2Int> m_FreeCells;
    private Snake m_Snake;
    
    // Start is called before the first frame update
    void Awake()
    {
        CreateBoard(m_Row, m_Column);
    }


    void CreateBoard(int row, int column)
    {
        m_Objects = new PositionalObject[row * column];
        m_FreeCells = new List<Vector2Int>(row * column);
        
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                GameObject cellObj = Instantiate(m_CellPrefab, transform);
                cellObj.transform.position = GetCellPosition(new Vector2Int(c, r));
            }
        }
    }

    public Vector3 GetCellPosition(Vector2Int gridPos)
    {
        return new Vector3( (gridPos.x-m_Column/2f + 0.5f) * m_CellSize.x,
            (gridPos.y -m_Row/2f+0.5f) * m_CellSize.y, 0f) + transform.position;
    }
    
    public void AddObject(PositionalObject obj)
    {
        m_Objects[obj.gridPos.y * m_Column + obj.gridPos.x] = obj;
    }

    private int GridPos2Index(Vector2Int gridPos)
    {
        return m_Column * gridPos.y + gridPos.x;
    }

   
    public Vector2Int GetRandomFreeCellPos()
    {
        m_FreeCells.Clear();
        // collect all the free cells
        for (int r = 0; r < m_Row; r++)
        {
            for (int c = 0; c < m_Column; c++)
            {
                Vector2Int pos = new Vector2Int(c, r);
                if (m_Objects[GridPos2Index(pos)] == null && !m_Snake.IsPartOf(pos))
                {
                    m_FreeCells.Add(pos);
                }
            }
        }

        if (m_FreeCells.Count == 0)
            return new Vector2Int(-1, -1);
        
        // random
        return m_FreeCells[Random.Range(0, m_FreeCells.Count)];
    }

    public bool IsValidGridPos(Vector2Int targetGridPos)
    {
        return targetGridPos.x >= 0 && targetGridPos.x < m_Column 
                                    && targetGridPos.y >= 0 && targetGridPos.y < m_Row;
    }

    public PositionalObject GetObjectAtGridPos(Vector2Int targetGridPos)
    {
        return m_Objects[GridPos2Index(targetGridPos)];
    }

    public void RemoveObject(PositionalObject apple)
    {
        Assert.IsNotNull(m_Objects[GridPos2Index(apple.gridPos)]);
        Destroy(apple.gameObject);
        m_Objects[GridPos2Index(apple.gridPos)] = null;
    }

    public void AddSnake(Snake snake)
    {
        m_Snake = snake;
    }
}
