using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web
{
    public static class BotStatusExtensions
    {
        public static BotStatus ToBotStatus(this int? status)
        {
            return (BotStatus)(status ?? 0);
        }

        public static BotStatus[] GetAllowedStatuses(this BotStatus status)
        {
            var statusList = new List<BotStatus>
            {
                BotStatus.Draft, 
                BotStatus.Private, 
                BotStatus.Public, 
                BotStatus.Open
            };

            if (status != BotStatus.Draft)
                statusList.Remove(BotStatus.Draft);

            return statusList.ToArray();
        }

        public static bool IsAllowedStatus(this BotStatus status, BotStatus newStatus)
        {
            return status != newStatus && status.GetAllowedStatuses().Contains(newStatus);
        }

        public static BotStatus[] GetPublicStatuses()
        {
            return new []
            {
                BotStatus.Public, 
                BotStatus.Open
            };
        }

        public static bool IsPublicStatus(this BotStatus status)
        {
            return GetPublicStatuses().Contains(status);
        }
    }
}