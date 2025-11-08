using System.ComponentModel.DataAnnotations;
using challenge.Domain.Entity;

namespace challenge.Domain.DTOs
{
    /// <summary>
    /// DTO para criação de veículo
    /// </summary>
    public class CreateVehicleDto
    {
        /// <summary>
        /// Placa do veículo (formato: AAA1234 ou AAA1A23)
        /// </summary>
        /// <example>ABC1234</example>
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres")]
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Modelo do veículo
        /// </summary>
        /// <example>SPORT</example>
        [Required(ErrorMessage = "O modelo é obrigatório")]
        public VehicleModel VehicleModel { get; set; }
    }

    /// <summary>
    /// DTO para atualização de veículo
    /// </summary>
    public class UpdateVehicleDto
    {
        /// <summary>
        /// ID do veículo
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        [Required]
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Placa do veículo (formato: AAA1234 ou AAA1A23)
        /// </summary>
        /// <example>XYZ9876</example>
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres")]
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Modelo do veículo
        /// </summary>
        /// <example>POP</example>
        [Required(ErrorMessage = "O modelo é obrigatório")]
        public VehicleModel VehicleModel { get; set; }
    }
}
