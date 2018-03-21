using FileOps.Configuration.Entities;
using System;

namespace FileOps.Processors.Channels
{
    internal static class ChannelDirectionFactory
    {
        public static ChannelDirectionEnum Get(ChannelSettings channelSettings)
        {
            if(channelSettings.GetType().Name == typeof(FromSettings).Name)
                return ChannelDirectionEnum.Inbound;
            else if (channelSettings.GetType().Name == typeof(ToSettings).Name)
                    return ChannelDirectionEnum.Outbound;
            throw new InvalidCastException($"Invalid channel direction {channelSettings.GetType().Name}");
        }

    }
}
