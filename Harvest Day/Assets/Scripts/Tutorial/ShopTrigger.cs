using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            TutorialController.instance.SendShopMessage();
            gameObject.SetActive(false);
        }
    }
}
