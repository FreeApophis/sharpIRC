/*
 * SharpIRC- IRC library for .NET/C# <https://github.com/FreeApophis/sharpIRC>
 */

using System;

namespace SharpIrc.IrcConnection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class WriteLineEventArgs : EventArgs
    {
        internal WriteLineEventArgs(string line)
        {
            Line = line;
        }

        public string Line { get; }
    }
}