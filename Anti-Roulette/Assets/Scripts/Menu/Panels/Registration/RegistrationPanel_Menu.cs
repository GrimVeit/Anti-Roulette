using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel_Menu : MoveRotate3PointFadePanel
{
    [SerializeField] private Button buttonRegistrate;
    [SerializeField] private Button buttonAvatar;

    public override void Initialize()
    {
        base.Initialize();

        buttonRegistrate.onClick.AddListener(ClickRegistration);
        buttonAvatar.onClick.AddListener(ClickAvatar);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonRegistrate.onClick.RemoveListener(ClickRegistration);
        buttonAvatar.onClick.RemoveListener(ClickAvatar);
    }

    #region Output

    public event Action OnClickRegistration;
    public event Action OnClickAvatar;

    private void ClickRegistration()
    {
        OnClickRegistration?.Invoke();
    }

    private void ClickAvatar()
    {
        OnClickAvatar?.Invoke();
    }

    #endregion
}
