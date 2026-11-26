namespace StyleStore.Api.DTOs;

public record CreateOrderItemDto(int ProductId, string Size, int Quantity);
public record CreateOrderDto(List<CreateOrderItemDto> Items);
