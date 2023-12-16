using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAssault : MonoBehaviour, IDamageable
{
    //プレイヤーにも目を向けず突撃する敵の動き 主に画面外から出現する

    //移動用データ
    [SerializeField] private float speed = 1.0f;
    private Vector3 moveDirection = Vector3.zero;
    //速度
    private Vector3 velocity = Vector3.zero;
    //向いている方向
    public float DirectionFirst_x = -1;
    //向いている方向
    public float Direction_x = -1;
    //待ち時間
    [SerializeField] private float returnTime = 3f;
    //経過時間
    private float elapsedTime;
    //現在のx
    float current_x = 0f;
    //過去のx
    float past_x = 0f;

    //敵との接触時のダメージ判定
    public int damage = 1;
    //スクリプタブルのNPCデータ
    [SerializeField] private Scriptable_CharaData EnemyData;
    //敵のステータス
    int hp_max, hp, ep_max, ep, str, mgc, def, res;
    //SliderのHPゲージを指定
    [SerializeField] Slider Slider;
    private Rigidbody rigidBody;
    private CharacterController enemyController;
    //EnemySpawnerスクリプトの取得
    private EnemySpawner enemySpawner;

    //画面内に侵入したフラグ　このフラグがtrueの時に画面外に出ると死亡する
    public bool screenIn_flg = false;
    //ビューポート座標のxの値
    float view_x;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyController = GetComponent<CharacterController>();
        enemySpawner = GetComponent<EnemySpawner>();
        //スクリプタブルからデータを代入
        status_StartSet();

        Direction_x = DirectionFirst_x;
    }

    // Update is called once per frame
    void Update()
    {
        //一定時間xが変化しない時引き返す処理
        elapsedTime = elapsedTime + Time.deltaTime;
        if(elapsedTime > returnTime)
        {
            elapsedTime = 0;
            //定期的に時間を計測してxを取得
            past_x = current_x;
            current_x = transform.position.x;
            //xが変化していない時進む方向を変化
            if (current_x == past_x) Direction_x *= -1;
        }

        //現在位置をビューポート座標のxの値で取得
        view_x = Camera.main.WorldToViewportPoint(transform.position).x;
        //画面内に入ったらフラグをtrueに変える
        if (0 < view_x && view_x < 1)
        {
            screenIn_flg = true;
            Debug.Log(view_x);
        }
        //画面外で死亡
        else if (!(0 < view_x && view_x < 1) && screenIn_flg) Death();
 
    }
    void FixedUpdate()
    {
        //移動
        moveDirection = new Vector3(Direction_x, 0.0f, 0f);
        //　移動速度を計算
        var clampedInput = Vector3.ClampMagnitude(moveDirection, 1f);
        velocity = clampedInput * speed * 1.2f;
        transform.LookAt(rigidBody.position + moveDirection);
        //　今入力から計算した速度から現在のRigidbodyの速度を引く
        velocity = velocity - rigidBody.velocity;
        //　速度のXZを-walkSpeedとwalkSpeed内に収めて再設定
        velocity = new Vector3(Mathf.Clamp(velocity.x, -speed, speed), 0f, Mathf.Clamp(velocity.z, -speed, speed));

        //移動処理
        rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime, ForceMode.Force);
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
        //死亡時死んだ情報をEnemySpawnerに伝え現在の出現人数変数を減らす
        EnemyData.die = true;

        // ゲームオブジェクトを破壊
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision hit)
    {
        //当たったものがプレイヤーである場合
        if (hit.gameObject.tag == "Player")
        {
            Debug.Log("当たり");
            hit.gameObject.GetComponent<SideViewController>().TakeDamage(transform, damage);
        }
    }

    //スクリプタブルからステータスを設定するメソッド
    void status_StartSet()
    {
        hp_max = EnemyData.max_hp;
        hp = hp_max;
        ep_max = EnemyData.max_ep;
        ep = ep_max;
        str = EnemyData.str;
        mgc = EnemyData.mgc;
        def = EnemyData.def;
        res = EnemyData.res;
    }
}
