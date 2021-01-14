using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float rightRotate;

    public GameObject player;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraMovment();
    }

    public void CameraMovment()
    {
        rightRotate = Input.GetAxis("Mouse X");

        if (gameManager.isGameOver == false)
        {
            transform.Rotate(Vector3.up, rightRotate * Time.deltaTime * rotationSpeed);
        }
            transform.position = player.transform.position;
        
    }
}
