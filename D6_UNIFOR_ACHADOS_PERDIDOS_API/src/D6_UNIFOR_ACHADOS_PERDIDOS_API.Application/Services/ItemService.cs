using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.DTOs;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Entity;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain.Enums;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public async Task<IEnumerable<ItemEntity>> ListFoundItemsAsync()
    {
        var items = await _itemRepository.ListFoundItemsAsync();
        return items;
    }
    
    public async Task<IEnumerable<ItemEntity>> ListLostItemsAsync()
    {
        var items = await _itemRepository.ListLostItemsAsync();
        return items;
    }

    public async Task UploadFoundItemAsync(UploadItemDto item)
    {
        var itemEntity = new ItemEntity(
            nome: item.Nome,
            descricao: item.Descricao,
            categoria: item.Categoria,
            localEncontro: item.LocalEncontro,
            dataEncontro: item.DataEncontro,
            statusId: (int)ItemStatus.ENCONTRADO,
            nomeResponsavel: item.NomeResponsavel,
            contatoResponsavel: item.ContatoResponsavel
        );

        await _itemRepository.UploadFoundItemAsync(itemEntity);
    }

    public async Task UpdateItemStatusAsync(int Id, int StatusId)
    {
        await _itemRepository.UpdateItemStatusAsync(Id, StatusId);
    }

    public async Task RemoveItemAsync(int Id)
    {
        await _itemRepository.RemoveItemAsync(Id);
    }
}