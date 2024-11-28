using UnityEngine;

namespace Project.Utilities
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class Touchable : UnityEngine.UI.Graphic
    {
        protected override void UpdateGeometry() { }
    }
}
