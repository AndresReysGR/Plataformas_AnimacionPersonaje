using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    int health = 4;

    [SerializeField, Range(1, 10)]
    int damage = 1;
    protected int Health => health;
    protected int Damage => damage;

    public void GetDamage(int damage) => health = health - damage > 0 ? health - damage : 0;

}
