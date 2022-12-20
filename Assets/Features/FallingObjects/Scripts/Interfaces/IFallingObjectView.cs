using System;
using UnityEngine;

public interface IFallingObjectView
{
    public Transform BodyTransform { get; }
    public event EventHandler OnClickEvent;
    public event EventHandler EndOfScreenEvent;    
}
