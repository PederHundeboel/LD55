using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpellCast : MonoBehaviour
{
    //animationcontroller
    public Animator _animator;
    public Light2D _light2D;
    private static readonly int CastSpell = Animator.StringToHash("CastSpell");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _light2D = GetComponent<Light2D>();
    }

    public void Start()
    {
        _animator.SetTrigger(CastSpell);
    }
}
