using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellResources : MonoBehaviour
{
    public enum SpellType
    {
        Utility,
        Offensive,
        Defensive
    }

    public Orbs Orbs;
    public Container Utility, Offensive, Defensive;

    public AudioClip CastSound;
    
    public SpellCast SpellCast;

    public void CastSpell()
    {
        var consumed = Orbs.ConsumeOrbs(() =>
        {
            //play sound
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = CastSound;
            // audioSource.Play();
        });

        if (consumed.Count > 0)
        {
            //cast spell by instantiating a spell prefab at mouse position. keep z at 0
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Instantiate(SpellCast, mousePos, Quaternion.identity);
            

        }
        
    }
    
    public void EnhanceType(SpellType type)
    {
        //if any orbs are available, consume one and enhance the spell
        if (Orbs.HasPassiveOrb())
        {
            var orb = Orbs.GetPassiveOrb();
            switch (type)
            {
                case SpellType.Utility:
                    Utility.Add(1);
                    break;
                case SpellType.Offensive:
                    Offensive.Add(1);
                    break;
                case SpellType.Defensive:
                    Defensive.Add(1);
                    break;
            }
            
            orb.SetType(type);
        }
    } 
}
