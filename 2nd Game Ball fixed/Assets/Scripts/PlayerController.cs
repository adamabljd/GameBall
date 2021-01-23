using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float defaultForwardSpeed = 35;
    public float defaultHorizontalSpeed = 30;
    public float forwardSpeed = 35;
    public float horizontalSpeed = 30;
    public float powerUpStrengh = 30f;
    public float jumpForce = 25f;
    public float teleportForce = 20f;
    public float fasterSpeed = 60f;
    public float bulletPower = 30f;
    public int health = 100;
    public float agility = 0.5f;
    public float agilitySpeed = 20f;

    private float horizontalInput;
    private float forwardInput;
    private bool spaceInput;
    private bool vInput;
    private float spawnRangeX = 73f;
    private float spawnRangeZ = 73f;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject enemy;
    public GameManager gameManager;

    public bool hasPowerUp;
    public bool hasPowerUpPush;
    public bool hasPowerUpJump;
    public bool hasPowerUpDash;
    public bool hasPowerUpSpeed;
    public bool hasPowerUpAgility;
    public bool touchTrap;
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
        AgilityPowerUp();

        //Move forward, backwards, left and right
        if (gameManager.isGameOver == false)
        {
            playerRb.AddForce(focalPoint.transform.forward * forwardSpeed * forwardInput);
            playerRb.AddForce(focalPoint.transform.right * horizontalSpeed * horizontalInput);
        }

    }











//--------------------------------------------Triggers----------------------------------------------------------------------------------------------

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
            hasPowerUpDash= true;
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
            if(health > 0)
            {
                health -= 5;
            }
            Destroy(other.gameObject);
        }

        //adds +10 health if health > 90 or health becomes 100
        if (other.CompareTag("PowerUpHeart"))
        {
            if(health >= 90 && health < 100)
            {
                health = 100;
            }else if(health < 90 && health > 0)
            {
                health += 10;
            }
            Destroy(other.gameObject);
        }

        //become agile when powerUp4
        if (other.CompareTag("PowerUpAgility"))
        {
            hasPowerUpAgility = true;
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpAgilityCountdown());
        }

        //health -5 if touches trap
        if (other.CompareTag("Trap"))
        {
            if (health > 0)
            {
                health -= 5;
            }
            Destroy(other.gameObject);
        }

        //health -5 if touches trap
        if (other.CompareTag("Portal"))
        {
            PortalPlayerPosition();
            Destroy(other.gameObject);
        }
    }






//---------------------------------Collision-----------------------------------------------------------------------------------------------------

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






//-------------------------------------Power Ups Functions-------------------------------------------------------------------------------------------


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
            //transform.position += focalPoint.transform.forward * teleportForce;
            playerRb.MovePosition(playerRb.transform.position + focalPoint.transform.forward * teleportForce * Time.deltaTime);
        }
    }

    private void SpeedPowerUp()
    {
        //changes speed
        if (hasPowerUpSpeed)
        {
            forwardSpeed = fasterSpeed;
            horizontalSpeed = fasterSpeed - 10;
        }
    }



    private void AgilityPowerUp()
    {
        //changes agility
        if (hasPowerUpAgility)
        {
            forwardSpeed = agilitySpeed;
            horizontalSpeed = agilitySpeed;
            playerRb.mass = agility;
        }
    }

    private void PortalPlayerPosition()
    {
        transform.position = GenerateSpawnPosition();
    }

    //----------------------------PowerUps Countdown-----------------------------------------------------------------------------------------------------

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
        horizontalSpeed = defaultHorizontalSpeed;
        forwardSpeed = defaultForwardSpeed;
        hasPowerUpSpeed = false;
    }

    IEnumerator PowerUpAgilityCountdown()
    {
        yield return new WaitForSeconds(20);
        //return speed to normal
        horizontalSpeed = defaultForwardSpeed;
        forwardSpeed = defaultHorizontalSpeed;
        playerRb.mass = 2;
        hasPowerUpAgility = false;
    }





//-------------------------------------Others-------------------------------------------------------------------------------------------------------

    private void CheckHasPowerUp()
    {
        if (hasPowerUpPush || hasPowerUpJump || hasPowerUpDash || hasPowerUpSpeed || hasPowerUpAgility)
        {
            hasPowerUp = true;
        }else
        {
            hasPowerUp = false;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 randomPos = new Vector3(spawnPosX, 1f, spawnPosZ);
        return randomPos;
    }
}
