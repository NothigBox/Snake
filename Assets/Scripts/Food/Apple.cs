using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Food
{
    [SerializeField] ParticleSystem _particleSystem;

    public override void Eat()
    {
        _particleSystem.transform.localScale = transform.localScale;
        _particleSystem.transform.SetParent(null);
        _particleSystem.transform.position = transform.position;

        _particleSystem.Play();

        base.Eat();
    }
}
