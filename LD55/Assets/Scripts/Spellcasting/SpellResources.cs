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
        Defensive,
        None
    }

    public Orbs Orbs;
    public Container Utility, Offensive, Defensive;

    public AudioClip CastSound;
    
    public SpellCast SpellCast;

    private List<SpellType> _enhancedSpells;

    private void Awake()
    {
        _enhancedSpells = new List<SpellType>();
    }

    public void CastSpell()
    {
        var consumed = Orbs.ConsumeOrbs(() =>
        {
        });

        if (consumed.Count > 0)
        {
            //cast spell by instantiating a spell prefab at mouse position. keep z at 0
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var cast = Instantiate(SpellCast, mousePos, Quaternion.identity);
            cast.SetDamageCombination(_enhancedSpells);
            _enhancedSpells.Clear();

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
                    _enhancedSpells.Add(SpellType.Utility);
                    break;
                case SpellType.Offensive:
                    Offensive.Add(1);
                    _enhancedSpells.Add(SpellType.Offensive);
                    break;
                case SpellType.Defensive:
                    Defensive.Add(1);
                    _enhancedSpells.Add(SpellType.Defensive);
                    break;
            }
            
            orb.SetType(type);
        }
    } 
}
