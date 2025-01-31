using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour
{
    [SerializeField] private ScriptableRendererFeature deepRenderFeature;
    public PlayerAnimator Animator {  get; private set; }

    private EventBinding<CutSceneStartEvent> CutSceneStartEventBinding;
    private void Awake()
    {
        CutSceneStartEventBinding = new EventBinding<CutSceneStartEvent>(OnCutSceneStart);
    }

    private void OnEnable()
    {
        EventBus<CutSceneStartEvent>.Register(CutSceneStartEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CutSceneStartEvent>.Deregister(CutSceneStartEventBinding);
    }

    private void Start()
    {
        Animator = GetComponent<PlayerAnimator>();
    }

    public void ActiveDeepFeature(bool value)
    {
        deepRenderFeature.SetActive(value);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetRotation(Vector3 direction, Vector3 constrain)
    {
        transform.rotation = Quaternion.LookRotation(direction, constrain);
    }

    private void OnCutSceneStart()
    {
        SetParent(null);
    }
}

