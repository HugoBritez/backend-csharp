using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("whatsapp_chats")]
    public class WhatsappChats
    {
        [Key]
        [Column("wa_chat_codigo")]
        public uint Codigo { get; set;}
        [Column("wa_user_id")]
        public string? UserId { get; set;}
        [Column("wa_user_name")]
        public string? UserName { get; set; }
        [Column("wa_user_phone")]
        public string? UserPhone { get; set; }
    }
}