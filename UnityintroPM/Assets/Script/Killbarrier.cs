using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killbarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }

        if (collision.gameObject.tag == "basicenemy")
        {

            Destroy(collision.gameObject);

        }
    }

}
