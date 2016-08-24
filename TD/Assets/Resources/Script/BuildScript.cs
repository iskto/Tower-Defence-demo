using UnityEngine;
using System.Collections;

public class BuildScript : MonoBehaviour {

    private GameObject Tower; // 塔

    private string TowerPath = "Prefabs/Human/Cannon/Human_Cannon";

    private string TowerName = "Human_Cannon";

    private GameObject towerClone; // 暫存產生的塔

    private Color32 BulidColor, ErrorColor; // 可建築的地板顏色，不可建築的地板顏色

    private int TowerStyle; // 塔的種類

    private GameObject PassEnemy; // 暫存經過的Enemy

    private MeshRenderer FloorMeshRenderer; // 暫存地板的Mesh Renderer

	// Use this for initialization
	void Start () {
        // 初始化地板顏色
        BulidColor = new Color32(140, 255, 203, 70);
        ErrorColor = new Color32(221, 126, 126, 70);
        // 設定塔的種類
        Tower = (GameObject)Resources.Load(TowerPath, typeof(GameObject));
        // 暫存地板的Mesh Renderer
        FloorMeshRenderer = this.gameObject.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (TowerStyle == 0)
            {
                TowerStyle = 1;

                TowerPath = "Prefabs/Tower";
                TowerName = "Tower";
                Tower = (GameObject)Resources.Load(TowerPath, typeof(GameObject));
            }
            else if (TowerStyle == 1)
            {
                TowerStyle = 0;

                TowerPath = "Prefabs/Human/Cannon/Human_Cannon";
                TowerName = "Human_Cannon";
                Tower = (GameObject)Resources.Load(TowerPath, typeof(GameObject));
            }   
        }
	}

    // 經過方格時
    void OnMouseOver()
    {
        if (PassEnemy || towerClone)
        {
            FloorMeshRenderer.enabled = true; // 開啟renender
            this.GetComponent<Renderer>().material.color = ErrorColor;
        }
        else
        {
            FloorMeshRenderer.enabled = true;
            this.GetComponent<Renderer>().material.color = BulidColor;
        }
        
    }

    // 離開時
    void OnMouseExit()
    {
        FloorMeshRenderer.enabled = false; // 關閉renderer
    }

    void OnMouseUp()
    {
        if (!towerClone && !PassEnemy)
        {
            towerClone = (GameObject)Instantiate(Tower, transform.position, Quaternion.identity);
            towerClone.name = TowerName;
        }
   
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy")
        {
            PassEnemy = other.gameObject;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject == PassEnemy)
        {
            PassEnemy = null;
        }
    }

}
