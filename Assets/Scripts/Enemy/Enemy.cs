using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Damage = 10;
    public float Movement_Speed = 10;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * Movement_Speed); //아래로 움직임
    }
}
