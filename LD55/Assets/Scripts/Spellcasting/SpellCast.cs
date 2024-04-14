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

    public Vector2 boxSize = new Vector2(0.5f, 0.5f);
    public Vector2 boxOffset = Vector2.zero;
    private float windUpTime = 1.42f;

    private Dictionary<SpellResources.SpellType, int> values;
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
        //subtract from windup time
        windUpTime -= Time.deltaTime;
        if (windUpTime <= 0)
        {
            CheckCollisions();
            windUpTime = 1.42f;
        }
    }
    
    public void SetDamageCombination(Dictionary<SpellResources.SpellType, int> damageValues)
    {
        values = damageValues;
    }
    
    private void CheckCollisions()
    {
        Vector2 boxCenter = (Vector2)transform.position + boxOffset;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0);

        List<OrkController> hitOrks = new List<OrkController>();

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<OrkBoss>())
            {
                OrkBoss boss = hitCollider.GetComponent<OrkBoss>();
                boss.HitWithSpell(values);
                return;
            }
            if (hitCollider.GetComponent<OrkController>())
            {
                if (!hitOrks.Contains(hitCollider.GetComponent<OrkController>()))
                {
                    hitOrks.Add(hitCollider.GetComponent<OrkController>());
                }
            }
        }
        ApplyEffect(hitOrks);
    }

    private void ApplyEffect(List<OrkController> orks)
    {
        foreach (OrkController ork in orks)
        {
            HealthContainer health = ork.GetComponent<HealthContainer>();
            health.Subtract(3);
        }
    }
}
