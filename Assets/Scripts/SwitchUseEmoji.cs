using UnityEngine;
using System.Collections;

public class SwitchUseEmoji : MonoBehaviour {

    public UILabel label;
    public void OnClick () {
        label.useEmojis = !label.useEmojis;
    }
}
