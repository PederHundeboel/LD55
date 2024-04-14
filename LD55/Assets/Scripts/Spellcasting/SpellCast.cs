using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpellCast : MonoBehaviour
{
    //animationcontroller
    public Animator _animator;
    public AudioClip clip;
    public Light2D _light2D;
    private static readonly int CastSpell = Animator.StringToHash("CastSpell");


    public void Start()
    {
        _animator.SetTrigger(CastSpell);
        AudioController.Instance.PlayOneShotAudioClip(clip, transform.position);
    }
    
    public void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
}
