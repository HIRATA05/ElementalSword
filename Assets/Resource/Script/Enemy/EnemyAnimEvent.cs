using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    //敵の攻撃などのアニメーションイベント

    private EnemyAI enemy_ai;
    [SerializeField]
    private BoxCollider boxCollider;

    void Start()
    {
        enemy_ai = GetComponent<EnemyAI>();
    }

    void Attack_Start()
    {
        boxCollider.enabled = true;
    }

    public void Attack_End()
    {
        boxCollider.enabled = false;
    }

    public void StateEnd()
    {
        enemy_ai.SetState(EnemyAI.EnemyState.Freeze);
    }

    public void EndDamage()
    {
        enemy_ai.SetState(EnemyAI.EnemyState.Walk);
    }
}
