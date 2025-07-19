using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUIComponent { }

public interface IUIComponent : IBaseUIComponent
{
    void Setup();
}

public interface IUIComponent<T> : IBaseUIComponent
{
    void Setup(T data);
}