using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    Toggle _toggle;
    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();

        if (AudioListener.volume == 0)
        {
            _toggle.isOn = false;
        }

        AudioListener.volume = 0;
    }
    public void ToggleAudioOnValueChange(bool audioIn)
    {
        if (audioIn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
