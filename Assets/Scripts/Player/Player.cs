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
        float Horizontal    = Input.GetAxisRaw("Horizontal"); //사용자의 좌우 입력값
        float Vertical      = Input.GetAxisRaw("Vertical"); //사용자의 상하 입력값

        transform.position += new Vector3(Horizontal, Vertical, 0) * Time.deltaTime * MovementSpeed; //이동

        if (Invincibility) //무적상태라면
            AlphaChange(); //함수실행

        if(Enter_Enemy)
            GetDamage();
    }

    void AlphaChange()
    {
        Invincibility_Timer += Time.deltaTime; //타이머 재생
        if(Invincibility_Timer < Invincibility_Time) //최대 시간을 넘지 않았다면
        { //코드실행
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, Alpha, Time.deltaTime * 25)); //선형보간함수를 이용해 목표 알파값으로 변경
            
            if(Mathf.Abs(sr.color.a - Alpha) <= 0.2f) //목표 알파값과의 차이가 0.2이하라면
            {
                if (Alpha == 0)     //목표 알파값이 0이었다면
                    Alpha = 1;      //1으로 바꿈
                else                //아니었다면(목표 알파값이 1이었다면)
                    Alpha = 0;      //0으로 바꿈
            }
        }
        else //최대 시간을 넘었다면
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1); //알파값을 1으로 되돌림.
            Invincibility_Timer = 0; //타이머 초기화
            Invincibility = false; //무적상태 종료
        }
    }

    void GetDamage()
    {
        if (!Invincibility) //무적상태가 아니라면
        {
            if (Enter_Enemy_Obj.CompareTag("Enemy"))
            {
                HP -= Enter_Enemy_Obj.GetComponent<Enemy>().Damage / 2; //HP를 깎음.
                Invincibility = true; //무적상태로 만듦.
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) //적의 공격에 맞았다면
        {
            if (!Invincibility) //무적상태가 아니라면
            {
                HP -= collision.GetComponent<EnemyBullet>().Damage; //HP를 깎음.
                Invincibility = true; //무적상태로 만듦.
            }
            Destroy(collision.gameObject); //피격된 적의 공격을 삭제
        }

        if (collision.CompareTag("Enemy")) //적과 직접적으로 충돌했다면
        {
            Enter_Enemy = true;
            Enter_Enemy_Obj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) //적과 직접적으로 충돌했다면
        {
            Enter_Enemy = false;
            Enter_Enemy_Obj = null;
        }
    }
}
