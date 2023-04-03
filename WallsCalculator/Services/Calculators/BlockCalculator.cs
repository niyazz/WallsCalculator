using System;
using System.Linq;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Services.Abstractions;
using WallsCalculator.Utils;
#nullable enable

namespace WallsCalculator.Services.Calculators
{
    public class BlockCalculator : ICalculator<BlockCalculationInput, BlockCalculationOutput>
    {
        private const int MToMm = 1000;
        private const int MToCm = 100;
        private const int MmToCm = 10;

        public BlockCalculationOutput? Calculate(BlockCalculationInput input)
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
                var (l, w, h) = input.BlockType.GetMaterialSizes();
                var oneSmBlocksAmount = input.DepthType.GetMaterialAmountInSquareMeters(l, w, h, input.MortarValue);
                var totalBlockAmount = Convert.ToInt32(Math.Ceiling(areaToCoverSm * oneSmBlocksAmount));
                var columnBlocksAmount = (int) (input.AngleHeight / (input.BlockType.GetMaterialSizes().Item3 / MmToCm));
                var masonryGridRowsAmount = columnBlocksAmount / input.MasonryType.GetValue();
                if (masonryGridRowsAmount == columnBlocksAmount) masonryGridRowsAmount--;
                var wallDepthCm = input.DepthType.GetDepth(l, w, h, input.MortarValue) / MmToCm;
                var totalMaterialPrice = totalBlockAmount * input.Price;
                var allWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum();
                return new BlockCalculationOutput
                {
                    Input = input,
                    OneSquareBlocksAmount = oneSmBlocksAmount,
                    TotalMaterialAmount = totalBlockAmount,
                    TotalMaterialPrice = totalMaterialPrice,
                    ColumnBlocksAmount = columnBlocksAmount,
                    AreaToCoverSquareM = Math.Round(areaToCoverSm, 2),
                    AreaToNotCoverSquareM = Math.Round(areaToNotCoverSm, 2),
                    AllWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum(),
                    WallDepthCentimeters = Math.Round(wallDepthCm, 2),
                    AreaForMasonryGrid = Math.Round(input.Perimeter * (wallDepthCm / MToCm) * masonryGridRowsAmount, 2),
                    TotalMaterialAndWorkersPrice = totalMaterialPrice + allWorkersPrice,
                    MasonryGridRowsAmount = masonryGridRowsAmount,
                    ConstructionWeight = totalBlockAmount * input.BlockWeight,
                    TotalArea = Math.Round(perimeterM * heightM, 2)
                };
            }

            return null;
        }
    }
}