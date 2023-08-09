using UnityEngine;

public class PickupableContainer : MonoBehaviour
{
    private Pickupables _pickupables;
    public Pickupables Pickupables { get => _pickupables; set => _pickupables = value; }

    [SerializeField] private SpriteRenderer _iconSpirteRenderer;

    public void SetupPickupables(Pickupables pickupables)
    {
        _pickupables = pickupables;

        if (_iconSpirteRenderer != null) _iconSpirteRenderer.sprite = _pickupables.ItemSprite;
        if (_iconSpirteRenderer!= null) _iconSpirteRenderer.color = _pickupables.PickupColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_pickupables.PickupCheck.CheckPickup(collision))
            if (_pickupables.PickpupAction.PerformAction(collision)) Destroy(gameObject);
    }
}
