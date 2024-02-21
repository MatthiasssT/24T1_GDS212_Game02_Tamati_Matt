using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public GameObject upgradePanel;
    public List<PowerUpSystem> possiblePowerUps;
    public List<Sprite> upgradeSprites;
    public List<Button> upgradeButtons; // List of Button components representing the buttons in the upgrade panel

    private bool roundOver = true;

    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    private void Update()
    {
        if (roundOver)
        {
            roundOver = false;
            OpenUpgradePanel();
        }
    }

    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);

        // Assign random power ups to upgrade buttons
        List<int> chosenIndexes = new List<int>();
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            int randomIndex = Random.Range(0, possiblePowerUps.Count);
            while (chosenIndexes.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, possiblePowerUps.Count);
            }
            chosenIndexes.Add(randomIndex);

            // Set button image
            Image buttonImage = upgradeButtons[i].GetComponentInChildren<Image>();
            buttonImage.sprite = upgradeSprites[randomIndex];
            int index = randomIndex; // Ensure index value is captured in lambda
            upgradeButtons[i].onClick.AddListener(() => PlayerChose(index));
        }
    }

    public void PlayerChose(int powerUpIndex)
    {
        // Apply the selected power up to the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            possiblePowerUps[powerUpIndex].Apply(player);
        }

        // Hide the upgrade panel
        upgradePanel.SetActive(false);

        roundOver = true; // Allow next round to start
    }
}
