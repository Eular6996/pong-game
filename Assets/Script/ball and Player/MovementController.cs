using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float m_speed;
    private Rigidbody2D m_rigidbody2D;

    private bool m_canUpMove;
    private bool m_canDownMove;

    [SerializeField] ScoreAndGameOverManager scoreManager;

    private void Awake()
    {
        m_canUpMove = true;
        m_canDownMove = true;
    }
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!scoreManager.Player1Win &&  !scoreManager.Player2Win)
        {
            HandleInput();
        }
    }

    //Handle Input
    void HandleInput()
    {
        if (this.gameObject.CompareTag("Player1"))
        {
            TakeInput(KeyCode.UpArrow, KeyCode.DownArrow);
        }
        else if(this.gameObject.CompareTag("Player2"))
        {
            TakeInput(KeyCode.W, KeyCode.S);
        }
    }

    //take Actual Input
    void TakeInput(KeyCode up, KeyCode down)
    {
        if (Input.GetKey(up) && m_canUpMove)
        { 
            m_rigidbody2D.velocity = new Vector2(0, m_speed);
            m_canDownMove = true;
        }
        else if (Input.GetKey(down) && m_canDownMove)
        {
            m_rigidbody2D.velocity = new Vector2(0, -m_speed);
            m_canUpMove = true;
        }
        else
        {
            m_rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        handleCollisionWithObstacle(collision);
    }

    //if collider is obstacle then restict movement at that direction
    void handleCollisionWithObstacle(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Up"))
        {   
            m_canUpMove = false;
        }
        else if(collision.gameObject.CompareTag("Down"))
        {
            m_canDownMove = false;
        }
    }

}
