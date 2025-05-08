using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class ClientDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _telegramId = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private decimal _balance;

    public ClientDtoObservable() { }

    public ClientDtoObservable(ClientDto dto)
    {
        _id = dto.Id;
        _telegramId = dto.TelegramId;
        _name = dto.Name;
        _balance = dto.Balance;
    }

    public ClientDto ToDto()
    {
        return new ClientDto
        {
            Id = Id,
            TelegramId = TelegramId,
            Name = Name,
            Balance = Balance
        };
    }
}