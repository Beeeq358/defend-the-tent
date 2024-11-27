using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenMoving : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title, buttonText;
    private float titleSize = 140, buttonSize = 110;
    private Color randomColor;

    private void Start()
    {
        StartCoroutine(Grow(true));
        StartCoroutine(Shrink(false));
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
        List<Color> colors = new()
        {
            Color.white,
            Color.black,
            Color.magenta,
            Color.cyan,
            Color.gray
        };
        return colors[Random.Range(0, colors.Count)];
    }
}