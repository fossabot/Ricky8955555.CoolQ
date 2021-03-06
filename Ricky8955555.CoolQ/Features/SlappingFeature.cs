﻿using HuajiTech.CoolQ.Events;
using HuajiTech.CoolQ.Messaging;
using System.Linq;

namespace Ricky8955555.CoolQ.Features
{
    class SlappingFeature : Feature
    {
        internal override string Usage { get; } = "当发送者单条消息中含有 2 个或以上对同个人提及时，则发送 “@发送者 拍了拍 @提及者”";

        internal override void Invoke(MessageReceivedEventArgs e)
        {
            var message = e.Message.Parse();
            var mentions = message.OfType<Mention>();
            var distinctedMentions = mentions.Distinct();

            var slappeeMentions = distinctedMentions.Where(x => mentions.Where(x.Equals).Count() > 1);

            if (slappeeMentions.Any())
                e.Reply("拍了拍 " + string.Join(" ", slappeeMentions));
        }
    }
}
