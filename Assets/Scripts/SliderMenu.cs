using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenu : MonoBehaviour
{
    Animator _animator;
    public void ShowHideMenu()
    {
        _animator = GetComponent<Animator>();

        if (_animator != null)
        {
            bool isOpen = _animator.GetBool("showSideMenu");
            _animator.SetBool("showSideMenu", !isOpen);
        }
    
    }
}
