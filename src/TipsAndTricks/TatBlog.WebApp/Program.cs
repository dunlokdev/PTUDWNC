using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{ 
    // Thêm các dịch vụ được yêu cầu bởi MVC Framework
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<BlogDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"))
        );

    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}

var app = builder.Build();
{
    // Cấu hình HTTP Request pipeline

    // Thêm middleware để hiển thị thông báo lỗi
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");

        // Thêm middleware cho việc áp dụng HSTS (Thêm header Strict-Transport-Security vào HTTP Response
        app.UseHsts();
    }

    // Thêm middleware để chuyển hướng HTTP sang HTTPS
    app.UseHttpsRedirection();

    // Thêm midleware phục vụ các yêu cầu liên quan tới các tập tin tĩnh
    app.UseStaticFiles();
    
    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");
}

// Dữ liệu mẫu trước khi Run App
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();
