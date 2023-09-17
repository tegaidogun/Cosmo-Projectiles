using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private string[] gameOverTextsMomentum =
    {
        "Mission Unsuccessful! Your rocket couldn't maintain the necessary momentum to break through the atmosphere. Reevaluate your settings and try again!",
        "Oops! Your vessel couldn't handle the high momentum. Adjust your variables to ensure a steadier ascent!",
        "Attempt Failed! Overwhelming momentum caused your ship to lose its course. Fine-tune your settings for a more controlled flight!",
        "Tragedy Strikes! Your rocket succumbed to the forces of momentum. Reconfigure your parameters for a more balanced voyage."
    };

    private string[] gameOverTextsFalling =
    {
        "Mission Failed! Your ship fell back to Earth due to insufficient thrust. Adjust your settings for a stronger lift-off.",
        "Oh no! Your ship couldn't sustain its altitude and fell. Revisit your parameters to prevent a free fall on your next attempt!",
        "Game Over! Your spacecraft succumbed to gravity's pull. Modify your settings for a successful journey to the stars.",
        "Crash and Burn! Your ship plummeted back down. Redefine your strategy to prevent a fall on your next venture."
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

    public string GetRandomGameOverTextMomentum()
    {
        int index = Random.Range(0, gameOverTextsMomentum.Length);
        return gameOverTextsMomentum[index];
    }
    public string GetRandomGameOverTextFalling()
    {
        int index = Random.Range(0, gameOverTextsFalling.Length);
        return gameOverTextsFalling[index];
    }

    public string GetRandomVictoryText()
    {
        int index = Random.Range(0, victoryTexts.Length);
        return victoryTexts[index];
    }

    public void GameOverMomentum()
    {
        rocketShip.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverText.text = GetRandomGameOverTextMomentum();
        gameOver.SetActive(true);
        AudioManager.Instance.PlaySFX("Game Over");
    }
    public void GameOverFalling()
    {
        rocketShip.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverText.text = GetRandomGameOverTextFalling();
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
