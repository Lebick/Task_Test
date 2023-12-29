using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sr;

    public float    HP = 100,
                    MovementSpeed = 20,
                    Invincibility_Time = 1.0f;

    private float Invincibility_Timer = 0;

    float Alpha = 0;

    bool    Invincibility,
            Enter_Enemy;

    GameObject Enter_Enemy_Obj;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float Horizontal    = Input.GetAxisRaw("Horizontal"); //������� �¿� �Է°�
        float Vertical      = Input.GetAxisRaw("Vertical"); //������� ���� �Է°�

        transform.position += new Vector3(Horizontal, Vertical, 0) * Time.deltaTime * MovementSpeed; //�̵�

        if (Invincibility) //�������¶��
            AlphaChange(); //�Լ�����

        if(Enter_Enemy)
            GetDamage();
    }

    void AlphaChange()
    {
        Invincibility_Timer += Time.deltaTime; //Ÿ�̸� ���
        if(Invincibility_Timer < Invincibility_Time) //�ִ� �ð��� ���� �ʾҴٸ�
        { //�ڵ����
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, Alpha, Time.deltaTime * 25)); //���������Լ��� �̿��� ��ǥ ���İ����� ����
            
            if(Mathf.Abs(sr.color.a - Alpha) <= 0.2f) //��ǥ ���İ����� ���̰� 0.2���϶��
            {
                if (Alpha == 0)     //��ǥ ���İ��� 0�̾��ٸ�
                    Alpha = 1;      //1���� �ٲ�
                else                //�ƴϾ��ٸ�(��ǥ ���İ��� 1�̾��ٸ�)
                    Alpha = 0;      //0���� �ٲ�
            }
        }
        else //�ִ� �ð��� �Ѿ��ٸ�
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1); //���İ��� 1���� �ǵ���.
            Invincibility_Timer = 0; //Ÿ�̸� �ʱ�ȭ
            Invincibility = false; //�������� ����
        }
    }

    void GetDamage()
    {
        if (!Invincibility) //�������°� �ƴ϶��
        {
            if (Enter_Enemy_Obj.CompareTag("Enemy"))
            {
                HP -= Enter_Enemy_Obj.GetComponent<Enemy>().Damage / 2; //HP�� ����.
                Invincibility = true; //�������·� ����.
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) //���� ���ݿ� �¾Ҵٸ�
        {
            if (!Invincibility) //�������°� �ƴ϶��
            {
                HP -= collision.GetComponent<EnemyBullet>().Damage; //HP�� ����.
                Invincibility = true; //�������·� ����.
            }
            Destroy(collision.gameObject); //�ǰݵ� ���� ������ ����
        }

        if (collision.CompareTag("Enemy")) //���� ���������� �浹�ߴٸ�
        {
            Enter_Enemy = true;
            Enter_Enemy_Obj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) //���� ���������� �浹�ߴٸ�
        {
            Enter_Enemy = false;
            Enter_Enemy_Obj = null;
        }
    }
}
