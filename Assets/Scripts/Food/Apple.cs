using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Food
{
    [SerializeField] ParticleSystem _particleSystem;

    public override void Eat()
    {
        _particleSystem.Play();
        _particleSystem.transform.SetParent(null);
        _particleSystem.transform.position = transform.position;

        base.Eat();
    }
}
