using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressShrink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float shrinkScale = 0.9f;   
    [SerializeField] private float duration = 0.05f;      

    private Vector3 originalScale;
    private Coroutine currentRoutine;

    void OnEnable()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable) return; // Don't shrink if button is disabled
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ScaleTo(originalScale * shrinkScale));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable) return; // Don't scale back if button is disabled
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ScaleTo(originalScale));
    }

    System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}