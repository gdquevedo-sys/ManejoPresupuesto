using System.Net;
using System.Net.Mail;

namespace ManejoPresupuesto.Servicios
{
    public interface IServicioEmail
    {
        Task EnviarEmailCambioPassword(string receptor, string enlace);
    }

    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration configuration;

        public ServicioEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarEmailCambioPassword(string receptor, string enlace)
        {
            var email = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");

            if (string.IsNullOrWhiteSpace(receptor))
            {
                receptor = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:RECEPTOR");
            }

            var cliente = new SmtpClient(host, puerto);
            cliente.EnableSsl = true;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;

            cliente.Credentials = new NetworkCredential(email, password);
            var emisor = email;
            var subject = "¿Ha olvidado su contraseña?";
            var contenidoHtml = $@"Saludos,
Este mensaje le llega porque usted ha solicitado un cambio de contraseña. Si esta solicitud no fue hecha por usted, ignorar este mensaje.
Para cambiar su contraseña, haga click en el siguiente enlace:
{enlace}

Atentamente,
Equipo de manejo Presupuesto";

            var mensaje = new MailMessage(emisor, receptor, subject, contenidoHtml);
            mensaje.IsBodyHtml = false;
            try
            {
                await cliente.SendMailAsync(mensaje);

            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("Error al enviar el correo: " + ex.Message, ex);

            }
        }
    }
}
