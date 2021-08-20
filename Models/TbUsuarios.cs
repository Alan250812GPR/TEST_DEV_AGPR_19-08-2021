using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TokaAPI.Models
{
    [Table("Tb_Usuarios")]
    public partial class TbUsuarios
    {
        [Key]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("FechaRegistro",TypeName = "datetime")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de registro no es valida")]
        public DateTime? FechaRegistro { get; set; }

        [Column("FechaActualizacion",TypeName = "datetime")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de actualizacion no es valida")]
        public DateTime? FechaActualizacion { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(50)]
        [Column("Nombre")]
        [MinLength(3, ErrorMessage = "El nombre debe contener al menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El nombre debe contener maximo 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electronico es requerido.")]
        [StringLength(50)]
        [Column("Email")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        [MaxLength(50, ErrorMessage = "El apellido paterno de la persona debe contener maximo 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        
        [StringLength(500)]
        [Column("Clave")]
        public string Clave { get; set; }

        [Column("Salt")]
        [StringLength(500)]
        public string Salt { get; set; }

    }
}
