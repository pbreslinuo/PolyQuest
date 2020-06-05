﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{ // from https://youtu.be/MjH5rsmYmQY
    public AudioSource source;
    public AudioClip hover;
    public AudioClip click;

    public void HoverSound()
    {
        source.PlayOneShot(hover);
    }

    public void ClickSound()
    {
        source.PlayOneShot(click);
    }
}
