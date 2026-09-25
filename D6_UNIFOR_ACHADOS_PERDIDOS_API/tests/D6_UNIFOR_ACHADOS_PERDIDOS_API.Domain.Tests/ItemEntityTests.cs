using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Enums;
using Xunit;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Tests;

public class ItemEntityTests
{
    [Theory]
    [InlineData((int)ItemStatus.ENCONTRADO)]
    [InlineData((int)ItemStatus.PERDIDO)]
    [InlineData((int)ItemStatus.DEVOLVIDO)]
    public void UpdateStatus_AcceptsDefinedStatuses(int statusId)
    {
        var item = CreateItem();

        item.UpdateStatus(statusId);

        Assert.Equal(statusId, item.StatusId);
    }

    [Theory]
    [InlineData((int)ItemStatus.ENCONTRADO)]
    [InlineData((int)ItemStatus.PERDIDO)]
    public void UpdateStatus_CannotReactivateReturnedItem(int statusId)
    {
        var item = CreateItem();
        item.UpdateStatus((int)ItemStatus.DEVOLVIDO);

        Assert.Throws<ArgumentException>(() => item.UpdateStatus(statusId));
    }

    [Fact]
    public void UpdateStatus_RejectsUndefinedStatus()
    {
        var item = CreateItem();

        Assert.Throws<ArgumentException>(() => item.UpdateStatus(99));
    }

    private static ItemEntity CreateItem() => new(
        "Livro de programação",
        "Descrição",
        "Livros",
        "Biblioteca",
        DateTime.UtcNow.AddMinutes(-1),
        (int)ItemStatus.ENCONTRADO,
        "Responsável",
        null);
}