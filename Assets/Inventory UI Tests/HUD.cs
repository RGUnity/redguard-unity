using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private InventoryUIObject _activeObjectIndicator;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Dude this HUD is running on >>Update<<");
    }

    // Update is called once per frame
    void Update()
    {
        _activeObjectIndicator.label.text = inventory.activeObject.displayName;
        _activeObjectIndicator.image.sprite = inventory.activeObject.icon;
        //_activeObjectIndicator.icon

        if (inventory.activeObject.amount > 1)
        {
            _activeObjectIndicator.amount.text = inventory.activeObject.amount.ToString();
            print(inventory.activeObject.amount.ToString());
        }
        else
        {
            {
                _activeObjectIndicator.amount.text = "";
            }
        }
    }
}
