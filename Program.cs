using API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace API
{
    class Program
    {
        static public Account Clone(Account inputAccount, Account editAccount)
        {
            editAccount.Name = inputAccount.Name;
            editAccount.Email = inputAccount.Email;
            editAccount.Password = inputAccount.Password;
            editAccount.DateOfBirthday = inputAccount.DateOfBirthday;
            editAccount.IdImage = inputAccount.IdImage;
            return editAccount;
        }
        static public void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PlaceBookingContext>();

            var app = builder.Build();

            app.MapGet("/accounts", async (PlaceBookingContext db) =>
            await db.Accounts.ToListAsync());
            /*
            app.MapGet("/account/{IdAccount}", async (int idAccount, PlaceBookingContext db) =>
                await db.Accounts.FindAsync(idAccount)
                    is Account account ? Results.Ok(account) : Results.NotFound());
            */
            app.MapGet("/account/{Email}", async (string Email, PlaceBookingContext db) =>
                await db.Accounts.FirstOrDefaultAsync(acc => acc.Email == Email)
                    is Account account ? Results.Ok(account) : Results.NotFound());

            app.MapGet("/account/{Email}/{Password}", async (string Email, string Password, PlaceBookingContext db) =>
                await db.Accounts.FirstOrDefaultAsync(acc => acc.Email == Email && acc.Password == WorkFunctionsClass.Hashing(Password))
                    is Account account ? Results.Ok(account) : Results.NotFound());

            app.MapPost("/account", async (Account inputAccount, PlaceBookingContext db) =>
            {
                Account newAcc = new();
                newAcc.Name = inputAccount.Name;
                newAcc.Email = inputAccount.Email;
                newAcc.Password = WorkFunctionsClass.Hashing(inputAccount.Password);
                newAcc.DateOfBirthday = inputAccount.DateOfBirthday;
                newAcc.IdImage = inputAccount.IdImage;
                db.Accounts.Add(newAcc);
                await db.SaveChangesAsync();

                return Results.Ok(newAcc);
            });

            app.MapPut("/account/{idAccount}", async (int idAccount, Account inputAccount, PlaceBookingContext db) =>
            {
                var account = await db.Accounts.FindAsync(idAccount);
                if (account is null)
                {
                    return Results.NotFound();
                }

                account = Clone(inputAccount, account);

                await db.SaveChangesAsync();
                return Results.Ok(account);
            });

            app.MapDelete("/account/{idAccount}", async (int idAccount, PlaceBookingContext db) =>
            {
                if (await db.Accounts.FindAsync(idAccount) is Account account)
                {
                    db.Accounts.Remove(account);
                    await db.SaveChangesAsync();
                    return Results.Ok(account);
                }
                return Results.NotFound();
            });

            app.MapGet("/image/{idImage}", async (int idImage, PlaceBookingContext db) =>
            await db.Images.FindAsync(idImage)
                    is Image image ? Results.Ok(image) : Results.NotFound());

            app.MapPost("/image", async (Image inputImage, PlaceBookingContext db) =>
            {
                db.Images.Add(inputImage);
                await db.SaveChangesAsync();

                return Results.Ok(inputImage);
            });

            app.MapPost("/cinema", async (Cinema inputCinema, PlaceBookingContext db) =>
            {
                Cinema newCinema = new();
                newCinema.Name = inputCinema.Name;
                newCinema.Address = inputCinema.Address;
                newCinema.CityName = inputCinema.CityName;

                db.Cinemas.Add(newCinema);
                await db.SaveChangesAsync();

                return Results.Ok(newCinema);
            }
            );

            app.MapGet("/cinema/{cityName}", async (string cityName, PlaceBookingContext db) =>
            {
                return await db.Cinemas.Where(cinema => cinema.CityName == cityName).ToListAsync();
            });

            app.MapPost("/image", async (Image inputImage, PlaceBookingContext db) =>
            {
                db.Images.Add(inputImage);
                await db.SaveChangesAsync();

                return Results.Ok(inputImage);
            });

            app.MapGet("/cities", async (PlaceBookingContext db) =>
             await db.Cinemas.Select(cinema => cinema.CityName).Distinct().ToListAsync()
            );

            app.Run();
        }
    }
}