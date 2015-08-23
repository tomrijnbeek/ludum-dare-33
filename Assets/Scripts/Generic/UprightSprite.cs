using UnityEngine;
using System.Collections;

public class UprightSprite : MonoBehaviourBase {

	void LateUpdate() {
		transform.rotation = Quaternion.identity;
	}
}
