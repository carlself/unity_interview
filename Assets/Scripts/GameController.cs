using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 GameBoard
 Snake
    Move
    Shoot
    Eat
    Die
    Check collision
 Apple
 
 Enemy

 */

public enum GameState
{
    Running,
    Finished,
}
public class GameController : MonoBehaviour
{
    [SerializeField] private GameOverPanel m_GameOverPanel;
    [SerializeField] private Snake m_Snake;
    [SerializeField] private GameBoard m_GameBoard;
    [SerializeField] private ObjectSpawner m_ObjectSpawner;
    [SerializeField] private int m_TargetApples = 10;
    private GameState m_GameState;

    public GameBoard GameBoard => m_GameBoard;
    public static GameController Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        m_Snake.Init(new Vector2Int(5,5), m_GameBoard);
        m_Snake.onAppleEaten += (apple, numOfEatenApples) =>
        {
            m_ObjectSpawner.SpawnApple();
            m_GameBoard.RemoveObject(apple);
            
            if(numOfEatenApples >= m_TargetApples)
                GameEnd(true);
        };
        m_Snake.onDie += () =>
        {
            GameEnd(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GameState == GameState.Running)
        {
            m_Snake.HandleUpdate();
            m_ObjectSpawner.HandleUpdate();
        }
    }

    void GameEnd(bool win)
    {
        m_GameState = GameState.Finished;
        m_GameOverPanel.gameObject.SetActive(true);
    }
}
