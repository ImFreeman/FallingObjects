using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIService
{
    private readonly Transform _deactiveContainer;
    private readonly IInstantiator _instantiator;
    private Transform _mainCanvas;
    private Dictionary<Type, UIWindow> _windows = new Dictionary<Type, UIWindow>();

    public UIService(IInstantiator instantiator)
    {
        _instantiator = instantiator;
        _deactiveContainer = instantiator.CreateEmptyGameObject("PoolContainer").transform;
        _deactiveContainer.gameObject.SetActive(false);        
    }

    public void Init()
    {
        _mainCanvas = GameObject.Find("MainCanvas").transform;

        var windows = Resources.LoadAll<UIWindow>("UIWindows");
        foreach (var item in windows)
        {
            var window = _instantiator.InstantiatePrefabForComponent<UIWindow>(item);
            _windows.Add(item.GetType(), window);
        }
    }

    public T Show<T>() where T : UIWindow
    {
        var window = _windows[typeof(T)];

        window.transform.SetParent(_mainCanvas);
        window.transform.localScale = Vector3.one;
        window.transform.localRotation = Quaternion.identity;
        window.transform.localPosition = Vector3.zero;

        var component = window.GetComponent<T>();

        var rect = component.transform as RectTransform;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        component.Show();        
        return component;
    }

    public T Get<T>() where T : UIWindow
    {
        return _windows[typeof(T)].GetComponent<T>();
    }

    public void Hide<T>() where T : UIWindow
    {
        _windows[typeof(T)].transform.SetParent(_deactiveContainer);
    }
}
