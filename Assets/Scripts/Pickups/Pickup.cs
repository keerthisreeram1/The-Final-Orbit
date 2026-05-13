using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float objectRotationSpeed = 100f;
    const string PLAYER_STRING = "Player";

    private void Update()
    {
        transform.Rotate(0f, objectRotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            OnPickup(other.transform.root);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(Transform playerRoot);
}