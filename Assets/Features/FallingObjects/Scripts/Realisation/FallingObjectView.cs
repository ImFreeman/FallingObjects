using System;
using UnityEngine;
using Zenject;

public readonly struct FallingObjectViewProtocol
{
    public readonly Sprite Sprite;
    public readonly Vector3 Position;
    public FallingObjectViewProtocol(Sprite sprite, Vector3 position = default) : this()
    {
        Sprite = sprite;
        Position = position;
    }    
}

public class FallingObjectView : MonoBehaviour, IFallingObjectView
{
    public Transform BodyTransform => bodyTransform;
    public event EventHandler OnClickEvent;
    public event EventHandler EndOfScreenEvent;

    [SerializeField] private Transform bodyTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;    

    private void Init(FallingObjectViewProtocol protocol)
    {
        spriteRenderer.sprite = protocol.Sprite;
        bodyTransform.position = protocol.Position;
    }

    private void OnMouseDown()
    {
        OnClickEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EndOfLevel"))
        {
            EndOfScreenEvent?.Invoke(this, EventArgs.Empty);
        }
    }    

    public class Pool : MonoMemoryPool<FallingObjectViewProtocol, FallingObjectView>
    {
        protected override void Reinitialize(FallingObjectViewProtocol p1, FallingObjectView item)
        {
            item.Init(p1);
        }
    }
}
