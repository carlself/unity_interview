using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Apple m_ApplePrefab;
    [SerializeField] private BadGuy m_BadGuyPrefab;

    [SerializeField] private GameBoard m_GameBoard;

    [SerializeField] private float m_BadGuySpawnInterval = 1f;

    private float m_BadGuySpawnTimer;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnApple();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        m_BadGuySpawnTimer += Time.deltaTime;
        if (m_BadGuySpawnTimer >= m_BadGuySpawnInterval)
        {
            SpawnBadGuy();
            m_BadGuySpawnTimer = 0f;
        }
    }

    public void SpawnApple()
    {
        SpawnObject(m_ApplePrefab);
    }

    private void SpawnBadGuy()
    {
        SpawnObject(m_BadGuyPrefab);
    }

    private bool SpawnObject(PositionalObject prefab)
    {
        Vector2Int cellGridPos = m_GameBoard.GetRandomFreeCellPos();
        if(cellGridPos.x == -1)
            return false;
        
        PositionalObject obj = Instantiate(prefab, transform);
        obj.transform.position = m_GameBoard.GetCellPosition(cellGridPos);
        obj.SetGridPos(cellGridPos, m_GameBoard);
        m_GameBoard.AddObject(obj);

        return true;
    }
}
