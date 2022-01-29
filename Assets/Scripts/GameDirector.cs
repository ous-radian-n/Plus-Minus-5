using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    [SerializeField]
    Text scoreText, levelText;
    [SerializeField]
    List<Sprite> numImage = new List<Sprite>();

    [SerializeField]
    velt vl;

    public int Score = 0, Level = 1;
    int leftChainNum = 0, rightChainNum = 0;

    [SerializeField]
    int fiveScore = 200, plusMinusFiveScore = 2000, levelDist = 5000;

    int nowNum, nextNum, leftNum = 0, rightNum = 0;
    bool isFirstHolded = false, isHolded = false;
    int holdNum = 0;
    [SerializeField]
    Image nowNumImage, nextNumImage, leftNumImage, rightNumImage, holdNumImage;

    // Start is called before the first frame update
    void Start()
    {
        // float speed = 0;


        //int rnd = Random.Range(1, 10);
        //public static int Range(int min, int max);

        //textUI.text = "Game Start";

        nowNum = ReturnRandomNum();
        nextNum = ReturnRandomNum();
    }

    // Update is called once per frame
    void Update()
    {
        nowNumImage.sprite = numImage[ReturnNumImageIndex(nowNum)];
        nextNumImage.sprite = numImage[ReturnNumImageIndex(nextNum)];
        if (ReturnNumImageIndex(leftNum) >= 0 && ReturnNumImageIndex(leftNum) < numImage.Count)
            leftNumImage.sprite = numImage[ReturnNumImageIndex(leftNum)];
        if (ReturnNumImageIndex(rightNum) >= 0 && ReturnNumImageIndex(rightNum) < numImage.Count)
            rightNumImage.sprite = numImage[ReturnNumImageIndex(rightNum)];
        scoreText.text = Score.ToString();
        if (isFirstHolded)
            holdNumImage.sprite = numImage[ReturnNumImageIndex(holdNum)];
        Level = Score / levelDist + 1;
        levelText.text = Level.ToString();
    }

    private int ReturnNumImageIndex(int num)
    {
        return num + 9;
    }

    public int ReturnRandomNum()
    {
        int i = 0;
        while (i == 0 || i == -5 || i == 5 || ReturnNumImageIndex(i) < 0 || ReturnNumImageIndex(i) >= numImage.Count)
        {
            i = (Mathf.FloorToInt(Mathf.Pow(Random.value, 1.85f) * 9.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
        }
        return i;
    }

    public void SwitchNextNum()
    {
        nowNum = nextNum;
        nextNum = ReturnRandomNum();
    }

    public bool EnterNum2Stock(bool isRight)
    {
        bool isBurst = false;
        if (!isRight)
        {
            leftNum += nowNum;
            if(leftNum * leftNum >= 100)
            {
                /* ダメージ処理(表示演出) */
                leftNum = ReturnRandomNum();
                isBurst = true;
            }
        }
        else
        {
            rightNum += nowNum;
            if (rightNum * rightNum >= 100)
            {
                /* ダメージ処理(表示演出) */
                rightNum = ReturnRandomNum();
                isBurst = true;
            }
        }
        if (!isBurst)
        {
            if (IsPlusMinusFive())
            {
                Score += plusMinusFiveScore;
                /* 演出, お邪魔ブロック一掃 */
                leftNum = ReturnRandomNum();
                rightNum = ReturnRandomNum();
            }
            else
            {
                if (IsFive(false))
                {
                    Score += fiveScore;
                    /* 演出, お邪魔ブロックを除去 */
                    leftChainNum++;
                    if (leftChainNum >= 3)
                    {
                        leftChainNum = 0;
                        leftNum = ReturnRandomNum();
                    }
                }
                else
                {
                    leftChainNum = 0;
                }
                if (IsFive(true))
                {
                    Score += fiveScore;
                    /* 演出, お邪魔ブロックを除去 */
                    rightChainNum++;
                    if (rightChainNum >= 3)
                    {
                        rightChainNum = 0;
                        rightNum = ReturnRandomNum();
                    }
                }
                else
                {
                    rightChainNum = 0;
                }
            }
        }
        return isBurst;
    }

    bool IsFive(bool isRight)
    {
        if (!isRight) return leftNum * leftNum == 25;
        else return rightNum * rightNum == 25;
    }

    bool IsPlusMinusFive()
    {
        return leftNum * rightNum == -25;
    }

    public void HoldNum()
    {
        if (!isHolded)
        {
            int num = 0;
            if (isFirstHolded) num = holdNum;
            holdNum = nowNum;
            if (num == 0 && !isFirstHolded)
            {
                SwitchNextNum();
                isFirstHolded = true;
            }
            else
            {
                nowNum = num;
            }
            vl.ResetValue();
            isHolded = true;
        }
    }

    public void TurnOffHoldFlag()
    {
        isHolded = false;
    }
}



