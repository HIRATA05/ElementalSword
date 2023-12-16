using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAttack : MonoBehaviour
{
    [SerializeField] private Scriptable_CharaData ArmorKnight;
    //剣を持っているゲームオブジェクト(親)を指定。
    [SerializeField] GameObject AttackChara;

    int HHcount;
    int ATK;
    ICharaAttack Hcount;

    void Start()
    {
        if (ArmorKnight != null)
        {
            //ArmorKnightのatkを代入。
            ATK = ArmorKnight.str;
        }

        //ICharaAttackのインターフェースが定義されたコンポーネント(スクリプト)をHcounに代入。
        Hcount = AttackChara.GetComponent<ICharaAttack>();
    }

    //剣がゲームオブジェクトに侵入した瞬間に呼び出し
    void OnTriggerEnter(Collider other)
    {
        //ICharaAttackのインターフェースが定義されたスクリプトのHitCount()を呼び出し、返り値(残りヒット数)をHHcountに代入する。
        HHcount = Hcount.HitCount();

        //HHcountが0以下ならもう既にヒットしている。return;で処理を終わらせる。
        if (HHcount <= 0)
        {
            return;
        }

        if (other.tag == "Player")
        {
            //コライダーのあるゲームオブジェクトのインターフェースを呼び出す
            IDamageable damageable = other.GetComponent<IDamageable>();

            //damageableにnull値が入っていないかチェック
            if (damageable != null)
            {
                //ダメージが入るのが確定したのでヒット数-1
                Hcount.HitCountdown();


                //damageableのダメージ処理メソッドを呼び出す。引数としてArmorKnightのATKを指定
                damageable.Damage(ATK);


            }

        }

    }
}
