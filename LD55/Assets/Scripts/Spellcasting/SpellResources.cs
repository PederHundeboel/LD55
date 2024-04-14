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

    public void CastSpell(SpellType type)
    {
        switch (type)
        {
            case SpellType.Utility:
                if (Utility.GetValue() > 0)
                {
                    Utility.Subtract(1);
                    Debug.Log("Utility spell casted");
                }
                break;
            case SpellType.Offensive:
                if (Offensive.GetValue() > 0)
                {
                    Offensive.Subtract(1);
                    Debug.Log("Offensive spell casted");
                }
                break;
            case SpellType.Defensive:
                if (Defensive.GetValue() > 0)
                {
                    Defensive.Subtract(1);
                    Debug.Log("Defensive spell casted");
                }
                break;
        }
    }
    
    public void EnhanceType(SpellType type)
    {
        //if any orbs are available, consume one and enhance the spell
        if (Orbs.HasPassiveOrb())
        {
            Orbs.ActivateOrb();
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
        }
    } 
}
