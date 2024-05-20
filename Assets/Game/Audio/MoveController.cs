using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public void MoveIn(RectTransform moveTransform)
    {
        StopCoroutines();
        moveTransform.gameObject.SetActive(true);
        StartCoroutine(MoveInCoroutine(moveTransform));
    }

    public void MoveOut(RectTransform moveTransform)
    {
        StopCoroutines();
        StartCoroutine(MoveOutCoroutine(moveTransform));
    }

    private void StopCoroutines()
    {
        StopAllCoroutines();
    }



    public IEnumerator MoveInCoroutine(RectTransform moveTransform)
    {
        Vector2 desiredPosition = Vector2.zero;
        float timePassd = 0f;

        while(timePassd < moveSpeed)
        {
            float normalizedTime = timePassd / moveSpeed;
            moveTransform.anchoredPosition = Vector2.Lerp(moveTransform.anchoredPosition, desiredPosition, normalizedTime);

            yield return null;
            timePassd += Time.deltaTime;
        }

        moveTransform.anchoredPosition = Vector2.zero;
    }

    public IEnumerator MoveOutCoroutine(RectTransform moveTransform)
    {
        Vector2 desiredPosition = new Vector2(0, 1500f);
        float timePassd = 0f;

        while (timePassd < moveSpeed)
        {
            float normalizedTime = timePassd / moveSpeed;
            moveTransform.anchoredPosition = Vector2.Lerp(moveTransform.anchoredPosition, desiredPosition, normalizedTime);

            yield return null;
            timePassd += Time.deltaTime;
        }

        moveTransform.anchoredPosition = new Vector2(0, 1500f);
        moveTransform.gameObject.SetActive(false);
    }
}