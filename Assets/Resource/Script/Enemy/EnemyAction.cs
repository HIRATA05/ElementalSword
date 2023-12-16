using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAction : MonoBehaviour//, IDamageable
{
    /*
    private Rigidbody rigidBody;
    //索敵、移動の考え方　調べること
    //プレイヤーが敵より左にいると左に移動、右にいると右に移動、プレイヤーの手前=プレイヤーの位置+0.5のx座標に移動した時攻撃する

    //索敵範囲の設定　コライダーを設定して範囲内に入るとプレイヤーの追跡を始める
    public BoxCollider SearchCollider;
    //初期位置の設定　プレイヤーが索敵範囲外に出ると索敵をやめて初期位置に移動を始める
    private Vector3 start_Pos;
    //攻撃範囲の設定　コライダーを設定して範囲内に入ると攻撃モーションを始め、出現した武器のコライダーに当たるとプレイヤーがダメージを受ける
    public SphereCollider AttackRangeCollider;
    //武器・盾の当たり判定のコライダー
    public BoxCollider WeaponCollider;
    public BoxCollider Shield_Collider;

    //public GameObject destination; 　//目的地になるオブジェクトを取得するための変数
    public NavMeshAgent agent; //NavMeshAgent取得用の変数

    //　敵の状態
    private EnemyState state;
    //攻撃するターゲット
    [SerializeField] public GameObject targetObject;
    Vector3 targetPos;

    //自分の位置
    Vector3 Position;
    Vector3 moveDirection;
    public float moveDir_num = 1;
    //速度
    private Vector3 velocity;
    private float walkSpeed = 4f;
    //対象の近くで止まる範囲
    int front = 2;

    private Animator animator;

    //スクリプタブルのNPCデータ
    [SerializeField] private Scriptable_CharaData EnemyData;
    //SliderのHPゲージを指定
    [SerializeField] Slider Slider;
    int hp;

    //　プレイヤーTransform
    private Transform playerTransform;
    //　ジャンプ中かどうか
    private bool isJump;
    //　ジャンプ後に接地確認するまでの遅延時間
    private float jumpWaitTime = 0.5f;
    //　ジャンプ後の時間
    private float jumpTime = 0f;
    //　接地しているかどうか
    private bool isGrounded;
    //　接地確認のコライダの位置のオフセット
    private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    //　接地確認の球のコライダの半径
    private float groundColliderRadius = 0.29f;
    //　地面と判断するレイヤーマスク
    [SerializeField] private LayerMask groundLayers;

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Attack,
        Freeze
    };

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        //valueのHPゲージのスライダーを最大の1に
        Slider.value = 1;

        //NPCデータの最大HPを代入
        hp = EnemyData.hp_max;

        //agent = GetComponent<NavMeshAgent>(); //NavMeshAgentの取得
        //自分の現在の座標を入れる
        Position = transform.position;
        //velocity = Vector3.zero;
        SetState(EnemyState.Walk);
    }

    void Update()
    {
        //　接地確認
        CheckGround();

        //　見回りまたはキャラクターを追いかける状態
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //プレイヤーの位置によって移動方向を決める
            if (targetPos.x > Position.x)//プレイヤーの位置が敵より左にいる時、左に移動する
            {
                moveDir_num = 1;
            }
            else if (targetPos.x < Position.x)//プレイヤーの位置が敵より右にいる時、右に移動する
            {
                moveDir_num = -1;
            }
            moveDirection = new Vector3(moveDir_num, 0, 0);

            //　移動速度を計算
            var clampedInput = Vector3.ClampMagnitude(moveDirection, 1f);
            velocity = clampedInput * walkSpeed * 1.2f;
            transform.LookAt(rigidBody.position + moveDirection);
            //　今入力から計算した速度から現在のRigidbodyの速度を引く
            velocity = velocity - rigidBody.velocity;
            //　速度のXZを-walkSpeedとwalkSpeed内に収めて再設定
            velocity = new Vector3(Mathf.Clamp(velocity.x, -walkSpeed, walkSpeed), 0f, Mathf.Clamp(velocity.z, -walkSpeed, walkSpeed));

            //　キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Chase)
            {
                //プレイヤーの座標
                targetPos = playerTransform.position;

                if(state == EnemyState.Attack) { animator.SetBool("Attack", true); }
            }
            if (isGrounded)
            {
                Debug.Log("ground!!");

                //位置の更新
                //Position = transform.position;

                // マイナスをかけることで逆方向に移動する。
                //transform.Translate(transform.right * Time.deltaTime * 2 * -moveDir_num);
                

                if (clampedInput.magnitude > 0f)//移動している
                {
                    animator.SetFloat("Speed", clampedInput.magnitude);
                }
                else//止まる
                {
                    animator.SetFloat("Speed", 0f);
                    SetState(EnemyState.Wait);
                }
            }
            //移動処理
            rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime *1.5f, ForceMode.Force);
            //自分の現在の座標を入れる
            Position = transform.position;
        }
    }

    //敵の状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (tempState == EnemyState.Walk)
        {
            Debug.Log("walk!!");
            state = tempState;
        }
        else if (tempState == EnemyState.Chase)
        {
            Debug.Log("chase!!");
            state = tempState;
            //　追いかける対象をセット
            playerTransform = targetObj;
        }
        else if (tempState == EnemyState.Wait)
        {
            Debug.Log("wait!!");
            state = tempState;
            velocity = Vector3.zero;
        }
    }
    //敵の状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    // ダメージ処理のメソッド　valueにはPlayerのatkの値が入ってる
    public void Damage(int value)
    {
        Debug.Log("Damage!! " + value);
        // 敵データがnullでないかをチェック
        if (EnemyData != null)
        {
            // Playerのatkから敵のdefを引いた値をhpから引く
            hp -= value - EnemyData.def;
            //HPゲージに反映
            Slider.value = (float)hp / (float)EnemyData.hp_max;
        }

        // hpが0以下ならDeath()メソッドを呼び出す
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

    //IsTriggerにチェックが入れられた別のColliderに触れると
    //ダメージのアニメーションが出る
    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Damage", true);
    }
    //アニメーションイベント
    void DamageEnd()
    {
        animator.SetBool("Damage", false);
    }
    //攻撃時のアニメーション
    void ArmorAttack_Start()
    {
        //剣を有効にする
        WeaponCollider.enabled = true;
    }
    void ArmorAttack_End()
    {
        //剣を無効にする
        WeaponCollider.enabled = false;
        animator.SetBool("Attack", false);
    }

    //　地面のチェック
    private void CheckGround()
    {
        //　アニメーションパラメータのRigidbodyのYが0.1以下でGroundまたはEnemyレイヤーと球のコライダがぶつかった場合は地面に接地
        if (Physics.CheckSphere(rigidBody.position + groundPositionOffset, groundColliderRadius, groundLayers))
        {
            //　ジャンプ中
            if (isJump)
            {
                if (jumpTime >= jumpWaitTime)
                {
                    isGrounded = true;
                    isJump = false;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("IsGrounded", isGrounded);
    }
    */
}
