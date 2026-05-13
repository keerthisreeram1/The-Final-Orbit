using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float objectRotationSpeed = 100f;
    const string PLAYER_STRING = "Player";
    bool pickedUp = false;

    private void Update()
    {
        transform.Rotate(0f, objectRotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;
        if (other.CompareTag(PLAYER_STRING))
        {
            pickedUp = true;
            OnPickup(other.transform.root);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(Transform playerRoot);
}