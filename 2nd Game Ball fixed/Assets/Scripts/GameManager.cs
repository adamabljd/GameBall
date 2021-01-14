using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI powerUpText;
    //public Button startButton;
    public Button restartButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    private GameObject player;
    public GameObject titleScreen;
    [SerializeField] float mapRange = 151f;

    public bool isGameOver = true;
    public int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        isGameOver = true;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        GameOver();
        PowerUpIndicator();
    }

    public void GameOver()
    {

        if ((player.transform.position.y <= -1 ||
            player.transform.position.x >= mapRange || player.transform.position.x <= -mapRange ||
            player.transform.position.z >= mapRange || player.transform.position.z <= -mapRange) 
            && isGameOver == false)
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
    }

    private void PowerUpIndicator()
    {
        if (isGameOver == false)
        {
            if (/*player.GetComponent<PlayerController>().hasPowerUp1 == true || player.GetComponent<PlayerController>().hasPowerUp2 == true || player.GetComponent<PlayerController>().hasPowerUp3 == true*/
                player.GetComponent<PlayerController>().hasPowerUp == true)
            {
                powerUpText.text = "PowerUp: ON";
            }
            else
            {
                powerUpText.text = "PowerUp: OFF";
            }
        }
    }
}

