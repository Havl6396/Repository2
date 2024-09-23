using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public int health = 3;
    public int maxhealth = 3;
    public GameObject Healthpickup;
    public Transform Enemy;
    public GameObject Ammobag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameObject a = Instantiate(Ammobag, Enemy.position, Enemy.rotation);
            GameObject l = Instantiate(Healthpickup, Enemy.position, Enemy.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "shot")
        {
            health--;
            Destroy(collision.gameObject);
        }
    }
}