using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this component on an object,
/// when selected, a green button will apear, clic on it for playing the animation
/// </summary>
[ExecuteInEditMode]
public class AnimatorGUI : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator = gameObject.GetComponent<Animator>();
    }
}
