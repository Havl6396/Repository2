using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalsePlayerController : MonoBehaviour
{
    public int health = 3;
    public int maxhealth = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
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
