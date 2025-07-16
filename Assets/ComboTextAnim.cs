using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void AnimationTextCombo()
    {
        anim.SetTrigger("PlayComboTextAnim");
    }
}
