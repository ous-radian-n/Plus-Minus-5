using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class velt : MonoBehaviour
{
    [SerializeField]
    GameDirector director;
    [SerializeField]
    switching sw;
    [SerializeField]
    int steps = 4;

    public Slider slider;
    public float time;
    [SerializeField]
    public float velocity = 0.25f;
    float value = 1.00f;
    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        value -= velocity * Time.deltaTime;
        if (value <= 0.0f)
        {
            if (director.EnterNum2Stock(sw.isRight))
            {
                /* ダメージ処理(HP側) */
                director.SwitchNextNum();
            }
            director.SwitchNextNum();
            value = 1.00f;
        }
        else
        {
            slider.value = Mathf.Ceil(value * (float)steps) / (float)steps;
        }
    }
}
