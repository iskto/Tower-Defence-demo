using UnityEngine;
using System.Collections;

public class TowerScript : MonoBehaviour {

    public Queue passEnemy = new Queue(); // 存放進入攻擊範圍的敵人

    public GameObject Bullet; // 子彈 

    public GameObject BulletSpawnPoint; // 發射子彈的位置點

    private GameObject enemy; // 敵人

    private bool onFire, inFireErrorRange; // 判斷是否正在開火, 誤差

    private Transform TowerHead; // 暫存要轉動的炮台

    // 砲塔屬性
    public int attackDamage; // 攻擊傷害

    public float attackRange; // 攻擊範圍

    public int bulletPower; // 火力(砲彈速度)

    private float ShootStart = 0.1f; // 射擊開始時間

    public float ShootInterval = 0.5f; // 射擊間隔


	// Use this for initialization
	void Start () {
        // 計算攻擊範圍
        gameObject.GetComponent<SphereCollider>().radius = attackRange / ((this.gameObject.transform.localScale.x + this.gameObject.transform.localScale.z) / 2);
        // 計算障礙物(砲塔體積)大小
        gameObject.GetComponent<NavMeshObstacle>().size = new Vector3(0.9f / (this.gameObject.transform.localScale.x), 0.9f / (this.gameObject.transform.localScale.y), 0.9f / (this.gameObject.transform.localScale.z));
        // 暫存要轉動的炮台
        TowerHead = gameObject.transform.Find("TowerHead");       
	}
	
	// Update is called once per frame
	void Update () {
        // 朝最先進來的敵人開火
        if (passEnemy.Count != 0)
        {
            // 若queue裡發現被消滅的敵人則把它dequeue
            while (passEnemy.Peek().ToString() == "null")
            {
                passEnemy.Dequeue();
                if (passEnemy.Count == 0)
                    break;
            }
            // 朝最先進來的敵人開火
            if (passEnemy.Count != 0)
            {
                enemy = (GameObject)passEnemy.Peek();
                if (onFire == false && inFireErrorRange)
                {
                    CancelInvoke();
                    InvokeRepeating("Fire", ShootStart, ShootInterval);
                    onFire = true;
                }
            }
        }
        else // 若無敵人則停火
        {
            enemy = null;
            CancelInvoke();
            onFire = false;
        }

        // 看向敵人(砲台沒有上下移動的功能時)
        if (enemy != null)
        {
            //gameObject.transform.Find("TowerHead").LookAt(new Vector3(enemy.transform.position.x, gameObject.transform.Find("TowerHead").transform.position.y, enemy.transform.position.z));
            Vector3 enemyPoint = enemy.transform.position - transform.position;
            enemyPoint.y = 0;
            Quaternion enemyRotation = Quaternion.LookRotation(enemyPoint);
            TowerHead.rotation = Quaternion.Lerp(TowerHead.rotation, enemyRotation, Time.deltaTime * 5);

            // 判斷是否在誤差值內
            float FireErrorRange = Quaternion.Angle(enemyRotation, TowerHead.rotation); // 誤差的角度
            if (FireErrorRange < 15) // 若誤差值小於15度則能開火
            {
                inFireErrorRange = true;
            }
            else // 若多於誤差值停火
            {
                inFireErrorRange = false;
                CancelInvoke();
                onFire = false;
            }
        }
        else
        {
            CancelInvoke();
        }

	}

    void OnTriggerEnter(Collider other)
    {
        // 當有敵人進入攻擊範圍(Land Tower)
        if (this.gameObject.CompareTag("LandTower"))
        {
            if (other.gameObject.name == "Enemy" && other.gameObject.CompareTag("LandEnemy"))
            {
                passEnemy.Enqueue(other.gameObject);
                /*CancelInvoke();
                enemy = other.gameObject;
                InvokeRepeating("Fire", ShootStart, ShootInterval);*/
            }
        }
        // 當有敵人進入攻擊範圍(Air Tower)
        if (this.gameObject.CompareTag("AirTower"))
        {
            if (other.gameObject.name == "Enemy" && other.gameObject.CompareTag("AirEnemy"))
            {
                passEnemy.Enqueue(other.gameObject);
                /*CancelInvoke();
                enemy = other.gameObject;
                InvokeRepeating("Fire", ShootStart, ShootInterval);*/
            }
        }
       
        // 當有敵人進入攻擊範圍(Mix Tower)
        if (this.gameObject.CompareTag("MixTower"))
        {
            if (other.gameObject.name == "Enemy")
            {
                passEnemy.Enqueue(other.gameObject);
                /*CancelInvoke();
                enemy = other.gameObject;
                InvokeRepeating("Fire", ShootStart, ShootInterval);*/
            }
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        // 敵人走出範圍則Dequeue
        if (other.gameObject.name == "Enemy")
        {
            if (passEnemy.Count != 0)
            {
                passEnemy.Dequeue();
            }
        }
        /*// 敵人走出範圍則停火
        if (other.gameObject == enemy)
        { 
            CancelInvoke();
            enemy = null;
        }*/
    }

    void OnMouseEnter()
    {
        Color32 outlineColor = new Color32(0, 170, 70, 255);
        gameObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
    }

    void OnMouseExit()
    {
        Color32 outlineColor = new Color32(0, 170, 70, 0);
        gameObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
    }

    void OnMouseDown()
    {
        /*gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.green);*/
       
    }

    // 開火
    void Fire()
    {
        Transform towerHead = gameObject.transform.Find("TowerHead");
        GameObject bulletClone = (GameObject)Instantiate(Bullet, BulletSpawnPoint.transform.position, towerHead.rotation);
        bulletClone.name = "Bullet";
        bulletClone.transform.SetParent(this.gameObject.transform); // 設定父物件
        if (bulletClone.CompareTag("SingleAtk"))
        {
            bulletClone.GetComponent<BulletScript>().enemy = enemy;
            bulletClone.GetComponent<BulletScript>().bulletPower = bulletPower;
        }
        else
        {
            bulletClone.GetComponent<Rigidbody>().AddForce((enemy.transform.position - transform.position) * 500);
        } 
    }

}
