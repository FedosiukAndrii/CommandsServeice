﻿using System.ComponentModel.DataAnnotations;

namespace CommandsServeice.Models;

public class Platform
{
    [Key, Required]
    public int Id { get; set; }

    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Command> Commands { get; set; } = [];
}
