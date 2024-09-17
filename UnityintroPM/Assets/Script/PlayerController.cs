using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCam;

    Vector2 camRotation;

    public Transform Weaponslot;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 5;
    public int healthRestore = 1;

    [Header("Weapon Stats")]
    public int weaponID = 0;
    public float fireRate = 0.25f;
    public float Currentclip = 20;
    public float Clipsize = 20;
    public float maxAmmo = 400;
    public float currentAmmo = 200;
    public float reloadamount = 20;
    public bool canFire = true;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;

    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

   
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if(Input.GetKey(KeyCode.Mouse0) && canFire)
        {
            canFire = false;
            StartCoroutine("cooldownFire");
        }

        Vector3 temp = myRB.velocity;

        float verticalMove = Input.GetAxisRaw("Vertical");
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        if (!sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

            if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if (sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                sprintMode = true;

            if (verticalMove <= 0)
                sprintMode = false;
        }

        if (!sprintMode)
            temp.x = verticalMove * speed;

        if (sprintMode)
            temp.x = verticalMove * speed * sprintMultiplier;

        temp.z = horizontalMove * speed;


        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = jumpHeight;

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            other.gameObject.transform.SetParent(Weaponslot);
            other.gameObject.transform.position = Weaponslot.position;
            other.gameObject.transform.rotation = Weaponslot.rotation;

            switch(other.gameObject.name)
            {
                case "Weapon1":
                    weaponID = 0;
                    fireRate = 0;
                    Currentclip = 0;
                    Clipsize = 0;
                    maxAmmo = 0;
                    currentAmmo = 0;
                    reloadamount = 0;
                    break;
                
                default:
                    break;

    
            }   
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((health < maxHealth) && collision.gameObject.tag == "Healthpickup")
        {
            health += healthRestore;

            if (health > maxHealth)
                health = maxHealth;

            Destroy(collision.gameObject);
        }

        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Ammopickup")
        {
            currentAmmo += reloadamount;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }
    }

    public void Reloadclip()
    {
        if (Currentclip == Clipsize)
            return;

        else
        {
            float reloadCount = Clipsize = Currentclip;

            if (currentAmmo < reloadamount)
            {
                Currentclip += currentAmmo; 
                    
                currentAmmo = 0;

                return;
            }

            else
            {
                Currentclip += reloadamount;

                currentAmmo -= reloadamount;

                return;
            }
        }
    }
    IEnumerator cooldownFire(float time)
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}