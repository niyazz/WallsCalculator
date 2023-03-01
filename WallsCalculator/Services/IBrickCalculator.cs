using System;
using System.Linq;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Utils;

namespace WallsCalculator.Services
{
    /// <summary>
    /// Калькулятор для расчета по кирпичу.
    /// </summary>
    public interface IBrickCalculator
    {
        /// <summary>
        /// Получить расчет по кирпичу.
        /// </summary>
        public BrickCalculationOutput? Calculate(BrickCalculationInput input);
    }
    
    public class BrickCalculator : IBrickCalculator
    {
        private const int _mmToMeters = 1000000;
        
        public BrickCalculationOutput? Calculate(BrickCalculationInput input)
        {
            var brickWallArea = input.Perimeter * input.AngleHeight;
            var apertureAreaSum = input.Apertures.Select(x => x.Height * x.Width).Sum();

            if (brickWallArea > apertureAreaSum)
            {
                brickWallArea -= apertureAreaSum;
                    
                if (input.BrickType.GetBrickVolume().HasValue)
                {
                    var brickAmount = Convert.ToInt64(Math.Ceiling(brickWallArea / input.BrickType.GetBrickVolume()!.Value));
                    return new BrickCalculationOutput
                    {
                        BrickAmount = brickAmount,
                        FullPrice = brickAmount * input.Price,
                        Area = brickWallArea / _mmToMeters
                    };
                }
            }

            return null;
        }
    }
}