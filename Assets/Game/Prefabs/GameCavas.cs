using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using YG;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private AudioSource buttonPlayer;
    [SerializeField] private FadeController fadeController;
    [Space(10)]
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup gameMenu;
    [SerializeField] private CanvasGroup smoothTransition;

    public static UnityEvent PauseEvent = new UnityEvent();

    private void Awake()
    {
        LoadSoundsSettings();
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
        }
        else if (sounds == false)
        {
            buttonPlayer.volume = 0f;
        }
    }
    public void PauseBtn()
    {
        buttonPlayer.Play();
        PauseEvent.Invoke();
        fadeController.FadeIn(pauseMenu);
        fadeController.FadeOut(gameMenu);
    }
    public void ResumeBtn()
    {
        buttonPlayer.Play();
        PauseEvent.Invoke();
        fadeController.FadeIn(gameMenu);
        fadeController.FadeOut(pauseMenu);
    }
    public void RestartBtn()
    {
        buttonPlayer.Play();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(DelayLoad(sceneIndex));
        fadeController.FadeIn(smoothTransition);
    }
    public void HomeBut()
    {
        buttonPlayer.Play();
        StartCoroutine(DelayLoad(0));
        fadeController.FadeIn(smoothTransition);
    }
    private IEnumerator DelayLoad(int levelIndex)
    {
        yield return new WaitForSeconds(1f);
        StopCoroutine(DelayLoad(levelIndex));
        SceneManager.LoadScene(levelIndex);
    }
}