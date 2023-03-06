using System.Collections.Generic;
using WallsCalculator.Models.Enums;

namespace WallsCalculator.Utils
{
    public class BrickStandardOptions
    {
        public Dictionary<DepthType, Dictionary<BrickType, int>>? Standards;
    }
}