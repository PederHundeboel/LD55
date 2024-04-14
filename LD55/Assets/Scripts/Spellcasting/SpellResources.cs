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

    private void Awake()
    {
        
    }
    public void CastSpell()
    {
        var consumed = Orbs.ConsumeOrbs();
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
