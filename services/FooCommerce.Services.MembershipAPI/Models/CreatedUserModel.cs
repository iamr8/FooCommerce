﻿using FooCommerce.MembershipService.DbProvider.Entities;

namespace FooCommerce.MembershipService.Models;

public record CreatedUserModel
{
    public User User { get; set; }
    public UserPassword Password { get; set; }
    public UserRole Role { get; set; }
    public UserCommunication Communication { get; set; }
    public IList<UserSetting> Settings { get; set; }
    public IList<UserInformation> Information { get; set; }
}