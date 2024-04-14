using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite _utilitySprite;
    [SerializeField]
    private Sprite _offensiveSprite;
    [SerializeField]
    private Sprite _defensiveSprite;
   
    public SpellResources.SpellType Type { get; private set; }
    public bool IsActive { get; private set; }
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void SetType(SpellResources.SpellType type)
    {
        Type = type;
        switch (type)
        {
            case SpellResources.SpellType.Utility:
                _spriteRenderer.sprite = _utilitySprite;
                
                IsActive = true;
                break;
            case SpellResources.SpellType.Offensive:
                _spriteRenderer.sprite = _offensiveSprite;
                IsActive = true;
                break;
            case SpellResources.SpellType.Defensive:
                _spriteRenderer.sprite = _defensiveSprite;
                IsActive = true;
                break;
            default:
                _spriteRenderer.sprite = _defaultSprite;
                IsActive = false;
                break;
        }
    }
    
    public void SetDefault()
    {
        _spriteRenderer.sprite = _defaultSprite;
        IsActive = false;
    }
}
