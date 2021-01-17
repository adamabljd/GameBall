using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerUpText;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI healthText;
    private GameObject player;
    [SerializeField] GameObject titleScreen;
    [SerializeField] TextMeshProUGUI newEIText;
    [SerializeField] float mapRange = 151f;

    public bool isGameOver = true;
    public int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        isGameOver = true;
        player = GameObject.Find("Player");

    }
    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        GameOver();
        PowerUpIndicator();
        HealthText();

    }

    public void GameOver()
    {

        if ((player.transform.position.y <= -1 ||
            player.transform.position.x >= mapRange || player.transform.position.x <= -mapRange ||
            player.transform.position.z >= mapRange || player.transform.position.z <= -mapRange) 
            && isGameOver == false || player.GetComponent<PlayerController>().health <=0)
        {
            isGameOver = true;
            restartButton.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true); 
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameButtonStart()
    {
        isGameOver = false;
        score = 0;
        titleScreen.SetActive(false);
        StartCoroutine(NewEITimer());
    }

    private void PowerUpIndicator()
    {
        if (isGameOver == false)
        {
            if (player.GetComponent<PlayerController>().hasPowerUp == true)
            {
                powerUpText.text = "PowerUp: ON";
            }
            else
            {
                powerUpText.text = "PowerUp: OFF";
            }
        }
    }

    //Set New incoming enemy text active after 40seconds and lasts for 1sec
    IEnumerator NewEITimer()
    {
        yield return new WaitForSeconds(39f);
        newEIText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        newEIText.gameObject.SetActive(false);
    }

    private void HealthText()
    {
        healthText.text = "Health: " + player.GetComponent<PlayerController>().health;
    }
}

