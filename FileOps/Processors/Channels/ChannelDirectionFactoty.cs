using FileOps.Configuration.Entities;
using System;

namespace FileOps.Processors.Channels
{
    internal static class ChannelDirectionFactory
    {
        public static ChannelDirection Get(ChannelSettings channelSettings)
        {
            if(channelSettings.GetType().Name == typeof(FromSettings).Name)
                return ChannelDirection.Inbound;
            else if (channelSettings.GetType().Name == typeof(ToSettings).Name)
                    return ChannelDirection.Outbound;
            throw new InvalidCastException($"Invalid channel direction {channelSettings.GetType().Name}");
        }

    }
}
