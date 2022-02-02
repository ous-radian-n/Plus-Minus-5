using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    int fiveScore = 200, plusMinusFiveScore = 2000, levelDist = 5000, bubbleScore = 50;

    [SerializeField]
    float random_Num;//大きさ

    [SerializeField]
    GameObject[] bubble_Prefab;
    
    int random_Unit;//個数
    bool isBubbling = true;

    int nowNum, nextNum, leftNum = 0, rightNum = 0;
    bool isFirstHolded = false, isHolded = false;
    int holdNum = 0;
    [SerializeField]
    Image nowNumImage, nextNumImage, leftNumImage, rightNumImage, holdNumImage;

    bool isGameOver = false;
    [SerializeField]
    string resultSceneName;

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
        if(!isGameOver) {
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
        else
        {
            /* ゲームオーバー(リザルト画面へ) */
            ScoreManager.instance.ScoreMessage = Score;
            ScoreManager.instance.LevelMessage = Level;
            SceneManager.LoadScene(resultSceneName);
        }
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
            if (Level <= 3)
            {
                i = (Mathf.FloorToInt(Mathf.Pow(Random.value, 2.5f) * 4.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
            }
            else
            {
                i = (Mathf.FloorToInt(Mathf.Pow(Random.value, (float)(6 + Level) / (float)Level) * 9.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
            }
        }
        return i;
    }

    public void SwitchNextNum()
    {
        nowNum = nextNum;
        nextNum = ReturnRandomNum();
    }

    public void EnterNum2Stock(bool isRight)
    {
        if (isBubbling)
        {
            bool isBurst = false;
            if (!isRight)
            {
                leftNum += nowNum;
                if (leftNum * leftNum >= 100)
                {
                    /* ダメージエフェクト */
                    bubbleCloning(true, true);
                    leftNum = ReturnRandomNum();
                    isBurst = true;
                }
            }
            else
            {
                rightNum += nowNum;
                if (rightNum * rightNum >= 100)
                {
                    /* ダメージエフェクト */
                    bubbleCloning(true, true);
                    rightNum = ReturnRandomNum();
                    isBurst = true;
                }
            }
            if (!isBurst)
            {
                if (IsPlusMinusFive())
                {
                    Score += plusMinusFiveScore;

                    /* 「±5」エフェクト */
                    deleteBubblesAll(true);
                    deleteBubblesAll(false);

                    leftNum = ReturnRandomNum();
                    rightNum = ReturnRandomNum();

                    leftChainNum = 0;
                    rightChainNum = 0;
                }
                else if (IsFive())
                {
                    if (IsFive(true, false))
                    {
                        Score += fiveScore;
                        leftChainNum++;
                        if (leftChainNum >= 3)
                        {
                            leftChainNum = 0;
                            /* 連鎖終了エフェクト */
                            leftNum = ReturnRandomNum();
                        }
                        else
                        {
                            /* バブル一掃エフェクト */
                            if (leftNum == 5)
                            {
                                deleteBubblesAll(true);
                            }
                            else
                            {
                                deleteBubblesAll(false);
                            }
                        }
                    }
                    else
                    {
                        leftChainNum = 0;
                    }
                    if (IsFive(true, true))
                    {
                        Score += fiveScore;
                        rightChainNum++;
                        if (rightChainNum >= 3)
                        {
                            rightChainNum = 0;
                            /* 連鎖終了エフェクト */
                            rightNum = ReturnRandomNum();
                        }
                        else
                        {
                            /* バブル一掃エフェクト */
                            if (rightNum == 5)
                            {
                                deleteBubblesAll(true);
                            }
                            else
                            {
                                deleteBubblesAll(false);
                            }
                        }
                    }
                    else
                    {
                        rightChainNum = 0;
                    }
                }
                else
                {
                    leftChainNum = 0;
                    rightChainNum = 0;
                    if (nowNum > 0)
                    {
                        bubbleCloning(true);
                    }
                    else if (nowNum < 0)
                    {
                        bubbleCloning(false);
                    }
                }
            }
        }
    }

    bool IsFive(bool selection = false, bool isRight = false)
    {
        if (selection) {
            if (!isRight) return leftNum * leftNum == 25;
            else return rightNum * rightNum == 25;
        }
        return (leftNum * leftNum == 25) || (rightNum * rightNum == 25);
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

    public void SwitchGameOver()
    {
        isGameOver = true;
    }

    void bubbleCloning(bool isPlus, bool isBursted = false)
    {
        random_Num = Random.Range(1.0f, 3.1f);
        random_Unit = Random.Range(1, 4);
        if (isBursted) random_Unit = 5;
        Debug.Log(random_Num.ToString("f0"));
        Debug.Log(random_Unit.ToString());
        GameObject[] obj = new GameObject[random_Unit];
        for (int i = 0; i < random_Unit; i++)
        {
            if (isPlus || isBursted)
            {
                obj[i] = Instantiate(bubble_Prefab[0],
                    new Vector3(Random.Range(-2.0f, 2.0f), 6.0f, 0.0f), Quaternion.identity);
                //break;
            }
            if (!isPlus || isBursted)
            {
                obj[i] = Instantiate(bubble_Prefab[1],
                    new Vector3(Random.Range(-2.0f, 2.0f), 6.0f, 0.0f), Quaternion.identity);
                //break;
            }
        }
        isBubbling = false;
    }

    void deleteBubblesAll(bool isPlus)
    {
        if (isPlus)
        {
            var plusBubble = GameObject.FindGameObjectsWithTag("PlusBubble");
            if (plusBubble.Length > 0)
            {
                Score += bubbleScore * plusBubble.Length;
                for (int i = 0; i < plusBubble.Length; ++i)
                {
                    GameObject.Destroy(plusBubble[i]);
                }
            }
        }
        else
        {
            var minusBubble = GameObject.FindGameObjectsWithTag("MinusBubble");
            if (minusBubble.Length > 0)
            {
                Score += bubbleScore * minusBubble.Length;
                for (int i = 0; i < minusBubble.Length; ++i)
                {
                    GameObject.Destroy(minusBubble[i]);
                }
            }
        }
    }

    public void TurnOnBubblingFlag()
    {
        isBubbling = true;
    }

    public bool ReturnStockingFlag()
    {
        return isBubbling;
    }
}



