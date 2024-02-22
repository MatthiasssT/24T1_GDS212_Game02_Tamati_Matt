using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text countdownText; // Reference to the TextMeshPro text component for countdown
    public TMP_Text roundText; // Reference to the TextMeshPro text component for round
    public float countdownDuration = 3f; // Duration of the countdown in seconds
    private bool isCountdownComplete = false; // Flag to track if the countdown is complete
    private int currentRound = 0; // Variable to store the current round number

    public SpawnController spawnController; // Reference to the SpawnController

    void Start()
    {
        isCountdownComplete = false;
        roundText.text = "Round: " + currentRound; // Initialize round text
        StartCoroutine(StartCountdown());
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }

    IEnumerator StartCountdown()
    {
        float currentTime = countdownDuration;

        // Countdown loop
        while (currentTime > 0f)
        {
            // Fade out the countdown text
            StartCoroutine(FadeText(currentTime));

            countdownText.text = Mathf.CeilToInt(currentTime).ToString(); // Display the current countdown number

            // Wait for a fraction of a second longer
            float waitTime = 1.2f; // Adjust this value to control the duration of each countdown number display
            yield return new WaitForSeconds(waitTime);

            currentTime -= 1f; // Decrement the countdown timer
        }

        // Set the countdown text to "Go" when the countdown is complete
        countdownText.text = "Go";

        // Fade out the "Go" text over 2 seconds
        yield return StartCoroutine(FadeOutGoText());

        // Set the text object inactive at the end of the countdown
        countdownText.gameObject.SetActive(false);

        // Set the countdown complete flag to true
        isCountdownComplete = true;

        // Call method when countdown is complete
        CountdownComplete();
    }

    IEnumerator FadeText(float time)
    {
        // Calculate fade duration
        float fadeDuration = 0.5f;
        float fadeTimer = 0f;

        // Initial scale
        Vector3 initialScale = countdownText.transform.localScale;

        // Fade out loop
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration); // Calculate alpha value for fading out
            Color textColor = countdownText.color;
            textColor.a = alpha; // Set alpha value to text color
            countdownText.color = textColor; // Apply modified text color

            // Scale down the text
            countdownText.transform.localScale = Vector3.Lerp(initialScale, initialScale * 0.5f, fadeTimer / fadeDuration);

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // Set text color to fully transparent
        Color transparentColor = countdownText.color;
        transparentColor.a = 0f;
        countdownText.color = transparentColor;

        // Reset the scale to 1
        countdownText.transform.localScale = initialScale;
    }

    IEnumerator FadeOutGoText()
    {
        // Calculate fade duration
        float fadeDuration = 2f;
        float fadeTimer = 0f;

        Color startColor = countdownText.color;

        // Fade out loop
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration); // Calculate alpha value for fading out
            Color textColor = countdownText.color;
            textColor.a = alpha; // Set alpha value to text color
            countdownText.color = textColor; // Apply modified text color

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // Set text color to fully transparent
        Color transparentColor = countdownText.color;
        transparentColor.a = 0f;
        countdownText.color = transparentColor;
    }

    void CountdownComplete()
    {
        // Do something when the countdown is complete
        Debug.Log("Countdown is complete!");

        // Example: Start the round
        StartRound();
    }

    public void StartRound()
    {
        // Increment the round number
        currentRound++;

        // Update round text
        roundText.text = "Round: " + currentRound;

        // Start the round
        Debug.Log("Starting Round " + currentRound);

        // Start the round in the SpawnController
        spawnController.StartNextRound();
    }
}
