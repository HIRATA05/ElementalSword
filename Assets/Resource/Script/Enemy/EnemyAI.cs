using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    //こちらを追跡して襲ってくるタイプの敵AIの基本

    //敵の初期位置
    Vector3 Enemy_StartPos = Vector3.zero;

    public enum EnemyState
    {
        Walk,//徘徊、帰還
        Wait,//止まる
        Chase,//追跡
        Attack,//攻撃
        Freeze//行動後の硬直
    };

    private CharacterController enemyController;
    private Animator animator;
    private Rigidbody rigidBody;
    //武器のコライダー
    //[SerializeField] private BoxCollider WeaponCol;
    //　歩くスピード
    [SerializeField] private float walkSpeed = 1.0f;
    //　速度
    private Vector3 velocity;
    //　移動方向
    private Vector3 direction;
    //　到着フラグ
    private bool arrived;
    //　移動先
    private Vector3 setPosition;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    private float elapsedTime;
    // 敵の状態
    private EnemyState state;
    //　プレイヤーのTransform
    private Transform playerTransform;
    [SerializeField]
    private float freezeTime = 1f;

    //ノックバックの方向
    private Vector3 knockBack_dir;
    int moveDir_num;
    //ノックバックの吹き飛ぶ距離
    private float knockBack_power = 5f;
    bool knockBack_flg = false;

    //スクリプタブルのNPCデータ
    [SerializeField] private Scriptable_CharaData EnemyData;
    //SliderのHPゲージを指定
    [SerializeField] Slider Slider;
    public int hp, str, def;


    void Start()
    {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        Enemy_StartPos = transform.position;
        velocity = Vector3.zero;
        arrived = false;
        elapsedTime = 0f;
        SetState(EnemyState.Walk);

        //valueのHPゲージのスライダーを最大の1に
        Slider.value = 1;

        //NPCデータのステータスを代入
        hp = EnemyData.max_hp; str = EnemyData.str; def = EnemyData.def;
    }

    // 何故かプレイヤーを攻撃後移動速度が大きく低下する不具合がある、おそらくプレイヤーの位置を判定する処理が原因だと思われる------------------------------
    void Update()
    {
        //　見回りまたはキャラクターを追いかける状態
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //　キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Chase)
            {

                setPosition = playerTransform.position;
            }
            if (enemyController.isGrounded)
            {
                //移動の処理
                velocity = Vector3.zero;
                animator.SetFloat("Speed", 2.0f);
                direction = (setPosition - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.x, transform.position.y, setPosition.z));
                velocity = direction * walkSpeed;
            }

            if (state == EnemyState.Walk)
            {
                //　目的地に到着したかどうかの判定
                if (Vector3.Distance(transform.position, setPosition) < 0.5f)
                {
                    SetState(EnemyState.Wait);
                    animator.SetFloat("Speed", 0.0f);
                }
            }
            else if (state == EnemyState.Chase)
            {
                //　攻撃する距離だったら攻撃
                if (Vector3.Distance(transform.position, setPosition) < 2f)
                {
                    SetState(EnemyState.Attack);
                    //攻撃する時に武器がfalseの時武器のコライダーをtrueにする
                    //if(WeaponCol.enabled == false) WeaponCol.enabled = true;
                }
            }
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }
        //攻撃後のフリーズ状態
        else if (state == EnemyState.Freeze)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("フリーズ！！！！！！");
            if (elapsedTime > freezeTime)
            {
                SetState(EnemyState.Wait);
            }
        }
        //攻撃された時のノックバック時の移動 なぜかノックバックしない要修正
        if (knockBack_flg == true) { Debug.Log("knockback!! "); rigidBody.AddForce(transform.up * Time.deltaTime, ForceMode.VelocityChange); knockBack_flg = false; }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
            enemyController.Move(velocity * Time.deltaTime);
        }

    }

    //　敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (tempState == EnemyState.Walk)
        {
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            setPosition = Enemy_StartPos;//初期位置を設定
        }
        else if (tempState == EnemyState.Chase)
        {
            state = tempState;
            //　待機状態から追いかける場合もあるのでOff
            arrived = false;
            //　追いかける対象をセット
            playerTransform = targetObj;
        }
        else if (tempState == EnemyState.Wait)
        {
            elapsedTime = 0f;
            state = tempState;
            arrived = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if (tempState == EnemyState.Attack)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", true);
        }
        else if (tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
            state = tempState;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", false);
        }
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    //ダメージ処理のメソッド　valueにはPlayerのatkの値が入ってる
    public void Damage(int value)
    {
        Debug.Log("Damage!! " + value);
        // 敵データがnullでないかをチェック
        if (EnemyData != null)
        {
            //Playerのstrから敵のdefを引いた値をhpから引く
            hp -= value - def;
            //HPゲージに反映
            Slider.value = (float)hp / (float)EnemyData.max_hp;
        }
        //ノックバック
        animator.SetTrigger("Damage");
        //velocity = Vector3.zero;

        //攻撃の向きによって吹き飛ぶ方向が変化
        //攻撃の向きは敵とプレイヤーの位置によって決まる
        if (transform.position.x < playerTransform.position.x)//プレイヤーの位置が敵より左にいる時、左にノックバック
        {
            moveDir_num = -1;
        }
        else if (transform.position.x > playerTransform.position.x)//プレイヤーの位置が敵より右にいる時、右にノックバック
        {
            moveDir_num = 1;
        }

        knockBack_dir = new Vector3(moveDir_num, 1, 0);

        //rigidBody.AddForce(transform.up, ForceMode.VelocityChange);
        knockBack_flg = true;

        //死亡処理、hpが0以下ならDeath()メソッドを呼び出す
        if (hp <= 0)
        {
            Death();
        }
    }
    // 死亡処理のメソッド
    public void Death()
    {
        Debug.Log("Death!!");
        // ゲームオブジェクトを破壊
        Destroy(gameObject);
    }
}
