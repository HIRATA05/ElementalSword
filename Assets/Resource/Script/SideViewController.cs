using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideViewController : MonoBehaviour
{
	//2D視点のプレイヤーの基本操作


	//プレイヤーの状態
	public enum MyState
	{
		Normal,
		Damage
	};
	private MyState state;

	//当たり判定
	[SerializeField] private GameObject PlayerObj;
	[SerializeField] private CapsuleCollider WeaponCollider;
	[SerializeField] private BoxCollider ShieldCollider;
	[SerializeField] private BoxCollider CrouchingShieldCollider;

	//ノックバックの方向
	private Vector3 knockBack_dir;
	int moveDir_num;
	//ノックバックの吹き飛ぶ距離
	private float knockBack_power = 5f;

	//無敵時間
	[SerializeField] private float invisibleTime = 3f;
	//経過時間
	private float elapsedTime = 0f;
	private bool invisible_flg = false;

	private bool moveConditions = false;
	private Rigidbody rigidBody;
	//　移動速度
	private Vector3 velocity;
	//　入力値
	private Vector3 input;
	//　アニメーター
	private Animator animator;
	//　地面と判断するレイヤーマスク
	[SerializeField] private LayerMask groundLayers;
	//　移動の速さ
	private float walkSpeed = 4f;
	//　接地しているかどうか
	private bool isGrounded;
	//　接地確認のコライダの位置のオフセット
	private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
	//　接地確認の球のコライダの半径
	private float groundColliderRadius = 0.29f;

	//　ジャンプボタンを押したかどうか
	private bool pushJumpButton;
	//　ジャンプする高さ
	[SerializeField]
	private float jumpHeight = 5f;
	//　初期のジャンプ値
	private float initJumpHeightValue;
	//　現在のジャンプ値
	private float jumpValue;
	//　ジャンプ中かどうか
	private bool isJump;
	//　ジャンプ後に接地確認するまでの遅延時間
	private float jumpWaitTime = 0.5f;
	//　ジャンプ後の時間
	[SerializeField]
	private float jumpTime = 0f;
	//　衝突したかどうか
	private bool isCollision;
	//　衝突確認のコライダの位置のオフセット
	private Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0.1f);
	//　衝突確認の球のコライダの半径
	private float collisionColliderRadius = 0.3f;
	//　天井の衝突確認用コライダのオフセット値
	private Vector3 collisionCeilingOffset = new Vector3(0f, 1.8f, 0f);
	//　天井の衝突確認用コライダの球の半径
	private float collisionCeillingColliderRadius = 0.29f;
	//　衝突した面の方向
	private Vector3 collisionDirection;

	//プレイヤーのステータス設定のスクリプタブル
	[SerializeField] private Scriptable_Player PlayerData;
	//SliderのHPゲージを指定
	[SerializeField] Slider Slider;
	//プレイヤーのステータス
	int hp_max, hp, ep_max, ep, str, mgc, tec, spd, luc, def, res;
	int damage;//攻撃を受けた時のダメージ

	void Start()
	{
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		//valueのHPゲージのスライダーを最大の1に
		Slider.value = 1;
		//現在のステータスをスクリプタブルから入れる
		status_StartSet();
	}

	void FixedUpdate()
	{
		//　接地確認
		CheckGround();

		if (state == MyState.Normal)
		{
			//　入力値　moveConditionsがfalseのとき移動出来る、特定のモーション中は移動出来なくする
			input = new Vector3(Input.GetAxis("Horizontal"), 0, 0/*Input.GetAxis("Vertical")*/);

			//　移動速度を計算
			var clampedInput = Vector3.ClampMagnitude(input, 1f);
			velocity = clampedInput * walkSpeed * 1.2f;
			transform.LookAt(rigidBody.position + input);
			//　今入力から計算した速度から現在のRigidbodyの速度を引く
			velocity = velocity - rigidBody.velocity;
			//　速度のXZを-walkSpeedとwalkSpeed内に収めて再設定
			velocity = new Vector3(Mathf.Clamp(velocity.x, -walkSpeed, walkSpeed), 0f, Mathf.Clamp(velocity.z, -walkSpeed, walkSpeed));

			//　接地時の処理
			if (isGrounded)
			{
				if (clampedInput.magnitude > 0f)
				{
					animator.SetFloat("Speed", clampedInput.magnitude);
				}
				else
				{
					animator.SetFloat("Speed", 0f);
				}

				//通常攻撃
				if (Input.GetKey(KeyCode.Z))
				{
					animator.SetBool("AttackNomal", true);

				}
				//ガード時しゃがみ時は盾を構え防御する
				//ガード
				if (Input.GetKey(KeyCode.X))
				{
					animator.SetBool("GuardNormal", true);
					//盾のコライダーを表示
					ShieldCollider.enabled = true;
					if (Input.GetKeyUp(KeyCode.C)) animator.SetTrigger("GuardSuccess");
				}
				else
                {
					animator.SetBool("GuardNormal", false);
					ShieldCollider.enabled = false;
				}
				//しゃがむ
				if (Input.GetKey(KeyCode.DownArrow))
				{
					animator.SetBool("Crouching", true);
					//盾のコライダーを表示
					CrouchingShieldCollider.enabled = true;
					if (Input.GetKeyUp(KeyCode.C)) animator.SetTrigger("GuardSuccess");
				}
				else
				{
					animator.SetBool("Crouching", false);
					CrouchingShieldCollider.enabled = false;
				}

				// ジャンプ
				if (pushJumpButton)
				{
					pushJumpButton = false;
					isGrounded = false;
					isJump = true;
					jumpTime = 0f;
					animator.SetTrigger("Jump");
					//　初期のジャンプ値を計算
					initJumpHeightValue = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
					jumpValue = initJumpHeightValue;
					animator.SetFloat("JumpPower", initJumpHeightValue);

					//　即座にジャンプさせる
					rigidBody.velocity = new Vector3(rigidBody.velocity.x, initJumpHeightValue, rigidBody.velocity.z);
				}
			}

			//空中攻撃
			if (Input.GetKey(KeyCode.Z))
			{
				animator.SetBool("AttackNomal", true);
			}
		}
		//　現在のジャンプ値の計算
		if (jumpValue > -initJumpHeightValue)
		{
			jumpValue += Physics.gravity.y * Time.fixedDeltaTime;
			animator.SetFloat("JumpPower", jumpValue);
		}
		//　ジャンプ中はジャンプ時間を計測する
		if (isJump && jumpTime < jumpWaitTime)
		{
			jumpTime += Time.deltaTime;
		}

		//移動処理
		rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime, ForceMode.Force);

		//ジャンプ中等に横壁と接触している時、または階段を上っている時は重力以外の移動値を0にする
		if ((!isGrounded && isCollision))
		{
			//衝突した壁方向を向いている時だけ重力以外の移動値を0にする
			if (Vector3.Dot(transform.forward, collisionDirection) <= 0.5f)
			{
				velocity = new Vector3(0f, velocity.y, 0f);
			}
		}
	}

	private void Update()
	{
		if (state == MyState.Normal) //ステートがのときジャンプ出来る、攻撃等モーション中はジャンプ出来なくする
		{
			if (Input.GetButtonDown("Jump")
				&& isGrounded)
			{
				pushJumpButton = true;
			}
		}

		//無敵になったら時間を計測
        if(invisible_flg == true)
        {
			elapsedTime += Time.deltaTime;
		}
		//設定した無敵時間を経過時間が超えたらダメージを受けるようにする
		if (elapsedTime >= invisibleTime)
		{
			Debug.Log("無敵解除");
			PlayerObj.tag = "Player";
			elapsedTime = 0f;
			invisible_flg = false;
		}
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

	private void OnCollisionStay(Collision collision)
	{
		//　地面とするレイヤーと接触している
		if ((groundLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
		{
			isCollision = true;
			//　衝突した最初の面の向きを入れる
			collisionDirection = collision.contacts[0].normal;
		}
		if (jumpValue > 0f
			&& Physics.CheckSphere(rigidBody.position + collisionCeilingOffset, collisionCeillingColliderRadius, groundLayers)
			)
		{
			jumpValue = 0f;
			animator.SetFloat("JumpPower", jumpValue);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		//　地面とするレイヤーから離れた
		if ((groundLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
		{
			isCollision = false;
			collisionDirection = Vector3.zero;
		}
	}
	/*
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, 0.3f);
	}*/
	//プレイヤーのダメージ処理 敵に攻撃されたときに呼び出す 故にpublicにする
	public void TakeDamage(Transform enemyTransform, int Damage)
	{
		animator.SetTrigger("DamageHit");
		state = MyState.Damage;
		velocity = Vector3.zero;

		//敵の攻撃でHPが減少する処理
		damage = Damage - def;
		if (damage < 0) damage = 1;
		hp -= damage;

		//HPゲージに反映
		Slider.value = (float)hp / (float)hp_max;

		//攻撃の向きによって吹き飛ぶ方向が変化
		//攻撃の向きは敵とプレイヤーの位置によって決まる
		if (enemyTransform.position.x > transform.position.x)//プレイヤーの位置が敵より左にいる時、左にノックバック
		{
			moveDir_num = -1;
		}
		else if (enemyTransform.position.x < transform.position.x)//プレイヤーの位置が敵より右にいる時、右にノックバック
		{
			moveDir_num = 1;
		}
		knockBack_dir = new Vector3(moveDir_num, 1, 0);

		rigidBody.AddForce(knockBack_dir * knockBack_power, ForceMode.VelocityChange);

		//無敵時間の処理
		//プレイヤーのコライダーを非表示にして当たり判定を消す
		PlayerObj.tag = "Invisible";
		//無敵フラグをtrueにしてUpdateメソッドで経過時間を計測を始める
		invisible_flg = true;

		Debug.Log(damage + "ダメージ " + hp);

		//死亡処理
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
		//後でダンジョンの入口に戻すように変える
		// ゲームオブジェクトを破壊
		Destroy(gameObject);
	}


	//敵の攻撃を盾で受け止めた時
	public void ShildGuard_success(Collider weaponCol)
    {
		Debug.Log("GuardSuccess");
		animator.SetTrigger("GuardSuccess");
	}

	//以下プレイヤーのアニメーションイベント
	//アニメーションの開始と同時に攻撃当たり判定であるWeaponColliderを出す
	void Attack_NormalStart()
	{
		ShieldCollider.enabled = false;//Idle時以外盾は無効
		WeaponCollider.enabled = true;
		moveConditions = true;//移動不可にする
	}
	//アニメーションの終了と同時に攻撃当たり判定であるWeaponColliderを消す
	//アニメーションを攻撃から待機へ戻す
	void Attack_NormalEnd()
	{
		animator.SetBool("AttackNomal", false);
		ShieldCollider.enabled = true;//盾を有効
		WeaponCollider.enabled = false;
		moveConditions = false;//移動可能にする
	}
	//移動していない時ガード状態
	//IsTriggerにチェックが入れられた別のColliderに触れるとガードのアニメーションが出る
	/*void OnTriggerEnter(Collider other)
	{
		if (input == new Vector3(0, 0, 0) && other.gameObject.tag == "EnemyAttack")
		{
		//敵の遠隔攻撃を無効化する

		moveConditions = true;//移動不可にする
		animator.SetBool("GuardNormal", true);
		}
	}
	//アニメーションの終了と同時にColliderを消す
	//アニメーションを受け止めから待機へ戻す
	void Guard_NormalEnd()
	{
		
		moveConditions = false;//移動可能にする
		animator.SetBool("GuardNormal", false);
	}*/
	//アニメーションの開始と同時に攻撃当たり判定であるWeaponColliderを出す
	void Attack_AerialStart()
	{
		ShieldCollider.enabled = false;//Idle時以外盾は無効
		WeaponCollider.enabled = true;
		//moveConditions = true;//移動不可にする
	}
	//アニメーションの終了と同時に攻撃当たり判定であるWeaponColliderを消す
	//アニメーションを攻撃から待機へ戻す
	void Attack_AerialEnd()
	{
		animator.SetBool("AttackNomal", false);
		ShieldCollider.enabled = true;//盾を有効
		WeaponCollider.enabled = false;
		//moveConditions = false;//移動可能にする
	}
	//ダメージを受けた時ステートを変える、モーション終了時にステートを変える
	void EndDamage()
	{
		state = MyState.Normal;
		input = Vector3.zero;
	}


	//スクリプタブルからステータスを設定するメソッド
	void status_StartSet()
    {
		hp_max = PlayerData.max_hp;
		hp = hp_max;
		ep_max = PlayerData.max_ep;
		ep = ep_max;
		str = PlayerData.str;
		mgc = PlayerData.mgc;
		def = PlayerData.def;
		res = PlayerData.res;

	}
}
