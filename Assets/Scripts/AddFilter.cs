using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class AddFilter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<UILabel>().emojiProvider = new FilterEmojiProvider(DefaultEmojiProvider.instance, (text, emoji) =>
        {
			if (emoji.sequence.Contains("doge"))
                return false;

            return true;
        });
    }
}
