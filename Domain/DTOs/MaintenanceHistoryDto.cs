using System.ComponentModel.DataAnnotations;

namespace challenge.Domain.DTOs
{
    /// <summary>
    /// DTO para criação de histórico de manutenção
    /// </summary>
    public class CreateMaintenanceHistoryDto
    {
        /// <summary>
        /// ID do veículo
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        [Required(ErrorMessage = "O ID do veículo é obrigatório")]
        public Guid VehicleID { get; set; }

        /// <summary>
        /// ID do usuário responsável pela manutenção
        /// </summary>
        /// <example>456e7890-e89b-12d3-a456-426614174001</example>
        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public Guid UserID { get; set; }

        /// <summary>
        /// Data da manutenção
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [Required(ErrorMessage = "A data da manutenção é obrigatória")]
        public DateTime MaintenanceDate { get; set; }

        /// <summary>
        /// Descrição da manutenção realizada
        /// </summary>
        /// <example>Troca de óleo e filtros</example>
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de histórico de manutenção
    /// </summary>
    public class UpdateMaintenanceHistoryDto
    {
        /// <summary>
        /// ID do histórico de manutenção
        /// </summary>
        /// <example>789e0123-e89b-12d3-a456-426614174002</example>
        [Required]
        public Guid MaintenanceHistoryID { get; set; }

        /// <summary>
        /// ID do veículo
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        [Required(ErrorMessage = "O ID do veículo é obrigatório")]
        public Guid VehicleID { get; set; }

        /// <summary>
        /// ID do usuário responsável pela manutenção
        /// </summary>
        /// <example>456e7890-e89b-12d3-a456-426614174001</example>
        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public Guid UserID { get; set; }

        /// <summary>
        /// Data da manutenção
        /// </summary>
        /// <example>2024-01-15T14:30:00</example>
        [Required(ErrorMessage = "A data da manutenção é obrigatória")]
        public DateTime MaintenanceDate { get; set; }

        /// <summary>
        /// Descrição da manutenção realizada
        /// </summary>
        /// <example>Revisão completa do motor</example>
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; } = string.Empty;
    }
}

