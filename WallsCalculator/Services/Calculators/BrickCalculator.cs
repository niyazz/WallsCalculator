using System;
using System.Linq;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services.Abstractions;
using WallsCalculator.Utils;
#nullable enable

namespace WallsCalculator.Services.Calculators
{
    public class BrickCalculator : ICalculator<BrickCalculationInput, BrickCalculationOutput>
    {
        private const int MToMm = 1000;
        private const int MToCm = 100;
        private const int MmToCm = 10;

        public BrickCalculationOutput? Calculate(BrickCalculationInput input)
        {
            var perimeterM = input.Perimeter;
            var heightM = input.AngleHeight / MToCm;
            var areaToCoverSm = perimeterM * heightM;
            var areaToNotCoverSm = input.Apertures
                .Select(x => new
                {
                    HeightM = x.Height / MToMm,
                    WidthM = x.Width / MToMm,
                    x.Quantity
                })
                .Select(x => x.HeightM * x.WidthM * x.Quantity).Sum();

            if (areaToCoverSm > areaToNotCoverSm)
            {
                areaToCoverSm -= areaToNotCoverSm;
                var (l, w, h) = input.BrickType.GetMaterialSizes();
                var oneSmBricksAmount = input.DepthType.GetMaterialAmountInSquareMeters(l, w, h, input.MortarValue);
                var totalBricksAmount = Convert.ToInt32(Math.Ceiling(areaToCoverSm * oneSmBricksAmount));
                var columnBrickAmount = (int) (input.AngleHeight / (input.BrickType.GetMaterialSizes().Item3 / MmToCm));
                var masonryGridRowsAmount = columnBrickAmount / input.MasonryType.GetValue();
                if (masonryGridRowsAmount == columnBrickAmount) masonryGridRowsAmount--;
                var wallDepthCm = input.DepthType.GetDepth(l, w, h, input.MortarValue) / MmToCm;
                var totalMaterialPrice = totalBricksAmount * input.Price;
                var allWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum();
                return new BrickCalculationOutput
                {
                    Input = input,
                    OneSquareBricksAmount = oneSmBricksAmount,
                    TotalMaterialAmount = totalBricksAmount,
                    TotalMaterialPrice = totalMaterialPrice,
                    ColumnBricksAmount = columnBrickAmount,
                    AreaToCoverSquareM = Math.Round(areaToCoverSm, 2),
                    AreaToNotCoverSquareM = Math.Round(areaToNotCoverSm, 2),
                    AllWorkersPrice = allWorkersPrice,
                    WallDepthCentimeters = Math.Round(wallDepthCm, 2),
                    AreaForMasonryGrid = Math.Round(input.Perimeter * (wallDepthCm / MToCm) * masonryGridRowsAmount, 2),
                    TotalMaterialAndWorkersPrice = totalMaterialPrice + allWorkersPrice,
                    MasonryGridRowsAmount = masonryGridRowsAmount,
                    TotalArea = Math.Round(perimeterM * heightM, 2)
                };
            }

            return null;
        }
    }
}