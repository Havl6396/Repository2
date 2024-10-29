using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bossenemycontroller : MonoBehaviour
{
    [Header("EnemyStats")]
    public int health = 25;
    public int maxhealth = 25;
    public int damageGiven = 1;
    public int projforce = 3000;
    public GameObject shot;
    public float fireRate = 10f;
    public float bulletLifespan = 3;
    public float Bossvisor = 90;

    [Header("EnemyLoot")]
    public Transform Enemy;
    public GameObject Reward;
    public GameObject Body;

    [Header("EnemyDetection")]
    public bool fireplayer = false;
    public bool bossangered = false;
    public PlayerController player;
    public NavMeshAgent agent;
    public Transform LB;
    public Transform RB;
    public bool shootplayer = false;

    // Start is called before the first frame update
    void Start()
    {
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            agent = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gm.Enemycount == 0)
        {
            bossangered = true;
        }


        if (bossangered == true) 
        {
            Vector3 target = player.transform.position - transform.position;
            Quaternion test = Quaternion.Euler(0, Mathf.Atan2(target.x, target.z)*Mathf.Rad2Deg, 0);
            Rigidbody myrb = GetComponent<Rigidbody>();
            myrb.rotation = test;
            LB.LookAt(player.transform);
            RB.LookAt(player.transform);
            //transform.Rotate(0, target.y, 0
        }
            

        if (health <= 0)
        {
            GameObject a = Instantiate(Reward, Enemy.position, Enemy.rotation);
            GameObject b = Instantiate(Body, Enemy.position, Enemy.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "shot" && bossangered)
        {
            health--;
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag == "shot4" && bossangered)
        {
            health -= 9;
            Destroy(collision.gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shootplayer = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (shootplayer == true && bossangered)
        {

            GameObject ls = Instantiate(shot, LB.position, LB.rotation);
            ls.GetComponent<Rigidbody>().AddForce(LB.transform.forward * projforce);
            Destroy(ls, bulletLifespan);
            shootplayer = false;
            StartCoroutine("cooldownFire");

            GameObject rs = Instantiate(shot, RB.position, RB.rotation);
            rs.GetComponent<Rigidbody>().AddForce(RB.transform.forward * projforce);
            Destroy(rs, bulletLifespan);
            shootplayer = false;
            StartCoroutine("cooldownFire");

        }

    }



    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        shootplayer = true;

    }

}