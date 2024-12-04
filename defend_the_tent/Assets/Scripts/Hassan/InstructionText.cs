using TMPro;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI instructionText;

    [SerializeField]
    private float growDuration = 1f;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float timer;

    private void Start()
    {
        // Initialize the initial and target scales
        initialScale = new Vector3(50f / 500f, 80f / 500f, 1f);
        targetScale = new Vector3(3f, 3f, 1f);
        instructionText.transform.localScale = initialScale;

        // Destroy the instruction text after 3 seconds
        Destroy(instructionText.gameObject, 3f);
    }

    private void Update()
    {
        if (timer < growDuration)
        {
            // Increment the timer based on time elapsed
            timer += Time.deltaTime;

            // Interpolate between the initial and target scales
            instructionText.transform.localScale = Vector3.Lerp(initialScale, targetScale, timer / growDuration);
        }
    }
}
