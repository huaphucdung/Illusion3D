using DG.Tweening;
using Project.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Module
{
    public sealed class RotationModule : BlockModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 m_initialMousePosition;
        private Vector3 m_initialObjectPosition;
        private Camera m_camera;
        private Transform m_thisTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_camera = Camera.main;
            m_thisTransform = transform;
            // Record the initial mouse position in world coordinates
            m_initialMousePosition = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camera.WorldToScreenPoint(m_thisTransform.position).z));

            // Record the object's initial position
            m_initialObjectPosition = m_thisTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Get the current mouse position in world space
            Vector3 currentMousePosition = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camera.WorldToScreenPoint(m_thisTransform.position).z));

            // Calculate the displacement between the initial and current mouse positions along the up axis
            Vector3 dragDirection = m_thisTransform.up;
            float displacement = Vector3.Dot(currentMousePosition - m_initialMousePosition, dragDirection);

            // Move the object along its up axis by the calculated displacement
            m_thisTransform.position = m_initialObjectPosition + dragDirection * displacement;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Active event
            Vector3 currentPostion = VectorExtensions.Round(m_thisTransform.position);
            m_thisTransform.DOMove(currentPostion, .2f).SetEase(Ease.Linear).OnComplete(Active);
        }
    }
}