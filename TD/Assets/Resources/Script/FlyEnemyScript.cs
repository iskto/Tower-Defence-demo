using UnityEngine;
using System.Collections;

public class FlyEnemyScript : MonoBehaviour {

    private GameObject HPCanvas; // HP bar

    private RectTransform HPforeground;

    // 敵人屬性
    public int enemyHp, currentHp; // 血量

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        // set HP bar
        GameObject hpCanvas = (GameObject)Resources.Load("Prefabs/HPCanvas");
        HPCanvas = (GameObject)Instantiate(hpCanvas, transform.position, transform.rotation);
        HPCanvas.transform.SetParent(this.gameObject.transform);
        HPCanvas.GetComponent<RectTransform>().localPosition = new Vector3(0, 1, 0);
        HPforeground = HPCanvas.transform.FindChild("Foreground").GetComponent<RectTransform>(); // 暫存foreground
        currentHp = enemyHp; // 初始化HP
	}
	
	// Update is called once per frame
	void Update () {
        // 設定飛行終點
        transform.Translate(Vector3.forward * Time.deltaTime);
        // 目前HP比例
        HPforeground.localScale = new Vector3((float)currentHp / (float)enemyHp, 1, 1);
	}

    void OnCollisionEnter(Collision other)
    {
        // 若碰撞到主堡則敵人物件消失
        if (other.gameObject.name == "HomeMoney")
        {
            Destroy(this.gameObject); // 消滅此敵人
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // 若被子彈打到
        if (other.gameObject.name == "Bullet")
        {
            TowerScript tower = other.gameObject.transform.parent.GetComponent<TowerScript>();
            currentHp -= tower.attackDamage; // 扣除砲台的攻擊力
            // 若沒有了生命
            if (currentHp <= 0)
            {
                currentHp = 0;

                Destroy(this.gameObject); // 消滅此敵人
            }
        }
    }
}
