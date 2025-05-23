﻿namespace WalkerWebApp.ViewModels;

public class UserViewModel
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public int? Pace { get; set; }
    public int? Mileage { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public required string ProfileImageUrl { get; set; }

    public string Location => (City, State) switch
    {
        (string city, string state) => $"{city}, {state}",
        (string city, null) => city,
        (null, string state) => state,
        (null, null) => "",
    };

    public string? AboutMe { get; set; }
}