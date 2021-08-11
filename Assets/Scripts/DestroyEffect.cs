using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    private void Start() {
        Destroy(gameObject, 4.5f);
    }
}
