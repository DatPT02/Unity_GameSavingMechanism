using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerScore : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;

    Player player;
    private int p_score = 0;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        scoreText.text = player.Score.ToString();
    }
    public void AddScore(int value)
    {
        player.Score += value;
    }
}
