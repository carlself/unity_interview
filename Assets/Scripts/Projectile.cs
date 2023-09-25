using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;

    private Rigidbody2D m_Rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        m_Rigidbody.velocity = direction * m_Speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BadGuy"))
        {
            Destroy(gameObject);
            BadGuy badGuy = other.GetComponent<BadGuy>();
            GameController.Instance.GameBoard.RemoveObject(badGuy);
        }
    }
}
