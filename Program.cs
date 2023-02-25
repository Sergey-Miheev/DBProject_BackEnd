using API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

            app.MapGet("/images", async (PlaceBookingContext db) =>
            await db.Images.ToListAsync());

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
            
            app.MapGet("/placeExistenceCheck/{idHall}/{row}/{seatNumber}", async (int idHall, int row, int seatNumber, PlaceBookingContext db) =>
                await db.Places.FirstOrDefaultAsync(place => place.IdHall == idHall && place.Row == row && place.SeatNumber == seatNumber)
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

            ////////////// Actors functions

            app.MapGet("/actors", async (PlaceBookingContext db) =>
             await db.Actors.ToListAsync());

            app.MapPost("/actor", async (Actor inputActor, PlaceBookingContext db) =>
            {
                Actor newAct = new();
                newAct.Name = inputActor.Name;

                db.Actors.Add(newAct);
                await db.SaveChangesAsync();

                return Results.Ok(newAct);
            });

            app.MapPut("/editActor", async (Actor inputActor, PlaceBookingContext db) =>
            {
                var editActor = await db.Actors.FindAsync(inputActor.IdActor);
                if (editActor is null)
                {
                    return Results.NotFound();
                }
                editActor.Name = inputActor.Name;

                await db.SaveChangesAsync();
                return Results.Ok(editActor);
            });

            app.MapDelete("/actor/{idActor}", async (int idActor, PlaceBookingContext db) =>
            {
                if (await db.Actors.FindAsync(idActor) is Actor actor)
                {
                    db.Actors.Remove(actor);
                    await db.SaveChangesAsync();
                    return Results.Ok(actor);
                }
                return Results.NotFound();
            });

            ////////////// Role functions

            app.MapGet("/roles", async (PlaceBookingContext db) =>
             await db.Roles.ToListAsync());

            app.MapPost("/role", async (Role inputRole, PlaceBookingContext db) =>
            {
                Role newRole = new();
                newRole.IdActor = inputRole.IdActor;
                newRole.IdFilm = inputRole.IdFilm;
                newRole.NamePersonage = inputRole.NamePersonage;

                db.Roles.Add(newRole);
                await db.SaveChangesAsync();

                return Results.Ok(newRole);
            });

            app.MapPut("/editRole", async (Role inputRole, PlaceBookingContext db) =>
            {
                var editRole = await db.Roles.FindAsync(inputRole.IdRole);
                if (editRole is null)
                {
                    return Results.NotFound();
                }
                editRole.IdActor = inputRole.IdActor;
                editRole.IdFilm = inputRole.IdFilm;
                editRole.NamePersonage = inputRole.NamePersonage;

                await db.SaveChangesAsync();
                return Results.Ok(editRole);
            });

            app.MapDelete("/role/{idRole}", async (int idRole, PlaceBookingContext db) =>
            {
                if (await db.Roles.FindAsync(idRole) is Role role)
                {
                    db.Roles.Remove(role);
                    await db.SaveChangesAsync();
                    return Results.Ok(role);
                }
                return Results.NotFound();
            });

            ////////////// Film functions

            app.MapGet("/films", async (PlaceBookingContext db) =>
             await db.Films.ToListAsync());

            app.MapPost("/film", async (Film inputFilm, PlaceBookingContext db) =>
            {
                Film newFilm = new();
                newFilm.Duration = inputFilm.Duration;
                newFilm.Name = inputFilm.Name;
                newFilm.AgeRating = inputFilm.AgeRating;
                newFilm.Description = inputFilm.Description;

                db.Films.Add(newFilm);
                await db.SaveChangesAsync();

                return Results.Ok(newFilm);
            });

            app.MapPut("/editFilm", async (Film inputFilm, PlaceBookingContext db) =>
            {
                var editFilm = await db.Films.FindAsync(inputFilm.IdFilm);
                if (editFilm is null)
                {
                    return Results.NotFound();
                }
                editFilm.Duration = inputFilm.Duration;
                editFilm.Name = inputFilm.Name;
                editFilm.AgeRating = inputFilm.AgeRating;
                editFilm.Description = inputFilm.Description;

                await db.SaveChangesAsync();
                return Results.Ok(editFilm);
            });

            app.MapDelete("/film/{idFilm}", async (int idFilm, PlaceBookingContext db) =>
            {
                if (await db.Films.FindAsync(idFilm) is Film film)
                {
                    db.Films.Remove(film);
                    await db.SaveChangesAsync();
                    return Results.Ok(film);
                }
                return Results.NotFound();
            });

            ////////////// Session functions

            app.MapGet("/sessions", async (PlaceBookingContext db) =>
             await db.Sessions.ToListAsync());

            app.MapGet("/sessions/{idCinema}", async (int idCinema, PlaceBookingContext db) =>
            {              
                return await (from film in db.Films
                              join session in db.Sessions on film.IdFilm equals session.IdFilm
                              join hall in db.Halls on session.IdHall equals hall.IdHall
                              where hall.IdCinema == idCinema
                              select new
                              {
                                  idSession = session.IdSession,
                                  idHall = session.IdHall,
                                  hallNumber = hall.Number,
                                  hallType = hall.Type,
                                  dateTime = session.DateTime
                              }).ToListAsync();
            });

            app.MapPost("/session", async (Session inputSession, PlaceBookingContext db) =>
            {
                Session newSession = new();
                newSession.IdHall = inputSession.IdHall;
                newSession.IdFilm = inputSession.IdFilm;
                newSession.DateTime = inputSession.DateTime;

                db.Sessions.Add(newSession);
                await db.SaveChangesAsync();

                return Results.Ok(newSession);
            });

            app.MapPut("/editSession", async (Session inputSession, PlaceBookingContext db) =>
            {
                var editSession = await db.Sessions.FindAsync(inputSession.IdSession);
                if (editSession is null)
                {
                    return Results.NotFound();
                }
                editSession.IdHall = inputSession.IdHall;
                editSession.IdFilm = inputSession.IdFilm;
                editSession.DateTime = inputSession.DateTime;

                await db.SaveChangesAsync();
                return Results.Ok(editSession);
            });

            app.MapDelete("/session/{idSession}", async (int idSession, PlaceBookingContext db) =>
            {
                if (await db.Sessions.FindAsync(idSession) is Session session)
                {
                    db.Sessions.Remove(session);
                    await db.SaveChangesAsync();
                    return Results.Ok(session);
                }
                return Results.NotFound();
            });

            ////////////// Get films, shown in the cinema

            app.MapGet("/films/{idCinema}", async (int idCinema, PlaceBookingContext db) =>
            {
                return await (from film in db.Films
                              join session in db.Sessions on film.IdFilm equals session.IdFilm
                              join hall in db.Halls on session.IdHall equals hall.IdHall
                              where hall.IdCinema == idCinema
                              select film).ToListAsync();
            });

            ////////////// Get rows and seat numbers

            app.MapGet("/rows/{idHall}", async (int idHall, PlaceBookingContext db) =>
            {
                return await (from place in db.Places 
                              where place.IdHall == idHall 
                              select place.Row ).Distinct().ToListAsync();
            }
            );

            app.MapGet("/seatNums/{idHall}/{rowNum}", async (int idHall, int rowNum, PlaceBookingContext db) =>
            {
                return await (from place in db.Places 
                              where place.IdHall == idHall && place.Row == rowNum
                              select new
                              {
                                  idPlace = place.IdPlace,
                                  seatNumber = place.SeatNumber
                              }).ToListAsync();
            }
            );

            ////////////// Booking functions

            app.MapGet("/bookings", async (PlaceBookingContext db) =>
             await db.Bookings.ToListAsync());

            app.MapPost("/booking", async (Booking inputBooking, PlaceBookingContext db) =>
            {
                Booking newBooking = new();
                newBooking.IdSession = inputBooking.IdSession;
                newBooking.IdPlace = inputBooking.IdPlace;
                newBooking.IdAccount = inputBooking.IdAccount;
                newBooking.BookingCode = inputBooking.BookingCode;
                newBooking.DateTime = inputBooking.DateTime;

                db.Bookings.Add(newBooking);
                await db.SaveChangesAsync();

                return Results.Ok(newBooking);
            });

            app.MapPut("/editBookings", async (Booking inputBooking, PlaceBookingContext db) =>
            {
                var editBooking = await db.Bookings.FindAsync(inputBooking.IdBooking);
                if (editBooking is null)
                {
                    return Results.NotFound();
                }
                editBooking.IdSession = inputBooking.IdSession;
                editBooking.IdPlace = inputBooking.IdPlace;
                editBooking.IdAccount = inputBooking.IdAccount;
                editBooking.BookingCode = inputBooking.BookingCode;
                editBooking.DateTime = inputBooking.DateTime;

                await db.SaveChangesAsync();
                return Results.Ok(editBooking);
            });

            app.MapDelete("/booking/{idBooking}", async (int idBooking, PlaceBookingContext db) =>
            {
                if (await db.Bookings.FindAsync(idBooking) is Booking booking)
                {
                    db.Bookings.Remove(booking);
                    await db.SaveChangesAsync();
                    return Results.Ok(booking);
                }
                return Results.NotFound();
            });

            app.Run();
        }
    }
}