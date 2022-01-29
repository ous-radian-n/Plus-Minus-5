using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    public Image sliderNextNumImage;
    [SerializeField]
    List<Sprite> numImage = new List<Sprite>();

    public Text textUI;

    int nowNum, nextNum, leftNum = 0, rightNum = 0;
    [SerializeField]
    Image nowNumImage, nextNumImage, leftNumImage, rightNumImage;

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
        leftNumImage.sprite = numImage[ReturnNumImageIndex(leftNum)];
        rightNumImage.sprite = numImage[ReturnNumImageIndex(rightNum)];
    }

    private int ReturnNumImageIndex(int num)
    {
        return num + 9;
    }

    public int ReturnRandomNum()
    {
        int i = 0;
        while (i == 0 || i == -5 || i == 5)
        {
            i = Mathf.CeilToInt(Random.value * 19.0f - 9.0f);
        }
        return i;
    }

    public void SwitchNextNum()
    {
        nowNum = nextNum;
        nextNum = ReturnRandomNum();
    }
}



