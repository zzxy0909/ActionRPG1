using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineID {
    // Animator.SetTrigger 사용
    public const string Ani_idle = "idle";
    public const string Ani_avoid = "avoid";
    public const string Ani_run = "run";
    public const string Ani_attack = "attack";
    public const string Ani_attack_start = "attack_start";
    public const string Ani_dash = "dash";

    public const int Max_LinkAttackCount = 3;
    public const int Num_StartLinkAttack = 90;
    public const int Max_autoDashCheckValue = 10;
}
