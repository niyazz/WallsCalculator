﻿using System;
using System.Linq;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Utils;

namespace WallsCalculator.Services.Calculators
{
    public class BrickCalculator : ICalculator<BrickCalculationInput, BrickCalculationOutput>
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
                    AllWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum(),
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