﻿using Plataforma.Educacao.Api.Migrations;

namespace Plataforma.Educacao.Api.Configurations;
public static class AmbienteConfiguration
{
    public static WebApplication ExecutarConfiguracaoAmbiente(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("Dev");

            DbMigrationHelper.AutocarregamentoDadosAsync(app).Wait();
        }
        else
        {
            app.UseCors("Prod");
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseHsts();
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self'");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block"); // Proteção contra XSS
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff"); // Prevenir MIME-sniffing
            await next();
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
