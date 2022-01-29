using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    public Text textUI;


    // Start is called before the first frame update
    void Start()
    {
        float speed = 0;

    
        //int rnd = Random.Range(1, 10);
        //public static int Range(int min, int max);

        //textUI.text = "Game Start";


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.speed = 0.2f;

        }

        transform.Translate(this.speed, 0, 0);
        this.speed *= 0.98f;



        public float score = 0;
        public float totalScore  0;

        

    }
}
