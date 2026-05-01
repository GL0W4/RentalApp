using Microsoft.Extensions.Logging;
using StarterApp.ViewModels;
using StarterApp.Database.Data;
using StarterApp.Views;
using StarterApp.Repositories;
using System.Diagnostics;
using StarterApp.Services;
using Microsoft.EntityFrameworkCore;

namespace StarterApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IItemService, ItemService>();
        builder.Services.AddSingleton<IRentalService, RentalService>();
        builder.Services.AddSingleton<IItemRepository, ItemRepository>();
        builder.Services.AddSingleton<IRentalRepository, RentalRepository>();
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<IReviewRepository, ReviewRepository>();
        builder.Services.AddSingleton<IReviewService, ReviewService>();

        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddSingleton<ItemsListViewModel>();
        builder.Services.AddTransient<ItemsListPage>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<CreateItemViewModel>();
        builder.Services.AddTransient<CreateItemPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<TempPage>();
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddSingleton<SelectedItemService>();
        builder.Services.AddTransient<EditItemViewModel>();
        builder.Services.AddTransient<EditItemPage>();
        builder.Services.AddTransient<CreateRentalRequestViewModel>();
        builder.Services.AddTransient<CreateRentalRequestPage>();
        builder.Services.AddTransient<RentalRequestsViewModel>();
        builder.Services.AddTransient<RentalRequestsPage>();
        builder.Services.AddTransient<CreateReviewPage>();
        builder.Services.AddTransient<CreateReviewViewModel>();


        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        var app = builder.Build();

        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Debug.WriteLine($"DB Provider: {dbContext.Database.ProviderName}");
            Debug.WriteLine($"Can connect to DB: {dbContext.Database.CanConnect()}");

            var pending = dbContext.Database.GetPendingMigrations().ToList();
            Debug.WriteLine($"Pending migrations: {string.Join(",", pending)}");

            dbContext.Database.Migrate();

            var applied = dbContext.Database.GetAppliedMigrations().ToList();
            Debug.WriteLine($"Applied migrations: {string.Join(",", applied)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error applying migrations: {ex.Message}");
        }

        return app;
    }
}