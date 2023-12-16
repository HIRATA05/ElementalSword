using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAssault : MonoBehaviour, IDamageable
{
    //�v���C���[�ɂ��ڂ��������ˌ�����G�̓��� ��ɉ�ʊO����o������

    //�ړ��p�f�[�^
    [SerializeField] private float speed = 1.0f;
    private Vector3 moveDirection = Vector3.zero;
    //���x
    private Vector3 velocity = Vector3.zero;
    //�����Ă������
    public float DirectionFirst_x = -1;
    //�����Ă������
    public float Direction_x = -1;
    //�҂�����
    [SerializeField] private float returnTime = 3f;
    //�o�ߎ���
    private float elapsedTime;
    //���݂�x
    float current_x = 0f;
    //�ߋ���x
    float past_x = 0f;

    //�G�Ƃ̐ڐG���̃_���[�W����
    public int damage = 1;
    //�X�N���v�^�u����NPC�f�[�^
    [SerializeField] private Scriptable_CharaData EnemyData;
    //�G�̃X�e�[�^�X
    int hp_max, hp, ep_max, ep, str, mgc, def, res;
    //Slider��HP�Q�[�W���w��
    [SerializeField] Slider Slider;
    private Rigidbody rigidBody;
    private CharacterController enemyController;
    //EnemySpawner�X�N���v�g�̎擾
    private EnemySpawner enemySpawner;

    //��ʓ��ɐN�������t���O�@���̃t���O��true�̎��ɉ�ʊO�ɏo��Ǝ��S����
    public bool screenIn_flg = false;
    //�r���[�|�[�g���W��x�̒l
    float view_x;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        enemyController = GetComponent<CharacterController>();
        enemySpawner = GetComponent<EnemySpawner>();
        //�X�N���v�^�u������f�[�^����
        status_StartSet();

        Direction_x = DirectionFirst_x;
    }

    // Update is called once per frame
    void Update()
    {
        //��莞��x���ω����Ȃ��������Ԃ�����
        elapsedTime = elapsedTime + Time.deltaTime;
        if(elapsedTime > returnTime)
        {
            elapsedTime = 0;
            //����I�Ɏ��Ԃ��v������x���擾
            past_x = current_x;
            current_x = transform.position.x;
            //x���ω����Ă��Ȃ����i�ޕ�����ω�
            if (current_x == past_x) Direction_x *= -1;
        }

        //���݈ʒu���r���[�|�[�g���W��x�̒l�Ŏ擾
        view_x = Camera.main.WorldToViewportPoint(transform.position).x;
        //��ʓ��ɓ�������t���O��true�ɕς���
        if (0 < view_x && view_x < 1)
        {
            screenIn_flg = true;
            Debug.Log(view_x);
        }
        //��ʊO�Ŏ��S
        else if (!(0 < view_x && view_x < 1) && screenIn_flg) Death();
 
    }
    void FixedUpdate()
    {
        //�ړ�
        moveDirection = new Vector3(Direction_x, 0.0f, 0f);
        //�@�ړ����x���v�Z
        var clampedInput = Vector3.ClampMagnitude(moveDirection, 1f);
        velocity = clampedInput * speed * 1.2f;
        transform.LookAt(rigidBody.position + moveDirection);
        //�@�����͂���v�Z�������x���猻�݂�Rigidbody�̑��x������
        velocity = velocity - rigidBody.velocity;
        //�@���x��XZ��-walkSpeed��walkSpeed���Ɏ��߂čĐݒ�
        velocity = new Vector3(Mathf.Clamp(velocity.x, -speed, speed), 0f, Mathf.Clamp(velocity.z, -speed, speed));

        //�ړ�����
        rigidBody.AddForce(rigidBody.mass * velocity / Time.fixedDeltaTime, ForceMode.Force);
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
        //���S�����񂾏���EnemySpawner�ɓ`�����݂̏o���l���ϐ������炷
        EnemyData.die = true;

        // �Q�[���I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision hit)
    {
        //�����������̂��v���C���[�ł���ꍇ
        if (hit.gameObject.tag == "Player")
        {
            Debug.Log("������");
            hit.gameObject.GetComponent<SideViewController>().TakeDamage(transform, damage);
        }
    }

    //�X�N���v�^�u������X�e�[�^�X��ݒ肷�郁�\�b�h
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
