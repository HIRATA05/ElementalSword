using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //フィールドマップ用のプレイヤー移動 敵エンカウントも管理
    //一定歩数歩くと敵が出現する

    public enum FieldState
    {
        Road,//安全な道
        Grass,//草原
        Forest,//森
        Desert,//砂漠
    };
    private FieldState state;

    Animator animator;

    Quaternion targetRotation;

    //自分の位置の保存用
    private Vector3 Player_pos;
    //出現する敵のオブジェクト
    public GameObject enemyObject;
    //敵の出現位置
    float app_x, app_z;
    int rand_pos;

    //計測時間
    float elapsedTime;
    //敵の出現時間
    public float appearanceTime = 15f;

    //フィールドの情報を保存したスクリプタブル
    [SerializeField] private Field_Data FieldData;

    void Awake()
    {
        //コンポーネント
        TryGetComponent(out animator);

        //最初に初期化する
        targetRotation = transform.rotation;
    }
    private void Start()
    {
        //初期化
        elapsedTime = 0;

        //エンカウント時の戦闘フィールドから戻ってきた時フィールドマップの最後にいた地点に移動する
        if (FieldData.encTOfei_flg)
        {
            transform.position = FieldData.PlayerPosition_World;
            FieldData.encTOfei_flg = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //スクリプタブルに自分の位置を設定するために自分の位置を取得し続ける
        Player_pos = transform.position;

        //カメラの向きで補正した入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //速度の取得
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        //移動方向を向く
        if(velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        //移動速度をAnimatorに反映
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);


        //歩くと時間の計測
        if (state != FieldState.Road && horizontal != 0 || vertical != 0) elapsedTime += Time.deltaTime;
        else if (state == FieldState.Road) elapsedTime = 0;

        //設定した時間歩くと敵が一定距離に出現
        if (elapsedTime > appearanceTime)
        {
            elapsedTime = 0;//計測のリセット

            //出現位置を指定する変数にランダムな数字を入れるメソッド
            EnemyAppPos();

            //敵の出現
            Instantiate(enemyObject, new Vector3(app_x, 0.0f, app_z), Quaternion.identity);
        }
    }

    //敵とのエンカウント
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //当たったものがプレイヤーである場合
        if (hit.gameObject.tag == "Enemy")
        {
            Debug.Log("エンカウント");
            //エンカウント時スクリプタブルに座標データを保存 フィールドに戻る時の位置を決める
            Player_pos = transform.position;
            FieldData.PlayerPosition_World = Player_pos;
            FieldData.encTOfei_flg = true;

            //取得したフィールドタグによって移動先が変化
            SceneManager.LoadScene("BattleField");
        }
    }

    //足元のコライダーから地面のタグを取得、敵とエンカウントした時取得したタグにより移動するマップを変化させる
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Grass") state = FieldState.Grass;
        else if (other.gameObject.tag == "Forest") state = FieldState.Forest;
        else if (other.gameObject.tag == "Desert") state = FieldState.Desert;
        else state = FieldState.Road;

        Debug.Log(other.gameObject.tag);
    }

    private void EnemyAppPos()
    {
        //敵の出現位置は4パターンあり
        //プレイヤーの前後左右に出現 出現距離は10
        //1から100のランダムな数を出して25の範囲で4方向を区別して位置を決める
        rand_pos = (int)Random.Range(1.0f, 101.0f);
        if(rand_pos < 26) { app_x = 0; app_z = 10; }//プレイヤーの奥
        else if(rand_pos < 51 && rand_pos > 25) { app_x = -10; app_z = 0; }//プレイヤーの左
        else if (rand_pos < 76 && rand_pos > 50) { app_x = 10; app_z = 0; }//プレイヤーの右
        else if (rand_pos < 100 && rand_pos > 75) { app_x = 0; app_z = -10; }//プレイヤーの右

    }
}
