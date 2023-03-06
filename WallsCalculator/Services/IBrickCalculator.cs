using System;
using System.Linq;
using Microsoft.Extensions.Options;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
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
        private readonly BrickStandardOptions _options;
        private const int MmToM = 1000;
        private const int MToCm = 100;

        public BrickCalculator(BrickStandardOptions options)
        {
            _options = options;
        }

        public BrickCalculationOutput? Calculate(BrickCalculationInput input)
        {
            var perimeterM = input.Perimeter;
            var heightM = input.AngleHeight / MToCm;
            
            var areaToCover = perimeterM * heightM;
            var apertureAreaSumM = input.Apertures
                .Select(x => new
                {
                    HeightM = x.Height / MmToM,
                    WidthM = x.Width / MmToM
                })
                .Select(x => x.HeightM * x.WidthM).Sum();

            if (areaToCover > apertureAreaSumM)
            {
                areaToCover -= apertureAreaSumM;
                var bricksInOneSquareM = _options.Standards![input.DepthType][input.BrickType];
                var brickAmountOnBrickWallArea = Convert.ToInt32(Math.Ceiling(areaToCover * bricksInOneSquareM));
                var allWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price).Sum();
                return new BrickCalculationOutput
                {
                    Input = input,
                    BricksInOneSquareM = bricksInOneSquareM,
                    BricksAmount = brickAmountOnBrickWallArea,
                    AllBricksPrice = brickAmountOnBrickWallArea * input.Price,
                    AreaToCover = areaToCover,
                    AreaToNotCover = apertureAreaSumM,
                    AllWorkersPrice = allWorkersPrice
                };
            }

            return null;
        }
    }
}