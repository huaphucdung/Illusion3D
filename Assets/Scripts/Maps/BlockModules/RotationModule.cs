using DG.Tweening;
using Project.Utilities;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


public sealed class RotationModule : BlockModule, ITransformBlock, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<TransformGroupActive> dragGroups;
    [SerializeField] private Vector3 constrainXYZ;
    /* [SerializeField] float speed;
     Transform m_thisTransform;
     Vector2 m_initialDragPosition;*/

    [Header("Litmit Settings:")]
    [SerializeField] private bool isLimit;
    [SerializeField] private Vector3 objectDirection;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float maxAngle;

    [Inject] GameManager _gameManager;
    private BlockGroup _blockGroup;
    private Vector3 initialMousePosition;

    private float _cooldownTime;

    private void Start()
    {
        _blockGroup = GetComponent<BlockGroup>();
    }

    public override void Active()
    {
        _blockGroup?.Active();
    }
    public override void Active(Player player) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_cooldownTime > Time.time) return;
        /*m_thisTransform = transform;
        m_initialDragPosition = eventData.position;*/



        // Record the initial mouse position in world coordinates
        DisablePathAll();
        foreach (var group in dragGroups)
        {
            group.transformBlock.Value.DisablePathAll();
        }

        initialMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_cooldownTime > Time.time) return;
        if (_blockGroup != null && _blockGroup.IsLock) return;

        /*Vector2 dragDelta = eventData.position - m_initialDragPosition;

        // rotating in which direction
        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            // Horizontal drag, rotate along the Y-axis
            float rotationY = dragDelta.x * speed * Time.deltaTime * constrainXY.y;
            m_thisTransform.Rotate(0f, -rotationY, 0f, Space.World);
        }
        else
        {
            // Vertical drag, rotate along the X-axis
            float rotationX = dragDelta.y * speed * Time.deltaTime * constrainXY.x;
            m_thisTransform.Rotate(rotationX, 0f, 0f, Space.World);
        }

        m_initialDragPosition = eventData.position;*/

        // Get the current mouse position in world space
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // Calculate the displacement between the initial and current mouse positions along the up axis
        float displacement = Vector3.Dot(currentMousePosition - initialMousePosition, constrainXYZ);

        // Ration the object along its up axis by the calculated displacement
        //initialObjectPosition = Quaternion.Euler(constrainXYZ * displacement);

        foreach (var group in dragGroups)
        {
            group.transformBlock.Value.Handle(displacement, group.constrainXYZ);
        }

        Handle(displacement, constrainXYZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cooldownTime = Time.time + 1;
        foreach (var group in dragGroups)
        {
            group.transformBlock.Value.End();
        }
        End();
    }

    public void Handle(float dragAmount, Vector3 direciotn)
    {
        Vector3 rotationResult = direciotn * dragAmount;
        rotationResult.x = Mathf.Round(rotationResult.x);
        rotationResult.y = Mathf.Round(rotationResult.y);
        rotationResult.z = Mathf.Round(rotationResult.z);
        Quaternion quaternion = Quaternion.Euler(rotationResult);
        Quaternion rotation = transform.rotation;

        rotation *= Quaternion.Inverse(rotation) * quaternion * rotation;


        if (isLimit)
        {
            Vector3 forward = (rotation * objectDirection.normalized).normalized;
            if (Vector3.Angle(forward, direction.normalized) > maxAngle) return;
        }
        transform.rotation = rotation;
    }

    public void End()
    {
        Vector3 targetRotation = VectorExtensions.SnapRotation(transform.eulerAngles);
        transform.DORotate(targetRotation, .2f).SetEase(Ease.Linear).OnComplete(Active);
    }

    public void ActiveLockGroup(bool value)
    {
        _blockGroup?.LockNotCallAction(value);
    }

    private void OnEnable()
    {
        if (_blockGroup == null) return;
        _blockGroup.lockAction += LockDragGroup;
    }

    private void OnDisable()
    {
        if (_blockGroup == null) return;
        _blockGroup.lockAction -= LockDragGroup;
    }

    private void LockDragGroup(bool value)
    {
        foreach (var group in dragGroups)
        {
            group.transformBlock.Value.ActiveLockGroup(value);
        }
    }

    public void DisablePathAll()
    {
        _blockGroup?.DisablePathAll();
    }

    private float NormalAngle(float angle)
    {
        if (angle < 0) angle += 360;
        return angle % 360;
    }
}
