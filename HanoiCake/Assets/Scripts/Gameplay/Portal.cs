using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool _isActivated;
    public bool IsActivated => _isActivated;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            _isActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            _isActivated = false;
        }
    }
}
