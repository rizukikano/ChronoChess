using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject dangerUI;
    public GameObject gameOverUI;
    public GameObject[] healthUI;
    public GameObject[] queueUI;
    public Image radialTimer;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void ShowGameOver(int score)
    {
        finalScoreText.text = score.ToString();
        gameOverUI.SetActive(true);
    }
    public void UpdateRadialTimer(float elapsedTime,float totalDuration)
    {
        float remainingTime = Mathf.Clamp(totalDuration - elapsedTime, 0f, totalDuration);
        float fillAmount = Mathf.Clamp01(remainingTime / totalDuration);
        radialTimer.fillAmount = fillAmount;
    }
    void Start()
    {
        ResetHealthUI(true);
    }

    IEnumerator ShowInvalidPlacementUI()
    {
        dangerUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        dangerUI.SetActive(false);
    }
    public void HandleInvalidUI()
    {
        StartCoroutine(ShowInvalidPlacementUI());
    }
    public void UpdateHealthUI(int health){
        healthUI[health].SetActive(false);
    }
    public void UpdateScore(int score){
        scoreText.text = $"Score : {score}";
    }
    public void ResetHealthUI(bool show)
    {
        foreach (GameObject obj in healthUI)
        {
            if (obj != null)
            {
                obj.SetActive(show);
            }
        }
    }
    public void SetQueueUI(Queue<Piece> pieceQueue)
    {
        List<Piece> pieceList = pieceQueue.ToList();
        if (queueUI.Length == pieceList.Count)
        {
            for (int i = 0; i < queueUI.Length; i++)
            {
                Image pieceImageComponent = queueUI[i].GetComponent<Image>();
                if (pieceImageComponent != null)
                {
                    Piece piece = pieceList[i];
                    pieceImageComponent.sprite = piece.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        else
        {
            Debug.LogError("QueueUI length does not match the PieceQueue length.");
        }
    }
}
