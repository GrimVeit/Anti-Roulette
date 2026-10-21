using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AuthenticationLoadStatusModel
{
    public void Load()
    {
        OnLoad?.Invoke();
    }

    public void Show()
    {
        OnShow?.Invoke();
    }

    #region Output

    public event Action OnLoad;
    public event Action OnShow;

    #endregion
}
