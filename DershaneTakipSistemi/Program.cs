// Gerekli using bildirimleri dosyanın en üstünde toplanır.
using DershaneTakipSistemi.Data;
using DershaneTakipSistemi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // ILogger için
using System; // Exception için


namespace DershaneTakipSistemi // Kendi namespace'inizi kontrol edin
{
    public class Program
    {
        public static async Task Main(string[] args) // async Task olarak de�i�tirildi
        {
            var builder = WebApplication.CreateBuilder(args);
            // =======================================================
            // 1. SERVISLERI KONTEYNERE EKLEME (Dependency Injection)
            // =======================================================

            // Veritabanı bağlantısı
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Geliştirme ortamı için veritabanı hata sayfası
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity Servisleri (Rollerle birlikte)
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>() // Rol yönetimi
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Kendi yazdığımız yardımcı servisler
            builder.Services.AddScoped<OgrenciService>();
            builder.Services.AddScoped<PersonelService>();
            builder.Services.AddScoped<SinifService>();
            builder.Services.AddScoped<KasaHareketiService>();
            builder.Services.AddScoped<DashboardService>();



            // İleride diğer servisleri de buraya ekleyebilirsiniz:
            // builder.Services.AddScoped<PersonelService>(); 
            // builder.Services.AddScoped<SinifService>(); 

            // MVC Controller, View ve Razor Pages servisleri
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); // Identity UI için gerekli


            // =======================================================
            // 2. HTTP ISTEK PIPELINE'INI YAPILANDIRMA (Middleware)
            // =======================================================

            var app = builder.Build();

            // Geliştirme ortamı için özel ayarlar
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            // Üretim ortamı için hata yönetimi
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Önce kimlik doğrulama, sonra yetkilendirme gelmeli. Sıra önemli!
            app.UseAuthentication();
            app.UseAuthorization();

            // Rotaları eşleştirme
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages(); // Identity UI sayfaları için


            // =======================================================
            // 3. UYGULAMA BAŞLANGICINDA VERITABANINI HAZIRLAMA
            // (Migration ve Admin Kullanıcısını Oluşturma)
            // =======================================================

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    // 1. Adım: Veritabanı göçlerini (migration) uygula
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                    logger.LogInformation("Veritabanı migration'ları başarıyla uygulandı.");

                    // 2. Adım: Admin rolünü ve kullanıcısını oluştur
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await SeedIdentityData.Initialize(userManager, roleManager, logger);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Uygulama başlangıcında veritabanı hazırlanırken bir hata oluştu.");
                }
            }


            // =======================================================
            // 4. UYGULAMAYI ÇALIŞTIRMA
            // =======================================================

            app.Run();
        }

        // =======================================================
        // YARDIMCI SINIF: Başlangıç verilerini oluşturan kısım
        // Program.cs dosyasını temiz tutmak için ayrı bir statik sınıfa taşıdık.
        // =======================================================
        public static class SeedIdentityData
        {
            public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
            {
                const string adminRoleName = "Admin";
                const string adminEmail = "admin@dershane.com";
                const string adminPassword = "Password123*"; // GERÇEK PROJELERDE BU ŞİFREYİ KULLANMAYIN!

                // Admin rolü yoksa oluştur
                if (!await roleManager.RoleExistsAsync(adminRoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));
                    logger.LogInformation($"'{adminRoleName}' rolü oluşturuldu.");
                }

                // Admin kullanıcısı yoksa oluştur
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"'{adminEmail}' kullanıcısı oluşturuldu ve '{adminRoleName}' rolüne atandı.");
                        await userManager.AddToRoleAsync(adminUser, adminRoleName);
                    }
                    else
                    {
                        // Hataları logla
                        var errorString = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"'{adminEmail}' kullanıcısı oluşturulamadı: {errorString}");
                    }
                }
                // Admin kullanıcısı var ama rolde değilse, role ata
                else if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                {
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                    logger.LogInformation($"Mevcut '{adminEmail}' kullanıcısı '{adminRoleName}' rolüne atandı.");
                }
            }
        }
    }
}