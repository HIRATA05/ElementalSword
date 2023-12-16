using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    //�������ǐՂ��ďP���Ă���^�C�v�̓GAI�̊�{

    //�G�̏����ʒu
    Vector3 Enemy_StartPos = Vector3.zero;

    public enum EnemyState
    {
        Walk,//�p�j�A�A��
        Wait,//�~�܂�
        Chase,//�ǐ�
        Attack,//�U��
        Freeze//�s����̍d��
    };

    private CharacterController enemyController;
    private Animator animator;
    private Rigidbody rigidBody;
    //����̃R���C�_�[
    //[SerializeField] private BoxCollider WeaponCol;
    //�@�����X�s�[�h
    [SerializeField] private float walkSpeed = 1.0f;
    //�@���x
    private Vector3 velocity;
    //�@�ړ�����
    private Vector3 direction;
    //�@�����t���O
    private bool arrived;
    //�@�ړ���
    private Vector3 setPosition;
    //�@�҂�����
    [SerializeField]
    private float waitTime = 5f;
    //�@�o�ߎ���
    private float elapsedTime;
    // �G�̏��
    private EnemyState state;
    //�@�v���C���[��Transform
    private Transform playerTransform;
    [SerializeField]
    private float freezeTime = 1f;

    //�m�b�N�o�b�N�̕���
    private Vector3 knockBack_dir;
    int moveDir_num;
    //�m�b�N�o�b�N�̐�����ԋ���
    private float knockBack_power = 5f;
    bool knockBack_flg = false;

    //�X�N���v�^�u����NPC�f�[�^
    [SerializeField] private Scriptable_CharaData EnemyData;
    //Slider��HP�Q�[�W���w��
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

        //value��HP�Q�[�W�̃X���C�_�[���ő��1��
        Slider.value = 1;

        //NPC�f�[�^�̃X�e�[�^�X����
        hp = EnemyData.max_hp; str = EnemyData.str; def = EnemyData.def;
    }

    // ���̂��v���C���[���U����ړ����x���傫���ቺ����s�������A�����炭�v���C���[�̈ʒu�𔻒肷�鏈�����������Ǝv����------------------------------
    void Update()
    {
        //�@�����܂��̓L�����N�^�[��ǂ���������
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //�@�L�����N�^�[��ǂ��������Ԃł���΃L�����N�^�[�̖ړI�n���Đݒ�
            if (state == EnemyState.Chase)
            {

                setPosition = playerTransform.position;
            }
            if (enemyController.isGrounded)
            {
                //�ړ��̏���
                velocity = Vector3.zero;
                animator.SetFloat("Speed", 2.0f);
                direction = (setPosition - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.x, transform.position.y, setPosition.z));
                velocity = direction * walkSpeed;
            }

            if (state == EnemyState.Walk)
            {
                //�@�ړI�n�ɓ����������ǂ����̔���
                if (Vector3.Distance(transform.position, setPosition) < 0.5f)
                {
                    SetState(EnemyState.Wait);
                    animator.SetFloat("Speed", 0.0f);
                }
            }
            else if (state == EnemyState.Chase)
            {
                //�@�U�����鋗����������U��
                if (Vector3.Distance(transform.position, setPosition) < 2f)
                {
                    SetState(EnemyState.Attack);
                    //�U�����鎞�ɕ��킪false�̎�����̃R���C�_�[��true�ɂ���
                    //if(WeaponCol.enabled == false) WeaponCol.enabled = true;
                }
            }
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //�@�҂����Ԃ��z�����玟�̖ړI�n��ݒ�
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }
        //�U����̃t���[�Y���
        else if (state == EnemyState.Freeze)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("�t���[�Y�I�I�I�I�I�I");
            if (elapsedTime > freezeTime)
            {
                SetState(EnemyState.Wait);
            }
        }
        //�U�����ꂽ���̃m�b�N�o�b�N���̈ړ� �Ȃ����m�b�N�o�b�N���Ȃ��v�C��
        if (knockBack_flg == true) { Debug.Log("knockback!! "); rigidBody.AddForce(transform.up * Time.deltaTime, ForceMode.VelocityChange); knockBack_flg = false; }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
            enemyController.Move(velocity * Time.deltaTime);
        }

    }

    //�@�G�L�����N�^�[�̏�ԕύX���\�b�h
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (tempState == EnemyState.Walk)
        {
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            setPosition = Enemy_StartPos;//�����ʒu��ݒ�
        }
        else if (tempState == EnemyState.Chase)
        {
            state = tempState;
            //�@�ҋ@��Ԃ���ǂ�������ꍇ������̂�Off
            arrived = false;
            //�@�ǂ�������Ώۂ��Z�b�g
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
    //�G�L�����N�^�[�̏�Ԏ擾���\�b�h
    public EnemyState GetState()
    {
        return state;
    }

    //�_���[�W�����̃��\�b�h�@value�ɂ�Player��atk�̒l�������Ă�
    public void Damage(int value)
    {
        Debug.Log("Damage!! " + value);
        // �G�f�[�^��null�łȂ������`�F�b�N
        if (EnemyData != null)
        {
            //Player��str����G��def���������l��hp�������
            hp -= value - def;
            //HP�Q�[�W�ɔ��f
            Slider.value = (float)hp / (float)EnemyData.max_hp;
        }
        //�m�b�N�o�b�N
        animator.SetTrigger("Damage");
        //velocity = Vector3.zero;

        //�U���̌����ɂ���Đ�����ԕ������ω�
        //�U���̌����͓G�ƃv���C���[�̈ʒu�ɂ���Č��܂�
        if (transform.position.x < playerTransform.position.x)//�v���C���[�̈ʒu���G��荶�ɂ��鎞�A���Ƀm�b�N�o�b�N
        {
            moveDir_num = -1;
        }
        else if (transform.position.x > playerTransform.position.x)//�v���C���[�̈ʒu���G���E�ɂ��鎞�A�E�Ƀm�b�N�o�b�N
        {
            moveDir_num = 1;
        }

        knockBack_dir = new Vector3(moveDir_num, 1, 0);

        //rigidBody.AddForce(transform.up, ForceMode.VelocityChange);
        knockBack_flg = true;

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
        // �Q�[���I�u�W�F�N�g��j��
        Destroy(gameObject);
    }
}
