﻿using HuajiTech.CoolQ.Events;
using HuajiTech.CoolQ.Messaging;
using System;
using System.Linq;
using static Ricky8955555.CoolQ.Utilities;

namespace Ricky8955555.CoolQ.Features
{
    class SwitchCommand : Command<PlainText>
    {
        internal override string ResponseCommand { get; } = "switch";

        protected override string CommandUsage { get; } = "{0}switch <应用名称> <on/off>";

        protected override void Invoking(MessageReceivedEventArgs e, PlainText plainText)
        {
            string[] splitText = plainText.Content.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (splitText != null && splitText.Length == 2)
            {
                try
                {
                    var config = Commons.AppStatusConfig;
                    var app = GetApps(e.Source, e.Sender).Where(x => x.Name == splitText[0]).Single();
                    bool? operation = splitText[1].ToLower().ToBool("on", "off");

                    if (app.CanDisable)
                    {
                        if (!operation.HasValue)
                            NotifyIncorrectUsage(e);
                        else
                        {
                            config.Config[e.Source.ToString(true)][app.Name] = operation.Value;
                            e.Reply($"已{(operation.Value ? "启用" : "停用")}应用 {app.DisplayName}（{app.Name}） ✧(≖ ◡ ≖✿ ");
                            config.Save();
                        }
                    }
                    else
                        e.Reply($"该应用 {app.DisplayName}（{app.Name}）不允许被启用/停用 o(ﾟДﾟ)っ！");
                }
                catch (InvalidOperationException)
                {
                    e.Reply($"应用 {splitText[0]} 不存在");
                }
            }
            else
                NotifyIncorrectUsage(e);
        }
    }
}
