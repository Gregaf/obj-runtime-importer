using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{

    public static ReadOnlySpan<char> SliceUpTo(ref this ReadOnlySpan<char> span, char delimitter)
    {
        var offset = 1;
        var end = span.IndexOf(delimitter);
        
        if (end == -1)
        {
            end = span.Length;
            offset = 0;
        }

        var extractedSlice = span.Slice(0, end);

        span = span.Slice(end + offset, span.Length - end - offset);

        return extractedSlice;
    }

}
