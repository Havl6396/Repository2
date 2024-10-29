using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalsePlayerController : MonoBehaviour
{

    public Transform RB;
    [Header("EnemyStats")]
    public int health = 25;
    public int maxhealth = 25;
    public int damageGiven = 1;
    public int projforce = 3000;
    public GameObject shot;
    public GameObject shot2;
    public float fireRate = 1f;
    public float fireRate2 = 1f;
    public float bulletLifespan = 3;
    public float bulletLifespan2 = 3;
    public int strafe = 100;
    

    [Header("EnemyLoot")]
    public GameObject Healthpickup;
    public Transform Enemy;
    public GameObject Ammobag;
    public GameObject Body;

    [Header("EnemyDetection")]
    public bool fireplayer = false;
    public bool detectplayer = false;
    public PlayerController player;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform weaponSlot;
    public bool shootweap1 = false;
    public bool shootweap2 = false;
    public Transform weaponSlot2;

    // Start is called before the first frame update
    void Start()
    {
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (detectplayer == true)
        {
            agent.destination = player.transform.position;
            GetComponent<Rigidbody>().AddForce(Enemy.transform.right * strafe);
           
        }


        if (health <= 0)
        {
            GameObject a = Instantiate(Ammobag, Enemy.position, Enemy.rotation);
            GameObject l = Instantiate(Healthpickup, Enemy.position, Enemy.rotation);
            GameObject b = Instantiate(Body, Enemy.position, Enemy.rotation);
            player.gm.LoadLevel(0);
            Destroy(gameObject);
        }

        RB.LookAt(player.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "shot")
        {
            health--;
            Destroy(collision.gameObject);
            detectplayer = true;
        }

        if (collision.gameObject.tag == "shot4")
        {
            health-=2;
            Destroy(collision.gameObject);
            detectplayer = true;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shootweap1 = true;
            detectplayer = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && shootweap1 == true)
        {
            shootweap1 = false;
            GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
            s.GetComponent<Rigidbody>().AddForce(Enemy.transform.forward * projforce);
            Destroy(s, bulletLifespan);
            StartCoroutine("cooldownFire");
            shootweap1 = false;
            shootweap2 = true;
        }

        if (shootweap2 == true)
        {
            shootweap2 = false;
            GameObject s2 = Instantiate(shot2, weaponSlot2.position, weaponSlot2.rotation);
            s2.GetComponent<Rigidbody>().AddForce(Enemy.transform.forward * projforce);
            Destroy(s2, bulletLifespan2);
            StartCoroutine("cooldownFire2");
            shootweap2 = false;
        }

    }



    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        shootweap1 = true;

    }

    IEnumerator cooldownFire2()
    {
        yield return new WaitForSeconds(fireRate2);
        shootweap2 = true;

    }

}