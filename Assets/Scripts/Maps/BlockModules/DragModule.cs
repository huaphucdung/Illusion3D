using DG.Tweening;
using Project.Module;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Project.Module
{
    public class DragModule : BlockModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField] private BlockGroup blockGroup;
        [SerializeField] private List<DragGroupActive> dragGroups;
        [SerializeField] private Vector3 constrainXYZ;
        [SerializeField] private float maxDrag = 5f;
        

        private Vector3 initialMousePosition;
        private Vector3 initialObjectPosition;

        private Vector3 _defaultPosition;

        private void Start()
        {
            _defaultPosition = transform.position;
            // Record the object's initial position
            initialObjectPosition = transform.position;
        }

        public override void Active()
        {
            // Record the object's initial position
            initialObjectPosition = transform.position;
            blockGroup.Active();
        }

        public override void Active(Player player) {}

        public void HandleDrag(float dragAmount, Vector3 direction)
        {
            Vector3 newPosition = initialObjectPosition + direction * dragAmount;
            if (Vector3.Distance(newPosition, _defaultPosition) > maxDrag) return;
            transform.position = newPosition;
        }

        public void EndDrag()
        {
            Vector3 currentPostion = transform.position;
            currentPostion.x = Mathf.Round(currentPostion.x);
            currentPostion.y = Mathf.Round(currentPostion.y);
            currentPostion.z = Mathf.Round(currentPostion.z);
            transform.DOMove(currentPostion, .2f).SetEase(Ease.Linear).OnComplete(Active);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            blockGroup.DisablePathAll();
            // Record the initial mouse position in world coordinates
            initialMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (blockGroup.IsLock) return;

            // Get the current mouse position in world space
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

            // Calculate the displacement between the initial and current mouse positions along the up axis
            float displacement = Vector3.Dot(currentMousePosition - initialMousePosition, constrainXYZ);

            // Move another object along its up axis by the calculated displacement
            foreach (var group in dragGroups)
            {
                group.blockDrags.HandleDrag(displacement, group.constrainXYZ);
            }
            // Move the object along its up axis by the calculated displacement
            HandleDrag(displacement, constrainXYZ);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var group in dragGroups)
            {
                group.blockDrags.EndDrag();
            }
            EndDrag();
        }

        private void OnEnable()
        {
            blockGroup.lockAction += LockDragGroup;
        }

        private void OnDisable()
        {
            blockGroup.lockAction -= LockDragGroup;
        }

        private void LockDragGroup(bool value)
        {
            foreach(var group in dragGroups)
            {
                group.blockDrags.ActiveLockGroup(value);
            }
        }

        public void ActiveLockGroup(bool value)
        {
            blockGroup.LockNotCallAction(value);
        }
    }
}

[Serializable]
public class DragGroupActive
{
    public DragModule blockDrags;
    public Vector3 constrainXYZ;
}