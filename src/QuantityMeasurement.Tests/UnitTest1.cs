using BusinessLayer;
using ModelLayer;
using Moq;
using RepositoryLayer;

namespace QuantityMeasurement.Tests;

public class UnitTest1
{
    [Fact]
    public void Divide_WhenWeightTargetUnitIsGram_ReturnsConvertedResult()
    {
        var repository = new Mock<IQuantityMeasurementRepository>();
        repository.Setup(x => x.Save(It.IsAny<QuantityMeasurementEntity>()));

        var service = new QuantityMeasurementService(repository.Object);

        var first = new QuantityDTO(80, "Kilogram", "Weight");
        var second = new QuantityDTO(5, "Kilogram", "Weight");

        var result = service.Divide(first, second, targetUnit: "Gram");

        Assert.Equal("16000", result.ResultValue);
        Assert.Equal("Gram", result.ResultUnit);
    }
}
