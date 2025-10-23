using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public float EnemyAttackSpeed = 4f;
    private float NextAttack = 0f;
    public GameObject AttackTarget;

    public float DamageAmount = 5f;
    // Start is called before the first frame update

    public void ApplyDamage()
    {
        if (Time.time >= NextAttack)
        {
            AttackTarget.SendMessage("ReduceHealth", DamageAmount);
            NextAttack = Time.time + EnemyAttackSpeed;
        }
        
        return;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
