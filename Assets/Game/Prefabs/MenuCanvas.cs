using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private AudioSource buttonPlayer;
    [SerializeField] private FadeController fadeController;
    [Space(10)]
    [SerializeField] private Sprite soundsOnSprite;
    [SerializeField] private Sprite soundsOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [Space(10)]
    [SerializeField] private Image soundsButtonImage;
    [SerializeField] private Image musicButtonImage;
    [Space(10)]
    [SerializeField] private CanvasGroup gamesMenu;
    [SerializeField] private CanvasGroup smoothTransition;

    private void Awake()
    {
        LoadSoundsSettings();
        LoadMusicSettings();
    }
    private void Start()
    {       
        fadeController.FadeOut(smoothTransition);
    }

    private void LoadSoundsSettings()
    {
        bool sounds = YandexGame.savesData.sounds;

        if (sounds == true)
        {
            buttonPlayer.volume = 1f;
            soundsButtonImage.sprite = soundsOnSprite;         
        }
        else if(sounds == false)
        {
            buttonPlayer.volume = 0f;
            soundsButtonImage.sprite = soundsOffSprite;
        }
    }
    private void LoadMusicSettings()
    {
        bool music = YandexGame.savesData.music;
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer");
        AudioSource aus = musicPlayer.GetComponent<AudioSource>();

        if(music == true)
        {
            musicButtonImage.sprite = musicOnSprite;
        }
        else if(music == false)
        {
            aus.Stop();
            musicButtonImage.sprite = musicOffSprite;
        }
    }
    public void BtnSounds()
    {
        bool sounds = YandexGame.savesData.sounds;

        if (sounds == true)
        {
            buttonPlayer.volume = 0f;
            soundsButtonImage.sprite = soundsOffSprite;
            YandexGame.savesData.sounds = false;
            YandexGame.SaveProgress();
        }
        else if (sounds == false)
        {
            buttonPlayer.Play();
            buttonPlayer.volume = 1f;
            soundsButtonImage.sprite = soundsOnSprite;
            YandexGame.savesData.sounds = true;
            YandexGame.SaveProgress();
        }
    }
    public void BtnMusic()
    {
        bool music = YandexGame.savesData.music;
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer");
        AudioSource aus = musicPlayer.GetComponent<AudioSource>();
        buttonPlayer.Play();

        if (music == true)
        {
            aus.Pause();
            musicButtonImage.sprite = musicOffSprite;
            YandexGame.savesData.music = false;
            YandexGame.SaveProgress();
        }
        else if (music == false)
        {
            aus.Play();
            musicButtonImage.sprite = musicOnSprite;
            YandexGame.savesData.music = true;
            YandexGame.SaveProgress();
        }
    }

    public void BtnStartGame()
    {
        buttonPlayer.Play();
        fadeController.FadeIn(smoothTransition);
        int completedLevels = YandexGame.savesData.completedLevels;
        StartCoroutine(Delay(1));
    }
    public void BtnOpenAllGames()
    {
        buttonPlayer.Play();
        fadeController.FadeIn(gamesMenu);
    }
    public void BtnCloseAllGames()
    {
        buttonPlayer.Play();
        fadeController.FadeIn(gamesMenu);
    }
    public void BtnGamesNOorYES()
    {
        buttonPlayer.Play();
        fadeController.FadeOut(gamesMenu);
    }

    private IEnumerator Delay(int levelIndex)
    {
        yield return new WaitForSeconds(1f);
        StopCoroutine(Delay(levelIndex));
        SceneManager.LoadScene(levelIndex);
    }
}