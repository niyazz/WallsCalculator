using System;
using System.Linq;
using WallsCalculator.Models;
using WallsCalculator.Utils;

namespace WallsCalculator.Services.Calculators
{
    public class BalkCalculator : ICalculator<BalkCalculationInput, BalkCalculationOutput>
    {
        private const int MToMm = 1000;
        private const int MToCm = 100;
        private const int MmToCm = 10;
        
        public BalkCalculationOutput? Calculate(BalkCalculationInput input)
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
                var balkVolumeCubeM = (input.BalkHeight / MToMm) * (input.BalkWidth / MToMm) * input.BalkLength;
                var oneCubeBalkAmount = Convert.ToInt32(1 / balkVolumeCubeM);
                var balkRowsAmount = Math.Ceiling((input.AngleHeight / MToCm) / (input.BalkHeight / MToMm));
                var wallsAmount = perimeterM / input.BalkLength;
                var areaToCoverCubeM = (balkRowsAmount * wallsAmount) / oneCubeBalkAmount;
                var totalMaterialPrice = Math.Round((decimal) areaToCoverCubeM * input.Price, 2);
                var allWorkersPrice = input.Workers.Select(x => x.QuantityOfWorkers * x.Price * x.DurationInDays).Sum();

                return new BalkCalculationOutput
                {
                    Input = input,
                    OneCubeBalkAmount = oneCubeBalkAmount,
                    TotalMaterialAmount = Convert.ToInt32(areaToCoverCubeM * oneCubeBalkAmount),
                    TotalMaterialPrice =totalMaterialPrice,
                    AreaToCoverCubeM = Math.Round(areaToCoverCubeM, 2),
                    AreaToCoverSquareM = Math.Round(areaToCoverSm, 2),
                    AreaToNotCoverSquareM = Math.Round(areaToNotCoverSm, 2),
                    AllWorkersPrice = allWorkersPrice,
                    WallDepthCentimeters = Math.Round(input.BalkWidth / MmToCm, 2),
                    ConstructionWeight = Math.Round(areaToCoverCubeM * input.BalkType.GetDensity(), 2),
                    BalkRowsAmount = balkRowsAmount,
                    BalkVolumeCubeM = balkVolumeCubeM,
                    TotalMaterialAndWorkersPrice = totalMaterialPrice + allWorkersPrice,
                    TotalArea = Math.Round(perimeterM * heightM, 2)
                };
            }

            return null;
        }
    }
}