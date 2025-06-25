using DershaneTakipSistemi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc.Razor; // Bu genellikle gerekmez, kald�r�labilir.
using System.Threading.Tasks; // async Task Main i�in
using Microsoft.Extensions.Logging; // ILogger i�in
using System; // Exception i�in
using System.Linq; // LINQ metotlar� i�in (Select vb.)


namespace DershaneTakipSistemi // Kendi namespace'inizi kontrol edin
{
    public class Program
    {
        public static async Task Main(string[] args) // async Task olarak de�i�tirildi
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 1. Veritaban� Ba�lant�s� ve DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Geli�tirme ortam� i�in veritaban� hata sayfas�
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // 2. Identity Servisleri (Rollerle birlikte)
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>() // Rol y�netimi
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // 3. Repository Servisleri
            //builder.Services.AddScoped<IOgrenciRepository, EfOgrenciRepository>(); // ��renci Repository Kayd�
            // Buraya ileride IOdemeRepository vb. eklenecek

            // 4. MVC Controller ve View Servisleri
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); // Identity UI i�in Razor Pages deste�i

            // --- builder.Services ile ilgili eklemeler buraya ---


            var app = builder.Build(); // Uygulamay� olu�tur

            // Configure the HTTP request pipeline (Middleware S�ras� �nemli!).
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint(); // Geli�tirme ortam�nda migration endpoint'i
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Hata y�netimi
                app.UseHsts(); // HTTPS Strict Transport Security
            }

            app.UseHttpsRedirection(); // HTTPS y�nlendirmesi
            app.UseStaticFiles(); // CSS, JS, Resim gibi statik dosyalar i�in

            app.UseRouting(); // Y�nlendirme middleware'i

            app.UseAuthentication(); // Kimlik Do�rulama middleware'i (Authorization'dan �nce!)
            app.UseAuthorization(); // Yetkilendirme middleware'i

            // Endpoint Mapping
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages(); // Identity UI sayfalar� i�in endpointler

            // ----- Seed Data Kodu -----
            // (app nesnesi olu�turulduktan ve pipeline yap�land�r�ld�ktan sonra, app.Run() �ncesi)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var logger = services.GetRequiredService<ILogger<Program>>(); // Logger'� ba�ta alal�m

                    // Admin rol� yoksa olu�tur
                    string adminRoleName = "Admin";
                    if (!await roleManager.RoleExistsAsync(adminRoleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(adminRoleName));
                        logger.LogInformation($"'{adminRoleName}' rol� olu�turuldu."); // Loglama kullan�m�
                    }

                    // Admin kullan�c�s� yoksa olu�tur ve role ata
                    string adminEmail = "admin@dershane.com";
                    string adminPassword = "Password123!"; // Daha g��l� bir �ifre kullan�n!

                    var adminUser = await userManager.FindByEmailAsync(adminEmail);
                    if (adminUser == null)
                    {
                        adminUser = new IdentityUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            EmailConfirmed = true
                        };
                        var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);

                        if (createUserResult.Succeeded)
                        {
                            logger.LogInformation($"'{adminEmail}' kullan�c�s� olu�turuldu.");
                            await userManager.AddToRoleAsync(adminUser, adminRoleName);
                            logger.LogInformation($"'{adminEmail}' kullan�c�s� '{adminRoleName}' rol�ne atand�.");
                        }
                        else
                        {
                            logger.LogError($"'{adminEmail}' kullan�c�s� olu�turulamad�: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                        {
                            await userManager.AddToRoleAsync(adminUser, adminRoleName);
                            logger.LogInformation($"Mevcut '{adminEmail}' kullan�c�s� '{adminRoleName}' rol�ne atand�.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ba�lang�� verisi (seed) olu�turulurken bir hata olu�tu.");
                }
            }
            // --------------------------
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
            app.Run(); // Uygulamay� �al��t�r
        }
    }
}