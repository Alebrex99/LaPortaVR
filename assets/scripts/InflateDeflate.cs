using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InflateDeflate : MonoBehaviour
{
    public float lowBound = 0.2f;
    public float upBound = 5f;
    public float speed = 1.5f;
    public InputActionReference rightPrimary;
    public InputActionReference rightSecondary;
    public InputActionReference leftPrimary;
    public InputActionReference leftSecondary;
    public XRGrabInteractable grabInteractable;
    public EffectManager effectManager;

    private float _localScale = 1f;
    
    private void Start()
    {
        effectManager.onEffectEnabled.AddListener(EnableEnabling);
    }
    private void EnableEnabling()
    {
        grabInteractable.selectEntered.AddListener(EnableScaling);
        grabInteractable.selectExited.AddListener(DisableScaling);
    }
    private void EnableScaling(SelectEnterEventArgs arg0)
    {
        if (arg0.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            XRBaseController controller = controllerInteractor.xrController;
            if (controller.CompareTag("rightHand"))
            {
                rightPrimary.action.performed += Inflate;
                rightSecondary.action.performed += Deflate;
            }
            else if (controller.CompareTag("leftHand"))
            {
                leftPrimary.action.performed += Inflate;
                leftSecondary.action.performed += Deflate;
            }
        }
    }
    private void DisableScaling(SelectExitEventArgs arg0)
    {
        if (arg0.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            XRBaseController controller = controllerInteractor.xrController;
            if (controller.CompareTag("rightHand"))
            {
                rightPrimary.action.performed -= Inflate;
                rightSecondary.action.performed -= Deflate;
            }
            else if (controller.CompareTag("leftHand"))
            {
                leftPrimary.action.performed -= Inflate;
                leftSecondary.action.performed -= Deflate;
            }
        }
    }
    private void ChangeScale(InputAction.CallbackContext obj, float multiplier)
    {
        float newScale = _localScale * multiplier;
        if (newScale > upBound || newScale < lowBound)
            return;
        gameObject.transform.localScale *= multiplier;
        _localScale = newScale;
    }
   private void Inflate(InputAction.CallbackContext obj)
    {
        Debug.Log("inflating");
        ChangeScale(obj, speed);
    }
    private void Deflate(InputAction.CallbackContext obj)
    {
        Debug.Log("deflating");
        ChangeScale(obj, 1/speed);
    }
}
