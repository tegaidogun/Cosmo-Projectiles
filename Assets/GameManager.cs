using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private string[] gameOverTexts =
    {
        "Mission Failed! Your ship couldn't withstand the forces of space. Try again with different settings.",
        "Oh no! Your ship has exploded due to excessive momentum. Adjust your parameters and give it another go!",
        "Game Over! The journey to space is treacherous. Tweak your settings and try again.",
        "Crash and Burn! Your ship couldn't make it this time. Try again with a new strategy."
    };

    private string[] victoryTexts =
    {
        "Mission Accomplished! Your ship has successfully reached its destination. Well done!",
        "Success! Your skillful calculations have led your ship safely through the journey. Great job!",
        "Victory! Your ship soared through space and reached its target. Excellent work!",
        "Well Done! You've successfully navigated the dangers of space and achieved your mission."
    };

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI victoryText;

    public GameObject gameOver;
    public GameObject victory;
    public GameObject pauseScreen;
    public GameObject rocketShip;

    public string GetRandomGameOverText()
    {
        int index = Random.Range(0, gameOverTexts.Length);
        return gameOverTexts[index];
    }

    public string GetRandomVictoryText()
    {
        int index = Random.Range(0, victoryTexts.Length);
        return victoryTexts[index];
    }

    public void GameOver()
    {
        rocketShip.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverText.text = GetRandomGameOverText();
        gameOver.SetActive(true);
        AudioManager.Instance.PlaySFX("Game Over");
    }

    public void Victory()
    {
        rocketShip.SetActive(false);
        pauseScreen.SetActive(false);
        victoryText.text = GetRandomVictoryText();
        victory.SetActive(true);
        AudioManager.Instance.PlaySFX("Victory");
    }
}
