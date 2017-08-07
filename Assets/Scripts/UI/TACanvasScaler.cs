using UnityEngine;
using System.Collections;

public class TACanvasScaler : MonoBehaviour {
	void Awake () {
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
    }
}