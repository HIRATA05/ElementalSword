using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    Animator anim;
    [SerializeField] Scriptable_CharaData ArmorKnight;
    [SerializeField] MonoBehaviour EnemyAttack;

    int State;
    bool action;
    IEnemy EnemyAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        //IEnemyのインターフェースを宣言したコンポーネント(スクリプト)を手に入れ、EnemyActionへ代入。
        EnemyAction = GetComponent<IEnemy>();
    }

    void Update()
    {
        //毎フレーム処理を行う必要はない。コルーチンでEnemytimeの時間だけ待機
        StartCoroutine(Enemytime());

        //Attackパラメータの値を代入し0より大きいなら攻撃中なのでreturn;して処理を終了する。
        bool Attack = anim.GetBool("Attack");
        if (Attack) { return; }

        //値が入っていない場合に備えnullチェック。
        if (EnemyAction != null)
        {
            //スクリプトのEnemyAIkoudou()を呼び出し、返ってきた値をStateに代入する。
            State = EnemyAction.EnemyAction_AI();


            action = true;
            //Switch文でStateの値に応じて条件分岐。
            switch (State)
            {
                //Stateが0なら停止。
                case 0:

                    anim.SetBool("Attack", false);
                    break;

                //Stateが1なら攻撃。
                case 1:

                    anim.SetBool("Attack", true);
                    break;
            }
        }
    }

    IEnumerator Enemytime()
    {
        yield return new WaitForSeconds(ArmorKnight.EnemyTime);
        action = false;
    }
}
