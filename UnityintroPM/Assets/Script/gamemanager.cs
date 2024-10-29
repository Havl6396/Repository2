using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pauseMenu;
    public PlayerController playerData;
    public FalsePlayerController falseplayerData;
    public Bossenemycontroller bossenemyData;
    public int Enemycount;
    public Image healthBar;
    public Image staminaBar;
    public TextMeshProUGUI clipCounter;
    public TextMeshProUGUI ammoCounter;


    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerController>();
        bossenemyData = GameObject.Find("Alienbosstest").GetComponent<Bossenemycontroller>();
        Enemycount = GameObject.FindGameObjectsWithTag("basicenemy").Length;
        //falseplayerData = GameObject.Find("Falseplayer").GetComponent<FalsePlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            healthBar.fillAmount = Mathf.Clamp((float)playerData.health / (float)playerData.maxHealth, 0, 1);
            staminaBar.fillAmount = Mathf.Clamp((float)playerData.currentStam / (float)playerData.maxStam, 0, 1);

            if (playerData.weaponID < 0)
            {
                clipCounter.gameObject.SetActive(false);
                ammoCounter.gameObject.SetActive(false);
            }

            else
            {
                clipCounter.gameObject.SetActive(true);
                ammoCounter.gameObject.SetActive(true);

                clipCounter.text = "Clip: " + playerData.currentClip + "/" + playerData.clipSize;
                ammoCounter.text = "Ammo: " + playerData.currentAmmo;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    pauseMenu.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Time.timeScale = 0;

                    isPaused = true;
                }

                else
                    Resume();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int sceneID)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneID);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}