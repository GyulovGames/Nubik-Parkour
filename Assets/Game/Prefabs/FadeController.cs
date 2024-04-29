using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private float fadeDuration;


    public void FadeIn( CanvasGroup fadeInGroup)
    {
        fadeInGroup.gameObject.SetActive(true);
        StartCoroutine(InCoroutine(fadeInGroup));      
    }
    public void FadeOut(CanvasGroup fadeOutGroup)
    {
        StartCoroutine(OutCoroutine(fadeOutGroup));
    }



    public IEnumerator InCoroutine( CanvasGroup inGroup)
    {
        float appearGroupAlpha = inGroup.alpha;
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            float normalizedTime = timePassed / fadeDuration;
            inGroup.alpha = Mathf.Lerp(appearGroupAlpha, 1f, normalizedTime);

            yield return null;
            timePassed += Time.deltaTime;
        }

        inGroup.alpha = 1f;
    }

    public IEnumerator OutCoroutine(CanvasGroup inGroup)
    {
        float appearGroupAlpha = inGroup.alpha;
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            float normalizedTime = timePassed / fadeDuration;
            inGroup.alpha = Mathf.Lerp(appearGroupAlpha, 0f, normalizedTime);

            yield return null;
            timePassed += Time.deltaTime;
        }

        inGroup.alpha = 0f;
        inGroup.gameObject.SetActive(false);
    }
}