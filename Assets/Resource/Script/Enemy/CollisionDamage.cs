using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    //敵との接触時のダメージ判定
    public int damage = 1;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // 衝突があったさいに呼ばれる
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //当たったものがプレイヤーである場合
        if (hit.gameObject.tag == "Player")
        {
            Debug.Log("当たり");
            hit.gameObject.GetComponent<SideViewController>().TakeDamage(transform, damage);
        }
    }

}
