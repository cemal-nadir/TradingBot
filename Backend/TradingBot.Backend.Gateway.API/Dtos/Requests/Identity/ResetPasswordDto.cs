﻿namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;

public class ResetPasswordDto
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}