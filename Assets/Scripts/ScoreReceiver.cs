using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReceiver : MonoBehaviour
{
    int score = 0;
    [SerializeField]
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = ScoreManager.instance.ScoreMessage;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
