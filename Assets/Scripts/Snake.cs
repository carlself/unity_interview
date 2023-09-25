using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public event Action<Apple, int> onAppleEaten;
    public event Action onDie;
    [SerializeField] private int m_SnakeLength = 1;
    [SerializeField] private float m_MoveSpeed = 4f;
    [SerializeField] private float m_ShootInterval = 1f;
    [SerializeField] private int m_LenthPerApple = 1;
    [SerializeField] private SnakeSection m_SectionPrefab;
    [SerializeField] private Projectile m_ProjectilePrefab;
    [SerializeField] private Vector2Int m_Direction = Vector2Int.up;
    
    // [SerializeField] private GameBoard ;
    private List<SnakeSection> m_Sections = new List<SnakeSection>();
    private GameBoard m_GameBoard;
    private float m_MoveTimer;
    private float m_ShootTimer;
    private SnakeSection Head => m_Sections[0];
    private int m_NumOfAppleEaten;
    

    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    public void Init(Vector2Int gridPos, GameBoard gameBoard)
    {
        m_GameBoard = gameBoard;
        for (int i = 0; i < m_SnakeLength; i++)
        {
            SnakeSection section = Instantiate(m_SectionPrefab, transform);
            section.SetGridPos(gridPos + i * m_Direction, gameBoard);
            m_Sections.Add(section);
        }

        m_GameBoard.AddSnake(this);
    }
    // Update is called once per frame
    public void HandleUpdate()
    {
        // head pos
        // Vector2Int headGridPos = Head.gridPos;
        Vector2Int direction = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // TryToMove(headGridPos + Vector2Int.left);
            direction = Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // TryToMove(headGridPos + Vector2Int.right);
            direction = Vector2Int.right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 
            direction = Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
           
            direction = Vector2Int.down;
        }
        
        if (m_Direction != -direction && m_Direction != direction && direction != Vector2Int.zero)
        {
            m_Direction = direction;
        }
        
        m_MoveTimer += Time.deltaTime;
        if (m_MoveTimer >= 1f / m_MoveSpeed)
        {
            TryToMove(Head.gridPos + m_Direction);
            m_MoveTimer = 0f;
        }

        m_ShootTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

    }

    void TryToMove(Vector2Int targetGridPos)
    {
        // check if this pos valid
        if (m_GameBoard.IsValidGridPos(targetGridPos))
        {
            PositionalObject obj = m_GameBoard.GetObjectAtGridPos(targetGridPos);
            if (obj == null)
            {
                Move(targetGridPos);
            }
            else
            {
                switch (obj)
                {
                    case Apple apple:
                        Eat(apple);
                        break;
                    case BadGuy badGuy:
                        Die();
                        break;
                }
            }
        }
        else
        {
            // hit a wall
            Die();
        }
    }

    void Move(Vector2Int targetGridPos)
    {
        for (int i = m_Sections.Count - 1; i > 0; i--)
        {
            m_Sections[i].SetGridPos(m_Sections[i-1].gridPos, m_GameBoard);
        }
        
        Head.SetGridPos(targetGridPos, m_GameBoard);
    }
    
    

    void Eat(Apple apple)
    {
        Debug.Log("Eat apple");

        for (int i = 0; i < m_LenthPerApple; i++)
        {
            SnakeSection snakeSection = Instantiate(m_SectionPrefab, transform);
            snakeSection.SetGridPos(apple.gridPos + m_Direction * i, m_GameBoard);
            m_Sections.Insert(0, snakeSection);
        }
        
        m_NumOfAppleEaten++;
        onAppleEaten?.Invoke(apple, m_NumOfAppleEaten);
    }

    void Shoot()
    {
        if (m_ShootTimer < m_ShootInterval)
        {
            return;
        }

        Projectile projectile = Instantiate(m_ProjectilePrefab);
        projectile.transform.position = Head.transform.position;
        projectile.Move(m_Direction);
        m_ShootTimer = 0f;
    }

    void Die()
    {
        onDie?.Invoke();
    }

    public bool IsPartOf(Vector2Int pos)
    {
        foreach (var section in m_Sections)
        {
            if (section.gridPos == pos)
                return true;
        }

        return false;
    }
}
