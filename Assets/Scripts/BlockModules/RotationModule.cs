using DG.Tweening;
using Project.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Module
{
    public sealed class RotationModule : BlockModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private BlockGroup blockGround;
        [SerializeField] private Vector3 constrainXYZ;
       /* [SerializeField] float speed;
        Transform m_thisTransform;
        Vector2 m_initialDragPosition;*/

        private Vector3 initialMousePosition;

        public override void Active()
        {
            blockGround.Active();
        }
        public override void Active(Player player) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            /*m_thisTransform = transform;
            m_initialDragPosition = eventData.position;*/

            // Record the initial mouse position in world coordinates
            initialMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (blockGround.IsLock) return;

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
            transform.Rotate(constrainXYZ * displacement, Space.World);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 targetRotation = VectorExtensions.SnapRotation(transform.eulerAngles);
            transform.DORotate(targetRotation, .2f).SetEase(Ease.Linear).OnComplete(Active);
        }
    }
}