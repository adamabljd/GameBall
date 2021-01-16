using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 25;
    public float horizontalSpeed = 25;
    public float powerUpStrengh = 30f;
    public float jumpForce = 25f;
    public float dashForce = 20f;
    public float fasterSpeed = 60f;
    public float bulletPower = 30f;
    public int health = 100;

    private float horizontalInput;
    private float forwardInput;
    private bool spaceInput;
    private bool vInput;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject enemy;
    public GameManager gameManager;
    public GameObject bullet;
    public GameObject playerBullet;

    public bool hasPowerUp;
    public bool hasPowerUpPush;
    public bool hasPowerUpJump;
    public bool hasPowerUpDash;
    public bool hasPowerUpSpeed;
    [SerializeField] bool isOnGround = false;
    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        Jump();
        CheckHasPowerUp();
    }

    public void FixedUpdate()
    {
        PlayerMovement();
    }

    public void PlayerMovement()
    {
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        SpeedPowerUp();

        //Move forward, backwards, left and right
        if (gameManager.isGameOver == false)
        {
            playerRb.AddForce(focalPoint.transform.forward * forwardSpeed * forwardInput);
            playerRb.AddForce(focalPoint.transform.right * horizontalSpeed * horizontalInput);
        }

    }

    private void SpeedPowerUp()
    {
        //changes speed
        if (hasPowerUpSpeed)
        {
            forwardSpeed = fasterSpeed;
            horizontalSpeed = fasterSpeed;
        }
    }

    private void Jump()
    {
        spaceInput = Input.GetKeyDown(KeyCode.Space);

        //Jump when space btn is pressed and when player has PU2 and is colliding with the ground
        if (spaceInput && hasPowerUpJump && isOnGround)
        {
            Debug.Log("space");
            playerRb.AddForce(focalPoint.transform.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    private void Dash()
    {
        vInput = Input.GetKeyDown(KeyCode.V);

        //Dashing when click on V and player is on ground and has PU3
        if (vInput && isOnGround && hasPowerUpDash)
        {
            //Dash
            Debug.Log("V");
            //playerRb.velocity = focalPoint.transform.forward * dashForce;
            playerRb.AddForce(focalPoint.transform.forward * dashForce, ForceMode.Impulse);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if player is on the ground
        if (other.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        //Gives player PU1 when he collides with it
        if (other.CompareTag("PowerUpPush"))
        {
            hasPowerUpPush = true;
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpPushCountdown());
        }

        //Gives player PU2 when he collides with it
        if (other.CompareTag("PowerUpJump"))
        {
            hasPowerUpJump = true;
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpJumpCountdown());
        }

        //add Powerup 3 boost when v press
        if (other.CompareTag("PowerUpDash"))
        {
            hasPowerUpDash = true;
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpDashCountdown());
        }

        //add speed when powerUp4
        if (other.CompareTag("PowerUpSpeed"))
        {
            hasPowerUpSpeed = true;
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpSpeedCountdown());
        }

        //gets pushed away if it got hit by a bullet
        if (other.CompareTag("Bullet"))
        {
            Vector3 pushAway = (transform.position - other.gameObject.transform.position);
            playerRb.AddForce(pushAway * bulletPower,ForceMode.Impulse );
            health -= 5;
            Destroy(other.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Push Enemy away when collides with Player and player has PU1
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUpPush == true)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrengh, ForceMode.Impulse);
        }
    }

    IEnumerator PowerUpPushCountdown()
    {
        yield return new WaitForSeconds(10);
        hasPowerUpPush = false;
    }

    IEnumerator PowerUpJumpCountdown()
    {
        yield return new WaitForSeconds(20);
        hasPowerUpJump = false;
    }

    IEnumerator PowerUpDashCountdown()
    {
        yield return new WaitForSeconds(20);
        hasPowerUpDash = false;
    }

    IEnumerator PowerUpSpeedCountdown()
    {
        yield return new WaitForSeconds(20);
        //return speed to normal
        horizontalSpeed = 25f;
        forwardSpeed = 25f;
        hasPowerUpSpeed = false;
    }

    private void CheckHasPowerUp()
    {
        if (hasPowerUpPush || hasPowerUpJump || hasPowerUpDash || hasPowerUpSpeed)
        {
            hasPowerUp = true;
        }else
        {
            hasPowerUp = false;
        }
    }
}
