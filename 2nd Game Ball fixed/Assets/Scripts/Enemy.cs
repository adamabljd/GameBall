using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    private Rigidbody enemyRb;
    private GameObject player;
    private GameManager gameManager;
    private float mapRange = 151;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameManager.isGameOver == false)
        {
            followPlayer();
            EnemyFall();
        }
    }

    private void followPlayer()
    {
        //so that the ball wont go up
        if(transform.position.y >= 1.2f)
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);
    }

    private void EnemyFall()
    {
        if (transform.position.y <= -1 ||
           transform.position.x >= mapRange || transform.position.x <= -mapRange ||
            transform.position.z >= mapRange || transform.position.z <= -mapRange)
        {
            Destroy(gameObject);
            gameManager.score++;
        }
    }
}
