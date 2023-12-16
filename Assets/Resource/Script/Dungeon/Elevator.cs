using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //このスクリプトを付けたオブジェクトの上で上下キーを押すとオブジェクトが上下に動く rigitbodyで動かす
    //エレベーターに乗っている時の判定、Input.GetAxis("Vertical")でエレベーターのY軸を変化の処理を作る

    private Rigidbody rigidBody;
    //最大移動高度
    public float EV_top = 20f;
    //エレベーターの移動速度
    private float elevatorSpeed = 0.1f;
    //現在のエレベーターの移動速度
    private float elevatorSpeed_current = 0f;
    //最終的な移動速度
    private Vector3 velocity = Vector3.zero;
    private Vector3 force;

    private bool EVflag = false;
    //入力の受け取り
    private Vector3 input;
    private Vector3 player_pos;
    float top;

    float bottom;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        top = gameObject.transform.position.y + EV_top;
        bottom = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        //方向キーを押すと上下に動く
        //エレベーター内で上キーを押すと移動
        if (EVflag && Input.GetAxis("Vertical") > 0 && gameObject.transform.position.y < top)
        {
            force = new Vector3(0.0f, 15.0f, 0.0f);    // 力を設定
            //elevatorSpeed_current = elevatorSpeed;
            //elevatorSpeed = 0.02f;

        }
        else if (EVflag && Input.GetAxis("Vertical") < 0 && gameObject.transform.position.y > bottom)
        {
            force = new Vector3(0.0f, -15.0f, 0.0f);    // 力を設定
            //elevatorSpeed_current = elevatorSpeed * -1;
            //elevatorSpeed = -0.02f;
        }
        else
        {
            //elevatorSpeed_current = 0f;
            force = Vector3.zero;
        }

        rigidBody.MovePosition(transform.position + force * Time.deltaTime);
        //gameObject.transform.Translate(0, elevatorSpeed_current, 0);
    }

    //エレベーターの中に入ったら
    private void OnTriggerEnter(Collider other)
    {
        EVflag = true;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    //エレベーターから出たら
    private void OnTriggerExit(Collider other)
    {
        EVflag = false;
    }
}
