using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed = 10f;
    private float mapRange = 151;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        GameObject player = GameObject.Find("Player");
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        

        //so that the fire wont go up
        if (transform.position.y >= 1.01f)
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
        BulletFall();
    }


    private void BulletFall()
    {
        if (transform.position.y <= -1 ||
           transform.position.x >= mapRange || transform.position.x <= -mapRange ||
            transform.position.z >= mapRange || transform.position.z <= -mapRange)
        {
            Destroy(gameObject);
        }
    }
}
