using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    //敵がプレイヤーを発見するためのコライダーに付ける

    private EnemyAction enemyAction;
    private EnemyAI enemy_ai;

    void Start()
    {
        enemyAction = GetComponentInParent<EnemyAction>();
        enemy_ai = GetComponentInParent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        //プレイヤーを発見
        if (col.tag == "Player")
        {
            //敵キャラクターの状態を取得
            EnemyAI.EnemyState state = enemy_ai.GetState();
            //敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if (state == EnemyAI.EnemyState.Wait || state == EnemyAI.EnemyState.Walk)
            {
                Debug.Log("発見");
                enemy_ai.SetState(EnemyAI.EnemyState.Chase, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("見失う");
            enemy_ai.SetState(EnemyAI.EnemyState.Wait);
        }
    }
}
