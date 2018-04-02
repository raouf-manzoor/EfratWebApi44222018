using EfratWebApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EfratWebApi.Controllers
{
    [EnableCorsAttribute("*", "*", "*", SupportsCredentials = true)]
    public class AccountController : ApiController
    {
        // GET: Account
        [HttpPost]
        public dynamic SignUp(User input)
        {
            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                dbContext.Users.Add(input);
                dbContext.SaveChanges();
                return new
                {
                    IsSignUp = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsSignUp = false
                };
            }
        }

        [HttpPost]
        public dynamic AddVehicle(CarsForRent input)
        {
            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                dbContext.CarsForRents.Add(input);
                dbContext.SaveChanges();
                return new
                {
                    IsVehicleAdded = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsVehicleAdded = false
                };
            }
        }
        [HttpPost]
        public dynamic SignIn(User input)
        {
            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                var user = dbContext.Users.AsNoTracking().Single(e => e.Email == input.Email && e.Password == input.Password);
                user.RentsInfoes = null;
                user.RentsInfoes = null;
                return new
                {
                    IsSignIn = true,
                    signInUser = new
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email,
                        DateOfBirth = user.DateOfBirth,
                        Password = user.Password,
                        GenderBit = user.GenderBit,
                        PhotoID = user.PhotoID,
                        Type_Role = user.Type_Role,
                        Base64Image = user.Base64Image,
                        ImageType = user.ImageType,

                    }

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsSignIn = false,
                };
            }
        }
        [HttpPost]
        public dynamic GetPreviousOrders(User input)
        {
            EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            var ordersList=dbContext.RentsInfoes.Where(e => e.UserID == input.UserId);
            return ordersList.Select(e => new
            {
                ManufactorName = e.CarsForRent.ManufactorName,
                Model = e.CarsForRent.Model,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                ReturnDate = e.ReturnDate

            });
        }
        [HttpPost]
        public dynamic GetAllBranches()
        {
            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                var locations = dbContext.Branches;
                return locations.Select(e => new
                {
                    BranchID = e.BranchID,
                    BranchName = e.BranchName,
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public dynamic GetListOfAvailableCars(Branch branch)
        {
            EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            var availableCars = dbContext.CarsForRents.Where(e => e.Available && e.BranchID == branch.BranchID);
            return availableCars.Select(e => new
            {
                CarID = e.CarID,
                CarNumber = e.CarNumber,
                KM = e.KM,
                BranchID = e.BranchID,
                Base64Image = e.Base64Image,
                Model = e.Model,
                CostPerDay = e.CostPerDay,
                CostPerDayDelay = e.CostPerDayDelay,
                YearOfProduce = e.YearOfProduce,
                ManufactorName = e.ManufactorName,
                Gere = e.Gere,
                BranchName = e.Branch.BranchName,
            });
        }
        [HttpPost]
        public dynamic PlaceAnOrder(RentsInfo input)
        {

            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                dbContext.RentsInfoes.Add(input);
                var carToUpdate = dbContext.CarsForRents.Single(e => e.CarID == input.CarID);
                carToUpdate.Available = false;
                dbContext.SaveChanges();
                return new
                {
                    IsOrderPlaced = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderPlaced = false
                };
            }
        }

        // For Empolyee And Admin ALso
        [HttpPost]
        public dynamic GetListOfOrders()
        {
            EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            var ordersList = dbContext.RentsInfoes.Where(e => !e.ReturnDate.HasValue);
            return ordersList.Select(e => new
            {
                Id = e.Id,
                CarID = e.CarID,
                ManufactorName = e.CarsForRent.ManufactorName,
                Model = e.CarsForRent.Model,
                CarNumber = e.CarsForRent.CarNumber,
                Branch = e.CarsForRent.Branch.BranchName,
                Base64Image = e.CarsForRent.Base64Image,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                UserName = e.User.UserName

            });
        }


        [HttpPost]
        public dynamic SetCarToAvailable(RentsInfo input)
        {

            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                var orderToUpdate = dbContext.RentsInfoes.Single(e => e.Id == input.Id);
                var carToUpdate = dbContext.CarsForRents.Single(e => e.CarID == input.CarID);
                orderToUpdate.ReturnDate = input.ReturnDate;
                carToUpdate.Available = true;
                if (input.CurrentKM.HasValue)
                    carToUpdate.KM = input.CurrentKM.Value;

                // Make Dto For RentInfos With Additional Property of CurrentKms

                dbContext.SaveChanges();
                return new
                {
                    IsCarAvailable = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsCarAvailable = false
                };
            }
        }

        [HttpPost]
        public dynamic GetAllUsers()
        {
            EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            return dbContext.Users.ToList();
        }

        // Update User Method Uodate Whole User Project 

        [HttpPost]
        public dynamic DeleteUser(int userId)
        {

            try
            {
                EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
                var userToDelete = dbContext.Users.Single(e => e.UserId == userId);
                dbContext.Users.Remove(userToDelete);
                dbContext.SaveChanges();
                return new
                {
                    IsUserDeleted = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsUserDeleted = false
                };
            }
        }

        [HttpPost]
        public dynamic GetAllVehicles()
        {
            EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            return dbContext.CarsForRents.ToList();
        }

        [HttpPost]
        public dynamic RegisterUserOutPut()
        {
            //EfratDataBaseEntities1 dbContext = new EfratDataBaseEntities1();
            //var locations = dbContext.Branches.ToList();
            return new
            {
                IsRegistered = false,
                array = new List<string> { "Hello","Pakistan",
                "Mustang"},
                //locations= locations
            };
        }



        // Add Edit Delete For Vehicle Functions 
    }
}