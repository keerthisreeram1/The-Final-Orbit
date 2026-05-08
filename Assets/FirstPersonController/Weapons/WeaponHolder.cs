using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;

    void Update() {
        transform.localRotation = cameraRoot.localRotation;
    }
}