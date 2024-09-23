﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AIV4.Shared
{
    public class ConversationContent
    {
        public StringBuilder TextContent { get; set; }
        public Role Role { get; set; }

        public string PassPhrase { get; set; }

        public string UserName { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Text;//defaults to text
        public string Text {
            get {  return TextContent.ToString(); }
        }


        public ConversationContent()
        {
            TextContent = new StringBuilder();
            Role = Role.User;
            ContentType = ContentType.Text;
            UserName = "";
        }
        public ConversationContent(Role role, ContentType contentType)
        {
            TextContent ??= new StringBuilder();
            Role = role;
            ContentType = contentType;
            UserName = "";
        }
        public ConversationContent(Role role, ContentType contentType, string textContent)
        {
            TextContent ??= new StringBuilder(textContent);
            Role = role;
            ContentType = contentType;
            UserName = "";
        }
        public ConversationContent(string text)
        {
            TextContent ??= new StringBuilder(text);
            Role = Role.User;
            ContentType = ContentType.Text;
            UserName = "";
        }
        public ConversationContent(string text, string username)
        {
            TextContent ??= new StringBuilder(text);
            Role = Role.User;
            ContentType = ContentType.Text;
            UserName = username;
        }
        public void AppendTextResponse(string text)
        {
            TextContent.Append(text);
        }

        //public string ToJson()
        //{
        //    var json = JsonConvert.SerializeObject(this);
        //    return json; 
        //}
        //public ConversationContent FromJson(string json)
        //{
        //    ConversationContent newContent = new ConversationContent();
        //    try 
        //    {
        //        newContent = JsonConvert.DeserializeObject<ConversationContent>(json);
        //    }
        //    catch (Exception ex) 
        //    {
        //        var exc = ex;
        //    }
        //    return newContent;
        //}

    }

    public enum Role
    {
        User,      // Represents a user
        Assistant, // Represents an assistant
        System     // Represents a system entity
    }
    public enum ContentType
    {
        Text,      // Represents text chat
        Audio,   
        Image    
    }

}
