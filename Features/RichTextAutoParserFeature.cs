﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuajiTech.CoolQ;
using HuajiTech.CoolQ.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ricky8955555.CoolQ.Features
{
    class RichTextAutoParserFeature : Feature
    {
        public static List<Group> Groups = new List<Group>();

        public override void Invoke(MessageReceivedEventArgs e)
        {
            if (e.Source is Group group && Groups.Contains(group) && // 判断消息来源是否为群，且 Groups 中是否包括当前群
                e.Message.Parse().TryDeconstruct(out RichText richText) && // 尝试解构第一个元素为富文本
                richText["content"] != null) // 确保富文本中 content 不为空
                try
                {
                    var content = JObject.Parse(richText["content"].Replace(";", "")); // 替换 content 中的 ; 为空，转换为 JObject
                    var firstList = content.First.First.ToList(); // 取 content 中第一组数据中的第一组数据，并转换为 List
                    var url = firstList.Find(x => ((JProperty)x).Name.ToLower() == "url"); // 在该 List 中寻找 url 参数
                    var inaccurateUrl = firstList.Find(x => ((JProperty)x).Name.ToLower().Contains("url")); // 在该 List 中寻找带有 url 参数
                    string title = richText["title"] ?? "无标题"; // 定义 title 为 richText 中的 title 参数，如果该 title 参数 null 则为无标题

                    if (url != null) // 检测 url 是否为 null
                        e.Source.Send(new Share() { Title = title, Url = new Uri(url.First.ToString()) }); // 发送【分享】CQ 码
                    else if (inaccurateUrl != null) // 检测 inaccurateUrl 是否为 null
                        e.Source.Send(new Share() { Title = title, Url = new Uri(inaccurateUrl.First.ToString()) } ); // 发送【分享】CQ 码
                }
                catch { }
        }

        public static bool Switch(Group group)
        {
            var isExist = Groups.Contains(group); // 检测在 Groups 中是否含有当前群
            if (isExist) // 判断是否存在
                Groups.Remove(group); // 从 Groups 中移除当前群
            else
                Groups.Add(group); // 往 Groups 中添加当前群

            return isExist; // 返回是否存在
        }
    }
}