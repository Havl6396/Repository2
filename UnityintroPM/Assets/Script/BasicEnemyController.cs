using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    [Header("EnemyStats")]
    public int health = 3;
    public int maxhealth = 3;
    public int damageGiven = 1;
    public int projforce = 3000;
    public GameObject shot;
    public float fireRate = .5f;
    public float bulletLifespan = 3;

    [Header("EnemyLoot")]
    public GameObject Healthpickup;
    public Transform Enemy;
    public GameObject Ammobag;
    public GameObject Body;
    

    public PlayerController player;
    public NavMeshAgent agent;
    public Transform weaponSlot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine("cooldownFire");
    }

    // Update is called once per frame
    void Update()
    {
   


        agent.destination = player.transform.position;

        if (health <= 0)
        {
            GameObject a = Instantiate(Ammobag, Enemy.position, Enemy.rotation);
            GameObject l = Instantiate(Healthpickup, Enemy.position, Enemy.rotation);
            GameObject b = Instantiate(Body, Enemy.position, Enemy.rotation);
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

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
        s.GetComponent<Rigidbody>().AddForce(Enemy.transform.forward * projforce);
        Destroy(s, bulletLifespan);
        StartCoroutine("cooldownFire");
    }

}