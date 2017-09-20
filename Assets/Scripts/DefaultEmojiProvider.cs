using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class DefaultEmojiProvider : EmojiProvider
{
    string mAtlasPath = "";
    UIAtlas mAtlas;
    List<EmojiVO> mEmojis;
    Dictionary<int, EmojiVO> mEmojiMatchCache;
    string mLastMatchText = null;
    static DefaultEmojiProvider mInstance;
    public static DefaultEmojiProvider instance {
		get {
			if (mInstance == null) {
                mInstance = new DefaultEmojiProvider();
            }
            return mInstance;
        }
	}

	private DefaultEmojiProvider () {
        mAtlasPath = "Assets" + Path.DirectorySeparatorChar + "Atlases" + Path.DirectorySeparatorChar + "Emoji.prefab";
        mAtlas = AssetDatabase.LoadAssetAtPath<UIAtlas>(mAtlasPath);

        mEmojis = new List<EmojiVO>();
        mEmojiMatchCache = new Dictionary<int, EmojiVO>();
        LoadEmojis();

        EventDelegate.Add(mAtlas.onChange, () => { LoadEmojis(); });
    }

    private void LoadEmojis() {
		if (mAtlas == null || Utils.IsNullOrEmpty(mAtlas.spriteList))
            return;

        mEmojis.Clear();
        var spriteList = mAtlas.spriteList;
		for (int i = 0; i < spriteList.Count; i++)
		{
            BMSymbol sym = new BMSymbol() { sequence = "[#" + spriteList[i].name + "]", spriteName = spriteList[i].name };
            mEmojis.Add(new EmojiVO(sym));
        }
    }
    // public override void 
    public override List<EmojiVO> GetAllEmojis()
    {
        return mEmojis;
    }

    public virtual void CheckEmojiMatches (string text) {
        mEmojiMatchCache.Clear();
        
        var emojis = GetAllEmojis();
		if (Utils.IsNullOrEmpty(emojis))
            return;

        string regexStr = "(";
        regexStr += Regex.Escape(emojis[0].symbol.sequence);
        for (int i = 1; i < emojis.Count; i++)
        {
            regexStr += "|" + Regex.Escape(emojis[i].symbol.sequence);
        }
        regexStr += ")";
        Regex regex = new Regex(regexStr);
        var match = regex.Match(text);
        while (match.Success){
            var emoji = emojis.Find(e => string.Equals(@e.symbol.sequence, @match.Value));
            if (emoji != null && emoji.symbol != null)
                mEmojiMatchCache.Add(match.Index, emoji);

            match = match.NextMatch();
        }
    }

    public override BMSymbol MatchEmoji(string text, int offset, int length)
    {
		if (string.IsNullOrEmpty(text) || length <= 0)
            return null;

        if (!string.Equals(mLastMatchText, text)) {
            mLastMatchText = text;
            CheckEmojiMatches(text);
        }

         if (!mEmojiMatchCache.ContainsKey(offset))
            return null;

        var emoji = mEmojiMatchCache[offset];
        if (length < emoji.symbol.sequence.Length)
            return null;
            
        if (emoji.symbol.Validate(mAtlas))
            return emoji.symbol;

        return null;
    }

    public override bool HasEmojis(string text)
    {
        return !Utils.IsNullOrEmpty(mEmojis);
    }

    public override Texture mainTexture {
        get {
            if (mAtlas == null)
                return null;

            return mAtlas.texture;
        }
    }
}
