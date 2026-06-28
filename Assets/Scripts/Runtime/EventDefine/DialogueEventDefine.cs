using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;

public class DiaLogueEventDefine
{
    public class ShowUI : IEventMessage
    {
        public List<Speaker> speakers;
        public bool hasBackground;
        public Sprite BackGround;
        public static void SendEventMessage(List<Speaker> speakers, bool hasBackground = false, Sprite BackGround = null)
        {
            var eventMessage = new ShowUI();
            eventMessage.speakers = speakers;
            eventMessage.hasBackground = hasBackground;
            eventMessage.BackGround = BackGround;
            UniEvent.SendMessage(eventMessage);
        }
    }

    public class Next : IEventMessage
    {
        public static void SendEventMessage()
        {
            var eventMessage = new Next();
            UniEvent.SendMessage(eventMessage);
        }
    }

    public class UpdateUI : IEventMessage
    {
        public int leftRoleIndex;
        public int rightRoleIndex;
        public string content;
        public TalkingSpeaker speaker;

        public static void SendEventMessage(int leftRoleIndex, int rightRoleIndex, string content, TalkingSpeaker speaker)
        {
            var eventMessage = new UpdateUI();
            eventMessage.leftRoleIndex = leftRoleIndex;
            eventMessage.rightRoleIndex = rightRoleIndex;
            eventMessage.content = content;
            eventMessage.speaker = speaker;
            UniEvent.SendMessage(eventMessage);
        }
    }
}
