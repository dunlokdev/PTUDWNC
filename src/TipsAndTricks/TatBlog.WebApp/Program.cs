var builder = WebApplication.CreateBuilder(args);
{ 
    // Thêm các dịch vụ được yêu cầu bởi MVC Framework
    builder.Services.AddControllersWithViews();
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

app.Run();
