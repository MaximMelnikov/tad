using UnityEngine;
using UnityEngine.UI;

public class TAFpsCounter : MonoBehaviour {
    float deltaTime = 0.0f;
    Text text; 

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        text.text = string.Format(fps + " fps");
    }

    void Start()
    {
        text = GetComponent<Text>();
    }
}
