﻿using Ricky8955555.CoolQ.Features;

namespace Ricky8955555.CoolQ.Apps
{
    class BlacklistManagerApp : App
    {
        public override string Name { get; } = "BlacklistManager";

        public override string DisplayName { get; } = "黑名单管理器";

        public override Feature[] Features { get; } = new Feature[] {
            new BlacklistManagerCommand()
        };

        public override bool CanDisable { get; } = false;
    }
}
