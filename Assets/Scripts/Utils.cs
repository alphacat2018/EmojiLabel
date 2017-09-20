using UnityEngine;
using System.Collections;

public static class Utils {

	public static bool IsNullOrEmpty (IEnumerable e) {
		if (e == null || e.GetEnumerator() == null || !e.GetEnumerator().MoveNext())
            return true;

        return false;
    }
}
