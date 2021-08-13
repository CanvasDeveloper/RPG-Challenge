using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField]private string tutoString;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            UIController.Instance.OpenTutorialPanel(tutoString);
            Destroy(gameObject);
        }
    }
}
