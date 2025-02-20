using System;
using System.Collections.Generic;

namespace Stock.Application.Models
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class RoleWithUsersDto : RoleDto
    {
        public ICollection<UserDto> Users { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 