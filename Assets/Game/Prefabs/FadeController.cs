using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private float fadeDuration;


    public void Appear( CanvasGroup appearGroup)
    {
        StartCoroutine(AppearCoroutine(appearGroup));
        appearGroup.gameObject.SetActive(true);
    }

    public IEnumerator AppearCoroutine( CanvasGroup group)
    {
        float appearGroupAlpha = group.alpha;
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            float normalizedTime = timePassed / fadeDuration;
            group.alpha = Mathf.Lerp(appearGroupAlpha, 1f, normalizedTime);

            yield return null;
            timePassed += Time.deltaTime;
        }

        group.alpha = 1f;
    }



    public void Disappear(CanvasGroup disappearGroup)
    {
        StartCoroutine(DisappearCoroutine(disappearGroup));
    }

    public IEnumerator DisappearCoroutine(CanvasGroup group)
    {
        float disappearGroupAlpha = group.alpha;      
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            float normalizedTime = timePassed / fadeDuration;
            group.alpha = Mathf.Lerp(disappearGroupAlpha, 0f, normalizedTime);

            yield return null;
            timePassed += Time.deltaTime;
        }

        group.alpha = 0f;
        group.gameObject.SetActive(false);
    }
}