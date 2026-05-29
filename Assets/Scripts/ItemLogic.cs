using UnityEngine;

public class ItemLogic : ItemLogicBase
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerHit(other.gameObject);
        }
    }

    protected virtual void OnPlayerHit(GameObject player) // will change depending on the item / obstacle collided
    {
        TaggedObjectPooler.Instance.ReturnObject(gameObject, poolTag);
    }
}
