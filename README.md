# DershaneTakipSistemi

apsettings.json güncellenmeli.
veritabanı hatası varsa

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<DershaneTakipSistemi.Data.ApplicationDbContext>();
        dbContext.Database.Migrate(); // Veritabanı göçlerini uygula
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı migration sırasında bir hata oluştu.");
    }
}
