using UnityEngine;
using System.Collections;

public class EnemySpawnScript : MonoBehaviour {

    private GameObject Enemy; // Enemy的prefab

    private string EnemyPath = "Prefabs/Human/Enemy/Enemy";

    private GameObject EnemyClone; // 生產出的Enemy Clone

    public float SpawnTime = 1.0f, SpawnInterval = 3.0f; // 開始生產的時間， 生產的時間間隔


    /* 有BUG所以換方法
    public bool isReset; // 暫存所有Enemy是否可以ResetPath

    public int passEnemyNum; // 暫存所有PathPartial的Enemy數量

    public Queue passEnemy = new Queue(); // 暫存是PathPartial的Enemy
    */

	// Use this for initialization
	void Start () {
        //InvokeRepeating("Spawn", SpawnTime, SpawnInterval);
        Enemy = (GameObject)Resources.Load(EnemyPath, typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.S))
        {
            CancelInvoke();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            InvokeRepeating("Spawn", SpawnTime, SpawnInterval);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            EnemyPath = "Prefabs/Human/Enemy/Sphere";
            Enemy = (GameObject)Resources.Load(EnemyPath, typeof(GameObject));
        }
	}

    void Spawn()
    {
        EnemyClone = (GameObject)Instantiate(Enemy, this.transform.position, Quaternion.Euler(0, 180, 0));
        EnemyClone.name = "Enemy";
        EnemyClone.transform.SetParent(this.gameObject.transform); // 設定父物件
    }
}
