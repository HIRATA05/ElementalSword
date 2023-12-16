using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideViewController : MonoBehaviour
{
	//2D���_�̃v���C���[�̊�{����


	//�v���C���[�̏��
	public enum MyState
	{
		Normal,
		Damage
	};
	private MyState state;

	//�����蔻��
	[SerializeField] private GameObject PlayerObj;
	[SerializeField] private CapsuleCollider WeaponCollider;
	[SerializeField] private BoxCollider ShieldCollider;
	[SerializeField] private BoxCollider CrouchingShieldCollider;

	//�m�b�N�o�b�N�̕���
	private Vector3 knockBack_dir;
	int moveDir_num;
	//�m�b�N�o�b�N�̐�����ԋ���
	private float knockBack_power = 5f;

	//���G����
	[SerializeField] private float invisibleTime = 3f;
	//�o�ߎ���
	private float elapsedTime = 0f;
	private bool invisible_flg = false;

	private bool moveConditions = false;
	private Rigidbody rigidBody;
	//�@�ړ����x
	private Vector3 velocity;
	//�@���͒l
	private Vector3 input;
	//�@�A�j���[�^�[
	private Animator animator;
	//�@�n�ʂƔ��f���郌�C���[�}�X�N
	[SerializeField] private LayerMask groundLayers;
	//�@�ړ��̑���
	private float walkSpeed = 4f;
	//�@�ڒn���Ă��邩�ǂ���
	private bool isGrounded;
	//�@�ڒn�m�F�̃R���C�_�̈ʒu�̃I�t�Z�b�g
	private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
	//�@�ڒn�m�F�̋��̃R���C�_�̔��a
	private float groundColliderRadius = 0.29f;

	//�@�W�����v�{�^�������������ǂ���
	private bool pushJumpButton;
	//�@�W�����v���鍂��
	[SerializeField]
	private float jumpHeight = 5f;
	//�@�����̃W�����v�l
	private float initJumpHeightValue;
	//�@���݂̃W�����v�l
	private float jumpValue;
	//�@�W�����v�����ǂ���
	private bool isJump;
	//�@�W�����v��ɐڒn�m�F����܂ł̒x������
	private float jumpWaitTime = 0.5f;
	//�@�W�����v��̎���
	[SerializeField]
	private float jumpTime = 0f;
	//�@�Փ˂������ǂ���
	private bool isCollision;
	//�@�Փˊm�F�̃R���C�_�̈ʒu�̃I�t�Z�b�g
	private Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0.1f);
	//�@�Փˊm�F�̋��̃R���C�_�̔��a
	private float collisionColliderRadius = 0.3f;
	//�@�V��̏Փˊm�F�p�R���C�_�̃I�t�Z�b�g�l
	private Vector3 collisionCeilingOffset = new Vector3(0f, 1.8f, 0f);
	//�@�V��̏Փˊm�F�p�R���C�_�̋��̔��a
	private float collisionCeillingColliderRadius = 0.29f;
	//�@�Փ˂����ʂ̕���
	private Vector3 collisionDirection;

	//�v���C���[�̃X�e�[�^�X�ݒ�̃X�N���v�^�u��
	[SerializeField] private Scriptable_Player PlayerData;
	//Slider��HP�Q�[�W���w��
	[SerializeField] Slider Slider;
	//�v���C���[�̃X�e�[�^�X
	int hp_max, hp, ep_max, ep, str, mgc, tec, spd, luc, def, res;
	int damage;//�U�����󂯂����̃_���[�W

	void Start()
	{
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		//value��HP�Q�[�W�̃X���C�_�[���ő��1��
		Slider.value = 1;
		//���݂̃X�e�[�^�X���X�N���v�^�u����������
		status_StartSet();
	}

	void FixedUpdate()
	{
		//�@�ڒn�m�F
		CheckGround();

		if (state == MyState.Normal)
		{
			//�@���͒l�@moveConditions��false�̂Ƃ��ړ��o����A����̃��[�V�������͈ړ��o���Ȃ�����
			input = new Vector3(Input.GetAxis("Horizontal"), 0, 0/*Input.GetAxis("Vertical")*/);

			//�@�ړ����x���v�Z
			var clampedInput = Vector3.ClampMagnitude(input, 1f);
			velocity = clampedInput * walkSpeed * 1.2f;
			transform.LookAt(rigidBody.position + input);
			//�@�����͂���v�Z�������x���猻�݂�Rigidbody�̑��x������
			velocity = velocity - rigidBody.velocity;
			//�@���x��XZ��-walkSpeed��walkSpeed���Ɏ��߂čĐݒ�
			velocity = new Vector3(Mathf.Clamp(velocity.x, -walkSpeed, walkSpeed), 0f, Mathf.Clamp(velocity.z, -walkSpeed, walkSpeed));

			//�@�ڒn���̏���
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

				//�ʏ�U��
				if (Input.GetKey(KeyCode.Z))
				{
					animator.SetBool("AttackNomal", true);

				}
				//�K�[�h�����Ⴊ�ݎ��͏����\���h�䂷��
				//�K�[�h
				if (Input.GetKey(KeyCode.X))
				{
					animator.SetBool("GuardNormal", true);
					//���̃R���C�_�[��\��
					ShieldCollider.enabled = true;
					if (Input.GetKeyUp(KeyCode.C)) animator.SetTrigger("GuardSuccess");
				}
				else
                {
					animator.SetBool("GuardNormal", false);
					ShieldCollider.enabled = false;
				}
				//���Ⴊ��
				if (Input.GetKey(KeyCode.DownArrow))
				{
					animator.SetBool("Crouching", true);
					//���̃R���C�_�[��\��
					CrouchingShieldCollider.enabled = true;
					if (Input.GetKeyUp(KeyCode.C)) animator.SetTrigger("GuardSuccess");
				}
				else
				{
					animator.SetBool("Crouching", false);
					CrouchingShieldCollider.enabled = false;
				}

				// �W�����v
				if (pushJumpButton)
				{
					pushJumpButton = false;
					isGrounded = false;
					isJump = true;
					jumpTime = 0f;
					animator.SetTrigger("Jump");
					//�@�����̃W�����v�l���v�Z
					initJumpHeightValue = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
					jumpValue = initJumpHeightValue;
					animator.SetFloat("JumpPower", initJumpHeightValue);

					//�@�����ɃW�����v������
					rigidBody.velocity = new Vector3(rigidBody.velocity.x, initJumpHeightValue, rigidBody.velocity.z);
				}
			}

			//�󒆍U��
			if (Input.GetKey(KeyCode.Z))
			{
				animator.SetBool("AttackNomal", true);
			}
		}
		//�@���݂̃W�����v�l�̌v�Z
		if (jumpValue > -initJumpHeightValue)
		{
			jumpValue += Physics.gravity.y * Time.fixedDeltaTime;
			animator.SetFloat("JumpPower", jumpValue);
		}
		//�@�W�����v���̓W�����v���Ԃ��v������
		if (isJump && jumpTime < jumpWaitTime)
		{
			jumpTime += Time.deltaTime;
		}

		//�ړ�����
		rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime, ForceMode.Force);

		//�W�����v�����ɉ��ǂƐڐG���Ă��鎞�A�܂��͊K�i������Ă��鎞�͏d�͈ȊO�̈ړ��l��0�ɂ���
		if ((!isGrounded && isCollision))
		{
			//�Փ˂����Ǖ����������Ă��鎞�����d�͈ȊO�̈ړ��l��0�ɂ���
			if (Vector3.Dot(transform.forward, collisionDirection) <= 0.5f)
			{
				velocity = new Vector3(0f, velocity.y, 0f);
			}
		}
	}

	private void Update()
	{
		if (state == MyState.Normal) //�X�e�[�g���̂Ƃ��W�����v�o����A�U�������[�V�������̓W�����v�o���Ȃ�����
		{
			if (Input.GetButtonDown("Jump")
				&& isGrounded)
			{
				pushJumpButton = true;
			}
		}

		//���G�ɂȂ����玞�Ԃ��v��
        if(invisible_flg == true)
        {
			elapsedTime += Time.deltaTime;
		}
		//�ݒ肵�����G���Ԃ��o�ߎ��Ԃ���������_���[�W���󂯂�悤�ɂ���
		if (elapsedTime >= invisibleTime)
		{
			Debug.Log("���G����");
			PlayerObj.tag = "Player";
			elapsedTime = 0f;
			invisible_flg = false;
		}
	}

	//�@�n�ʂ̃`�F�b�N
	private void CheckGround()
	{
		//�@�A�j���[�V�����p�����[�^��Rigidbody��Y��0.1�ȉ���Ground�܂���Enemy���C���[�Ƌ��̃R���C�_���Ԃ������ꍇ�͒n�ʂɐڒn
		if (Physics.CheckSphere(rigidBody.position + groundPositionOffset, groundColliderRadius, groundLayers))
		{
			//�@�W�����v��
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
		//�@�n�ʂƂ��郌�C���[�ƐڐG���Ă���
		if ((groundLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
		{
			isCollision = true;
			//�@�Փ˂����ŏ��̖ʂ̌���������
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
		//�@�n�ʂƂ��郌�C���[���痣�ꂽ
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
	//�v���C���[�̃_���[�W���� �G�ɍU�����ꂽ�Ƃ��ɌĂяo�� �̂�public�ɂ���
	public void TakeDamage(Transform enemyTransform, int Damage)
	{
		animator.SetTrigger("DamageHit");
		state = MyState.Damage;
		velocity = Vector3.zero;

		//�G�̍U����HP���������鏈��
		damage = Damage - def;
		if (damage < 0) damage = 1;
		hp -= damage;

		//HP�Q�[�W�ɔ��f
		Slider.value = (float)hp / (float)hp_max;

		//�U���̌����ɂ���Đ�����ԕ������ω�
		//�U���̌����͓G�ƃv���C���[�̈ʒu�ɂ���Č��܂�
		if (enemyTransform.position.x > transform.position.x)//�v���C���[�̈ʒu���G��荶�ɂ��鎞�A���Ƀm�b�N�o�b�N
		{
			moveDir_num = -1;
		}
		else if (enemyTransform.position.x < transform.position.x)//�v���C���[�̈ʒu���G���E�ɂ��鎞�A�E�Ƀm�b�N�o�b�N
		{
			moveDir_num = 1;
		}
		knockBack_dir = new Vector3(moveDir_num, 1, 0);

		rigidBody.AddForce(knockBack_dir * knockBack_power, ForceMode.VelocityChange);

		//���G���Ԃ̏���
		//�v���C���[�̃R���C�_�[���\���ɂ��ē����蔻�������
		PlayerObj.tag = "Invisible";
		//���G�t���O��true�ɂ���Update���\�b�h�Ōo�ߎ��Ԃ��v�����n�߂�
		invisible_flg = true;

		Debug.Log(damage + "�_���[�W " + hp);

		//���S����
		//���S�����Ahp��0�ȉ��Ȃ�Death()���\�b�h���Ăяo��
		if (hp <= 0)
		{
			Death();
		}
	}
	// ���S�����̃��\�b�h
	public void Death()
	{
		Debug.Log("Death!!");
		//��Ń_���W�����̓����ɖ߂��悤�ɕς���
		// �Q�[���I�u�W�F�N�g��j��
		Destroy(gameObject);
	}


	//�G�̍U�������Ŏ󂯎~�߂���
	public void ShildGuard_success(Collider weaponCol)
    {
		Debug.Log("GuardSuccess");
		animator.SetTrigger("GuardSuccess");
	}

	//�ȉ��v���C���[�̃A�j���[�V�����C�x���g
	//�A�j���[�V�����̊J�n�Ɠ����ɍU�������蔻��ł���WeaponCollider���o��
	void Attack_NormalStart()
	{
		ShieldCollider.enabled = false;//Idle���ȊO���͖���
		WeaponCollider.enabled = true;
		moveConditions = true;//�ړ��s�ɂ���
	}
	//�A�j���[�V�����̏I���Ɠ����ɍU�������蔻��ł���WeaponCollider������
	//�A�j���[�V�������U������ҋ@�֖߂�
	void Attack_NormalEnd()
	{
		animator.SetBool("AttackNomal", false);
		ShieldCollider.enabled = true;//����L��
		WeaponCollider.enabled = false;
		moveConditions = false;//�ړ��\�ɂ���
	}
	//�ړ����Ă��Ȃ����K�[�h���
	//IsTrigger�Ƀ`�F�b�N�������ꂽ�ʂ�Collider�ɐG���ƃK�[�h�̃A�j���[�V�������o��
	/*void OnTriggerEnter(Collider other)
	{
		if (input == new Vector3(0, 0, 0) && other.gameObject.tag == "EnemyAttack")
		{
		//�G�̉��u�U���𖳌�������

		moveConditions = true;//�ړ��s�ɂ���
		animator.SetBool("GuardNormal", true);
		}
	}
	//�A�j���[�V�����̏I���Ɠ�����Collider������
	//�A�j���[�V�������󂯎~�߂���ҋ@�֖߂�
	void Guard_NormalEnd()
	{
		
		moveConditions = false;//�ړ��\�ɂ���
		animator.SetBool("GuardNormal", false);
	}*/
	//�A�j���[�V�����̊J�n�Ɠ����ɍU�������蔻��ł���WeaponCollider���o��
	void Attack_AerialStart()
	{
		ShieldCollider.enabled = false;//Idle���ȊO���͖���
		WeaponCollider.enabled = true;
		//moveConditions = true;//�ړ��s�ɂ���
	}
	//�A�j���[�V�����̏I���Ɠ����ɍU�������蔻��ł���WeaponCollider������
	//�A�j���[�V�������U������ҋ@�֖߂�
	void Attack_AerialEnd()
	{
		animator.SetBool("AttackNomal", false);
		ShieldCollider.enabled = true;//����L��
		WeaponCollider.enabled = false;
		//moveConditions = false;//�ړ��\�ɂ���
	}
	//�_���[�W���󂯂����X�e�[�g��ς���A���[�V�����I�����ɃX�e�[�g��ς���
	void EndDamage()
	{
		state = MyState.Normal;
		input = Vector3.zero;
	}


	//�X�N���v�^�u������X�e�[�^�X��ݒ肷�郁�\�b�h
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
