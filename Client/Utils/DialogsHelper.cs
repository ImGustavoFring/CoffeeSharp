using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Client.Utils;

public static class DialogsHelper
{
    public static async Task ShowOk(string? message = null)
    {
        await MessageBoxManager
            .GetMessageBoxStandard("Успех", !string.IsNullOrEmpty(message) ? message : "Успех!", ButtonEnum.Ok)
            .ShowAsync();
    }

    public static async Task ShowError(string? message = null)
    {
        await MessageBoxManager
            .GetMessageBoxStandard("Ошибка", !string.IsNullOrEmpty(message) ? message : "Ошибка!", ButtonEnum.Ok)
            .ShowAsync();
    }
}