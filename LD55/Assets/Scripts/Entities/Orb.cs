using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
   
    [Header("Lighting")]
    
    [SerializeField]
    private Color _greenColor;
    [SerializeField]
    private Color _blueColor;
    [SerializeField]
    private Color _redColor;
    
    public float InactiveBrightness { get; set; } = 0.5f;

    public float ActiveBrightness { get; set; } = 1.5f;
    
    public SpellResources.SpellType Type { get; private set; }
    public bool IsActive { get; private set; }
    
    private SpriteRenderer _spriteRenderer;
    private Light2D _light2D;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light2D = GetComponent<Light2D>();
        ColorUtility.TryParseHtmlString("#75a743", out _greenColor);
        ColorUtility.TryParseHtmlString("#4f8fba", out _blueColor);
        ColorUtility.TryParseHtmlString("#a53030", out _redColor);

    }
    
    public void SetType(SpellResources.SpellType type)
    {
        Type = type;
        //set strength of light if type is not default
        switch (type)
        {
            case SpellResources.SpellType.Utility:
                _spriteRenderer.sprite = _utilitySprite;
                _light2D.color = _greenColor;
                _light2D.intensity = ActiveBrightness;
                IsActive = true;
                break;
            case SpellResources.SpellType.Offensive:
                _spriteRenderer.sprite = _offensiveSprite;
                _light2D.color = _redColor;
                _light2D.intensity = ActiveBrightness;
                IsActive = true;
                break;
            case SpellResources.SpellType.Defensive:
                _spriteRenderer.sprite = _defensiveSprite;
                _light2D.color = _blueColor;
                _light2D.intensity = ActiveBrightness;
                IsActive = true;
                break;
            default:
                _spriteRenderer.sprite = _defaultSprite;
                _light2D.color = Color.white;
                _light2D.intensity = InactiveBrightness;
                IsActive = false;
                break;
        }
    }

   

    public void SetDefault()
    {
        _spriteRenderer.sprite = _defaultSprite;
        _light2D.color = Color.white;
        IsActive = false;
    }
}
