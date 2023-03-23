using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class TextSimilarityAnalyzer
{
    public bool IsSimilar(string target, string actual)
    {
        string regexTarget = Regex.Replace(target, @"[^\w\s]", string.Empty);
        string regexActual = Regex.Replace(actual, @"[^\w\s]", string.Empty);

        return string.Equals(regexActual, regexTarget, StringComparison.OrdinalIgnoreCase);
    }
}
