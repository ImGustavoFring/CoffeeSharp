using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class FeedbackDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private long? _ratingId;

    [ObservableProperty]
    private long? _orderId;

    public FeedbackDtoObservable() { }

    public FeedbackDtoObservable(FeedbackDto dto)
    {
        _id = dto.Id;
        _content = dto.Content;
        _ratingId = dto.RatingId;
        _orderId = dto.OrderId;
    }

    public FeedbackDto ToDto()
    {
        return new FeedbackDto
        {
            Id = Id,
            Content = Content,
            RatingId = RatingId,
            OrderId = OrderId
        };
    }
}