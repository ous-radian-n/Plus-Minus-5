using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class velt : MonoBehaviour
{
    [SerializeField]
    GameDirector director;

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
        if(value <= 0.0f)
        {
            /* ストックに送る処理 */

            director.SwitchNextNum();
            value = 1.00f;
        }
        slider.value = value;
    }
}
