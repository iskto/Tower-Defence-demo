using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public GameObject Explosion;

    public GameObject enemy;

    public int bulletPower;

    private Vector3 startPos; // 發射起始位置

    private float attackRange; // 砲台的攻擊範圍

    // 傷害敵人的效果(如緩速)是寫在子彈這的

	// Use this for initialization
	void Start () {
        startPos = this.gameObject.transform.position; // 暫存起始位置
        attackRange = this.gameObject.transform.parent.GetComponent<SphereCollider>().radius; // 設定砲台攻擊範圍
	}
	
	// Update is called once per frame
	void Update () {
        // 跟蹤導彈
        if (enemy && this.CompareTag("SingleAtk"))
        {
            this.gameObject.transform.LookAt(enemy.transform.position);
            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * bulletPower);
        }
        else if (!enemy && this.CompareTag("SingleAtk"))
        {  
            Destroy(this.gameObject);
        }

        // 當子彈超出塔的射擊範圍則消失
        if (Vector3.Distance(this.gameObject.transform.position, startPos) >= attackRange && this.CompareTag("SingleAtk"))
        {
            Instantiate(Explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        
        // 範圍攻擊
        if (this.gameObject.transform.position.y <= 0.3f && this.gameObject.CompareTag("RangeAtk"))
        {
            GetComponent<SphereCollider>().radius = 5;
        }
        if (this.gameObject.transform.position.y <= 0.1f && this.gameObject.CompareTag("RangeAtk"))
        {
            Instantiate(Explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
	}

    // 子彈是Trigger
    void OnTriggerEnter(Collider other)
    {
        // 當子彈碰到敵人則子彈消失(單體攻擊)
        if (this.gameObject.CompareTag("SingleAtk") && (other.gameObject.name == "Enemy" || other.gameObject.CompareTag("Floor")))
        {
            Instantiate(Explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
