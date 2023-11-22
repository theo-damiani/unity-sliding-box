using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteToggle : MonoBehaviour
{
    [SerializeField] private BoolReference flag;
    [SerializeField] private Sprite sprite0;
    [SerializeField] private Sprite sprite1;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite()
    {
        if(flag.Value)
        {
            spriteRenderer.sprite = sprite1;
        }
        else
        {
            spriteRenderer.sprite = sprite0;
        }
    }
}
