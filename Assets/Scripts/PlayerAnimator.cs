using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator m_animator;

    public int MovingProperty {  get; private set; }
    public int NormalingKey { get; private set; }
    public int ClimbingUpKey { get; private set; }
    public int ClimbingDownKey { get; private set; }

    private int m_movingTargetValue;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        MovingProperty = Animator.StringToHash("Moving");

        NormalingKey = Animator.StringToHash("Normaling");
        ClimbingUpKey = Animator.StringToHash("ClimbingUp");
        ClimbingDownKey = Animator.StringToHash("ClimbingDown");
    }

    private void Update()
    {
        m_animator.SetFloat(MovingProperty, m_movingTargetValue, 0.1f, Time.deltaTime);
    }


    public void ChangeMoving(int value)
    {
        m_movingTargetValue = value;
    }

    public void ChangeState(int key)
    {
        m_animator.CrossFade(key, GameManager.GameData.StateChangeDuriation);
    }
}
