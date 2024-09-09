using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRB;
    public float speed = 1.0f;
    public float jumpheight = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = myRB.velocity;


        temp.x = Input.GetAxisRaw("Vertical") * speed;
        temp.z = Input.GetAxisRaw("Horizontal") * speed;

        myRB.velocity = temp.x * transform.forward + (temp.z * transform.right);

        if (Input.GetKeyDown(KeyCode.Space)) 
            temp.y = jumpheight;
    }
}
