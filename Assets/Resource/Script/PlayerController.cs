using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //�t�B�[���h�}�b�v�p�̃v���C���[�ړ� �G�G���J�E���g���Ǘ�
    //�����������ƓG���o������

    public enum FieldState
    {
        Road,//���S�ȓ�
        Grass,//����
        Forest,//�X
        Desert,//����
    };
    private FieldState state;

    Animator animator;

    Quaternion targetRotation;

    //�����̈ʒu�̕ۑ��p
    private Vector3 Player_pos;
    //�o������G�̃I�u�W�F�N�g
    public GameObject enemyObject;
    //�G�̏o���ʒu
    float app_x, app_z;
    int rand_pos;

    //�v������
    float elapsedTime;
    //�G�̏o������
    public float appearanceTime = 15f;

    //�t�B�[���h�̏���ۑ������X�N���v�^�u��
    [SerializeField] private Field_Data FieldData;

    void Awake()
    {
        //�R���|�[�l���g
        TryGetComponent(out animator);

        //�ŏ��ɏ���������
        targetRotation = transform.rotation;
    }
    private void Start()
    {
        //������
        elapsedTime = 0;

        //�G���J�E���g���̐퓬�t�B�[���h����߂��Ă������t�B�[���h�}�b�v�̍Ō�ɂ����n�_�Ɉړ�����
        if (FieldData.encTOfei_flg)
        {
            transform.position = FieldData.PlayerPosition_World;
            FieldData.encTOfei_flg = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�X�N���v�^�u���Ɏ����̈ʒu��ݒ肷�邽�߂Ɏ����̈ʒu���擾��������
        Player_pos = transform.position;

        //�J�����̌����ŕ␳�������̓x�N�g���̎擾
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //���x�̎擾
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        //�ړ�����������
        if(velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        //�ړ����x��Animator�ɔ��f
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);


        //�����Ǝ��Ԃ̌v��
        if (state != FieldState.Road && horizontal != 0 || vertical != 0) elapsedTime += Time.deltaTime;
        else if (state == FieldState.Road) elapsedTime = 0;

        //�ݒ肵�����ԕ����ƓG����苗���ɏo��
        if (elapsedTime > appearanceTime)
        {
            elapsedTime = 0;//�v���̃��Z�b�g

            //�o���ʒu���w�肷��ϐ��Ƀ����_���Ȑ��������郁�\�b�h
            EnemyAppPos();

            //�G�̏o��
            Instantiate(enemyObject, new Vector3(app_x, 0.0f, app_z), Quaternion.identity);
        }
    }

    //�G�Ƃ̃G���J�E���g
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�����������̂��v���C���[�ł���ꍇ
        if (hit.gameObject.tag == "Enemy")
        {
            Debug.Log("�G���J�E���g");
            //�G���J�E���g���X�N���v�^�u���ɍ��W�f�[�^��ۑ� �t�B�[���h�ɖ߂鎞�̈ʒu�����߂�
            Player_pos = transform.position;
            FieldData.PlayerPosition_World = Player_pos;
            FieldData.encTOfei_flg = true;

            //�擾�����t�B�[���h�^�O�ɂ���Ĉړ��悪�ω�
            SceneManager.LoadScene("BattleField");
        }
    }

    //�����̃R���C�_�[����n�ʂ̃^�O���擾�A�G�ƃG���J�E���g�������擾�����^�O�ɂ��ړ�����}�b�v��ω�������
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
        //�G�̏o���ʒu��4�p�^�[������
        //�v���C���[�̑O�㍶�E�ɏo�� �o��������10
        //1����100�̃����_���Ȑ����o����25�͈̔͂�4��������ʂ��Ĉʒu�����߂�
        rand_pos = (int)Random.Range(1.0f, 101.0f);
        if(rand_pos < 26) { app_x = 0; app_z = 10; }//�v���C���[�̉�
        else if(rand_pos < 51 && rand_pos > 25) { app_x = -10; app_z = 0; }//�v���C���[�̍�
        else if (rand_pos < 76 && rand_pos > 50) { app_x = 10; app_z = 0; }//�v���C���[�̉E
        else if (rand_pos < 100 && rand_pos > 75) { app_x = 0; app_z = -10; }//�v���C���[�̉E

    }
}
