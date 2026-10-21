using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileAvatarVisualView : View
{
    [SerializeField] private List<Sprite> avatars = new();
    [SerializeField] private Sprite spriteZero;
    [SerializeField] private List<Image> imageAvatars;

    public void SetAvatar(int index)
    {
        if(index == -1)
        {
            ApplyAvatar(spriteZero);
            return;
        }

        ApplyAvatar(avatars[index]);
    }

    private void ApplyAvatar(Sprite sprite)
    {
        imageAvatars.ForEach(a => a.sprite = sprite);
    }
}
