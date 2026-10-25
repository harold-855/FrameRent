using FrameRent.Application.UseCases.EquipmentManagement.CreateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentById;
using FrameRent.Application.UseCases.EquipmentManagement.GetAllEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.UpdateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.ActivateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.DeactivateEquipment;
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
            services.AddScoped<UpdateEquipmentUseCase>();
            services.AddScoped<ActivateEquipmentUseCase>();
            services.AddScoped<DeactivateEquipmentUseCase>();

            return services;
        }
    }
}