using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] Alarm;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i<Alarm.Length; i++)
            {
                Alarm[i].GetComponent<BasicEnemyController>().detectplayer = true;
            }
        }
    }
}
