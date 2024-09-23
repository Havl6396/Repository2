using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCam;

    Vector2 camRotation;

    public Transform weaponSlot;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 5;
    public int healthRestore = 1;

    [Header("Weapon Stats")]
    public GameObject shot;
    public float shotVel = 0;
    public int weaponID = -1;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifespan = 0;
    public bool canFire = true;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;
    public float dashduration = .0001f;
    public float dashspeed = 10.0f;
    public bool dashMode = false;
    public bool candash = true;
    public float currentStam = 3;
    public float maxStam = 3;
    public float dashcooldown = 3;

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

        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetMouseButton(0) && canFire && currentClip > 0 && weaponID >= 0)
        {
            GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
            s.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * shotVel);
            Destroy(s, bulletLifespan);

            canFire = false;
            currentClip--;
            StartCoroutine("cooldownFire");
        }

        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();

        if (Input.GetKeyDown(KeyCode.Mouse2) && candash && currentStam > 0)
        {
            dashMode = true;
            StartCoroutine("cooldowndash");
            candash = false;
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

        temp.x = verticalMove * speed;
        temp.z = horizontalMove * speed;

        if (sprintMode)
        {   
            temp.x *= sprintMultiplier;
            temp.z *= sprintMultiplier;
        }
        if (dashMode)
        {
            temp.x *= dashspeed;
            temp.z *= dashspeed;
            currentStam -= 2;
        }

        if (Physics.Raycast(transform.position, -transform.up, groundDetectDistance) && dashMode == false)
        {
            currentStam ++;
        }

        if (currentStam > maxStam)
            currentStam = maxStam;

        if (Input.GetKeyDown(KeyCode.Space) && currentStam > 0)
        {
            temp.y = jumpHeight;
            currentStam--;
        }
        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            other.gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);

            other.gameObject.transform.SetParent(weaponSlot);

            switch (other.gameObject.name)
            {
                case "Weapon1":

                    weaponID = 0;
                    shotVel = 3000;
                    fireMode = 0;
                    fireRate = 0.25f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    bulletLifespan = 1;
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

        if (collision.gameObject.tag == "shot")
        {
            health--;
            Destroy(collision.gameObject);
        }

        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Ammopickup")
        {
            currentAmmo += reloadAmt;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }
    }

    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;

        else
        {
            float reloadCount = clipSize - currentClip;

            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;
                currentAmmo = 0;
                return;
            }

            else
            {
                currentClip += reloadCount;
                currentAmmo -= reloadCount;
                return;
            }
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    IEnumerator cooldowndash()
    {
        yield return new WaitForSeconds(dashduration);
        dashMode = false;
        yield return new WaitForSeconds(dashcooldown);
        candash = true;
    }
}