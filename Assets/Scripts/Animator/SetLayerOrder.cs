using UnityEngine;

public class SetLayerOrder : StateMachineBehaviour
{
    [SerializeField] private int OrderInLayer;

    private SpriteRenderer spriteRenderer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (spriteRenderer == null) spriteRenderer = animator.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = OrderInLayer;
    }
}
