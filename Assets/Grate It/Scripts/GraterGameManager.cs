using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraterGameManager : MonoBehaviour
{
    public static GraterGameManager instance;
    public AudioSource AudioSource;
    public AudioClip CompleteSound;
    public AudioClip FailSound;
    public AudioClip KnifeCutSound;
    public GameObject levelcomplete, levelfail, ingame, playerRank, aiRank, lfPRank, lfAiRank;

    public Text leveltext, scoretext;

    public Image fill1, ailevelfill, aiFillBar, aiImage, playerImage;
    public Text AiWinpercentage, playerwinper;
    public float score;
    public float aiscore;
    public Text AiscoreText;

    public GameObject particale;
    public Material AiBoard;

    public float speed;
    public GraterColliderScript gmcontrol;

    void Start()
    {
        RenderSettings.fogStartDistance = 60;
        RenderSettings.fogEndDistance = 100;
        gmcontrol = FindObjectOfType<GraterColliderScript>();
        //RenderSettings.fogColor = Random.ColorHSV(0,360,24,25,89,90,99,100);
        GameObject.FindGameObjectWithTag("AIsetup").transform.position = new Vector3(-10.5f, 0, -6.1f);
        leveltext.text = "LEVEL" + (SceneManager.GetActiveScene().buildIndex).ToString();

        if (MenuScript.mul == 0)
        {
            GameObject.FindGameObjectWithTag("AiObjectSpawnPoint").SetActive(false);
            GameObject.FindGameObjectWithTag("AIsetup").SetActive(false);
            GameObject.FindGameObjectWithTag("AiPlayer").SetActive(false);
        }
        if (MenuScript.mul == 1)
        {
            Camera.main.transform.position = new Vector3(1f, 18.7f, -19.4f);
            Camera.main.fieldOfView = 78;
            aiFillBar.gameObject.SetActive(true);
            AiscoreText.gameObject.SetActive(true);
        }
    }

    int i = 0;
    public platform[] pl;
    public platform[] pl2;

    void Update()
    {
        scoretext.text = score.ToString();
        if (MenuScript.mul == 1)
        {
            AiscoreText.text = aiscore.ToString();
        }

        if (fill1.fillAmount == 1 && i == 0)
        {
            FindObjectOfType<cameramove>().enabled = true;
            gameover();
            Debug.Log("levelcomplete");
            Invoke("LevelComplete", 1.2f);
            pl = FindObjectsOfType<platform>();
            FindObjectOfType<PlatforMove>().tomakecancelinvoke();

            for (int i = 0; i < pl.Length; i++)
            {
                pl[i].stopmovement = false;
            }
            Instantiate(particale, new Vector3(100, 25, 61), Quaternion.Euler(23, 29, -4));
            i = 1;
        }
        if (ailevelfill.fillAmount == 1 && i == 0 && FindObjectOfType<AIScript>())
        {
            LevelFail();
            Debug.Log("levelfail");
            FindObjectOfType<PlatforMove>().tomakecancelinvoke();

            pl2 = FindObjectsOfType<platform>();
            for (int i = 0; i < pl2.Length; i++)
            {
                pl2[i].stopmovement = false;
            }
            i = 1;
        }
    }

    public void LevelComplete()
    {
        AudioSource.PlayOneShot(CompleteSound);
        levelcomplete.SetActive(true);
        ingame.SetActive(false);

        int level = int.Parse(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("level", (level + 1).ToString());

        if (MenuScript.mul == 1)
        {
            playerwinper.text = (int)((score / gmcontrol.totalscore) * 100) + "%";
            AiWinpercentage.text = (int)((aiscore / gmcontrol.totalscore) * 100) + "%";
            GameObject.FindGameObjectWithTag("AiPlayer").SetActive(false);
            aiImage.gameObject.SetActive(true);
            playerImage.gameObject.SetActive(true);
        }
        else
        {
            playerRank.SetActive(false);
            aiRank.SetActive(false);
        }
    }
    public void LevelFail()
    {
        AudioSource.PlayOneShot(FailSound);
        levelfail.SetActive(true);
        ingame.SetActive(false);
        Admanager.instance.ShowFullScreenAd();
        if (MenuScript.mul == 1)
        {
            playerwinper.text = (int)((score / gmcontrol.totalscore) * 100) + "%";
            AiWinpercentage.text = (int)((aiscore / gmcontrol.totalscore) * 100) + "%";
            aiImage.gameObject.SetActive(true);
            playerImage.gameObject.SetActive(true);
        }
        else
        {
            lfAiRank.SetActive(false);
            lfPRank.SetActive(false);
        }
    }

    public void RestartButton()
    {
        LevelFail_Ads();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Admanager.instance.ShowFullScreenAd();
    }
    public void NextButton()
    {
        LevelCompleted_Ads();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Admanager.instance.ShowFullScreenAd();
    }

    public void gameover()
    {
        FindObjectOfType<GraterInputManager>().enabled = false;
        FindObjectOfType<GraterInputManager>().GetComponent<Animator>().enabled = false;
        FindObjectOfType<GraterColliderScript>().GetComponent<MeshCollider>().enabled = false;
        FindObjectOfType<PlatforMove>().enabled = false;
        FindObjectOfType<platform>().enabled = false;
        Admanager.instance.ShowFullScreenAd();
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        Admanager.instance.RequestBanner();
    }
   
    public void LevelFail_Ads()
    {
        PlayerPrefs.SetInt("LF_Ads", PlayerPrefs.GetInt("LF_Ads", 0) + 1);

        int num = PlayerPrefs.GetInt("LF_Ads", 0);
        if (num > 2)
        {
            PlayerPrefs.SetInt("LF_Ads", 0);
           // AdManager.instance.ShowAd();
        }
    }

    public void LevelCompleted_Ads()
    {
        PlayerPrefs.SetInt("LC_Ads", PlayerPrefs.GetInt("LC_Ads", 0) + 1);

        int num = PlayerPrefs.GetInt("LC_Ads", 0);
        if (num > 2)
        {
            PlayerPrefs.SetInt("LC_Ads", 0);
          //  AdManager.instance.ShowAd();
        }
    }
}
