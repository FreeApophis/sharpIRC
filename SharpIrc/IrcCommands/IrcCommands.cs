/*
 * SharpIRC- IRC library for .NET/C# <https://github.com/FreeApophis/sharpIRC>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpIrc.IrcClient;

namespace SharpIrc.IrcCommands
{
    /// <summary>
    ///
    /// </summary>
    /// <threadsafety static="true" instance="true" />
    public class IrcCommands : IrcConnection.IrcConnection
    {
        public IrcCommands()
        {
            MaxModeChanges = 3;
        }

        protected int MaxModeChanges { get; set; }

        // API commands
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void SendMessage(SendType type, string destination, string message, Priority priority)
        {
            switch (type)
            {
                case SendType.Message:
                    RfcPrivmsg(destination, message, priority);
                    break;
                case SendType.Action:
                    RfcPrivmsg(destination, "\x1" + "ACTION " + message + "\x1", priority);
                    break;
                case SendType.Notice:
                    RfcNotice(destination, message, priority);
                    break;
                case SendType.CtcpRequest:
                    RfcPrivmsg(destination, "\x1" + message + "\x1", priority);
                    break;
                case SendType.CtcpReply:
                    RfcNotice(destination, "\x1" + message + "\x1", priority);
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        public void SendMessage(SendType type, string destination, string message)
        {
            SendMessage(type, destination, message, Priority.Medium);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void SendReply(IrcMessageData data, string message, Priority priority)
        {
            switch (data.Type)
            {
                case ReceiveType.ChannelMessage:
                    SendMessage(SendType.Message, data.Channel, message, priority);
                    break;
                case ReceiveType.QueryMessage:
                    SendMessage(SendType.Message, data.Nick, message, priority);
                    break;
                case ReceiveType.QueryNotice:
                    SendMessage(SendType.Notice, data.Nick, message, priority);
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public void SendReply(IrcMessageData data, string message)
        {
            SendReply(data, message, Priority.Medium);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void Op(string channel, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "+o " + nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Op(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "+o").ToArray(), nicknames);
        }

        public void Op(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "+o " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void Deop(string channel, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "-o " + nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void Deop(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "-o " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Deop(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "-o").ToArray(), nicknames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void Voice(string channel, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "+v " + nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void Voice(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "+v " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Voice(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "+v").ToArray(), nicknames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void Devoice(string channel, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "-v " + nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void Devoice(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "-v " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Devoice(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "-v").ToArray(), nicknames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void Ban(string channel, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "+b"), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void Ban(string channel)
        {
            WriteLine(Rfc2812.Mode(channel, "+b"));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMask"></param>
        /// <param name="priority"></param>
        public void Ban(string channel, string hostMask, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "+b " + hostMask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMask"></param>
        public void Ban(string channel, string hostMask)
        {
            WriteLine(Rfc2812.Mode(channel, "+b " + hostMask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMasks"></param>
        public void Ban(string channel, string[] hostMasks)
        {
            if (hostMasks == null)
            {
                throw new ArgumentNullException(nameof(hostMasks));
            }

            Mode(channel, hostMasks.Select(n => "+b").ToArray(), hostMasks);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMask"></param>
        /// <param name="priority"></param>
        public void Unban(string channel, string hostMask, Priority priority)
        {
            WriteLine(Rfc2812.Mode(channel, "-b " + hostMask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMask"></param>
        public void Unban(string channel, string hostMask)
        {
            WriteLine(Rfc2812.Mode(channel, "-b " + hostMask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="hostMasks"></param>
        public void Unban(string channel, string[] hostMasks)
        {
            if (hostMasks == null)
            {
                throw new ArgumentNullException(nameof(hostMasks));
            }

            Mode(channel, hostMasks.Select(n => "-b").ToArray(), hostMasks);
        }

        // non-RFC commands
        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void Halfop(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "+h " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Halfop(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "+h").ToArray(), nicknames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void Dehalfop(string channel, string nickname)
        {
            WriteLine(Rfc2812.Mode(channel, "-h " + nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void Dehalfop(string channel, string[] nicknames)
        {
            if (nicknames == null)
            {
                throw new ArgumentNullException(nameof(nicknames));
            }

            Mode(channel, nicknames.Select(n => "-h").ToArray(), nicknames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newModes"></param>
        /// <param name="newModeParameters"></param>
        public void Mode(string target, string[] newModes, string[] newModeParameters)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (newModes == null)
            {
                throw new ArgumentNullException(nameof(newModes));
            }
            if (newModeParameters == null)
            {
                throw new ArgumentNullException(nameof(newModeParameters));
            }
            if (newModes.Length == 0)
            {
                throw new ArgumentException("newModes must not be empty.", nameof(newModes));
            }
            if (newModeParameters.Length == 0)
            {
                throw new ArgumentException("newModeParameters must not be empty.", "newModeParameters");
            }
            if (newModes.Length != newModeParameters.Length)
            {
                throw new ArgumentException("newModes and newModeParameters must have the same size.", "newModes");
            }

            int maxModeChanges = MaxModeChanges;
            for (int i = 0; i < newModes.Length; i += maxModeChanges)
            {
                var newModeChunks = new List<string>(maxModeChanges);
                var newModeParameterChunks = new List<string>(maxModeChanges);
                for (int j = 0; j < maxModeChanges; j++)
                {
                    if (i + j >= newModes.Length)
                    {
                        break;
                    }
                    newModeChunks.Add(newModes[i + j]);
                    newModeParameterChunks.Add(newModeParameters[i + j]);
                }
                WriteLine(Rfc2812.Mode(target, newModeChunks.ToArray(), newModeParameterChunks.ToArray()));
            }
        }

        #region Client capability commands

        public enum CapabilitySubCommand
        {
            LS,
            LIST,
            REQ,
            CLEAR,
            END
        }

        public void Cap(CapabilitySubCommand subCmd, Priority priority)
        {
            WriteLine("CAP " + subCmd, priority);
        }

        public void CapReq(string[] caps, Priority priority)
        {
            if (caps.Length == 0)
                throw new ArgumentException("Capability list must not be empty");

            var sb = new StringBuilder("CAP REQ ");
            string ch = ":";
            foreach (string cap in caps)
            {
                sb.Append(ch);
                sb.Append(cap);
                ch = " ";
            }
            WriteLine(sb.ToString(), priority);
        }

        #endregion Client capability commands

        public enum SaslAuthMethod
        {
            Plain,
            DiffieHellmanBlowfish
        }

        public void Authenticate(SaslAuthMethod method, Priority priority)
        {
            switch (method)
            {
                case SaslAuthMethod.Plain:
                    WriteLine("AUTHENTICATE PLAIN", priority);
                    break;

                //                case SaslAuthMethod.DiffieHellmanBlowfish:
                //                    WriteLine ("AUTHENTICATE DH-BLOWFISH", priority);
                //                    break;

                default:
                    throw new ArgumentException("Unsupported SASL authentication method");
            }
        }

        public void Authenticate(string authData, Priority priority)
        {
            int len = authData.Length;

            for (int i = 0; i < len / 400; i++)
            {
                WriteLine("AUTHENTICATE " + authData.Substring(400 * i, 400));
            }

            if (len % 400 > 0)
                WriteLine("AUTHENTICATE " + authData.Substring(len - len % 400), priority);
        }

        #region RFC commands

        /// <summary>
        ///
        /// </summary>
        /// <param name="password"></param>
        /// <param name="priority"></param>
        public void RfcPass(string password, Priority priority)
        {
            WriteLine(Rfc2812.Pass(password), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="password"></param>
        public void RfcPass(string password)
        {
            WriteLine(Rfc2812.Pass(password));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userMode"></param>
        /// <param name="realName"></param>
        /// <param name="priority"></param>
        public void RfcUser(string username, int userMode, string realName, Priority priority)
        {
            WriteLine(Rfc2812.User(username, userMode, realName), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userMode"></param>
        /// <param name="realName"></param>
        public void RfcUser(string username, int userMode, string realName)
        {
            WriteLine(Rfc2812.User(username, userMode, realName));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="priority"></param>
        public void RfcOper(string name, string password, Priority priority)
        {
            WriteLine(Rfc2812.Oper(name, password), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        public void RfcOper(string name, string password)
        {
            WriteLine(Rfc2812.Oper(name, password));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void RfcPrivmsg(string destination, string message, Priority priority)
        {
            WriteLine(Rfc2812.Privmsg(destination, message), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        public void RfcPrivmsg(string destination, string message)
        {
            WriteLine(Rfc2812.Privmsg(destination, message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void RfcNotice(string destination, string message, Priority priority)
        {
            WriteLine(Rfc2812.Notice(destination, message), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="message"></param>
        public void RfcNotice(string destination, string message)
        {
            WriteLine(Rfc2812.Notice(destination, message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcJoin(string channel, Priority priority)
        {
            WriteLine(Rfc2812.Join(channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void RfcJoin(string channel)
        {
            WriteLine(Rfc2812.Join(channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="priority"></param>
        public void RfcJoin(string[] channels, Priority priority)
        {
            WriteLine(Rfc2812.Join(channels), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        public void RfcJoin(string[] channels)
        {
            WriteLine(Rfc2812.Join(channels));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="key"></param>
        /// <param name="priority"></param>
        public void RfcJoin(string channel, string key, Priority priority)
        {
            WriteLine(Rfc2812.Join(channel, key), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="key"></param>
        public void RfcJoin(string channel, string key)
        {
            WriteLine(Rfc2812.Join(channel, key));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="keys"></param>
        /// <param name="priority"></param>
        public void RfcJoin(string[] channels, string[] keys, Priority priority)
        {
            WriteLine(Rfc2812.Join(channels, keys), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="keys"></param>
        public void RfcJoin(string[] channels, string[] keys)
        {
            WriteLine(Rfc2812.Join(channels, keys));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcPart(string channel, Priority priority)
        {
            WriteLine(Rfc2812.Part(channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void RfcPart(string channel)
        {
            WriteLine(Rfc2812.Part(channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="priority"></param>
        public void RfcPart(string[] channels, Priority priority)
        {
            WriteLine(Rfc2812.Part(channels), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        public void RfcPart(string[] channels)
        {
            WriteLine(Rfc2812.Part(channels));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="partMessage"></param>
        /// <param name="priority"></param>
        public void RfcPart(string channel, string partMessage, Priority priority)
        {
            WriteLine(Rfc2812.Part(channel, partMessage), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="partMessage"></param>
        public void RfcPart(string channel, string partMessage)
        {
            WriteLine(Rfc2812.Part(channel, partMessage));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="partMessage"></param>
        /// <param name="priority"></param>
        public void RfcPart(string[] channels, string partMessage, Priority priority)
        {
            WriteLine(Rfc2812.Part(channels, partMessage), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="partMessage"></param>
        public void RfcPart(string[] channels, string partMessage)
        {
            WriteLine(Rfc2812.Part(channels, partMessage));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void RfcKick(string channel, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channel, nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        public void RfcKick(string channel, string nickname)
        {
            WriteLine(Rfc2812.Kick(channel, nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void RfcKick(string[] channels, string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channels, nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nickname"></param>
        public void RfcKick(string[] channels, string nickname)
        {
            WriteLine(Rfc2812.Kick(channels, nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        /// <param name="priority"></param>
        public void RfcKick(string channel, string[] nicknames, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channel, nicknames), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        public void RfcKick(string channel, string[] nicknames)
        {
            WriteLine(Rfc2812.Kick(channel, nicknames));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nicknames"></param>
        /// <param name="priority"></param>
        public void RfcKick(string[] channels, string[] nicknames, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channels, nicknames), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nicknames"></param>
        public void RfcKick(string[] channels, string[] nicknames)
        {
            WriteLine(Rfc2812.Kick(channels, nicknames));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcKick(string channel, string nickname, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channel, nickname, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        public void RfcKick(string channel, string nickname, string comment)
        {
            WriteLine(Rfc2812.Kick(channel, nickname, comment));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcKick(string[] channels, string nickname, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channels, nickname, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        public void RfcKick(string[] channels, string nickname, string comment)
        {
            WriteLine(Rfc2812.Kick(channels, nickname, comment));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcKick(string channel, string[] nicknames, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channel, nicknames, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nicknames"></param>
        /// <param name="comment"></param>
        public void RfcKick(string channel, string[] nicknames, string comment)
        {
            WriteLine(Rfc2812.Kick(channel, nicknames, comment));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nicknames"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcKick(string[] channels, string[] nicknames, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Kick(channels, nicknames, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="nicknames"></param>
        /// <param name="comment"></param>
        public void RfcKick(string[] channels, string[] nicknames, string comment)
        {
            WriteLine(Rfc2812.Kick(channels, nicknames, comment));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcMotd(Priority priority)
        {
            WriteLine(Rfc2812.Motd(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcMotd()
        {
            WriteLine(Rfc2812.Motd());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcMotd(string target, Priority priority)
        {
            WriteLine(Rfc2812.Motd(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcMotd(string target)
        {
            WriteLine(Rfc2812.Motd(target));
        }

        public void RfcLusers(Priority priority)
        {
            WriteLine(Rfc2812.Lusers(), priority);
        }

        public void RfcLusers()
        {
            WriteLine(Rfc2812.Lusers());
        }

        public void RfcLusers(string mask, Priority priority)
        {
            WriteLine(Rfc2812.Lusers(mask), priority);
        }

        public void RfcLusers(string mask)
        {
            WriteLine(Rfc2812.Lusers(mask));
        }

        public void RfcLusers(string mask, string target, Priority priority)
        {
            WriteLine(Rfc2812.Lusers(mask, target), priority);
        }

        public void RfcLusers(string mask, string target)
        {
            WriteLine(Rfc2812.Lusers(mask, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcVersion(Priority priority)
        {
            WriteLine(Rfc2812.Version(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcVersion()
        {
            WriteLine(Rfc2812.Version());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcVersion(string target, Priority priority)
        {
            WriteLine(Rfc2812.Version(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcVersion(string target)
        {
            WriteLine(Rfc2812.Version(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcStats(Priority priority)
        {
            WriteLine(Rfc2812.Stats(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcStats()
        {
            WriteLine(Rfc2812.Stats());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="priority"></param>
        public void RfcStats(string query, Priority priority)
        {
            WriteLine(Rfc2812.Stats(query), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        public void RfcStats(string query)
        {
            WriteLine(Rfc2812.Stats(query));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcStats(string query, string target, Priority priority)
        {
            WriteLine(Rfc2812.Stats(query, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="target"></param>
        public void RfcStats(string query, string target)
        {
            WriteLine(Rfc2812.Stats(query, target));
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcLinks()
        {
            WriteLine(Rfc2812.Links());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="servermask"></param>
        /// <param name="priority"></param>
        public void RfcLinks(string servermask, Priority priority)
        {
            WriteLine(Rfc2812.Links(servermask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="servermask"></param>
        public void RfcLinks(string servermask)
        {
            WriteLine(Rfc2812.Links(servermask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="remoteserver"></param>
        /// <param name="servermask"></param>
        /// <param name="priority"></param>
        public void RfcLinks(string remoteserver, string servermask, Priority priority)
        {
            WriteLine(Rfc2812.Links(remoteserver, servermask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="remoteserver"></param>
        /// <param name="servermask"></param>
        public void RfcLinks(string remoteserver, string servermask)
        {
            WriteLine(Rfc2812.Links(remoteserver, servermask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcTime(Priority priority)
        {
            WriteLine(Rfc2812.Time(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcTime()
        {
            WriteLine(Rfc2812.Time());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcTime(string target, Priority priority)
        {
            WriteLine(Rfc2812.Time(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcTime(string target)
        {
            WriteLine(Rfc2812.Time(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetserver"></param>
        /// <param name="port"></param>
        /// <param name="priority"></param>
        public void RfcConnect(string targetserver, string port, Priority priority)
        {
            WriteLine(Rfc2812.Connect(targetserver, port), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetserver"></param>
        /// <param name="port"></param>
        public void RfcConnect(string targetserver, string port)
        {
            WriteLine(Rfc2812.Connect(targetserver, port));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetserver"></param>
        /// <param name="port"></param>
        /// <param name="remoteserver"></param>
        /// <param name="priority"></param>
        public void RfcConnect(string targetserver, string port, string remoteserver, Priority priority)
        {
            WriteLine(Rfc2812.Connect(targetserver, port, remoteserver), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetserver"></param>
        /// <param name="port"></param>
        /// <param name="remoteserver"></param>
        public void RfcConnect(string targetserver, string port, string remoteserver)
        {
            WriteLine(Rfc2812.Connect(targetserver, port, remoteserver));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcTrace(Priority priority)
        {
            WriteLine(Rfc2812.Trace(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcTrace()
        {
            WriteLine(Rfc2812.Trace());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcTrace(string target, Priority priority)
        {
            WriteLine(Rfc2812.Trace(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcTrace(string target)
        {
            WriteLine(Rfc2812.Trace(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcAdmin(Priority priority)
        {
            WriteLine(Rfc2812.Admin(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcAdmin()
        {
            WriteLine(Rfc2812.Admin());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcAdmin(string target, Priority priority)
        {
            WriteLine(Rfc2812.Admin(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcAdmin(string target)
        {
            WriteLine(Rfc2812.Admin(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcInfo(Priority priority)
        {
            WriteLine(Rfc2812.Info(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcInfo()
        {
            WriteLine(Rfc2812.Info());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcInfo(string target, Priority priority)
        {
            WriteLine(Rfc2812.Info(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcInfo(string target)
        {
            WriteLine(Rfc2812.Info(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcServlist(Priority priority)
        {
            WriteLine(Rfc2812.Servlist(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcServlist()
        {
            WriteLine(Rfc2812.Servlist());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="priority"></param>
        public void RfcServlist(string mask, Priority priority)
        {
            WriteLine(Rfc2812.Servlist(mask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        public void RfcServlist(string mask)
        {
            WriteLine(Rfc2812.Servlist(mask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        public void RfcServlist(string mask, string type, Priority priority)
        {
            WriteLine(Rfc2812.Servlist(mask, type), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="type"></param>
        public void RfcServlist(string mask, string type)
        {
            WriteLine(Rfc2812.Servlist(mask, type));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="servicename"></param>
        /// <param name="servicetext"></param>
        /// <param name="priority"></param>
        public void RfcSquery(string servicename, string servicetext, Priority priority)
        {
            WriteLine(Rfc2812.Squery(servicename, servicetext), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="servicename"></param>
        /// <param name="servicetext"></param>
        public void RfcSquery(string servicename, string servicetext)
        {
            WriteLine(Rfc2812.Squery(servicename, servicetext));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcList(string channel, Priority priority)
        {
            WriteLine(Rfc2812.List(channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void RfcList(string channel)
        {
            WriteLine(Rfc2812.List(channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="priority"></param>
        public void RfcList(string[] channels, Priority priority)
        {
            WriteLine(Rfc2812.List(channels), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        public void RfcList(string[] channels)
        {
            WriteLine(Rfc2812.List(channels));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcList(string channel, string target, Priority priority)
        {
            WriteLine(Rfc2812.List(channel, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="target"></param>
        public void RfcList(string channel, string target)
        {
            WriteLine(Rfc2812.List(channel, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcList(string[] channels, string target, Priority priority)
        {
            WriteLine(Rfc2812.List(channels, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="target"></param>
        public void RfcList(string[] channels, string target)
        {
            WriteLine(Rfc2812.List(channels, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcNames(string channel, Priority priority)
        {
            WriteLine(Rfc2812.Names(channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void RfcNames(string channel)
        {
            WriteLine(Rfc2812.Names(channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="priority"></param>
        public void RfcNames(string[] channels, Priority priority)
        {
            WriteLine(Rfc2812.Names(channels), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        public void RfcNames(string[] channels)
        {
            WriteLine(Rfc2812.Names(channels));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcNames(string channel, string target, Priority priority)
        {
            WriteLine(Rfc2812.Names(channel, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="target"></param>
        public void RfcNames(string channel, string target)
        {
            WriteLine(Rfc2812.Names(channel, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcNames(string[] channels, string target, Priority priority)
        {
            WriteLine(Rfc2812.Names(channels, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="target"></param>
        public void RfcNames(string[] channels, string target)
        {
            WriteLine(Rfc2812.Names(channels, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcTopic(string channel, Priority priority)
        {
            WriteLine(Rfc2812.Topic(channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        public void RfcTopic(string channel)
        {
            WriteLine(Rfc2812.Topic(channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="newtopic"></param>
        /// <param name="priority"></param>
        public void RfcTopic(string channel, string newtopic, Priority priority)
        {
            WriteLine(Rfc2812.Topic(channel, newtopic), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="newtopic"></param>
        public void RfcTopic(string channel, string newtopic)
        {
            WriteLine(Rfc2812.Topic(channel, newtopic));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcMode(string target, Priority priority)
        {
            WriteLine(Rfc2812.Mode(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcMode(string target)
        {
            WriteLine(Rfc2812.Mode(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newmode"></param>
        /// <param name="priority"></param>
        public void RfcMode(string target, string newmode, Priority priority)
        {
            WriteLine(Rfc2812.Mode(target, newmode), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newmode"></param>
        public void RfcMode(string target, string newmode)
        {
            WriteLine(Rfc2812.Mode(target, newmode));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="distribution"></param>
        /// <param name="info"></param>
        /// <param name="priority"></param>
        public void RfcService(string nickname, string distribution, string info, Priority priority)
        {
            WriteLine(Rfc2812.Service(nickname, distribution, info), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="distribution"></param>
        /// <param name="info"></param>
        public void RfcService(string nickname, string distribution, string info)
        {
            WriteLine(Rfc2812.Service(nickname, distribution, info));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcInvite(string nickname, string channel, Priority priority)
        {
            WriteLine(Rfc2812.Invite(nickname, channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="channel"></param>
        public void RfcInvite(string nickname, string channel)
        {
            WriteLine(Rfc2812.Invite(nickname, channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newnickname"></param>
        /// <param name="priority"></param>
        public void RfcNick(string newnickname, Priority priority)
        {
            WriteLine(Rfc2812.Nick(newnickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newnickname"></param>
        public void RfcNick(string newnickname)
        {
            WriteLine(Rfc2812.Nick(newnickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcWho(Priority priority)
        {
            WriteLine(Rfc2812.Who(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcWho()
        {
            WriteLine(Rfc2812.Who());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="priority"></param>
        public void RfcWho(string mask, Priority priority)
        {
            WriteLine(Rfc2812.Who(mask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        public void RfcWho(string mask)
        {
            WriteLine(Rfc2812.Who(mask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="ircop"></param>
        /// <param name="priority"></param>
        public void RfcWho(string mask, bool ircop, Priority priority)
        {
            WriteLine(Rfc2812.Who(mask, ircop), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="ircop"></param>
        public void RfcWho(string mask, bool ircop)
        {
            WriteLine(Rfc2812.Who(mask, ircop));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="priority"></param>
        public void RfcWhois(string mask, Priority priority)
        {
            WriteLine(Rfc2812.Whois(mask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        public void RfcWhois(string mask)
        {
            WriteLine(Rfc2812.Whois(mask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="masks"></param>
        /// <param name="priority"></param>
        public void RfcWhois(string[] masks, Priority priority)
        {
            WriteLine(Rfc2812.Whois(masks), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="masks"></param>
        public void RfcWhois(string[] masks)
        {
            WriteLine(Rfc2812.Whois(masks));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mask"></param>
        /// <param name="priority"></param>
        public void RfcWhois(string target, string mask, Priority priority)
        {
            WriteLine(Rfc2812.Whois(target, mask), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mask"></param>
        public void RfcWhois(string target, string mask)
        {
            WriteLine(Rfc2812.Whois(target, mask));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="masks"></param>
        /// <param name="priority"></param>
        public void RfcWhois(string target, string[] masks, Priority priority)
        {
            WriteLine(Rfc2812.Whois(target, masks), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="masks"></param>
        public void RfcWhois(string target, string[] masks)
        {
            WriteLine(Rfc2812.Whois(target, masks));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        public void RfcWhowas(string nickname)
        {
            WriteLine(Rfc2812.Whowas(nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string[] nicknames, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nicknames), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        public void RfcWhowas(string[] nicknames)
        {
            WriteLine(Rfc2812.Whowas(nicknames));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="count"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string nickname, string count, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nickname, count), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="count"></param>
        public void RfcWhowas(string nickname, string count)
        {
            WriteLine(Rfc2812.Whowas(nickname, count));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="count"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string[] nicknames, string count, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nicknames, count), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="count"></param>
        public void RfcWhowas(string[] nicknames, string count)
        {
            WriteLine(Rfc2812.Whowas(nicknames, count));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="count"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string nickname, string count, string target, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nickname, count, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="count"></param>
        /// <param name="target"></param>
        public void RfcWhowas(string nickname, string count, string target)
        {
            WriteLine(Rfc2812.Whowas(nickname, count, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="count"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcWhowas(string[] nicknames, string count, string target, Priority priority)
        {
            WriteLine(Rfc2812.Whowas(nicknames, count, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="count"></param>
        /// <param name="target"></param>
        public void RfcWhowas(string[] nicknames, string count, string target)
        {
            WriteLine(Rfc2812.Whowas(nicknames, count, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcKill(string nickname, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Kill(nickname, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        public void RfcKill(string nickname, string comment)
        {
            WriteLine(Rfc2812.Kill(nickname, comment));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="priority"></param>
        public void RfcPing(string server, Priority priority)
        {
            WriteLine(Rfc2812.Ping(server), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        public void RfcPing(string server)
        {
            WriteLine(Rfc2812.Ping(server));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="server2"></param>
        /// <param name="priority"></param>
        public void RfcPing(string server, string server2, Priority priority)
        {
            WriteLine(Rfc2812.Ping(server, server2), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="server2"></param>
        public void RfcPing(string server, string server2)
        {
            WriteLine(Rfc2812.Ping(server, server2));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="priority"></param>
        public void RfcPong(string server, Priority priority)
        {
            WriteLine(Rfc2812.Pong(server), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        public void RfcPong(string server)
        {
            WriteLine(Rfc2812.Pong(server));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="server2"></param>
        /// <param name="priority"></param>
        public void RfcPong(string server, string server2, Priority priority)
        {
            WriteLine(Rfc2812.Pong(server, server2), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="server2"></param>
        public void RfcPong(string server, string server2)
        {
            WriteLine(Rfc2812.Pong(server, server2));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcAway(Priority priority)
        {
            WriteLine(Rfc2812.Away(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcAway()
        {
            WriteLine(Rfc2812.Away());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="awaytext"></param>
        /// <param name="priority"></param>
        public void RfcAway(string awaytext, Priority priority)
        {
            WriteLine(Rfc2812.Away(awaytext), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="awaytext"></param>
        public void RfcAway(string awaytext)
        {
            WriteLine(Rfc2812.Away(awaytext));
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcRehash()
        {
            WriteLine(Rfc2812.Rehash());
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcDie()
        {
            WriteLine(Rfc2812.Die());
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcRestart()
        {
            WriteLine(Rfc2812.Restart());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="priority"></param>
        public void RfcSummon(string user, Priority priority)
        {
            WriteLine(Rfc2812.Summon(user), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        public void RfcSummon(string user)
        {
            WriteLine(Rfc2812.Summon(user));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcSummon(string user, string target, Priority priority)
        {
            WriteLine(Rfc2812.Summon(user, target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        public void RfcSummon(string user, string target)
        {
            WriteLine(Rfc2812.Summon(user, target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="channel"></param>
        /// <param name="priority"></param>
        public void RfcSummon(string user, string target, string channel, Priority priority)
        {
            WriteLine(Rfc2812.Summon(user, target, channel), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="channel"></param>
        public void RfcSummon(string user, string target, string channel)
        {
            WriteLine(Rfc2812.Summon(user, target, channel));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcUsers(Priority priority)
        {
            WriteLine(Rfc2812.Users(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcUsers()
        {
            WriteLine(Rfc2812.Users());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="priority"></param>
        public void RfcUsers(string target, Priority priority)
        {
            WriteLine(Rfc2812.Users(target), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        public void RfcUsers(string target)
        {
            WriteLine(Rfc2812.Users(target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wallopstext"></param>
        /// <param name="priority"></param>
        public void RfcWallops(string wallopstext, Priority priority)
        {
            WriteLine(Rfc2812.Wallops(wallopstext), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wallopstext"></param>
        public void RfcWallops(string wallopstext)
        {
            WriteLine(Rfc2812.Wallops(wallopstext));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void RfcUserhost(string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Userhost(nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        public void RfcUserhost(string nickname)
        {
            WriteLine(Rfc2812.Userhost(nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="priority"></param>
        public void RfcUserhost(string[] nicknames, Priority priority)
        {
            WriteLine(Rfc2812.Userhost(nicknames), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        public void RfcUserhost(string[] nicknames)
        {
            WriteLine(Rfc2812.Userhost(nicknames));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="priority"></param>
        public void RfcIson(string nickname, Priority priority)
        {
            WriteLine(Rfc2812.Ison(nickname), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nickname"></param>
        public void RfcIson(string nickname)
        {
            WriteLine(Rfc2812.Ison(nickname));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        /// <param name="priority"></param>
        public void RfcIson(string[] nicknames, Priority priority)
        {
            WriteLine(Rfc2812.Ison(nicknames), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nicknames"></param>
        public void RfcIson(string[] nicknames)
        {
            WriteLine(Rfc2812.Ison(nicknames));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        public void RfcQuit(Priority priority)
        {
            WriteLine(Rfc2812.Quit(), priority);
        }

        /// <summary>
        ///
        /// </summary>
        public void RfcQuit()
        {
            WriteLine(Rfc2812.Quit());
        }

        public void RfcQuit(string quitmessage, Priority priority)
        {
            WriteLine(Rfc2812.Quit(quitmessage), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="quitmessage"></param>
        public void RfcQuit(string quitmessage)
        {
            WriteLine(Rfc2812.Quit(quitmessage));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comment"></param>
        /// <param name="priority"></param>
        public void RfcSquit(string server, string comment, Priority priority)
        {
            WriteLine(Rfc2812.Squit(server, comment), priority);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comment"></param>
        public void RfcSquit(string server, string comment)
        {
            WriteLine(Rfc2812.Squit(server, comment));
        }

        #endregion RFC commands
    }
}