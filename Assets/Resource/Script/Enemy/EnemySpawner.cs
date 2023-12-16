using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //コライダーにアタッチする
    //コライダーで設定したエリアに入ったら画面外から敵を出現
    //EnemyAssaultを付けた敵を出現させる、死亡時現在の出現人数を減らす

    //出現させる敵を設定
    [SerializeField] GameObject[] enemys;
    //次に敵が出現するまでの時間
    [SerializeField] float appearNextTime;
    //この場所から出現する敵の数
    [SerializeField] int maxNumOfEnemys;
    //現在の出現人数
    public int numberOfEnemys;
    //待ち時間計測
    private float elapsedTime;

    //敵の出現位置
    public Vector3 enemyPosition;
    //敵の出現位置のx位置
    float enemyPos_x = 15;

    //スクリプタブルのNPCデータ スケルトンのデータを入れる
    [SerializeField] private Scriptable_CharaData EnemyData;
    //EnemyAssaultスクリプトの取得
    private EnemyAssault enemyAssault;

    // Start is called before the first frame update
    void Start()
    {
        //変数の初期化
        numberOfEnemys = 0;
        elapsedTime = 0f;

        enemyAssault = GetComponent<EnemyAssault>();
    }

    // Update is called once per frame
    void Update()
    {
        //経過時間を計測
        elapsedTime += Time.deltaTime;
        //敵出現位置をカメラ外に更新
        enemyPosition = Camera.main.ViewportToWorldPoint(new Vector3(enemyPos_x, -1, Camera.main.nearClipPlane));
        enemyPosition.z = 0;

        //スケルトンの死亡を確認したら現在の敵出現数を減らしてdieをfalseにする
        if(EnemyData.die == true)
        {
            numberOfEnemys--;
            EnemyData.die = false;
        }
        
    }

    //侵入したのがプレイヤーの時敵出現
    void OnTriggerStay(Collider col)
    {
        //現在の数がこの場所から出現する最大数を超えてたら何もしない
        if (numberOfEnemys >= maxNumOfEnemys)
        {
            return;
        }
        if (col.gameObject.tag == "Player")
        {
            //時間を計測して一定時間ごとに出現させる
            if (elapsedTime > appearNextTime)
            {
                elapsedTime = 0f;
                AppearEnemy();
            }
        }
    }

    //敵出現メソッド
    void AppearEnemy()
    {
        //敵を複数出現させる場合ランダムに選ぶ
        var randomValue = Random.Range(0, enemys.Length);
        //プレイヤーの向きによって出現する方向、向きを変える
        //var randomRotationY = Random.value * 360f;

        //画面外を指定して敵を出現
        GameObject.Instantiate(enemys[randomValue], enemyPosition, Quaternion.Euler(0f, 180f, 0f));

        //敵の出現数を増やして時間計測をリセット
        numberOfEnemys++;
        elapsedTime = 0f;
    }
}
