using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody enemyRb;
    private GameObject player;
    private GameManager gameManager;
    private float mapRange = 151;
    
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("Bullet", 0.25f, 2f);

    }

    void FixedUpdate()
    {
        if (gameManager.isGameOver == false)
        {
            EnemyFall();
            transform.LookAt(player.transform.position);
        }
    }

    private void Bullet()
    {

        if(gameManager.isGameOver == false)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
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
