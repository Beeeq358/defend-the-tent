using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenMoving : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title, buttonText;
    private float titleSize = 140, buttonSize = 110;
    private Color randomColor;

    private void Start()
    {
        StartCoroutine(Grow(true)); // Start with growing the title
        StartCoroutine(Shrink(false)); // Start with growing the button
    }

    private void Update()
    {
        title.fontSize = titleSize;
        buttonText.fontSize = buttonSize;
    }

    private IEnumerator Grow(bool isTitle)
    {
        float time = 0;
        float duration = 0.7f;
        float startSize = 110;
        float endSize = 140;

        while (time < duration)
        {
            if (isTitle)
                titleSize = Mathf.Lerp(startSize, endSize, time / duration);
            else
                buttonSize = Mathf.Lerp(startSize, endSize, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        if (isTitle)
        {
            titleSize = endSize;
            title.color = GetRandomColor();

        }
        else
        {
            buttonSize = endSize;
            buttonText.color = GetRandomColor();
        }

        StartCoroutine(Shrink(isTitle));
    }

    private IEnumerator Shrink(bool isTitle)
    {
        float time = 0;
        float duration = 0.7f;
        float startSize = 140;
        float endSize = 110;

        while (time < duration)
        {
            if (isTitle)
                titleSize = Mathf.Lerp(startSize, endSize, time / duration);
            else
                buttonSize = Mathf.Lerp(startSize, endSize, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        if (isTitle)
        {
            titleSize = endSize;
            title.color = GetRandomColor();
        }
        else
        {
            buttonSize = endSize;
            buttonText.color = GetRandomColor();
        }
        StartCoroutine(Grow(isTitle));
    }


    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}