using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTicker : MonoBehaviour
{
    [SerializeField] private int _healValuer;
    [SerializeField] private int _manValuer;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D info)
    {
         info.GetComponent<PlayerItems>().RestoreHP(_healValuer);
        info.GetComponent<PlayerItems>().ChangeMP(_manValuer);
        Destroy(gameObject);
    }
}
