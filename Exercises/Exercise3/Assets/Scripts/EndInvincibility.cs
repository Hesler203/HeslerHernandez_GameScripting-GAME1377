using UnityEngine;

public class EndInvincibility : MonoBehaviour
{
    [SerializeField] private SpaceshipController spaceshipController;
    void Start()
    {
        if (spaceshipController == null)
        {
            GetComponentInParent<SpaceshipController>();
        }
    }

    private void CallEndInvincibility()
    {
        spaceshipController.EndInvincibility();
    }

    private void CallDisableAnimatorBool(string boolName)  
    {
        spaceshipController.DisableAnimationBool(boolName);
    }
}
