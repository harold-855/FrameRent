using FrameRent.Application.UseCases.EquipmentManagement.CreateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentById;
using FrameRent.Application.UseCases.EquipmentManagement.GetAllEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetActiveEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentAvailability;
using FrameRent.Application.UseCases.EquipmentManagement.UpdateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.ActivateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.DeactivateEquipment;
using FrameRent.Application.UseCases.Rentals.ApproveRental;
using FrameRent.Application.UseCases.Rentals.CancelRental;
using FrameRent.Application.UseCases.Rentals.CreateRental;
using FrameRent.Application.UseCases.Rentals.DeliverRental;
using FrameRent.Application.UseCases.Rentals.GetAllRentals;
using FrameRent.Application.UseCases.Rentals.GetMyRentals;
using FrameRent.Application.UseCases.Rentals.GetRentalById;
using FrameRent.Application.UseCases.Rentals.RejectRental;
using FrameRent.Application.UseCases.Rentals.ReturnRental;
using FrameRent.Application.UseCases.Reports.GetTopEquipment;
using Microsoft.Extensions.DependencyInjection;



namespace FrameRent.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<CreateEquipmentUseCase>();
            services.AddScoped<GetEquipmentByIdUseCase>();
            services.AddScoped<GetAllEquipmentUseCase>();
            services.AddScoped<GetActiveEquipmentUseCase>();
            services.AddScoped<GetEquipmentAvailabilityUseCase>();
            services.AddScoped<UpdateEquipmentUseCase>();
            services.AddScoped<ActivateEquipmentUseCase>();
            services.AddScoped<DeactivateEquipmentUseCase>();

            services.AddScoped<CreateRentalUseCase>();
            services.AddScoped<GetRentalByIdUseCase>();
            services.AddScoped<GetMyRentalsUseCase>();
            services.AddScoped<GetAllRentalsUseCase>();
            services.AddScoped<CancelRentalUseCase>();
            services.AddScoped<ApproveRentalUseCase>();
            services.AddScoped<RejectRentalUseCase>();
            services.AddScoped<DeliverRentalUseCase>();
            services.AddScoped<ReturnRentalUseCase>();
            services.AddScoped<GetTopEquipmentUseCase>();

            return services;
        }
    }
}