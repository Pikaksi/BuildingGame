using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderScript : MonoBehaviour
{
    public bool OnGround = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ground") {
            OnGround = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ground") {
            OnGround = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Ground") {
            OnGround = true;
        }
    }
}
