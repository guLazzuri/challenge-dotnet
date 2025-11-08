using System.ComponentModel.DataAnnotations;
using challenge.Domain.Entity;

namespace challenge.Domain.DTOs
{
    /// <summary>
    /// DTO para criação de usuário
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        /// <example>usuario@exemplo.com</example>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário (mínimo 8 caracteres, com maiúscula, minúscula, número e caractere especial)
        /// </summary>
        /// <example>MinhaSenh@123</example>
        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        /// <example>ADMIN</example>
        [Required(ErrorMessage = "O tipo do usuário é obrigatório")]
        public UserType Type { get; set; }
    }

    /// <summary>
    /// DTO para atualização de usuário
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        [Required]
        public Guid UserID { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        /// <example>novoemail@exemplo.com</example>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário (mínimo 8 caracteres, com maiúscula, minúscula, número e caractere especial)
        /// </summary>
        /// <example>NovaSenh@456</example>
        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        /// <example>CLIENT</example>
        [Required(ErrorMessage = "O tipo do usuário é obrigatório")]
        public UserType Type { get; set; }
    }
}
