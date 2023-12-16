using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    Animator anim;
    [SerializeField] Scriptable_CharaData ArmorKnight;
    [SerializeField] MonoBehaviour EnemyAttack;

    int State;
    bool action;
    IEnemy EnemyAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        //IEnemy�̃C���^�[�t�F�[�X��錾�����R���|�[�l���g(�X�N���v�g)����ɓ���AEnemyAction�֑���B
        EnemyAction = GetComponent<IEnemy>();
    }

    void Update()
    {
        //���t���[���������s���K�v�͂Ȃ��B�R���[�`����Enemytime�̎��Ԃ����ҋ@
        StartCoroutine(Enemytime());

        //Attack�p�����[�^�̒l������0���傫���Ȃ�U�����Ȃ̂�return;���ď������I������B
        bool Attack = anim.GetBool("Attack");
        if (Attack) { return; }

        //�l�������Ă��Ȃ��ꍇ�ɔ���null�`�F�b�N�B
        if (EnemyAction != null)
        {
            //�X�N���v�g��EnemyAIkoudou()���Ăяo���A�Ԃ��Ă����l��State�ɑ������B
            State = EnemyAction.EnemyAction_AI();


            action = true;
            //Switch����State�̒l�ɉ����ď�������B
            switch (State)
            {
                //State��0�Ȃ��~�B
                case 0:

                    anim.SetBool("Attack", false);
                    break;

                //State��1�Ȃ�U���B
                case 1:

                    anim.SetBool("Attack", true);
                    break;
            }
        }
    }

    IEnumerator Enemytime()
    {
        yield return new WaitForSeconds(ArmorKnight.EnemyTime);
        action = false;
    }
}
