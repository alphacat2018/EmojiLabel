using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FilterEmojiProvider : EmojiProvider
{
    EmojiProvider mEmojiProvider;
    
	EmojiFilter mEmojiFilter;

    public FilterEmojiProvider (EmojiProvider provider, EmojiFilter filter) {
        mEmojiProvider = provider;
        mEmojiFilter = filter;
    }

    public override Texture mainTexture {
		get {
            if (mEmojiProvider != null)
                return mEmojiProvider.mainTexture;

            return null;
        }
	}

    public override List<EmojiVO> GetAllEmojis()
    {
        if (mEmojiProvider != null)
            return mEmojiProvider.GetAllEmojis();

        return null;
    }

    public override bool HasEmojis(string text)
    {
        if (mEmojiProvider != null)
            return mEmojiProvider.HasEmojis(text);

        return false;
    }

    public override BMSymbol MatchEmoji(string text, int offset, int length)
    {
        if (mEmojiProvider != null) {
            var emoji = mEmojiProvider.MatchEmoji(text, offset, length);
			if (emoji == null)
                return null;

			if (mEmojiFilter == null || mEmojiFilter(text, emoji)) {
                return emoji;
            }
        }

        return null;
    }
}
