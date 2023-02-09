using API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace API
{
    class Program
    {
        static public void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PlaceBookingContext>();

            var app = builder.Build();

            ////////////// Account functions

            app.MapGet("/accounts", async (PlaceBookingContext db) =>
            await db.Accounts.ToListAsync());

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
                newAcc.Role = inputAccount.Role;
                db.Accounts.Add(newAcc);
                await db.SaveChangesAsync();

                return Results.Ok(newAcc);
            });

            app.MapPut("/account/{idAccount}", async (int idAccount, Account inputAccount, PlaceBookingContext db) =>
            {
                var editAccount = await db.Accounts.FindAsync(idAccount);
                if (editAccount is null)
                {
                    return Results.NotFound();
                }
                editAccount.Name = inputAccount.Name;
                editAccount.Email = inputAccount.Email;
                editAccount.Password = inputAccount.Password;
                editAccount.DateOfBirthday = inputAccount.DateOfBirthday;
                editAccount.IdImage = inputAccount.IdImage;

                await db.SaveChangesAsync();
                return Results.Ok(editAccount);
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

            ////////////// Image functions

            app.MapGet("/image/{idImage}", async (int idImage, PlaceBookingContext db) =>
            await db.Images.FindAsync(idImage)
                    is Image image ? Results.Ok(image) : Results.NotFound());

            app.MapPost("/image", async (Image inputImage, PlaceBookingContext db) =>
            {
                db.Images.Add(inputImage);
                await db.SaveChangesAsync();

                return Results.Ok(inputImage);
            });

            ////////////// Cinema functions

            app.MapGet("/cinema/{cityName}", async (string cityName, PlaceBookingContext db) =>
            {
                return await db.Cinemas.Where(cinema => cinema.CityName == cityName).ToListAsync();
            });

            app.MapGet("/cinema/{Name}/{CityName}", async (string Name, string CityName, PlaceBookingContext db) =>
                await db.Cinemas.FirstOrDefaultAsync(cin => cin.Name == Name && cin.CityName == CityName)
                    is Cinema cinema ? Results.Ok(cinema) : Results.NotFound());


            app.MapGet("/cinemas", async (PlaceBookingContext db) =>
            await db.Cinemas.ToListAsync());


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

            app.MapPut("/editCinema", async (Cinema inputCinema, PlaceBookingContext db) =>
            {
                var editCinema = await db.Cinemas.FindAsync(inputCinema.IdCinema);
                if (editCinema is null)
                {
                    return Results.NotFound();
                }
                editCinema.Name = inputCinema.Name;
                editCinema.CityName = inputCinema.CityName;
                editCinema.Address = inputCinema.Address;

                await db.SaveChangesAsync();
                return Results.Ok(editCinema);
            });

            app.MapDelete("/cinema/{idCinema}", async (int idCinema, PlaceBookingContext db) =>
            {
                if (await db.Cinemas.FindAsync(idCinema) is Cinema cinema)
                {
                    db.Cinemas.Remove(cinema);
                    await db.SaveChangesAsync();
                    return Results.Ok(cinema);
                }
                return Results.NotFound();
            });

            ////////////// Get cities

            app.MapGet("/cities", async (PlaceBookingContext db) =>
             await db.Cinemas.Select(cinema => cinema.CityName).Distinct().ToListAsync()
            );

            ////////////// Hall functions

            app.MapGet("/halls/{idCinema}", async (int idCinema, PlaceBookingContext db) =>
            {
                return await db.Halls.Where(hall => hall.IdCinema == idCinema).ToListAsync();
            });

            app.MapPost("/hall", async (Hall inputHall, PlaceBookingContext db) =>
            {
                Hall newHall = new();
                newHall.IdCinema = inputHall.IdCinema;
                newHall.Number = inputHall.Number;
                newHall.Type = inputHall.Type;
                newHall.Capacity = inputHall.Capacity;

                db.Halls.Add(newHall);
                await db.SaveChangesAsync();

                return Results.Ok(newHall);
            }
            );

            app.MapPut("/editHall", async (Hall inputHall, PlaceBookingContext db) =>
            {
                var editHall = await db.Halls.FindAsync(inputHall.IdHall);
                if (editHall is null)
                {
                    return Results.NotFound();
                }
                editHall.Number = inputHall.Number;
                editHall.Type = inputHall.Type;
                editHall.Capacity = inputHall.Capacity;

                await db.SaveChangesAsync();
                return Results.Ok(editHall);
            });

            app.MapDelete("/hall/{idHall}", async (int idHall, PlaceBookingContext db) =>
            {
                if (await db.Halls.FindAsync(idHall) is Hall hall)
                {
                    db.Halls.Remove(hall);
                    await db.SaveChangesAsync();
                    return Results.Ok(hall);
                }
                return Results.NotFound();
            });

            ////////////// Place functions

            app.MapGet("/places/{idHall}", async (int idHall, PlaceBookingContext db) =>
            {
                return await db.Places.Where(place => place.IdHall == idHall).ToListAsync();
            });
           
            app.MapPost("/placeExistenceCheck", async ([FromBody] Place inputPlace, PlaceBookingContext db) =>
                await db.Places.FirstOrDefaultAsync(place => place.IdHall == inputPlace.IdHall && place.Row == inputPlace.Row && place.SeatNumber == inputPlace.SeatNumber)
                    is Place truePlace ? Results.Ok(truePlace) : Results.NotFound());

            app.MapPost("/place", async (Place inputPlace, PlaceBookingContext db) =>
            {
                Place newPlace = new();
                newPlace.IdHall = inputPlace.IdHall;
                newPlace.Row = inputPlace.Row;
                newPlace.SeatNumber = inputPlace.SeatNumber;

                db.Places.Add(newPlace);
                await db.SaveChangesAsync();

                return Results.Ok(newPlace);
            }
            );

            app.MapPut("/editPlace", async (Place inputPlace, PlaceBookingContext db) =>
            {
                var editPlace = await db.Places.FindAsync(inputPlace.IdPlace);
                if (editPlace is null)
                {
                    return Results.NotFound();
                }
                
                editPlace.IdHall = inputPlace.IdHall;
                editPlace.Row = inputPlace.Row;
                editPlace.SeatNumber = inputPlace.SeatNumber;
                
                await db.SaveChangesAsync();
                return Results.Ok(editPlace);
            });

            app.MapDelete("/place/{idPlace}", async (int idPlace, PlaceBookingContext db) =>
            {
                if (await db.Places.FindAsync(idPlace) is Place place)
                {
                    db.Places.Remove(place);
                    await db.SaveChangesAsync();
                    return Results.Ok(place);
                }
                return Results.NotFound();
            });

            app.Run();
        }
    }
}