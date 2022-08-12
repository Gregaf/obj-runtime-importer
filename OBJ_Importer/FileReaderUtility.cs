using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Contain static Utility methods for the FileReader class. Note that this
/// class is marked internal as it is only to be used within this assembly.
/// </summary>
[assembly: InternalsVisibleTo("Testing.ObjParsing")]
internal class FileReaderUtility
{

    /// <summary>
    /// Simply converts a string of tokens into a Vector3 parsing each float.
    /// Only a check for valid tokens length is verified, thus input must be valid.
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    internal static Vector3 ConvertVertex(string[] tokens)
    {
        var coordinates = new float[3];
        var index = 0;

        for (int i = 1; i < tokens.Length; i++)
        {
            if (string.IsNullOrEmpty(tokens[i])) continue;

            coordinates[index] = float.Parse(tokens[i]);
            index++;
        }

        return new Vector3(coordinates[0], coordinates[1], coordinates[2]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static int ParseIntFast(string token)
    {
        // These are here just for debugging purposes. May not work as well as exceptions or can be changed to Debug.Logs.
        if (string.IsNullOrEmpty(token)) throw new ArgumentException("Invalid argument passed, it is either null or empty.");
        if (token.Any(char.IsLetter)) throw new ArgumentException("Invalid argument passed, letters are not valid.");

        var num = 0;
        // Check if the number is negative.
        var modifier = token[0] == '-' ? -1 : 1;

        foreach (var c in token)
        {
            if (c == '-' || c == ' ') continue;

            num *= 10;
            num += c - '0';
        }


        return (num * modifier);
    }


    /// <summary>
    /// Converts a ReadOnlySpan of characters to an int. It handles negative and positive characters.
    /// If there are any non digit characters found within the passed Span the operation will be aborted
    /// as it is an invalid input.
    /// </summary>
    /// <remarks>
    /// I am unsure if the error checking should be left for digits as it may slow down and defeat
    /// the purpose of creating this extension method. Profiling would need to be done to check.
    /// </remarks>
    /// <param name="token"></param>
    /// <returns>An int based on the passed string.</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static int ParseIntFast(ReadOnlySpan<char> token)
    {
        // TODO: May be able to reduce garbage by not using Enumerator.
        var num = 0;

        // Check if the number is negative.
        var modifier = token[0] == '-' ? -1 : 1;

        token = token.Trim();

        var enumerator = token.GetEnumerator();

        // Skip the negative sign.
        if (modifier == -1)
            enumerator.MoveNext();

        while (enumerator.MoveNext())
        {
            var c = enumerator.Current;

            if (!char.IsDigit(c)) throw new ArgumentException("Invalid Input, letters can not be converted to numbers!");

            num *= 10;
            num += c - '0';
        }
 
        return (num * modifier);
    }

    internal static int[] ParseFace(ReadOnlySpan<char> span, int vertCount)
    {
        // Possible Input layouts
        // 1/1/1 2/2/2 3/3/3
        // 1/1 2/2 3/3
        // 1//1 2//2 3//3

        int[] values = new int[3];

        // This will represent how many spaces to skip based on the delim.
        int offset = 1;

        // Remove any whitespaces from each end.
        span = span.Trim();
        bool exit = false;
        bool vAndVn = false;
        var index = 0;

        while (index < 3)
        {
            var end = span.IndexOf('/');

            if (end == -1)
            {
                end = span.Length;
                offset = 0;
                exit = true;
            }
            // There is a double slash.
            else if (end + 1 < span.Length && span[end + 1] == '/')
            {
                offset = 2;
                vAndVn = true;
            }

            var num = span.Slice(0, end);

            if (num.Length == 0)
                break;

            var parsedNum = FileReaderUtility.ParseIntFast(num);

            // Wrap the index face around.
            if (parsedNum < 0)
                parsedNum = vertCount + parsedNum;

            values[index] = (parsedNum);

            index++;
            span = span.Slice(end + offset, span.Length - end - offset);

            if (exit)
                break;
        }

        // Swap numbers because / layout.
        if (vAndVn)
        {
            values[2] = values[1];
            values[1] = 0;
        }

        return values;
    }

    //TODO: Clean up code, would like to create better way of avoiding the space.
    // dont like the current workaround. But it works.
    internal static Vector3 ParseVertex(ReadOnlySpan<char> currentSpan, int count = 3)
    {

        int vertexIndex = 0;
        var values = new float[3];

        // This should ensure there are no trailing or leading whitespace.
        currentSpan = currentSpan.Trim();

        while (vertexIndex < count)
        {
            int space = 1;
            var end = currentSpan.IndexOf(' ');

            // That means that it did not encounter a space
            // There is nothing left to delimit, thus take the whole line.
            if (end == -1)
            {
                end = currentSpan.Length;
                space = 0;
            }

            try
            {
                var part = currentSpan.Slice(0, end);
                values[vertexIndex] = float.Parse(part, System.Globalization.NumberStyles.Float);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }

            vertexIndex++;
            currentSpan = currentSpan.Slice(end + space, currentSpan.Length - end - space);
        }

        return new Vector3(values[0], values[1], values[2]);
    }

    /// <summary>
    /// Check whether a filePath extension matches the target file extension.
    /// </summary>
    /// <remarks>Note that this method does not verify whether the path is valid.</remarks>
    /// <param name="path"></param>
    /// <param name="extension"></param>
    /// <returns>True if there is a match or false otherwise.</returns>
    internal static bool FileExtensionMatches(string path, string extension)
    {

        var extractedExtension = Path.GetExtension(@path.AsSpan());
        
        if (extractedExtension.IsEmpty || extractedExtension.SequenceEqual("".AsSpan())) return false;

        if (!extractedExtension.SequenceEqual(extension.AsSpan()))
        {
            Debug.LogError($"The passed file's extension '{extractedExtension.ToString()}' is not the specified extension '{extension}'");
            return false;
        }
        
        return true;
    }

}
