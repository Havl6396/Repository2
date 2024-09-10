using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRB;
    private Camera PlayerCam;
    public float speed = 1.0f;
    
    public float jumpheight = 1.0f;
    private Vector2 camerarotation;
    public float sprintmult = 2.5f;
    public float mousesensitivity = 1.0f;
    public float xsensitivity = 1.0f;
    public float ysensitivity = 1.0f;
    public float camerarotationlimit = 100f;
    public bool sprintmode = false;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        PlayerCam = transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = myRB.velocity;


        temp.x = Input.GetAxisRaw("Vertical") * speed;
        temp.z = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, 2.2f))
        {
            temp.y = jumpheight;
        }

        myRB.velocity = temp.x * transform.forward + (temp.z * transform.right) + (temp.y * transform.up);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintmode = true;
        }

        if (sprintmode)
        {
            temp.x = Input.GetAxisRaw("Vertical") * speed * sprintmult;
        }

        if (!sprintmode)
        {
            temp.x = Input.GetAxisRaw("Vertical") * speed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintmode = false;
        }

        if()

        camerarotation.x += Input.GetAxisRaw("Mouse X") * mousesensitivity;
        
        camerarotation.y += Input.GetAxisRaw("Mouse Y") * mousesensitivity;

        camerarotation.y = Mathf.Clamp(camerarotation.y, -camerarotationlimit, camerarotationlimit);

        PlayerCam.transform.localRotation = Quaternion.AngleAxis(camerarotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camerarotation.x, Vector3.up);
    }
}
