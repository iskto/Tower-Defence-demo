using UnityEngine;
using System.Collections;

public class HomeScript : MonoBehaviour {

    public int homeHP;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy")
        {
            homeHP -= 1;
            if (homeHP <= 0)
            {
                homeHP = 0;
                Destroy(this.gameObject);
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

}
