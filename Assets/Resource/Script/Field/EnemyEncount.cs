using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyEncount : MonoBehaviour
{
    //プレイヤーが一定時間歩くと一定範囲内に出現してプレイヤーを追いかける、当たるとエンカウント
    //プレイヤーを追いかける時はNavMeshAgentを使用
    //一定時間追いかけると消滅

    public Transform target;
    private NavMeshAgent agent;
    //計測時間
    float elapsedTime;
    //敵の消滅時間
    public float eraseTime = 8f;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        elapsedTime = 0;

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーを追跡
        agent.destination = target.position;

        //時間の計測
        elapsedTime += Time.deltaTime;
        if (elapsedTime > eraseTime) Destroy(this.gameObject);
    }
    
}
