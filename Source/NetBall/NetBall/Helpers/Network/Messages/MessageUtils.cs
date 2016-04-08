using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers.Network.Messages
{
    public enum MessageType
    {
        BALL_THROW,
        BALL_SETUP,
        GOAL
    }

    public static class MessageUtils
    {
        private static Dictionary<MessageType, List<EventListener>> eventLists;


        public static void initialize()
        {
            eventLists = new Dictionary<MessageType, List<EventListener>>();

            // Set up lists
            for (int i = 0; i < Enum.GetNames(typeof(MessageType)).Length; i++)
            {
                eventLists.Add((MessageType)i, new List<EventListener>());
            }
        }


        public static void parseMessage(string message)
        {
            // Split the message apart
            char[] delims = new char[] { '~' };
            string[] pieces = message.Split(delims);

            if (pieces.Length > 1)
            {
                MessageType type = (MessageType) Enum.Parse(typeof(MessageType), pieces[0]);
                MessageData data = null;

                // Handle the message
                switch(type)
                {
                    case MessageType.BALL_THROW:
                    {
                        Vector2 pos = new Vector2(float.Parse(pieces[1]), float.Parse(pieces[2]));
                        float speed = float.Parse(pieces[3]);
                        float angle = float.Parse(pieces[4]);
                        data = new MessageDataBallThrow(pos, speed, angle);

                        break;
                    }
                    case MessageType.BALL_SETUP:
                    {
                        Vector2 pos = new Vector2(float.Parse(pieces[1]), float.Parse(pieces[2]));
                        data = new MessageDataBallSetup(pos);

                        break;
                    }
                    case MessageType.GOAL:
                    {
                        data = new MessageDataGoal();
                        break;
                    }
                }

                triggerEvent(type, data);
            }
        }

        public static string constructMessage(MessageType type, MessageData data)
        {
            string message = "";
            message += (int)type + "~";

            switch(type)
            {
                case MessageType.BALL_THROW:
                {
                    MessageDataBallThrow castData = (MessageDataBallThrow)data;
                    message += castData.position.X + "~" + castData.position.Y + "~";
                    message += castData.speed + "~";
                    message += castData.angle;

                    break;
                }
                case MessageType.BALL_SETUP:
                {
                    MessageDataBallSetup castData = (MessageDataBallSetup)data;
                    message += castData.position.X + "~" + castData.position.Y;

                    break;
                }
                case MessageType.GOAL:
                {
                    break;
                }
            }

            return message;
        }

        public static void registerListener(EventListener listener, MessageType type)
        {
            eventLists[type].Add(listener);
        }

        private static void triggerEvent(MessageType type, MessageData data)
        {
            foreach (EventListener listener in eventLists[type])
            {
                listener.eventTriggered(data);
            }
        }
    }
}
