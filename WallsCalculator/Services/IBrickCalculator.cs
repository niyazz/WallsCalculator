using System;
using System.Linq;
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
        private const int MToMm = 1000;
        private const int MToCm = 100;
        private const int MmToCm = 10;

        public BrickCalculator(BrickStandardOptions options)
        {
            _options = options;
        }

        public BrickCalculationOutput? Calculate(BrickCalculationInput input)
        {
            var perimeterM = input.Perimeter;
            var heightM = input.AngleHeight / MToCm;
            var areaToCoverSm = perimeterM * heightM;
            var areaToNotCoverSm = input.Apertures
                .Select(x => new
                {
                    HeightM = x.Height / MToMm,
                    WidthM = x.Width / MToMm
                })
                .Select(x => x.HeightM * x.WidthM).Sum();

            if (areaToCoverSm > areaToNotCoverSm)
            {
                areaToCoverSm -= areaToNotCoverSm;
                var oneSmBricksAmount = _options.Standards![input.DepthType][input.BrickType];
                var totalBricksAmount = Convert.ToInt32(Math.Ceiling(areaToCoverSm * oneSmBricksAmount));
                var columnBrickAmount = (int)(input.AngleHeight / (input.BrickType.GetBrickSizes().Item3 / MmToCm));
                var masonryGridRowsAmount = columnBrickAmount / input.MasonryType.GetValue();
                if (masonryGridRowsAmount == columnBrickAmount) masonryGridRowsAmount--;
                var wallDepthCm = input.DepthType.GetDepth(input.BrickType, input.MortarType) / MmToCm;
                
                return new BrickCalculationOutput
                {
                    Input = input,
                    OneSquareBricksAmount = oneSmBricksAmount,
                    TotalBricksAmount = totalBricksAmount,
                    TotalBricksPrice = totalBricksAmount * input.Price,
                    ColumnBricksAmount = columnBrickAmount,
                    AreaToCover = areaToCoverSm,
                    AreaToNotCover = areaToNotCoverSm,
                    AllWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum(),
                    WallDepth = wallDepthCm,
                    AreaForMasonryGrid = Math.Round(input.Perimeter * (wallDepthCm / MToCm) * masonryGridRowsAmount, 2),
                    MasonryGridRowsAmount = masonryGridRowsAmount,
                };
            }

            return null;
        }
    }
}