﻿namespace AppRpgEtec.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordString { get; set; }
        public string Perfil{ get; set; }
        public string Token { get; set; }
        public byte[] Foto { get; set; }
        public string Email { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
