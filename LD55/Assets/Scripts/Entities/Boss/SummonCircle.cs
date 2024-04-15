using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    private static readonly int CastSpell = Animator.StringToHash("CastSpell");
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger(CastSpell);
        
        Destroy(gameObject, 1.3f);
    }

}
