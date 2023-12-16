using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //プレイヤーが攻撃を与える時の判定

    [SerializeField]
    private Scriptable_Player PlayerData;

    //剣がゲームオブジェクトに侵入した瞬間に呼び出し
    void OnTriggerEnter(Collider other)
    {
        //otherのゲームオブジェクトのインターフェースを呼び出す
        IDamageable damageable = other.GetComponent<IDamageable>();

        //damageableにnull値が入っていないかチェック
        if (damageable != null)
        {
            if (other.CompareTag("Enemy"))
            {
                //damageableのダメージ処理メソッドを呼び出す。引数としてスクリプタブルPlayerdataのatk_tesを指定
                damageable.Damage(PlayerData.str);
            }
        }
    }
}
