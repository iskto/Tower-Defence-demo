using UnityEngine;
using System.Collections;

public class EnamyScript : MonoBehaviour
{

    private GameObject Home; // 終點(家)

    private NavMeshAgent Enemy; // Enemy自動巡路

    private GameObject HPCanvas; // HP bar

    private RectTransform HPforeground;

    // 敵人屬性
    public int enemyHp, currentHp; // 血量


    /* 有BUG所以換方法
    //private bool isCollisionTower; // 判斷是否碰撞到塔(當為死路的時候設定為true)

    //private GameObject CollisionTower; // Enemy碰撞到的塔

    //private EnemySpawnScript EnemySpawn; // 宣告EnemySpawnScript物件

    //private bool isPathPartial; // 此敵人是否是PathPartial
    */

    // Use this for initialization
    void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        Home = GameObject.Find("Home");
        // set HP bar
        GameObject hpCanvas = (GameObject)Resources.Load("Prefabs/HPCanvas"); // 讀取血條的prefab
        HPCanvas = (GameObject)Instantiate(hpCanvas, transform.position, transform.rotation); // 產生hp bar
        HPCanvas.transform.SetParent(this.gameObject.transform); // 設定父物件到此enemy
        HPCanvas.GetComponent<RectTransform>().localPosition = new Vector3(0, 1.5f, 0); // hp bar的位置(enemy高1.5的地方)
        HPforeground = HPCanvas.transform.FindChild("Foreground").GetComponent<RectTransform>(); // 暫存foreground
        currentHp = enemyHp; // 初始化HP
        //EnemySpawn = gameObject.transform.parent.GetComponent<EnemySpawnScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // 設定敵人的終點
        Enemy.destination = Home.transform.position;
        // 判斷路徑情況若為死路則把塔都毀滅
        CheckPath();
        // 目前HP比例
        HPforeground.localScale = new Vector3((float)currentHp/(float)enemyHp, 1, 1);
    }

    void OnCollisionEnter(Collision other)
    {
        // 若碰撞到主堡則敵人物件消失
        if (other.gameObject.name == "HomeMoney")
        {
            /*// 此敵人若是PathPartial則將passEnemyNum(暫存所有PathPartial的Enemy數量)減去
            if (isPathPartial == true)
            {
                EnemySpawn.passEnemyNum--;
            }*/

            Destroy(this.gameObject); // 消滅此敵人
        }

        /*// 若碰撞到塔
        if (other.gameObject.tag == "Tower")
        {
            // 當Queue有null存在時去除它
            while (EnemySpawn.passEnemy.Peek().ToString() == "null")
            {
                EnemySpawn.passEnemy.Dequeue();
                if (EnemySpawn.passEnemy.Count == 0)
                    break;
            }
            // 發現是死路且是最前面的敵人的話，則把碰撞到塔設定為true
            if (Enemy.pathStatus.ToString() == "PathPartial" && this.gameObject == EnemySpawn.passEnemy.Peek())
            {
                CollisionTower = other.gameObject.transform.root.gameObject; // 暫存碰撞到的砲塔
                isCollisionTower = true;
            }       
        }*/ 

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

                /*// 此敵人若是PathPartial則將passEnemyNum(暫存所有PathPartial的Enemy數量)減去
                if (isPathPartial == true)
                {
                    EnemySpawn.passEnemyNum--;
                }*/

                Destroy(this.gameObject); // 消滅此敵人
            }
        }
    }

    // 判斷為死路則毀滅塔
    void CheckPath()
    {
        // 當判斷被堵路時把塔全毀滅
        if (Enemy.pathStatus.ToString() == "PathPartial" && GameObject.FindGameObjectWithTag("LandTower"))
        {
            Destroy(GameObject.FindGameObjectWithTag("LandTower").gameObject);
        }
        else if (Enemy.pathStatus.ToString() == "PathPartial" && GameObject.FindGameObjectWithTag("AirEnemy"))
        {
            Destroy(GameObject.FindGameObjectWithTag("AirEnemy").gameObject);
        }
        else if (Enemy.pathStatus.ToString() == "PathPartial" && GameObject.FindGameObjectWithTag("MixTower"))
        {
            Destroy(GameObject.FindGameObjectWithTag("MixTower").gameObject);
        }
        else if (Enemy.pathStatus.ToString() == "PathPartial")
        {
            Enemy.ResetPath();
        }

        /* 有BUG所以換方法
        // 若是PathPartial則把isPathPartial設為true
        if (Enemy.pathStatus.ToString() == "PathPartial" && isPathPartial == false)
        {
            EnemySpawn.passEnemy.Enqueue(this.gameObject); // 將此Enemy存入Queue裡
            EnemySpawn.passEnemyNum++; // 將passEnemyNum(暫存所有PathPartial的Enemy數量)增加     
            isPathPartial = true;
        }
        else if (Enemy.pathStatus.ToString() == "PathComplete") // 若是PathComplete則把isPathPartial設為false
        {
            isPathPartial = false;  
        }

        // 判斷若碰撞到塔且為死路狀態則消除碰撞到的那座塔
        if (isCollisionTower)
        {
            if (Enemy.pathStatus.ToString() == "PathPartial" && CollisionTower) // 當為死路狀態
            {
                Destroy(CollisionTower); // 消滅塔 
            }
            if (!CollisionTower) // 當塔被消滅了
            {
                isCollisionTower = false; // 重新設定為非碰撞到塔
                EnemySpawn.isReset = true; // 設定其他Enemy可以ResetPath
            }
        }

        // 若isReset(是否可以繼續ResetPath)為true且passEnemyNum(暫存所有PathPartial的Enemy數量)大於0
        // 意即可以ResetPath且還有Enemy為PathPartial
        if (EnemySpawn.isReset == true && EnemySpawn.passEnemyNum > 0)
        {
            if (Enemy.pathStatus.ToString() != "PathComplete")
                Enemy.ResetPath(); // 敵人重新尋路
     
            EnemySpawn.passEnemyNum--; // 將passEnemyNum(暫存所有PathPartial的Enemy數量)減去

            // 若passEnemyNum(暫存所有PathPartial的Enemy數量)為0，代表所有Enemy都是PathComplete，則把isReset(是否可以繼續ResetPath)改成false
            if (EnemySpawn.passEnemyNum <= 0)
            {
                EnemySpawn.isReset = false;
            }
        }

        if (Enemy.pathStatus.ToString() == "PathInvalid")
        {
            Enemy.ResetPath(); // 敵人重新尋路
        }
        */

    }

}
