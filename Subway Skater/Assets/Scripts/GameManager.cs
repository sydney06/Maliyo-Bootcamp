using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;

    public static bool mute = false;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }

    private bool isGameStarted = false;
    private PlayerMotor motor;

    // UI and UI fields
    public Animator gameCanvas, menuAnim, diamondAnim;
    public Text scoreText, coinText, modifierText, highscoreText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    // Death Menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadCoinText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();

        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");

        highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update()
    {
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
        }
        if (isGameStarted && !IsDead)
        {
            // Bump the score up
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore ++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        //Check for highscore
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("Highscore", (int)s);
        }
    }
}