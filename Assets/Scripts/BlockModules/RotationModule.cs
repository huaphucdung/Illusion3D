using System;
using DG.Tweening;
using Project.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Module
{
    public sealed class RotationModule : BlockModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] float speed;
        Transform m_thisTransform;
        Vector2 m_initialDragPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_thisTransform = transform;
            m_initialDragPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 dragDelta = eventData.position - m_initialDragPosition;

            // rotating in which direction
            if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
            {
                // Horizontal drag, rotate along the Y-axis
                float rotationY = dragDelta.x * speed * Time.deltaTime;
                m_thisTransform.Rotate(0f, -rotationY, 0f, Space.World);
            }
            else
            {
                // Vertical drag, rotate along the X-axis
                float rotationX = dragDelta.y * speed * Time.deltaTime;
                m_thisTransform.Rotate(rotationX, 0f, 0f, Space.World);
            }

            m_initialDragPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 targetRotation = VectorExtensions.SnapRotation(m_thisTransform.eulerAngles);
            m_thisTransform.DORotate(targetRotation, .2f).SetEase(Ease.Linear).OnComplete(Active);
        }
    }
}