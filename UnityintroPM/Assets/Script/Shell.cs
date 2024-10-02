using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Transform Enemy;
    public int projforce = 3000;
    public GameObject shot;
    public float fireRate = 1f;
    public float bulletLifespan = 3;
    public Transform bullet;

    // Start is called before the first frame update

    private void Start()
    {
        
    }

    private void Update()
    {
        StartCoroutine("shell");
    }

    IEnumerator shell()
    {
        yield return new WaitForSeconds(fireRate);
        GameObject s = Instantiate(shot, bullet.position, bullet.rotation);
        
        Destroy(s, bulletLifespan);

    }
}
