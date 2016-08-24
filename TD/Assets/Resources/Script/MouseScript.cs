using UnityEngine;
using System.Collections;

public class MouseScript : MonoBehaviour {

    public float y;
    public float ySpeed = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        y += Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        if (Input.mousePosition.y <= 5 || Input.mousePosition.y >= Screen.height - 5)
        {
            if (transform.position.z + y < 9.8f && transform.position.z + y > -17.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + y);
            }
        }
    }
}
