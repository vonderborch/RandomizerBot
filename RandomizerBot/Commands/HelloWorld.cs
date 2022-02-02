﻿using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class HelloWorld : AbstractCommand
{
    public HelloWorld() : base("hello_world", "Tells the user that they ran Hello World!")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        SendMessage(messageArgs, $"User '{messageArgs.Author.Username}' successfully ran helloworld!");

        return true;
    }
}