using System;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
namespace CoolParking
{
    class Program
    {
        static void DisplayMainMessage()
        {
            Console.WriteLine("Enter a number what have to do:\n" +
               "1:Display current balance of parking\n" +
               "2:Display earned money for current period\n" +
               "3:Display amount of free place on parking\n" +
               "4:Display history of transactions for current period\n" +
               "5:Display list of transports which on the parking now\n" +
               "6:Add transpory on parking\n" +
               "7:Remove transport from the parking\n" +
               "8:Top up balance of a specific transport\n" +
               "9:Read information from log file(can be a lot of text)\n" +
               "10:Clear console and come back)\n" +
               "11:Exit program\n");
        }
        static Parking parking = Parking.GetParking();
        static ParkingService service;
        static string GetNormalStringOfTransactions()
        {
            string rez = "";
            for (int i = 0; i < parking.TransactionInfos.Count; i++)
            {
                rez += $"{parking.TransactionInfos[i].TimeTransaction:T} -- {parking.TransactionInfos[i].VehicleId}  {parking.TransactionInfos[i].Sum}\n";
            }
            return rez;
        }
        static void Main(string[] args)
        {
            service = new ParkingService(true);
            DisplayMainMessage();
            MainMethod();

        }
        static void MainMethod()
        {

            string number = Console.ReadLine();
            try
            {
                switch (number)
                {
                    case "1":
                        Console.WriteLine($"Current balance is {service.GetBalance()}\n");
                        break;
                    case "2":
                        Console.WriteLine($"Earned money on current period {parking.CurentBalance}\n");//to do
                        break;
                    case "3":
                        Console.WriteLine($"Amount free place of parking is {service.GetFreePlaces(true)}");
                        break;
                    case "4":
                        Console.WriteLine($"The history of transactions for current period is {GetNormalStringOfTransactions()}");
                        break;
                    case "5":
                        for (int i = 0; i < parking.Vehicles.Count; i++)
                            Console.Write(parking.Vehicles[i].Id + " " + parking.Vehicles[i].Balance + "\n");
                        break;
                    case "6":
                        Console.WriteLine("Enter transport type" +
                            "(1 - Bus, 2 - PassengerCar, 3 - Truck and 4 - Motorcycle) and how much money you give");
                        string id = Vehicle.GenerateRandomRegistrationPlateNumber();
                        string type = Console.ReadLine();
                        var Type = type switch
                        {
                            "1" => VehicleType.Bus,
                            "2" => VehicleType.PassengerCar,
                            "3" => VehicleType.Truck,
                            "4" => VehicleType.Motorcycle,
                            _ => throw new ArgumentException("Incorrect type"),
                        };
                        decimal money = Convert.ToDecimal(Console.ReadLine());
                        Vehicle vehicle = new Vehicle(id, Type, money);
                        service.AddVehicle(vehicle);
                        Console.WriteLine("Successfully added\n");
                        break;
                    case "7":
                        Console.WriteLine("Enter the id of transport you want to remove:");
                        string idRemove = Console.ReadLine();
                        service.RemoveVehicle(idRemove);
                        Console.WriteLine("Successfully removed");
                        break;
                    case "8":
                        Console.WriteLine("Enter sum and id of transport:");
                        decimal sum = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine(sum);
                        string ids = Console.ReadLine();
                        service.TopUpVehicle(ids, sum);
                        break;
                    case "9":
                        Console.WriteLine(service.ReadFromLog());
                        break;
                    case "10":
                        Console.Clear();
                        break;
                    case "11":
                        service.Dispose();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Entered incorrect number, try again");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            finally
            {
                DisplayMainMessage();
                MainMethod();
            }
        }
    }
}
