using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAction : MonoBehaviour//, IDamageable
{
    /*
    private Rigidbody rigidBody;
    //���G�A�ړ��̍l�����@���ׂ邱��
    //�v���C���[���G��荶�ɂ���ƍ��Ɉړ��A�E�ɂ���ƉE�Ɉړ��A�v���C���[�̎�O=�v���C���[�̈ʒu+0.5��x���W�Ɉړ��������U������

    //���G�͈͂̐ݒ�@�R���C�_�[��ݒ肵�Ĕ͈͓��ɓ���ƃv���C���[�̒ǐՂ��n�߂�
    public BoxCollider SearchCollider;
    //�����ʒu�̐ݒ�@�v���C���[�����G�͈͊O�ɏo��ƍ��G����߂ď����ʒu�Ɉړ����n�߂�
    private Vector3 start_Pos;
    //�U���͈͂̐ݒ�@�R���C�_�[��ݒ肵�Ĕ͈͓��ɓ���ƍU�����[�V�������n�߁A�o����������̃R���C�_�[�ɓ�����ƃv���C���[���_���[�W���󂯂�
    public SphereCollider AttackRangeCollider;
    //����E���̓����蔻��̃R���C�_�[
    public BoxCollider WeaponCollider;
    public BoxCollider Shield_Collider;

    //public GameObject destination; �@//�ړI�n�ɂȂ�I�u�W�F�N�g���擾���邽�߂̕ϐ�
    public NavMeshAgent agent; //NavMeshAgent�擾�p�̕ϐ�

    //�@�G�̏��
    private EnemyState state;
    //�U������^�[�Q�b�g
    [SerializeField] public GameObject targetObject;
    Vector3 targetPos;

    //�����̈ʒu
    Vector3 Position;
    Vector3 moveDirection;
    public float moveDir_num = 1;
    //���x
    private Vector3 velocity;
    private float walkSpeed = 4f;
    //�Ώۂ̋߂��Ŏ~�܂�͈�
    int front = 2;

    private Animator animator;

    //�X�N���v�^�u����NPC�f�[�^
    [SerializeField] private Scriptable_CharaData EnemyData;
    //Slider��HP�Q�[�W���w��
    [SerializeField] Slider Slider;
    int hp;

    //�@�v���C���[Transform
    private Transform playerTransform;
    //�@�W�����v�����ǂ���
    private bool isJump;
    //�@�W�����v��ɐڒn�m�F����܂ł̒x������
    private float jumpWaitTime = 0.5f;
    //�@�W�����v��̎���
    private float jumpTime = 0f;
    //�@�ڒn���Ă��邩�ǂ���
    private bool isGrounded;
    //�@�ڒn�m�F�̃R���C�_�̈ʒu�̃I�t�Z�b�g
    private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    //�@�ڒn�m�F�̋��̃R���C�_�̔��a
    private float groundColliderRadius = 0.29f;
    //�@�n�ʂƔ��f���郌�C���[�}�X�N
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

        //value��HP�Q�[�W�̃X���C�_�[���ő��1��
        Slider.value = 1;

        //NPC�f�[�^�̍ő�HP����
        hp = EnemyData.hp_max;

        //agent = GetComponent<NavMeshAgent>(); //NavMeshAgent�̎擾
        //�����̌��݂̍��W������
        Position = transform.position;
        //velocity = Vector3.zero;
        SetState(EnemyState.Walk);
    }

    void Update()
    {
        //�@�ڒn�m�F
        CheckGround();

        //�@�����܂��̓L�����N�^�[��ǂ���������
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //�v���C���[�̈ʒu�ɂ���Ĉړ����������߂�
            if (targetPos.x > Position.x)//�v���C���[�̈ʒu���G��荶�ɂ��鎞�A���Ɉړ�����
            {
                moveDir_num = 1;
            }
            else if (targetPos.x < Position.x)//�v���C���[�̈ʒu���G���E�ɂ��鎞�A�E�Ɉړ�����
            {
                moveDir_num = -1;
            }
            moveDirection = new Vector3(moveDir_num, 0, 0);

            //�@�ړ����x���v�Z
            var clampedInput = Vector3.ClampMagnitude(moveDirection, 1f);
            velocity = clampedInput * walkSpeed * 1.2f;
            transform.LookAt(rigidBody.position + moveDirection);
            //�@�����͂���v�Z�������x���猻�݂�Rigidbody�̑��x������
            velocity = velocity - rigidBody.velocity;
            //�@���x��XZ��-walkSpeed��walkSpeed���Ɏ��߂čĐݒ�
            velocity = new Vector3(Mathf.Clamp(velocity.x, -walkSpeed, walkSpeed), 0f, Mathf.Clamp(velocity.z, -walkSpeed, walkSpeed));

            //�@�L�����N�^�[��ǂ��������Ԃł���΃L�����N�^�[�̖ړI�n���Đݒ�
            if (state == EnemyState.Chase)
            {
                //�v���C���[�̍��W
                targetPos = playerTransform.position;

                if(state == EnemyState.Attack) { animator.SetBool("Attack", true); }
            }
            if (isGrounded)
            {
                Debug.Log("ground!!");

                //�ʒu�̍X�V
                //Position = transform.position;

                // �}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
                //transform.Translate(transform.right * Time.deltaTime * 2 * -moveDir_num);
                

                if (clampedInput.magnitude > 0f)//�ړ����Ă���
                {
                    animator.SetFloat("Speed", clampedInput.magnitude);
                }
                else//�~�܂�
                {
                    animator.SetFloat("Speed", 0f);
                    SetState(EnemyState.Wait);
                }
            }
            //�ړ�����
            rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime *1.5f, ForceMode.Force);
            //�����̌��݂̍��W������
            Position = transform.position;
        }
    }

    //�G�̏�ԕύX���\�b�h
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
            //�@�ǂ�������Ώۂ��Z�b�g
            playerTransform = targetObj;
        }
        else if (tempState == EnemyState.Wait)
        {
            Debug.Log("wait!!");
            state = tempState;
            velocity = Vector3.zero;
        }
    }
    //�G�̏�Ԏ擾���\�b�h
    public EnemyState GetState()
    {
        return state;
    }

    // �_���[�W�����̃��\�b�h�@value�ɂ�Player��atk�̒l�������Ă�
    public void Damage(int value)
    {
        Debug.Log("Damage!! " + value);
        // �G�f�[�^��null�łȂ������`�F�b�N
        if (EnemyData != null)
        {
            // Player��atk����G��def���������l��hp�������
            hp -= value - EnemyData.def;
            //HP�Q�[�W�ɔ��f
            Slider.value = (float)hp / (float)EnemyData.hp_max;
        }

        // hp��0�ȉ��Ȃ�Death()���\�b�h���Ăяo��
        if (hp <= 0)
        {
            Death();
        }
    }
    // ���S�����̃��\�b�h
    public void Death()
    {
        Debug.Log("Death!!");
        // �Q�[���I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

    //IsTrigger�Ƀ`�F�b�N�������ꂽ�ʂ�Collider�ɐG����
    //�_���[�W�̃A�j���[�V�������o��
    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Damage", true);
    }
    //�A�j���[�V�����C�x���g
    void DamageEnd()
    {
        animator.SetBool("Damage", false);
    }
    //�U�����̃A�j���[�V����
    void ArmorAttack_Start()
    {
        //����L���ɂ���
        WeaponCollider.enabled = true;
    }
    void ArmorAttack_End()
    {
        //���𖳌��ɂ���
        WeaponCollider.enabled = false;
        animator.SetBool("Attack", false);
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
    */
}
