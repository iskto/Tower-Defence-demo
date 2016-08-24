using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    private float baseWidth = 1280;
    private float baseHeight = 720;
    private float baseOrthographicSize = 93f;

    private Vector3 dirCamera;

    void Awake()
    {
        if (((float)Screen.height / (float)Screen.width) < 0.7f)
        {
            baseWidth = 1280;
            baseHeight = 720;
        }
        else
        {
            baseWidth = 1024;
            baseHeight = 768;
        }
        float newOrthographicSize = (float)Screen.height / (float)Screen.width * this.baseWidth / this.baseHeight * this.baseOrthographicSize;
        GetComponent<Camera>().fieldOfView = newOrthographicSize;
    }

	// Use this for initialization
	void Start () {
        Resources.UnloadUnusedAssets(); // clean memory
	}
	
	// Update is called once per frame
	void Update () {
        // 主動回收垃圾
        if (Time.frameCount % 100 == 0)
        {
            System.GC.Collect();
        }
    }
}
