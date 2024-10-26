using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Project.Module
{
    public class DragModule : BlockModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private List<BlockGroup> groups;
        [SerializeField] private Vector3 constrainXYZ;
        private Vector3 initialMousePosition;
        private Vector3 initialObjectPosition;

        public override void Active()
        {
            foreach (var group in groups)
            {
                group.Active();
            }
        }

        public override void Active(Player player) {}

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Record the initial mouse position in world coordinates
            initialMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

            // Record the object's initial position
            initialObjectPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Get the current mouse position in world space
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

            // Calculate the displacement between the initial and current mouse positions along the up axis
            float displacement = Vector3.Dot(currentMousePosition - initialMousePosition, constrainXYZ);

            // Move the object along its up axis by the calculated displacement
            transform.position = initialObjectPosition + constrainXYZ * displacement;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Active event
            Vector3 currentPostion = transform.position;
            currentPostion.x = Mathf.Round(currentPostion.x);
            currentPostion.y = Mathf.Round(currentPostion.y);
            currentPostion.z = Mathf.Round(currentPostion.z);
            transform.DOMove(currentPostion, .2f).SetEase(Ease.Linear).OnComplete(Active);
        }
    }
}