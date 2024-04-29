using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using YG;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] public int goldenBullets;
    [Space(20)]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private FadeController fadeController;
    [Space(10)]
    [SerializeField] private Sprite goldenBulletSprite;
    [SerializeField] private Sprite regularBulletSprite;
    [SerializeField] private Sprite emptyBulletSprite;
    [Space(10)]
    [SerializeField] private Image starImage1;
    [SerializeField] private Image starImage2;
    [SerializeField] private Image starImage3;
    [Space(10)]
    [SerializeField] private CanvasGroup resultMenu;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup gameMenu;
    [SerializeField] private CanvasGroup smoothTransition;
    [Space(20)]
    [SerializeField] private Text levelIndexIndicator;
    [SerializeField] private Text resultMenuLevelIndexIndicator;
    [Space(20)]
    [SerializeField] private Image[] bulletsCells;

    private bool complete = true;
    private int zombiesNumber;
    private int bulletsNumber;
    private int regularBullets;
    private int shootsCounter = -1;
    private int startRegularBulletsNumber;

    public static UnityEvent pauseEvent = new UnityEvent();


    private void Start()
    {
        UpdateOnStart();
        RefillBulletsMag();
        CountZombiesNumber();
    }



    public void PauseBtn()
    {
        audioSource.Play();
        pauseEvent.Invoke();
        fadeController.FadeIn(pauseMenu);
        fadeController.FadeOut(gameMenu);
    }

    public void ResumeBtn()
    {
        audioSource.Play();
        pauseEvent.Invoke();
        fadeController.FadeIn(gameMenu);
        fadeController.FadeOut(pauseMenu);
    }

    public void RestartBtn()
    {
        audioSource.Play();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(DelayLoad(sceneIndex));
        fadeController.FadeIn(smoothTransition);
    }

    public void HomeBut()
    {
        audioSource.Play();
        StartCoroutine(DelayLoad(0));
        fadeController.FadeIn(smoothTransition);
    }



    public void ResultMenuResumeBtn()
    {
        int i = SceneManager.GetActiveScene().buildIndex;

        if(i == 130)
        {
            audioSource.Play();
            int randomScene = Random.Range(1, 99);
            StartCoroutine(DelayLoad(randomScene));
            fadeController.FadeIn(smoothTransition);
        }
        else
        {
            audioSource.Play();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(DelayLoad(sceneIndex + 1));
            fadeController.FadeIn(smoothTransition);
        }
    }

    public void ResultMenuRestartBtn() 
    {
        audioSource.Play();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(DelayLoad(sceneIndex));
        fadeController.FadeIn(smoothTransition);
    }

    public void ResultMenuHomeBtn()
    {
        audioSource.Play();
        StartCoroutine(DelayLoad(0));
        fadeController.FadeIn(smoothTransition);
    }



    private void UpdateOnStart()
    {
        fadeController.FadeOut(smoothTransition);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelIndexIndicator.text   += sceneIndex;
        resultMenuLevelIndexIndicator.text  += sceneIndex;

        bool buttonsSounds = YandexGame.savesData.sounds;

        if (buttonsSounds)
        {
            audioSource.volume = 1.0f;
        }
        else
        {
            audioSource.volume = 0.0f;
        }
    }

    private IEnumerator DelayLoad(int levelIndex)
    {
        yield return new WaitForSeconds(1f);
        StopCoroutine(DelayLoad(levelIndex));
        SceneManager.LoadScene(levelIndex);
    }

    private void RefillBulletsMag()
    {
        int goldenShoots = 0;

        for (int i = 0; i < bulletsCells.Length; i++)
        {
            if (goldenShoots != goldenBullets)
            {
                bulletsCells[i].sprite = goldenBulletSprite;
                goldenShoots++;
            }
            else
            {
                bulletsCells[i].sprite = regularBulletSprite;
                regularBullets++; 
                startRegularBulletsNumber++;
            }

            bulletsNumber++;
        }
    }

    private void CountZombiesNumber()
    {
        zombiesNumber = GameObject.FindGameObjectsWithTag("Zombie").Length;
    }



    private void HitZombie()
    {
        zombiesNumber--;
        GameResult();
    }

    private void Shoot()
    {
        shootsCounter++;
        bulletsNumber--;

        if (goldenBullets > 0)
        {
            goldenBullets --;
            bulletsCells[shootsCounter].sprite = emptyBulletSprite;
            GameResult();
        }
        else if (regularBullets > 0)
        {          
            regularBullets --;
            bulletsCells[shootsCounter].sprite = emptyBulletSprite;
            GameResult();
        }
    }

    private void GameResult()
    {
        if (zombiesNumber <= 0 || bulletsNumber <= 0)
        {
            Invoke("LoadResultMenu", 2.4f);
            fadeController.FadeOut(gameMenu);
            pauseEvent.Invoke();

        }
        else if(bulletsNumber <= 0 && zombiesNumber <= 0)
        {
            Invoke("LoadResultMenu", 2.4f);
            fadeController.FadeOut(gameMenu);
            pauseEvent.Invoke();
        }
    }




    private void LoadResultMenu()
    {
        if (bulletsNumber <= 0 && zombiesNumber > 0)
        {
            fadeController.FadeIn(resultMenu);
            complete = false;
            SaveLevelData(0);
        }
        else if (startRegularBulletsNumber == 0)
        {
            if (goldenBullets >= 0)
            {
                starImage1.enabled = true;
                starImage2.enabled = true;
                starImage3.enabled = true;
                fadeController.FadeIn(resultMenu);
                SaveLevelData(3);
            }
        }
        else if (startRegularBulletsNumber > 0)
        {
            if (goldenBullets >= 0 && regularBullets == startRegularBulletsNumber)
            {
                starImage1.enabled = true;
                starImage2.enabled = true;
                starImage3.enabled = true;
                fadeController.FadeIn(resultMenu);
                SaveLevelData(3);

            }
            else if (goldenBullets == 0 && regularBullets < startRegularBulletsNumber && regularBullets >= 2)
            {
                starImage1.enabled = true;
                starImage2.enabled = true;
                fadeController.FadeIn(resultMenu);
                SaveLevelData(2);
            }
            else if (goldenBullets == 0 && regularBullets < 2)
            {
                starImage1.enabled = true;
                SaveLevelData(1);
                fadeController.FadeIn(resultMenu);
            }
        }
    }

    private void SaveLevelData(int stars)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int completeLevels = YandexGame.savesData.completedLevels;


        if (complete)
        {
            if (sceneIndex >= completeLevels && sceneIndex != 130)
            {
                YandexGame.savesData.completedLevels = sceneIndex + 1;
            }
            else if (sceneIndex == 130)
            {
                YandexGame.savesData.completedLevels = sceneIndex;
            }

            if (sceneIndex == 1)
            {
                YandexGame.savesData.completedLevelsStars[0] = stars;
            }
            else
            {
                sceneIndex--;
                YandexGame.savesData.completedLevelsStars[sceneIndex] = stars;
            }
        }
        else
        {
            if (sceneIndex == 1)
            {
                YandexGame.savesData.completedLevelsStars[0] = stars;
            }
            else
            {
                sceneIndex--;
                YandexGame.savesData.completedLevelsStars[sceneIndex] = stars;
            }

            YandexGame.savesData.completedLevels = completeLevels;
        }

        YandexGame.SaveProgress();
    }
}