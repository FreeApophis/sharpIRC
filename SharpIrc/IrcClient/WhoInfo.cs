/*
 * SharpIRC- IRC library for .NET/C# <https://github.com/FreeApophis/sharpIRC>
 */

using System;

namespace SharpIrc.IrcClient
{
    [Serializable]
    public class WhoInfo
    {
        private WhoInfo()
        {
        }

        public string Channel { get; private set; }

        public string Ident { get; private set; }

        public string Host { get; private set; }

        public string Server { get; private set; }

        public string Nick { get; private set; }

        public int HopCount { get; private set; }

        public string RealName { get; private set; }

        public bool IsAway { get; private set; }

        public bool IsOp { get; private set; }

        public bool IsVoice { get; private set; }

        public bool IsIrcOp { get; private set; }

        public bool IsRegistered { get; private set; }

        public static WhoInfo Parse(IrcMessageData data)
        {
            var whoInfo = new WhoInfo
            {
                Channel = data.RawMessageArray[3],
                Ident = data.RawMessageArray[4],
                Host = data.RawMessageArray[5],
                Server = data.RawMessageArray[6],
                Nick = data.RawMessageArray[7],
                RealName = String.Join(" ", data.MessageArray, 1, data.MessageArray.Length - 1)
            };

            // skip hop count

            int hopCount = 0;
            string hopCountString = data.MessageArray[0];
            try
            {
                hopCount = int.Parse(hopCountString);
            }
            catch (FormatException)
            {
            }

            string userMode = data.RawMessageArray[8];
            bool op = false;
            bool voice = false;
            bool ircOp = false;
            bool away = false;
            bool registered = false;

            foreach (char c in userMode)
            {
                switch (c)
                {
                    case 'H':
                        away = false;
                        break;
                    case 'G':
                        away = true;
                        break;
                    case '@':
                        op = true;
                        break;
                    case '+':
                        voice = true;
                        break;
                    case '*':
                        ircOp = true;
                        break;
                    case 'r':
                        registered = true;
                        break;
                }
            }
            whoInfo.IsAway = away;
            whoInfo.IsOp = op;
            whoInfo.IsVoice = voice;
            whoInfo.IsIrcOp = ircOp;
            whoInfo.HopCount = hopCount;
            whoInfo.IsRegistered = registered;

            return whoInfo;
        }
    }
}