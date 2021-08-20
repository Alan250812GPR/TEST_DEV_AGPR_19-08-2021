using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TokaAPI.Models
{
    [Table("Tb_PersonasFisicas")]
    public partial class TbPersonasFisica
    {
        [Key]
        public int IdPersonaFisica { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de registro no es valida")]
        public DateTime? FechaRegistro { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de actualizacion no es valida")]
        public DateTime? FechaActualizacion { get; set; }

        [Required(ErrorMessage = "El nombre de la persona fisica es requerida.")]
        [StringLength(50)]
        [MinLength(3,ErrorMessage = "El nombre de la persona debe contener al menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El nombre de la persona debe contener maximo 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno de la persona fisica es requerida.")]
        [StringLength(50)]
        [MinLength(4,ErrorMessage = "El apellido paterno de la persona debe contener al menos 4 caracteres")]
        [MaxLength(50, ErrorMessage = "El apellido paterno de la persona debe contener maximo 50 caracteres")]

        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno de la persona fisica es requerida.")]
        [StringLength(50)]
        [MinLength(4,ErrorMessage = "El apellido materno de la persona debe contener al menos 4 caracteres")]
        [MaxLength(50, ErrorMessage = "El apellido pmterno de la persona debe contener maximo 50 caracteres")]
        public string ApellidoMaterno { get; set; }
        
        [Required(ErrorMessage = "El RFC es requerido")]
        [Column("RFC")]
        [StringLength(13)]
        [MinLength(13,ErrorMessage = "El RFC debe contener al menos 13 caracteres")]
        [MaxLength(13, ErrorMessage = "El RFC debe contener maximo 13 caracteres")]
        public string Rfc { get; set; }
        
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de nacimiento no es valida")]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        public int? UsuarioAgrega { get; set; }

        public bool? Activo { get; set; }
    }
}
