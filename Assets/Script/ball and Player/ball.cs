using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] private Transform m_upperLimitPoint;
    [SerializeField] private Transform m_lowerLimitPoint;

    [SerializeField] private float m_pauseTime;
    Vector2 m_respawnPoint;
    Vector2 m_velocityDirection;

    [SerializeField] float m_velocityMagnitude;
    [SerializeField] float m_velocityIncrementCount;
    float m_initialVelocityMagnitude;

    Rigidbody2D m_Rigidbody;
    Renderer m_renderer;

    [SerializeField] ScoreAndGameOverManager scoreManager;

    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<Renderer>();
    }
    void Start()
    {
        m_initialVelocityMagnitude = m_velocityMagnitude;
        StartCoroutine(StartGame(0.1f));
    }
            
    IEnumerator StartGame(float _pauseTime)
    {
        //initally disable renderer
        m_renderer.enabled = false;

        ResetValue(); //reset Velocity to 0 
        
        //if None Of player Win then continue game
        SetRespawnPoint();
        SetDirection();
        
        yield return new WaitForSeconds(_pauseTime);

            //after Certain Pause Time Enable renderer
        m_renderer.enabled = true;

        m_velocityMagnitude = m_initialVelocityMagnitude;
        SetVelocity();
       
    }

    void SetRespawnPoint()
    {
        //randomly select Y direction and Fixed X direction in the center
        m_respawnPoint.y = Random.Range(m_lowerLimitPoint.position.y, m_upperLimitPoint.position.y);
        m_respawnPoint.x = m_lowerLimitPoint.position.x;

        transform.position = m_respawnPoint; 
    }
    void SetDirection()
    {
        bool isHorizontalBarrier;
        bool isVerticallyBarrier;

        float randomAngle;
        
        //repeat until get Acceptable angle
        do
        {
            randomAngle = Random.Range(0f, 360f);
            
            /*If Direction is to much Horizontal then reset
            (at least is should more then 1 degree slant )*/
            isHorizontalBarrier = (randomAngle < 1f && randomAngle > -1f) || 
                (randomAngle > 179f && randomAngle < 181f);
            
            /*If Direction is to much vertical then reset
            (greater then 70 degree is not acceptable)*/
            isVerticallyBarrier = ((randomAngle > 70f && randomAngle < 110) || 
                (randomAngle > 250 && randomAngle < 290));

        } while(isHorizontalBarrier || isVerticallyBarrier);

        //convert degree to radian
        randomAngle *= Mathf.Deg2Rad;

        //Set velocity direction as (Unit Vector)
        m_velocityDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }
    void SetVelocity()
    {
        m_Rigidbody.velocity = m_velocityDirection * m_velocityMagnitude;
    }
    void ResetValue()
    {
        //Set velocity magnityde and direction to 0
        m_velocityMagnitude = 0f;
        m_Rigidbody.velocity = Vector2.zero;
    }


    //When Triggered with collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collide with obstacle
        if(collision.gameObject.CompareTag("Up") || collision.gameObject.CompareTag("Down"))
        {
            m_velocityDirection.y *= -1f;
            SetVelocity();

            PlaySountEffect("HitByObject");
        }

        //hit by player
        else if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            m_velocityDirection.x *= -1f;

            //increase Speed as player hit the ball
            m_velocityMagnitude += m_velocityIncrementCount;
            SetVelocity();

            PlaySountEffect("HitByPlayer");
        }

        //miss the ball by player 
        else if(collision.gameObject.CompareTag("Right"))
        {
            scoreManager.UpdatePlayerScore("Player2"); //update score
            StartCoroutine(StartGame(m_pauseTime));

            PlaySountEffect("BallMissed");
        }
        else if(collision.gameObject.CompareTag("Left"))
        {
            scoreManager.UpdatePlayerScore("Player1"); //update score
            StartCoroutine(StartGame(m_pauseTime));

            PlaySountEffect("BallMissed");
        }
    }


    void PlaySountEffect(string hitter)
    {
        //In gameOver No Hitting SOundEffect will Play
        if(!scoreManager.IsGameOver)
        {
            AudioManager.instance.PlaySFX(hitter);
        }
    }
}
